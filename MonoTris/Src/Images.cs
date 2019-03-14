using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MonoTris
{
    /// <summary> Holds references to all the Images loaded in by Content </summary>
    class Images
    {
        private static Texture2D _blocks;
        public static Texture2D Blocks
        {
            get { return _blocks; }
        }

        public static void LoadContent(ContentManager content)
        {
            System.Diagnostics.Debug.WriteLine("Loading content...");
            _blocks = content.Load<Texture2D>("blocks");
        }
    }
}