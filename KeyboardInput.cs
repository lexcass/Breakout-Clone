using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Breakout
{
    public static class KeyboardInput
    {
        private static KeyboardState _state;
        private static KeyboardState _prevState;
        private static List<Keys> _pressedKeys;

        public static void Initialize()
        {
            _pressedKeys = new List<Keys>();
            _prevState = Keyboard.GetState();
        }

        public static bool WasKeyPressed(Keys key)
        {
            Console.WriteLine(_state.IsKeyDown(key));
            Console.WriteLine(_prevState.IsKeyUp(key));
            return _state.IsKeyDown(key) && _prevState.IsKeyUp(key);
        }

        public static void Update()
        {
            _state = Keyboard.GetState();
            _prevState = _state;
        }
    }
}
