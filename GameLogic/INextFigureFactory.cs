namespace UglyTetris.GameLogic
{
    public interface INextFigureFactory
    {
        Figure GetNextFigure();
        Figure GetFigureFromNext();
    }
}