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
                }, 1, 1
            ),
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
                }, 1, 1
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
                }, 1, 1
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
                    {null, null, null, null, null, T(), null, T(),},
                    {null, null, null, null, null, null, null, T(),},
                    {T(), T(), T(), T(), T(), T(), T(), T(),},
                }, 1, 1
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
        [InlineData(12, 24)]
        [InlineData(15, 27)]
        public void CreateField(int width, int height)
        {
            var field = Field.CreateField(width, height, "DimGray");
            field.Width.Should().Be(width + 1);
            field.Height.Should().Be(height);
        }

        [Theory]
        [InlineData(0, 1, 3)]
        [InlineData(0, 2, 4)]
        [InlineData(0, 3, 5)]
        public void IsInBounds(int fieldIndex, int x, int y)
        {
            var field = Fields[fieldIndex];
            field.IsInBounds(x, y).Should().Be(true);
        }
        
        [Theory]
        [InlineData(0, 0, 15)]
        [InlineData(0, -1, 3)]
        public void IsNotInBounds(int fieldIndex, int x, int y)
        {
            var field = Fields[fieldIndex];
            field.IsInBounds(x, y).Should().Be(false);
        }
        
        [Theory]
        [InlineData(3, 2, 3)]
        [InlineData(3, 7, 5)]
        public void IsNotEmpty(int fieldIndex, int x, int y)
        {
            var field = Fields[fieldIndex];
            field.IsEmpty(x, y).Should().Be(false);
        }
        
        [Theory]
        [InlineData(3, 3, 3)]
        [InlineData(3, 7, 4)]
        public void IsEmpty(int fieldIndex, int x, int y)
        {
            var field = Fields[fieldIndex];
            field.IsEmpty(x, y).Should().Be(true);
        }
    }
}