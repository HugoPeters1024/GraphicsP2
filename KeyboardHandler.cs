using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Input;

namespace template
{
    public static class KeyboardHandler
    {
        static KeyboardState curKeyboardState, prevKeyboardState;

        public static void Init()
        {
            curKeyboardState = new KeyboardState();
            prevKeyboardState = new KeyboardState();
        }

        public static void Update()
        {
            prevKeyboardState = curKeyboardState;
            curKeyboardState = Keyboard.GetState();         
        }

        public static bool KeyPressed(Key k)
        {
            return curKeyboardState.IsKeyDown(k) && !prevKeyboardState.IsKeyDown(k);
        }

        public static bool KeyDown(Key k)
        {
            return curKeyboardState.IsKeyDown(k);
        }

        public static bool IsAnyKeyDown()
        {
            return curKeyboardState.IsKeyDown(Key.Space);
        }
    }
}
