using System.Security.Policy;
using FluentAssertions;
using UglyTetris.GameLogic;
using Xunit;

namespace Tests
{
    public class GameTest
    {
        [Fact]
        public void RotateFigure()
        {
            //SUT: 
            // Game:
            //   - field
            //   - figure
            // Game.RotateACW

            var game = new Game(new NextFigureFactoryStub());

            Tile TileF() => new Tile("Brown");

            game.Field = new Field(new Tile[,]
                {
                    // 0 -> Y
                    // | 
                    // v X 
                    {TileF(), TileF(), TileF(), TileF(), TileF(), TileF(), TileF(), TileF(), },
                    {null, null, null, null, null, null, null, TileF(), },
                    {null, null, null, null, null, null, null, TileF(), },
                    {null, null, null, null, null, null, null, TileF(), },
                    {null, null, null, null, null, null, null, TileF(), },
                    {null, null, null, null, null, null, null, TileF(), },
                    {null, null, null, null, null, null, null, TileF(), },
                    {null, null, null, null, null, null, null, TileF(), },
                    {null, null, null, null, null, null, null, TileF(), },
                    {TileF(), TileF(), TileF(), TileF(), TileF(), TileF(), TileF(), TileF(), },
                }, 1, 1
            );

            var figureFactory = new FigureFactory();

            var figure = figureFactory.CreateStandardFigure(FigureType.I);

            var notRotatedFigure = new Figure(figure);

            game.ResetFigure(figure).Should().BeTrue();

            game.Figure.Should().BeEquivalentTo(notRotatedFigure);

            game.RotateAntiClockWise();

            game.Figure.Should().NotBeEquivalentTo(notRotatedFigure);

            game.MoveLeft();
            game.RotateAntiClockWise();
            game.MoveLeft();
            game.RotateAntiClockWise();
            game.MoveLeft();
            game.RotateAntiClockWise();
            game.MoveLeft();
            game.RotateAntiClockWise();
            game.MoveLeft();
            game.RotateAntiClockWise();
            game.MoveLeft();
            
            // now the figure is at the most left
            // the wall should not let it rotate

            var figureAtLeftWallCopy = new Figure(game.Figure);
            
            game.RotateAntiClockWise();
            
            game.Figure.Should().BeEquivalentTo(figureAtLeftWallCopy);
        }
        
        [Fact]
        public void PauseGame()
        {
            var game = new Game(new NextFigureFactoryStub());

            game.PauseGame();
            
            game.State.Should().Be(GameState.Pause);
        }
        
        [Fact]
        public void UnpauseGame()
        {
            var game = new Game(new NextFigureFactoryStub());

            game.PauseGame();
            game.UnpauseGame();
            
            game.State.Should().Be(GameState.Running);
        }

        [Fact]
        public void CheckNextFigure()
        {
            var figureFactory = new FigureFactory();

            var nextFigure = figureFactory.CreateNextFigure();
            figureFactory.GetFigureFromNext().Should().Be(nextFigure);
        }
        
        private class NextFigureFactoryStub : INextFigureFactory
        {
            public Figure GetNextFigure()
            {
                return new Figure();
            }

            public Figure GetFigureFromNext()
            {
                return new Figure();
            }
        }
    }
}