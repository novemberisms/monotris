using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MonoTris
{
    public class Fonts
    {
        public static SpriteFont Title { get; private set; }
        public static SpriteFont Regular { get; private set; }

        public static void LoadContent(ContentManager content)
        {
            Title = content.Load<SpriteFont>("TitleFont");
            Regular = content.Load<SpriteFont>("Regular");
        }
    }
}
