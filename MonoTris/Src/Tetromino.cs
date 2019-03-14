using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MonoTris
{
    public class Tetromino
    {

    } 

    public class TetrominoShapeDatabase
    {

        public static List<TetrominoShape> Shapes;

        public static void LoadData(ContentManager content)
        {
            Shapes = new List<TetrominoShape>();
            Shapes.Add(content.Load<TetrominoShape>("shape0"));
        }
    }

    public class TetrominoShape
    {
        public int Width;
        public int Height;
        // public List<bool> Shape;

        public TetrominoShape()
        {

        }
    }
}