using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoTris
{
    public abstract class GameState
    {
        protected MonoTris GameInstance;

        public void SetGameInstance(MonoTris gameInstance)
        {
            GameInstance = gameInstance;
        }
        public virtual void Initialize()
        {
            OnInitialize();
        }
        public virtual void Cleanup()
        {
            OnCleanup();
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);

        // assigning empty delegates to events will allow you to call them even if nobody else
        // is subsribed

        public event Action OnInitialize = delegate() {};
        public event Action OnCleanup = delegate() {};
    }
}
