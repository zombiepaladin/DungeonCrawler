using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DungeonCrawler
{
    /// <summary>
    /// Manages user input.
    /// </summary>
    public class UserInput
    {
        #region Static Members

        //Stores all the player's inputs.
        private static Dictionary<PlayerIndex, UserInput> _inputs = new Dictionary<PlayerIndex, UserInput>(4);

        /// <summary>
        /// Load inputs for up to four players.
        /// </summary>
        public static void Load(int players)
        {
            _inputs.Add(PlayerIndex.One, new UserInput(PlayerIndex.One));
            _inputs.Add(PlayerIndex.Two, new UserInput(PlayerIndex.Two));
            _inputs.Add(PlayerIndex.Three, new UserInput(PlayerIndex.Three));
            _inputs.Add(PlayerIndex.Four, new UserInput(PlayerIndex.Four));
        }

        /// <summary>
        /// Gets the input instance for the given player.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static UserInput GetInput(PlayerIndex index)
        {
            return _inputs[index];
        }

        /// <summary>
        /// Disables all instances of input.
        /// </summary>
        public static void DisableAll()
        {
            _inputs[PlayerIndex.One]._disabled = true;
            _inputs[PlayerIndex.Two]._disabled = true;
            _inputs[PlayerIndex.Three]._disabled = true;
            _inputs[PlayerIndex.Four]._disabled = true;
        }

        /// <summary>
        /// Disables the given player's input.
        /// </summary>
        /// <param name="index"></param>
        public static void Disable(PlayerIndex index)
        {
            _inputs[index]._disabled = true;
        }

        /// <summary>
        /// Enables all instances of input.
        /// </summary>
        public static void EnableAll()
        {
            _inputs[PlayerIndex.One]._disabled = false;
            _inputs[PlayerIndex.Two]._disabled = false;
            _inputs[PlayerIndex.Three]._disabled = false;
            _inputs[PlayerIndex.Four]._disabled = false;
        }

        /// <summary>
        /// Enables the given players input.
        /// </summary>
        /// <param name="index"></param>
        public static void Enable(PlayerIndex index)
        {
            _inputs[index]._disabled = false;
        }

        #endregion

        //Conditional variables
#if WINDOWS 
        KeyboardState _curKeyboardState;
        KeyboardState _oldKeyboardState;
        bool _gamePadActive;
#endif
#if XBOX || WINDOWS
        GamePadState _curGamePadState;
        GamePadState _oldGamePadState;
#endif

        //Local variables.
        private PlayerIndex _pIndex;
        private bool _disabled;

        //Creates a new input.
        private UserInput(PlayerIndex index)
        {
            _pIndex = index;
            _disabled = false;
#if WINDOWS
            initKeyboard();
#elif XBOX
            initGamePad();
#endif
        }

        /// <summary>
        /// Saves the current state of the player's input. Call this before checking to get the most recent results.
        /// </summary>
        public void GetState()
        {
            if(_disabled)
                return;
#if WINDOWS
            if(_gamePadActive)
                getGamePadState();
            getKeyboardState();
#elif XBOX
            getGamePadState();
#endif
        }

        /// <summary>
        /// Returns the direction the player is inputing.
        /// </summary>
        /// <returns></returns>
        public Vector2 GetDirection()
        {
            if(_disabled)
                return new Vector2(0);

#if WINDOWS
            if(_gamePadActive)
                return getGamePadDirection();
            return getKeyboardDirection();
#elif XBOX
            return getGamePadDirection();
#endif
        }

        /// <summary>
        /// Returns true if the given key or button is pressed.
        /// </summary>
        /// <param name="key">Keyboard key to check.</param>
        /// <param name="button">GamePad button to check.</param>
        /// <returns></returns>
        public bool IsPressed(Keys key, Buttons button)
        {
            if(_disabled)
                return false;

#if WINDOWS
            if(_gamePadActive)
                return isButtonPressed(button);
            return isKeyPressed(key);
#elif XBOX
            return isButtonPressed((Buttons)button);
#endif
        }

        /// <summary>
        /// Returns true if the given key or button is held.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool IsHeld(Keys key, Buttons button)
        {
            if(_disabled)
                return false;

#if WINDOWS
            if(_gamePadActive)
                return isButtonHeld(button);
            return isKeyHeld(key);
#elif XBOX
            return isButtonHeld((Buttons)button);
#endif

        }

        //Keyboard handlers
        #region KEYBOARD
#if WINDOWS
        private void initKeyboard()
        {
            _curKeyboardState = Keyboard.GetState(_pIndex);
            _oldKeyboardState = _curKeyboardState;
            _gamePadActive = GamePad.GetState(_pIndex).IsConnected;
            if(_gamePadActive)
                initGamePad();
        }

        private void getKeyboardState()
        {
            _oldKeyboardState = _curKeyboardState;
            _curKeyboardState = Keyboard.GetState(_pIndex);
        }

        private Vector2 getKeyboardDirection()
        {
            Vector2 direction = new Vector2(0);

            if (_curKeyboardState.IsKeyDown(Keys.W))
            {
                direction.Y += -1;
            }
            if (_curKeyboardState.IsKeyDown(Keys.S))
            {
                direction.Y += 1;
            }
            if (_curKeyboardState.IsKeyDown(Keys.A))
            {
                direction.X += -1;
            }
            if (_curKeyboardState.IsKeyDown(Keys.D))
            {
                direction.X += 1;
            }

            return direction;
        }

        private bool isKeyPressed(Keys key)
        {
            return _curKeyboardState.IsKeyDown(key);
        }

        private bool isKeyHeld(Keys key)
        {
            return _curKeyboardState.IsKeyDown(key) && _oldKeyboardState.IsKeyDown(key);
        }
#endif
        #endregion

        //Gamepad handlers
        #region GAMEPAD
#if XBOX || WINDOWS
        private void initGamePad()
        {
            _curGamePadState = GamePad.GetState(_pIndex);
            _oldGamePadState = _curGamePadState;
        }

        private void getGamePadState()
        {
            _oldGamePadState = _curGamePadState;
            _curGamePadState = GamePad.GetState(_pIndex);
#if WINDOWS
            _gamePadActive = _curGamePadState.IsConnected;
#endif
        }

        private Vector2 getGamePadDirection()
        {
            return _curGamePadState.ThumbSticks.Left;
        }

        private bool isButtonPressed(Buttons button)
        {
            return _curGamePadState.IsButtonDown(button);
        }

        private bool isButtonHeld(Buttons button)
        {
            return _curGamePadState.IsButtonDown(button) && _oldGamePadState.IsButtonDown(button);
        }
#endif
        #endregion
    }
}
