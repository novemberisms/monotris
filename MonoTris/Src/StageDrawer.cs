using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoTris
{
    public class StageDrawer
    {
        public const int TileSize = 16;
        public Vector2 Position;
        public float Scale;
        public Color ColorMask = Color.White;

        public StageDrawer(StageDrawerConfiguration config)
        {
            Position = config.Position;
            Scale = config.Scale;
        }

        public void DrawStage(SpriteBatch spriteBatch, Stage stage)
        {
            stage.IterCells(delegate (StageCell cell)
            {
                if (cell.Value == BlockColor.None) return;
                var drawPosition = Position + cell.Coordinates * (TileSize * Scale);
                spriteBatch.Draw(
                    Images.Blocks,
                    drawPosition,
                    _quads[cell.Value],
                    ColorMask,
                    0,
                    Vector2.Zero,
                    Scale,
                    SpriteEffects.None,
                    0
                );
            });
        }

        public Vector2 GetDrawDimensions(Stage stage)
        {
            return new Vector2(stage.Width, stage.Height) * TileSize * Scale;
        }

        private static readonly Dictionary<BlockColor, Rectangle> _quads = new Dictionary<BlockColor, Rectangle>
        {
            { BlockColor.Red, new Rectangle(TileSize * 0, 0, TileSize, TileSize) },
            { BlockColor.Orange, new Rectangle(TileSize * 1, 0, TileSize, TileSize) },
            { BlockColor.Yellow, new Rectangle(TileSize * 2, 0, TileSize, TileSize) },
            { BlockColor.Green, new Rectangle(TileSize * 3, 0, TileSize, TileSize) },
            { BlockColor.Blue, new Rectangle(TileSize * 4, 0, TileSize, TileSize) },
            { BlockColor.Indigo, new Rectangle(TileSize * 5, 0, TileSize, TileSize) },
            { BlockColor.Violet, new Rectangle(TileSize * 6, 0, TileSize, TileSize) },
            { BlockColor.Gray, new Rectangle(TileSize * 7, 0, TileSize, TileSize) },
        };
    }

    public struct StageDrawerConfiguration
    {
        public Vector2 Position;
        public float Scale;
    }
}
