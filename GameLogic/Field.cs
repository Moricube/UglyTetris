using System;
using System.Collections.Generic;

namespace UglyTetris.GameLogic
{
    public class Field
    {
        public Field(Tile[,] initialTiles, int marginFromLeft, int marginFromRight)
        {
            _tiles = initialTiles;
            leftBorder = _tiles.GetLowerBound(0) + marginFromLeft;
            rightBorder = _tiles.GetUpperBound(0) - marginFromRight;
            bottomBorder = _tiles.GetUpperBound(1) - 1;
        }
        
        public static Field CreateField(int width, int height, string color)
        {
            var tiles = new Tile[width + 2, height + 1];

            for (var i = 0; i < height + 1; i++) // +1 each side for the walls
            {
                tiles[0, i] = new Tile(color);
                tiles[width + 1, i] = new Tile(color);
            }

            for (var i = 1; i < width + 1; i++)
            {
                tiles[i, height] = new Tile("DimGray");
            }

            return new Field(tiles, 1, 1);
        }
        
        public int Xmin => _tiles.GetLowerBound(0);
        public int Xmax => _tiles.GetUpperBound(0);
        public int Width => Xmax - Xmin;
        public int Ymin => _tiles.GetLowerBound(1);
        public int Ymax => _tiles.GetUpperBound(1);
        
        private int leftBorder;
        private int rightBorder;
        private int bottomBorder;

        public int Height => Ymax - Ymin;

        public bool IsInBounds(int x, int y)
        {
            return x >= Xmin && x <= Xmax && y >= Ymin && y <= Ymax;
        }

        public bool IsEmpty(int x, int y)
        {
            if (!IsInBounds(x, y))
            {
                return false;
            }

            return _tiles[x, y] == null;
        }
        
        public Tile GetTile(int x, int y)
        {
            if (IsInBounds(x, y))
            {
                return _tiles[x, y];
            }

            return null;
        }

        public IEnumerable<TileXy> GetTiles()
        {
            for (var i = Xmin; i <= Xmax; i++)
            {
                for (var j = Ymin; j <= Ymax; j++)
                {
                    var tile = GetTile(i, j);
                    if (tile != null)
                    {
                        yield return new TileXy {Tile = tile, X = i, Y = j};
                    }
                }
            }
        }

        public void SetTile(int x, int y, Tile tile)
        {
            if (!IsInBounds(x, y))
            {
                throw new ArgumentOutOfRangeException("Coordinates are out of bounds");
            }

            var oldTile = _tiles[x, y];

            if (oldTile != tile)
            {
                _tiles[x, y] = tile;
                RaiseTileChanged(
                    new TileXy{Tile = oldTile, X = x, Y = y}, 
                    new TileXy{Tile = tile, X = x, Y = y});
            }
        }

        public void MoveTile(int x, int y, int newX, int newY)
        {
            var replacedTile = GetTile(newX, newY);

            if (replacedTile != null)
            {
                SetTile(newX, newY, null);
            }

            var movingTile = GetTile(x, y);

            if (movingTile == null)
            {
                return;
            }
            
            _tiles[newX, newY] = movingTile;
            _tiles[x, y] = null;
            
            RaiseTileChanged(
                new TileXy() {Tile = movingTile, X = x, Y = y},
                new TileXy(){Tile = movingTile, X = newX, Y = newY});
        }

        public event EventHandler<TileChangedEventArgs> TileChanged;
        
        protected void RaiseTileChanged(TileXy oldTile, TileXy newTile)
        {
            TileChanged?.Invoke(this, new TileChangedEventArgs(oldTile, newTile));
        }
        
        
        /// <summary>
        /// Removes full lines from the field, shifts down the field tiles above the removed lines.
        /// </summary>
        /// <returns>The number of removed lines</returns>
        public int RemoveFullLines() // !note that the old name 'CheckLines' was bad
        {
            var left = leftBorder;
            var right = rightBorder;

            var remove = 0;

            for (var i = Ymin; i < Ymax; i++)
            {
                var isFull = true;
                for (var x = left; x <= right ; x++) // check line
                {
                    if (_tiles[x, i] == null)
                    {
                        isFull = false;
                        break;
                    }
                }

                if (isFull)
                {
                    // remove the line
                    remove++;

                    for (var x = left; x <= right; x++)
                    {
                        for (var y = i - 1; y >= 0; y--)
                        {
                            MoveTile(x, y, x, y + 1);
                        }
                    }
                }
            }

            return remove;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        /// <param name="figureX"></param>
        /// <param name="figureY"></param>
        /// <returns></returns>
        public bool IsPossibleToPlaceFigure(Figure f, int figureX, int figureY)
        {
            for (var i = f.Tiles.GetLowerBound(0); i <= f.Tiles.GetUpperBound(0); i++)
            {
                for (var j = f.Tiles.GetLowerBound(1); j <= f.Tiles.GetUpperBound(1); j++)
                {
                    if (f.Tiles[i, j] != null)
                    {
                        var figureTileX = figureX + i;
                        var figureTileY = figureY + j;

                        if (!IsEmpty(figureTileX, figureTileY))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public void LockFigure(Figure f, int x, int y, bool removeTilesFromFigure)
        {
            for (var i = f.Tiles.GetLowerBound(0); i <= f.Tiles.GetUpperBound(0); i++)
            {
                for (var j = f.Tiles.GetLowerBound(1); j <= f.Tiles.GetUpperBound(1); j++)
                {
                    if (f.Tiles[i, j] != null)
                    {
                        SetTile(x + i, y + j, f.Tiles[i, j]);

                        if (removeTilesFromFigure)
                        {
                            f.Tiles[i, j] = null;
                        }
                    }
                }
            }
        }
        
        private Tile[,] _tiles;
    }

    public class FieldMoveTileResult
    {
        public int xLength;
        public int yLength;

        public FieldMoveTileResult(int x, int y)
        {
            this.xLength = x;
            this.yLength = y;
        }
    }
}