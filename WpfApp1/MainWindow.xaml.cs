using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using UglyTetris.GameLogic;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            _figureDrawer = new FigureDrawer(new TileDrawer(MainCanvas));
            _fieldDrawer = new FieldDrawer(new TileDrawer(MainCanvas));
            _nextFigureDrawer = new FigureDrawer(new TileDrawer(NextFigureCanvas));

            Game = new Game(new RandomNextFigureFactory());
            Game.FigureStateChanged += GameOnFigureStateChanged;
            Game.LinesChanged += GameOnLinesChanged;
            Game.StateChanged += GameOnStateChanged;
            
            Game.Field = Field.CreateField(FieldHelper.FieldDefaultWidth, FieldHelper.FieldDefaultHeight, "DimGray");
            Game.ResetFigure(Game.GetCurrentFigureValue());

            _figureDrawer.DrawFigure(Game.Figure, Game.FigurePositionX, Game.FigurePositionY);
            _nextFigureDrawer.DrawFigure(Game.GetNextFigureValue(), 0, 0);
            _fieldDrawer.AttachToField(Game.Field);


            _timer = new System.Windows.Threading.DispatcherTimer {Interval = TimeSpan.FromMilliseconds(10)};
            _timer.Tick += (sender, args) => { Game.Tick(); };
            _timer.Start();
                
            TimeTextBlock.Text = Game.GetGameTimerValue().ToString("hh\\:mm\\:ss");
            
            _userGameSessionTimer = new System.Windows.Threading.DispatcherTimer {Interval = TimeSpan.FromSeconds(1)};
            _userGameSessionTimer.Tick += (sender, args) =>
            {
                Game.GameTimerTick();
                TimeTextBlock.Text = Game.GetGameTimerValue().ToString("hh\\:mm\\:ss");
            };
            _userGameSessionTimer.Start();
        }

        private void GameOnStateChanged(object sender, EventArgs e)
        {
            if (Game.State == GameState.Unpause)
            {
                _timer.Start();
                _userGameSessionTimer.Start();
            }
            
            if (Game.State == GameState.Pause)
            {
                _timer.Stop();
                _userGameSessionTimer.Stop();
            }
            
            if (Game.State == GameState.GameOver)
            {
                _timer.Stop();
                _userGameSessionTimer.Stop();
                MessageBox.Show("GAME OVER");
            }
        }

        private void GameOnLinesChanged(object sender, EventArgs e)
        {
            LineCountTextBlock.Text = Game.Lines.ToString(CultureInfo.InvariantCulture);
        }

        private void GameOnFigureStateChanged(object sender, EventArgs e)
        {
            _figureDrawer.DrawFigure(Game.Figure, Game.FigurePositionX, Game.FigurePositionY);
            _nextFigureDrawer.DrawFigure(Game.GetNextFigureValue(), 0, 0);
        }


        public Game Game;
        private readonly System.Windows.Threading.DispatcherTimer _timer;
        private readonly System.Windows.Threading.DispatcherTimer _userGameSessionTimer;
        private TimeSpan _userGameSessionSpendTime;

        private FieldDrawer _fieldDrawer;
        private FigureDrawer _figureDrawer;
        private FigureDrawer _nextFigureDrawer;
        
        private void MoveLeft()
        {
            Game.MoveLeft();
        }

        private void MoveRight()
        {
            Game.MoveRight();
        }

        private void LeftArrowBtnPress(object sender, RoutedEventArgs e)
        {
            MoveLeft();
        }

        private void RotateAntiClockWise()
        {
            Game.RotateAntiClockWise();
        }

        private void RotateClockWise()
        {
            Game.RotateClockWise();
        }

        private new void Drop()
        {
            Game.Drop();
        }
        
        private FigureFactory _figureFactory = new FigureFactory();

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.IsRepeat)
            {
                return;
            }

            if (e.Key == Key.P)
            {
                if (Game.State == GameState.Running)
                {
                    Game.PauseGame();
                    return;
                }
                if (Game.State == GameState.Pause)
                {
                    Game.UnpauseGame();
                    return;
                }
            }

            if (e.Key == Key.Left)
            {
                MoveLeft();
            }
            else if (e.Key == Key.Right)
            {
                MoveRight();
            }
            else if (e.Key == Key.Up)
            {
                RotateAntiClockWise();
            }
            else if (e.Key == Key.Down)
            {
                RotateClockWise();
            }

            else if (e.Key == Key.Space)
            {
                Drop();
            }
        }
        private void TopArrowBtnPress(object sender, RoutedEventArgs e)
        {
            RotateAntiClockWise();
        }

        private void BottomArrowBtnPress(object sender, RoutedEventArgs e)
        {
            RotateClockWise();
        }
        private void RightArrowBtnPress(object sender, RoutedEventArgs e)
        {
            MoveRight();
        }

        private void SpaceBtnPress(object sender, RoutedEventArgs e)
        {
            Drop();
        }
    }

    internal class RandomNextFigureFactory : INextFigureFactory
    {
        public Figure GetRandomFigure()
        {
            return _figureFactory.CreateRandomFigure();
        }

        readonly FigureFactory _figureFactory = new FigureFactory();
    }
}
