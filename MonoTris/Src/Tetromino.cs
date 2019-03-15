using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Newtonsoft.Json;
using System.IO;

namespace MonoTris
{
    public class Tetromino
    {
        public readonly TetrominoShape Shape;
        public Vector2 Position;
    }

    public class TetrominoShapeDatabase
    {

        public static List<TetrominoShape> Shapes { get { return _shapes; } }

        private static List<TetrominoShape> _shapes;

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
            _shapes = new List<TetrominoShape>();

            var shapeData = JsonConvert.DeserializeObject<List<TetrominoShapeSchema>>(
                    File.ReadAllText("Content/TetrominoShapes.json"));

            foreach (var shape in shapeData)
            {
                _shapes.Add(new TetrominoShape(
                    shape.width,
                    shape.height,
                    _colorLookup[shape.color],
                    shape.data
                ));
            }
        }
    }

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
            PivotY = (height -1) / 2;

            Shape = new bool[height, width];
            for (var i = 0; i < data.Count; i++)
            {
                var x = i % width;
                var y = (int)(i / width);
                Shape[y, x] = data[i] == 1;
            }
        }
    }

    internal struct TetrominoShapeSchema
    {
        public int width;
        public int height;
        public string color;
        public List<int> data;
    }
}