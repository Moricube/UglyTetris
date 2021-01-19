using System;

namespace UglyTetris.GameLogic
{
    public class Game
    {
        public Game(INextFigureFactory nextFigureFactory)
        {
            _nextFigureFactory = nextFigureFactory;
            _currentFigure = _nextFigureFactory.GetRandomFigure();
            _nextFigure = _nextFigureFactory.GetRandomFigure();
        }
        
        public bool IsFalling { get; set; }

        private int _tickCount = 0;
        private Figure _currentFigure;
        private Figure _nextFigure;
        
        int MoveDownPeriodTicks { get; } = 50;

        private int FallDownPeriodTicks { get; } = 3;
        private TimeSpan _gameTimer = new TimeSpan(0, 0, 0);

        public Figure GetNextFigure()
        {
            _currentFigure = _nextFigure;

            _nextFigure = _nextFigureFactory.GetRandomFigure();
            
            return _currentFigure;
        }

        public Figure GetCurrentFigureValue()
        {
            return _currentFigure;
        }
        
        public Figure GetNextFigureValue()
        {
            return _nextFigure;
        }

        public TimeSpan GetGameTimerValue()
        {
            return _gameTimer;
        }
        public void GameTimerTick()
        {
            if (State == GameState.Running)
            {
                _gameTimer = _gameTimer.Add(new TimeSpan(0, 0, 1));   
            }
        }

        private int _lines = 0;
        public int Lines
        {
            get => _lines;
            private set
            {
                _lines = value;
                LinesChanged?.Invoke(this, EventArgs.Empty);
            }
        }


        private GameState _state = GameState.Running;
        public GameState State
        {
            get => _state;
            private set
            {
                _state = value;
                StateChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void PauseGame()
        {
            State = GameState.Pause;
        }
        
        public void UnpauseGame()
        {
            State = GameState.Unpause;
            State = GameState.Running;
        }

        public void Tick()
        {
            if (State == GameState.GameOver)
            {
                return;
            }
            
            _tickCount++;

            bool moveDown = IsFalling
                ? (_tickCount % FallDownPeriodTicks == 0)
                : (_tickCount % MoveDownPeriodTicks == 0);


            if (moveDown)
            {
                var y = FigurePositionY + 1;
                var x = FigurePositionX;

                if (!Field.IsPossibleToPlaceFigure(Figure, x, y))
                {
                    Field.LockFigure(Figure, FigurePositionX, FigurePositionY, true);
                    
                    var lineCount = Field.RemoveFullLines();
                    Lines += lineCount;

                    RaiseFigureStateChanged();

                    var figure = GetNextFigure();

                    if (!ResetFigure(figure))
                    {
                        State = GameState.GameOver;
                    }
                    
                    _tickCount = 0;
                    IsFalling = false;
                }
                else
                {
                    FigurePositionX = x;
                    FigurePositionY = y;
                    RaiseFigureStateChanged();
                }
            }
        }

        public void MoveLeft()
        {
            FigurePositionX--;

            if (!Field.IsPossibleToPlaceFigure(Figure, FigurePositionX, FigurePositionY))
            {
                FigurePositionX++;
                return;
            }

            RaiseFigureStateChanged();
        }

        public void MoveRight()
        {
            FigurePositionX++;

            if (!Field.IsPossibleToPlaceFigure(Figure, FigurePositionX, FigurePositionY))
            {
                FigurePositionX--;
                return;
            }

            RaiseFigureStateChanged();
        }

        public void RotateAntiClockWise()
        {
            Figure.RotateLeft();
            if (!Field.IsPossibleToPlaceFigure(Figure, FigurePositionX, FigurePositionY))
            {
                Figure.RotateRight();
            }
            RaiseFigureStateChanged();
        }

        public void RotateClockWise()
        {
            Figure.RotateRight();
            if (!Field.IsPossibleToPlaceFigure(Figure, FigurePositionX, FigurePositionY))
            {
                Figure.RotateLeft();
            }
            RaiseFigureStateChanged();
        }

        public void Drop()
        {
            IsFalling = true;
        }

        public Figure Figure { get; private set; } = new Figure();

        public event EventHandler FigureStateChanged;
        protected void RaiseFigureStateChanged()
        {
            FigureStateChanged?.Invoke(this, EventArgs.Empty);
        }
        
        public event EventHandler LinesChanged; // may be replaced with INotifyPropertyChanged interface implementation
        
        public event EventHandler StateChanged; // may be replaced with INotifyPropertyChanged interface implementation

        public int FigurePositionX { get; private set; } = 6;

        public int FigurePositionY { get; private set; } = 0;

        public Field Field;
        

        public bool ResetFigure(Figure newFigure)
        {
            FigurePositionX = (Field.Xmax - Field.Xmin) / 2;
            FigurePositionY = 0;

            if (Field.IsPossibleToPlaceFigure(newFigure, FigurePositionX, FigurePositionY))
            {
                Figure = newFigure;
                return true;
            }

            return false; //cannot reset figure
        }

        private INextFigureFactory _nextFigureFactory;
    }
}