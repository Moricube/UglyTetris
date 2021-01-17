using System;

namespace UglyTetris.GameLogic
{
    public enum FigureType
    {
        I,O,T,S,Z,J,L
    }

    public class FigureFactory
    {
        private Figure _nextFigure;
        
        public Figure CreateStandardFigure(FigureType figureType)
        {
            string tiles = "";
            var nl = Environment.NewLine;
            string color;

            switch (figureType)
            {
                case FigureType.I:
                    tiles = "  x " + nl +
                            "  x " + nl +
                            "  x " + nl +
                            "  x ";
                    color = "Cyan";
                    break;

                case FigureType.O:
                    tiles = "    " + nl +
                            " xx " + nl +
                            " xx " + nl +
                            "    ";
                    color = "Yellow";
                    break;

                case FigureType.T:
                    tiles = "    " + nl +
                            " xxx" + nl +
                            "  x " + nl +
                            "    ";
                    color = "BlueViolet";
                    break;

                case FigureType.S:
                    tiles = " x  " + nl +
                            " xx " + nl +
                            "  x " + nl +
                            "    ";
                    color = "Red";
                    break;

                case FigureType.Z:
                    tiles = "  x " + nl +
                            " xx " + nl +
                            " x  " + nl +
                            "    ";
                    color = "Brown";
                    break;

                case FigureType.J:
                    tiles = "  x " + nl +
                            "  x " + nl +
                            " xx " + nl +
                            "    ";
                    color = "Blue";
                    break;

                case FigureType.L:
                    tiles = " x  " + nl +
                            " x  " + nl +
                            " xx " + nl +
                            "    ";
                    color = "Green";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new Figure(tiles, color);
        }

        public Figure CreateRandomFigure()
        {
            var r = new Random();
            var randomType = r.Next(0, Enum.GetNames(typeof(FigureType)).Length);

            return CreateStandardFigure((FigureType) randomType);
        }
        
        public Figure CreateNextFigure()
        {
            var r = new Random();
            var randomType = r.Next(0, Enum.GetNames(typeof(FigureType)).Length);

            _nextFigure = CreateStandardFigure((FigureType) randomType);

            return _nextFigure;
        }

        public Figure GetFigureFromNext()
        {
            return _nextFigure;
        }
    }
}
