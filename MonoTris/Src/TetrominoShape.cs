using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using Microsoft.Xna.Framework;

namespace MonoTris
{
    public class TetrominoShape
    {
        public readonly int Width;
        public readonly int Height;
        public readonly bool[,] Shape;
        public readonly BlockColor Color;
        public readonly int PivotX;
        public readonly int PivotY;

        public TetrominoShape(int width, int height, BlockColor color, List<int> data)
        {
            Width = width;
            Height = height;
            Color = color;
            PivotX = (width - 1) / 2;
            PivotY = (height - 1) / 2;

            Shape = new bool[height, width];
            for (var i = 0; i < data.Count; i++)
            {
                var x = i % width;
                var y = i / width;
                Shape[y, x] = data[i] == 1;
            }
        }

        public bool[,] GetShapeRotated(ShapeRotation rotation)
        {
            var rotatedWidth = Height;
            var rotatedHeight = Width;
            bool[,] result;

            switch (rotation)
            {
                case ShapeRotation.Zero:
                    return (bool[,])Shape.Clone();
                case ShapeRotation.OneEighty:
                    result = new bool[Height, Width];
                    for (var y = 0; y < Height; y++)
                    {
                        for (var x = 0; x < Width; x++)
                        {
                            var resultX = Width - x - 1;
                            var resultY = Height - y - 1;
                            result[resultY, resultX] = Shape[y, x];
                        }
                    }
                    return result;
                case ShapeRotation.TwoSeventy:
                    result = new bool[rotatedHeight, rotatedWidth];
                    for (var y = 0; y < Height; y++)
                    {
                        for (var x = 0; x < Width; x++)
                        {
                            var resultX = y;
                            var resultY = Width - x - 1;
                            result[resultY, resultX] = Shape[y, x];
                        }
                    }
                    return result;
                case ShapeRotation.Ninety:
                    result = new bool[rotatedHeight, rotatedWidth];
                    for (var y = 0; y < Height; y++)
                    {
                        for (var x = 0; x < Width; x++)
                        {
                            var resultX = Height - y - 1;
                            var resultY = x;
                            result[resultY, resultX] = Shape[y, x];
                        }
                    }
                    return result;
            }
            // unreachable
            return null;
        }

        public Vector2 GetPivotRotated(ShapeRotation rotation)
        {
            switch (rotation)
            {
                case ShapeRotation.Zero:
                case ShapeRotation.OneEighty:
                    return new Vector2(PivotX, PivotY);
                case ShapeRotation.Ninety:
                case ShapeRotation.TwoSeventy:
                    var rotatedPivotX = (Height - 1) / 2;
                    var rotatedPivotY = (Width - 1) / 2;
                    return new Vector2(rotatedPivotX, rotatedPivotY);
            }
            // unreachable
            return new Vector2();
        }
    }

    public class TetrominoShapeDatabase
    {
        public static List<TetrominoShape> Shapes { get; private set; }

        private static Dictionary<string, BlockColor> _colorLookup = new Dictionary<string, BlockColor>
        {
            { "red", BlockColor.Red },
            { "orange", BlockColor.Orange },
            { "yellow", BlockColor.Yellow },
            { "green", BlockColor.Green },
            { "blue", BlockColor.Blue },
            { "indigo", BlockColor.Indigo },
            { "violet", BlockColor.Violet },
            { "gray", BlockColor.Gray },
        };

        public static void LoadData()
        {
            Shapes = new List<TetrominoShape>();

            var shapeData = JsonConvert.DeserializeObject<List<TetrominoShapeSchema>>(
                    File.ReadAllText("Content/TetrominoShapes.json"));

            foreach (var shape in shapeData)
            {
                Shapes.Add(new TetrominoShape(
                    shape.width,
                    shape.height,
                    _colorLookup[shape.color],
                    shape.data
                ));
            }
        }
    }

    internal class TetrominoShapeSchema
    {
        public int width;
        public int height;
        public string color;
        public List<int> data;
    }

    public enum ShapeRotation
    {
        Zero,
        Ninety,
        OneEighty,
        TwoSeventy,
    }

    public enum RotationDirection
    {
        Clockwise,
        CounterClockwise,
    }
}
