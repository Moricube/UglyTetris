using System.Collections.Generic;
using System.Windows.Documents;
using FluentAssertions;
using UglyTetris.GameLogic;
using Xunit;

namespace Tests
{
    public class FieldTest
    {
        private static Tile T() => new Tile("Brown");

        private static List<Field> Fields = new List<Field>()
        {
            new Field(new Tile[,]
            {
                // 0 -> Y
                // | 
                // v X 
                {T(), T(), T(), T(), T(), T(), T(), T(),},
                {null, null, null, null, null, null, T(), T(),},
                {null, null, null, null, null, null, T(), T(),},
                {null, null, null, null, null, null, T(), T(),},
                {null, null, null, null, null, null, T(), T(),},
                {null, null, null, null, null, null, T(), T(),},
                {null, null, null, null, null, null, T(), T(),},
                {null, null, null, null, null, null, T(), T(),},
                {null, null, null, null, null, null, T(), T(),},
                {T(), T(), T(), T(), T(), T(), T(), T(),},
            }),
            new Field(new Tile[,]
                {
                    // 0 -> Y
                    // | 
                    // v X 
                    {T(), T(), T(), T(), T(), T(), T(), T(),},
                    {null, null, null, null, T(), T(), T(), T(),},
                    {null, null, null, null, T(), T(), T(), T(),},
                    {null, null, null, null, T(), T(), T(), T(),},
                    {null, null, null, null, T(), T(), T(), T(),},
                    {null, null, null, null, T(), T(), T(), T(),},
                    {null, null, null, null, T(), T(), T(), T(),},
                    {null, null, null, null, T(), T(), T(), T(),},
                    {null, null, null, null, null, T(), T(), T(),},
                    {T(), T(), T(), T(), T(), T(), T(), T(),},
                }
            ),
            new Field(new Tile[,]
                {
                    // 0 -> Y
                    // | 
                    // v X 
                    {T(), T(), T(), T(), T(), T(), T(), T(),},
                    {null, null, null, null, T(), T(), T(), T(),},
                    {null, null, null, null, T(), T(), T(), T(),},
                    {null, null, null, null, T(), null, T(), T(),},
                    {null, null, null, null, T(), T(), T(), T(),},
                    {null, null, null, null, T(), T(), null, T(),},
                    {null, null, null, null, T(), T(), T(), T(),},
                    {null, null, null, null, T(), T(), T(), T(),},
                    {null, null, null, null, null, T(), T(), T(),},
                    {T(), T(), T(), T(), T(), T(), T(), T(),},
                }
            ),
            new Field(new Tile[,]
                {
                    // 0 -> Y
                    // | 
                    // v X 
                    {T(), T(), T(), T(), T(), T(), T(), T(),},
                    {null, null, null, null, null, null, null, T(),},
                    {null, null, null, T(), null, null, null, T(),},
                    {null, null, null, null, null, null, null, T(),},
                    {null, null, null, null, null, null, null, T(),},
                    {null, null, null, null, null, null, null, T(),},
                    {null, null, null, null, null, null, null, T(),},
                    {null, null, null, null, null, null, null, T(),},
                    {null, null, null, null, null, null, null, T(),},
                    {T(), T(), T(), T(), T(), T(), T(), T(),},
                }
            ),
        };


        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 2)]
        [InlineData(2, 0)]
        public void RemoveLines(int fieldIndex, int lineCount)
        {
            var field = Fields[fieldIndex];
            field.RemoveFullLines().Should().Be(lineCount);
        }
        
        [Theory]
        [InlineData(2, 3, 2, 2)]
        [InlineData(2, 3, 2, 4)]
        public void CheckTileMove0(int x, int y, int newX, int newY)
        {
            var field = new Field(new Tile[,]
                {
                    // 0 -> Y
                    // | 
                    // v X 
                    {T(), T(), T(), T(), T(), T(), T(), T(),},
                    {null, null, null, null, null, null, null, T(),},
                    {null, null, null, T(), null, null, null, T(),},
                    {null, null, null, null, null, null, null, T(),},
                    {null, null, null, null, null, null, null, T(),},
                    {null, null, null, null, null, null, null, T(),},
                    {null, null, null, null, null, null, null, T(),},
                    {null, null, null, null, null, null, null, T(),},
                    {null, null, null, null, null, null, null, T(),},
                    {T(), T(), T(), T(), T(), T(), T(), T(),},
                }
            );
            
            field.MoveTile(x, y, newX, newY).Should().Be((0, 1));
        }
        
        [Theory]
        [InlineData(2, 3, 3, 2)]
        [InlineData(2, 3, 1, 4)]
        public void CheckTileMove45(int x, int y, int newX, int newY)
        {
            var field = new Field(new Tile[,]
                {
                    // 0 -> Y
                    // | 
                    // v X 
                    {T(), T(), T(), T(), T(), T(), T(), T(),},
                    {null, null, null, null, null, null, null, T(),},
                    {null, null, null, T(), null, null, null, T(),},
                    {null, null, null, null, null, null, null, T(),},
                    {null, null, null, null, null, null, null, T(),},
                    {null, null, null, null, null, null, null, T(),},
                    {null, null, null, null, null, null, null, T(),},
                    {null, null, null, null, null, null, null, T(),},
                    {null, null, null, null, null, null, null, T(),},
                    {T(), T(), T(), T(), T(), T(), T(), T(),},
                }
            );
            
            field.MoveTile(x, y, newX, newY).Should().Be((1, 1));
        }
        
        [Theory] 
        [InlineData(2, 3, 3, 3)]
        [InlineData(2, 3, 1, 3)]
        public void CheckTileMove90(int x, int y, int newX, int newY)
        {
            var field = new Field(new Tile[,]
                {
                    // 0 -> Y
                    // | 
                    // v X 
                    {T(), T(), T(), T(), T(), T(), T(), T(),},
                    {null, null, null, null, null, null, null, T(),},
                    {null, null, null, T(), null, null, null, T(),},
                    {null, null, null, null, null, null, null, T(),},
                    {null, null, null, null, null, null, null, T(),},
                    {null, null, null, null, null, null, null, T(),},
                    {null, null, null, null, null, null, null, T(),},
                    {null, null, null, null, null, null, null, T(),},
                    {null, null, null, null, null, null, null, T(),},
                    {T(), T(), T(), T(), T(), T(), T(), T(),},
                }
            );
            
            field.MoveTile(x, y, newX, newY).Should().Be((1, 0));
        }

        [Theory]
        [InlineData(2, 3, 1, 2)]
        [InlineData(2, 3, 3, 4)]
        public void CheckTileMove135(int x, int y, int newX, int newY)
        {
            var field = new Field(new Tile[,]
                {
                    // 0 -> Y
                    // | 
                    // v X 
                    {T(), T(), T(), T(), T(), T(), T(), T(),},
                    {null, null, null, null, null, null, null, T(),},
                    {null, null, null, T(), null, null, null, T(),},
                    {null, null, null, null, null, null, null, T(),},
                    {null, null, null, null, null, null, null, T(),},
                    {null, null, null, null, null, null, null, T(),},
                    {null, null, null, null, null, null, null, T(),},
                    {null, null, null, null, null, null, null, T(),},
                    {null, null, null, null, null, null, null, T(),},
                    {T(), T(), T(), T(), T(), T(), T(), T(),},
                }
            );
            
            field.MoveTile(x, y, newX, newY).Should().Be((1, 1));
        }
    }
}