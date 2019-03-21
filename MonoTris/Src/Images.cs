using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MonoTris
{
    /// <summary> Holds references to all the Images loaded in by Content </summary>
    abstract class Images
    {
        public static Texture2D Blocks { get; private set; }

        public static void LoadContent(ContentManager content)
        {
            System.Diagnostics.Debug.WriteLine("Loading content...");
            Blocks = content.Load<Texture2D>("blocks");
        }
    }
}