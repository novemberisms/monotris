using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoTris
{
    public class StageDrawer
    {
        public Vector2 Position = new Vector2();
        public float Scale = 1.0F;

        public void DrawStage(SpriteBatch spriteBatch, Stage stage)
        {
            stage.IterCells(delegate (StageCell cell)
            {
                if (cell.Value == BlockColor.None) return;
                var drawPosition = Position + cell.Coordinates * (16 * Scale);
                spriteBatch.Draw(
                    Images.Blocks, 
                    drawPosition, 
                    _quads[cell.Value], 
                    Color.White, 
                    0, 
                    Vector2.Zero, 
                    Scale, 
                    SpriteEffects.None, 
                    0
                );
            });
        }

        private static readonly Dictionary<BlockColor, Rectangle> _quads = new Dictionary<BlockColor, Rectangle>
        {
            { BlockColor.Red, new Rectangle(16 * 0, 0, 16, 16) },
            { BlockColor.Orange, new Rectangle(16 * 1, 0, 16, 16) },
            { BlockColor.Yellow, new Rectangle(16 * 2, 0, 16, 16) },
            { BlockColor.Green, new Rectangle(16 * 3, 0, 16, 16) },
            { BlockColor.Blue, new Rectangle(16 * 4, 0, 16, 16) },
            { BlockColor.Indigo, new Rectangle(16 * 5, 0, 16, 16) },
            { BlockColor.Violet, new Rectangle(16 * 6, 0, 16, 16) },
            { BlockColor.Gray, new Rectangle(16 * 7, 0, 16, 16) },
        };
    }
}
