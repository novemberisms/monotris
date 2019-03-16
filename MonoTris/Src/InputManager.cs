using Microsoft.Xna.Framework.Input;

namespace MonoTris
{
    public abstract class InputManager
    {
        private static KeyboardState _prevState;

        /// <summary> Should be called after all polling is done. </summary>
        public static void PostUpdate()
        {
            _prevState = Keyboard.GetState();
        }

        public static bool IsKeyPressed(Keys key)
        {
            var currentState = Keyboard.GetState();
            if (_prevState == null) return currentState.IsKeyDown(key);
            return _prevState.IsKeyUp(key) && currentState.IsKeyDown(key);
        }

        public static bool IsKeyDown(Keys key)
        {
            return Keyboard.GetState().IsKeyDown(key);
        }
    }
}
