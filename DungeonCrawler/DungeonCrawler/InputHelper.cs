﻿#region File Description
//-----------------------------------------------------------------------------
// UserInput.cs 
//
// Author: Devin Kelly-Collins
//
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence 
//-----------------------------------------------------------------------------
#endregion

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
    public class InputHelper
    {
        #region Static Members

        //Stores all the player's inputs.
        private static Dictionary<PlayerIndex, InputHelper> _inputs = new Dictionary<PlayerIndex, InputHelper>(4);

        /// <summary>
        /// Load inputs for up to four players.
        /// </summary>
        public static void Load()
        {
            _inputs.Add(PlayerIndex.One, new InputHelper(PlayerIndex.One));
            _inputs.Add(PlayerIndex.Two, new InputHelper(PlayerIndex.Two));
            _inputs.Add(PlayerIndex.Three, new InputHelper(PlayerIndex.Three));
            _inputs.Add(PlayerIndex.Four, new InputHelper(PlayerIndex.Four));
        }

        /// <summary>
        /// Gets the input instance for the given player.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static InputHelper GetInput(PlayerIndex index)
        {
            InputHelper input = _inputs[index];
            input.updateState();
            return input;
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
#endif
#if XBOX || WINDOWS
        GamePadState _curGamePadState;
        GamePadState _oldGamePadState;
#endif

        //Local variables.
        private PlayerIndex _pIndex;
        private bool _disabled;

        //Creates a new input.
        private InputHelper(PlayerIndex index)
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
        private void updateState()
        {
            if(_disabled)
                return;
#if WINDOWS
            getGamePadState();
            getKeyboardState();
#elif XBOX
            getGamePadState();
#endif
        }

        /// <summary>
        /// Returns the direction the player is inputing. (With the left stick on the gamepad and the WASD keys on the keyboard)
        /// </summary>
        /// <returns></returns>
        public Vector2 GetLeftDirection()
        {
            if(_disabled)
                return new Vector2(0);

#if WINDOWS
            if (_curGamePadState.IsConnected)
                return getGamePadLeftStick();
            return getKeyboardWASD();
#elif XBOX
            return getGamePadLeftStick();
#endif
        }

        public Vector2 GetRightDirection()
        {
            if (_disabled)
                return new Vector2(0);

#if WINDOWS
            if(_curGamePadState.IsConnected)
                return getGamePadRightStick();
            return getKeyboardArrows();
#elif XBOX
            return getGamePadRightStick();
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
            if (_curGamePadState.IsConnected)
                return isButtonPressed(button);
            return isKeyPressed(key); //Only allow keyboard input for the first player.
#elif XBOX
            return isButtonPressed((Buttons)button);
#endif
        }

        /// <summary>
        /// Overloaded function that returns true if the given key is pressed.
        /// This allows for keyboard only commands
        /// </summary>
        /// <param name="key">Keyboard key to check.</param>
        /// <returns></returns>
        public bool IsPressed(Keys key)
        {
            if (_disabled)
                return false;
            
            return isKeyPressed(key);
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
            if (_curGamePadState.IsConnected)
                return isButtonHeld(button);
            return isKeyHeld(key);
#elif XBOX
            return isButtonHeld((Buttons)button);
#endif

        }

        public bool IsGamePadConnected()
        {
            return _curGamePadState.IsConnected;
        }

        //Keyboard handlers
        #region KEYBOARD
#if WINDOWS
        private void initKeyboard()
        {
            _curKeyboardState = Keyboard.GetState(_pIndex);
            initGamePad();
        }

        public void SetOldKeyboardState()
        {
            _oldKeyboardState = _curKeyboardState;
        }

        private void getKeyboardState()
        {
            
            _curKeyboardState = Keyboard.GetState(_pIndex);
        }

        private Vector2 getKeyboardWASD()
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

        private Vector2 getKeyboardArrows()
        {
            Vector2 direction = new Vector2(0);

            if (_curKeyboardState.IsKeyDown(Keys.Down))
            {
                direction.Y += -1;
            }
            if (_curKeyboardState.IsKeyDown(Keys.Up))
            {
                direction.Y += 1;
            }
            if (_curKeyboardState.IsKeyDown(Keys.Left))
            {
                direction.X += -1;
            }
            if (_curKeyboardState.IsKeyDown(Keys.Right))
            {
                direction.X += 1;
            }

            return direction;
        }

        private bool isKeyPressed(Keys key)
        {
            return _curKeyboardState.IsKeyDown(key);// && _oldKeyboardState.IsKeyUp(key);
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
        }

        private Vector2 getGamePadLeftStick()
        {
            Vector2 direction = _curGamePadState.ThumbSticks.Left;
            direction.Y *= -1; //Invert the Y axis
            return direction;
        }

        private Vector2 getGamePadRightStick()
        {
            Vector2 direction = _curGamePadState.ThumbSticks.Right;
            direction.Y *= -1;
            return direction;
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
