using System;
using Microsoft.Xna.Framework;

namespace MonoTris
{
    public class Stage
    {
        private StageCell[,] _cells;
        private readonly Vector2 _dimensions;

        public int Width => (int)_dimensions.X;
        public int Height => (int)_dimensions.Y;

        public Stage(StageConfiguration config)
        {
            _dimensions = new Vector2(config.Width, config.Height);
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            _cells = new StageCell[Height, Width];
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    _cells[y, x] = new StageCell(x, y);
                }
            }
        }

        /// <summary>
        /// Performs `fn` for each cell in the stage, passing in a `StageCell`
        /// </summary>
        public void IterCells(Action<StageCell> fn)
        {
            foreach (var cell in _cells) fn(cell);
        }

        public StageCell GetCell(int x, int y)
        {
            return _cells[y, x];
        }

        public void SetCellColor(int x, int y, BlockColor color)
        {
            _cells[y, x].Value = color;
        }

        public int CountCompletedLines()
        {
            var linesCompleted = 0;
            for (var y = 0; y < Height; y++)
            {
                if (IsLineComplete(y)) linesCompleted++;
            }
            return linesCompleted;
        }

        public void ClearCompletedLines()
        {
            for (var y = 0; y < Height; y++)
            {
                if (IsLineComplete(y))
                {
                    // for all lines above, copy them down
                    for (var yy = y; yy >= 0; yy--)
                    {
                        CopyLineAbove(yy);
                    }
                }
            }
        }

        public void ClearAll()
        {
            IterCells(cell => cell.Value = BlockColor.None);
        }

        private bool IsLineComplete(int y)
        {
            for (var x = 0; x < Width; x++)
            {
                if (_cells[y, x].Value == BlockColor.None)
                {
                    return false;
                }
            }
            return true;
        }

        private void CopyLineAbove(int y)
        {
            for (var x = 0; x < Width; x++)
            {
                var blockAboveColor = y == 0 ? BlockColor.None : _cells[y - 1, x].Value;
                SetCellColor(x, y, blockAboveColor);
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
        public BlockColor Value = BlockColor.None;
        public readonly Vector2 Coordinates;

        public StageCell(int x, int y)
        {
            Coordinates = new Vector2(x, y);
        }
    }
}
