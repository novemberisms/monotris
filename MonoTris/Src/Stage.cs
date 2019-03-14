using System;
using Microsoft.Xna.Framework;

namespace MonoTris
{
    public class Stage
    {
        private StageCell[,] _cells;
        private readonly Vector2 _dimensions;

        public int Width
        {
            get { return (int)_dimensions.X; }
        }

        public int Height
        {
            get { return (int)_dimensions.Y; }
        }

        public Stage(StageConfiguration config)
        {
            _dimensions = new Vector2((float)config.Width, (float)config.Height);
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            _cells = new StageCell[Height, Width];
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    _cells[y, x] = new StageCell(x, y, BlockColor.Blue);
                }
            }
        }

        /// <summary>
        /// Performs `fn` for each cell in the stage, passing in the `x`, `y`, and `values` of the cell.
        /// </summary>
        public void IterCells(Action<StageCell> fn)
        {
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    fn(_cells[y, x]);
                }
            }
        }
    }

    public struct StageConfiguration
    {
        public int Width;
        public int Height;
    }

    public class StageCell
    {
        public BlockColor Value;
        readonly public Vector2 Coordinates;
        internal StageCell(int x, int y, BlockColor value)
        {
            Coordinates = new Vector2(x, y);
            Value = value;
        }
    }
}
