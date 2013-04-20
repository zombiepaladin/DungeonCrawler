#region File Description
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

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace DungeonCrawler
{
    public static class Inputs
    {
        public const string ESCAPE = "Escape";
        public const string MENU = "Menu";
        public const string START = "Start";
        public const string SELECT = "Select";
        public const string ENTER = "Enter";
        public const string BACK = "Back";
        public const string DELETE = "Delete";
        public const string UP = "Up";
        public const string DOWN = "Down";
        public const string LEFT = "Left";
        public const string RIGHT = "Right";
        public const string DISPLAY_QUEST = "DisplayQuest";
        public const string TRIGGER_WEAPON = "TriggerWeapon";
        public const string TRIGGER_SKILL = "TriggerSkill";
        public const string CYCLE_WEAPON = "CycleWeapon";
        public const string CYCLE_SKILL = "CycleSkill";
        public const string SELECT_HOTKEY_1 = "SelectHotKey1";
        public const string SELECT_HOTKEY_2 = "SelectHotKey2";
        public const string SELECT_HOTKEY_3 = "SelectHotKey3";
        public const string SELECT_HOTKEY_4 = "SelectHotKey4";
        public const string TRIGGER_ITEM_1 = "TriggerItem1";
        public const string TRIGGER_ITEM_2 = "TriggerItem2";
        public const string TRIGGER_ITEM_3 = "TriggerItem3";
        public const string TRIGGER_ITEM_4 = "TriggerItem4";
    }
    
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
        /// <param name="numPlayers">How many players to load.</param>
        public static void Load(int numPlayers)
        {
            if(numPlayers < 1 || numPlayers > 4)
                throw new ArgumentOutOfRangeException("numPlayers must be between 1 and 4.");

            for (int i = 1; i <= numPlayers; i++)
            {
                PlayerIndex index = (PlayerIndex)(i-1);
                _inputs.Add(index, new InputHelper(index));
            }
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
            foreach (InputHelper input in _inputs.Values)
            {
                input._disabled = true;
            }
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
            foreach (InputHelper input in _inputs.Values)
            {
                input._disabled = false;
            }
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

        #region Private Members

        //Conditional variables
#if WINDOWS 
        private KeyboardState _curKeyboardState;
        private KeyboardState _oldKeyboardState;
        private Dictionary<string, Keys> _mappedKeys;
#endif
#if XBOX || WINDOWS
        private GamePadState _curGamePadState;
        private GamePadState _oldGamePadState;
        private Dictionary<string, Buttons> _mappedButtons;
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
            initGamePad();
#elif XBOX
            initGamePad();
#endif

            loadMappings();
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
        
        private void loadMappings()
        {
#if WINDOWS
            mapKey(Inputs.MENU, Keys.Escape);
            mapKey(Inputs.START, Keys.Enter);
            mapKey(Inputs.SELECT, Keys.Escape);
            mapKey(Inputs.ENTER, Keys.Space);
            mapKey(Inputs.BACK, Keys.Escape);
            mapKey(Inputs.DELETE, Keys.Delete);
            mapKey(Inputs.UP, Keys.W);
            mapKey(Inputs.RIGHT, Keys.D);
            mapKey(Inputs.DOWN, Keys.S);
            mapKey(Inputs.LEFT, Keys.A);
            mapKey(Inputs.DISPLAY_QUEST, Keys.L);
            mapKey(Inputs.TRIGGER_WEAPON, Keys.Enter);
            mapKey(Inputs.CYCLE_WEAPON, Keys.E);
            mapKey(Inputs.TRIGGER_SKILL, Keys.Space);
            mapKey(Inputs.CYCLE_SKILL, Keys.Q);
            mapKey(Inputs.SELECT_HOTKEY_1, Keys.D1);
            mapKey(Inputs.SELECT_HOTKEY_2, Keys.D2);
            mapKey(Inputs.SELECT_HOTKEY_3, Keys.D3);
            mapKey(Inputs.SELECT_HOTKEY_4, Keys.D4);
            mapKey(Inputs.TRIGGER_ITEM_1, Keys.Up);
            mapKey(Inputs.TRIGGER_ITEM_2, Keys.Right);
            mapKey(Inputs.TRIGGER_ITEM_3, Keys.Down);
            mapKey(Inputs.TRIGGER_ITEM_4, Keys.Left);
#endif
#if WINDOWS || XBOX
            mapButton(Inputs.MENU, Buttons.Start);
            mapButton(Inputs.START, Buttons.Start);
            mapButton(Inputs.SELECT, Buttons.Back);
            mapButton(Inputs.ENTER, Buttons.A);
            mapButton(Inputs.BACK, Buttons.B);
            mapButton(Inputs.DELETE, Buttons.X);
            mapButton(Inputs.UP, Buttons.LeftThumbstickUp);
            mapButton(Inputs.RIGHT, Buttons.LeftThumbstickRight);
            mapButton(Inputs.DOWN, Buttons.LeftThumbstickDown);
            mapButton(Inputs.LEFT, Buttons.LeftThumbstickLeft);
            mapButton(Inputs.DISPLAY_QUEST, Buttons.RightStick);
            mapButton(Inputs.TRIGGER_WEAPON, Buttons.RightTrigger);
            mapButton(Inputs.CYCLE_WEAPON, Buttons.RightShoulder);
            mapButton(Inputs.TRIGGER_SKILL, Buttons.LeftTrigger);
            mapButton(Inputs.CYCLE_SKILL, Buttons.LeftShoulder);
            mapButton(Inputs.SELECT_HOTKEY_1, Buttons.Y);
            mapButton(Inputs.SELECT_HOTKEY_2, Buttons.B);
            mapButton(Inputs.SELECT_HOTKEY_3, Buttons.A);
            mapButton(Inputs.SELECT_HOTKEY_4, Buttons.X);
            mapButton(Inputs.TRIGGER_ITEM_1, Buttons.DPadUp);
            mapButton(Inputs.TRIGGER_ITEM_2, Buttons.DPadRight);
            mapButton(Inputs.TRIGGER_ITEM_3, Buttons.DPadDown);
            mapButton(Inputs.TRIGGER_ITEM_4, Buttons.DPadLeft);
#endif
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Maps the given key and button to the name.
        /// </summary>
        /// <param name="name">Name of mapping</param>
        /// <param name="key">Keyboard key to map.</param>
        /// <param name="button">Button to map</param>
        public void MapInput(string name, Keys key, Buttons button)
        {
#if WINDOWS
            mapKey(name, key);
            mapButton(name, button);
#elif XBOX
            mapButton(name, button);
#endif
        }

        /// <summary>
        /// Maps the given key to the name.
        /// </summary>
        /// <param name="name">Name of mapping</param>
        /// <param name="key">Keyboard key to map.</param>
        public void MapInput(string name, Keys key)
        {
#if WINDOWS
            mapKey(name, key);
#endif
        }

        /// <summary>
        /// Maps the given button to the name.
        /// </summary>
        /// <param name="name">Name of mapping</param>
        /// <param name="button">Button to map</param>
        public void MapInput(string name, Buttons button)
        {
#if WINDOWS || XBOX
            mapButton(name, button);
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

        /// <summary>
        /// Return the dirction the player is inputing. (With the right stick on the gamepad and arrow keys on the keyboard)
        /// </summary>
        /// <returns></returns>
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
            return isKeyPressed(key); 
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
#if WINDOWS
            return isKeyPressed(key);
#else 
            return false;
#endif
        }

        /// <summary>
        /// Returns true if the given key or button is pressed.
        /// </summary>
        /// <param name="name">Name of mapped key/button.</param>
        /// <returns></returns>
        public bool IsPressed(string name)
        {
            if (_disabled)
                return false;

#if WINDOWS
            if (_curGamePadState.IsConnected)
                return isButtonPressed(name);
            return isKeyPressed(name);
#elif XBOX
            return isButtonPressed(name);
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
            if (_curGamePadState.IsConnected)
                return isButtonHeld(button);
            return isKeyHeld(key);
#elif XBOX
            return isButtonHeld((Buttons)button);
#endif

        }

        /// <summary>
        /// Returns true if the given key or button is held.
        /// </summary>
        /// <param name="name">Name of mapped key/button.</param>
        /// <returns></returns>
        public bool IsHeld(string name)
        {
            if (_disabled)
                return false;

#if WINDOWS
            if(_curGamePadState.IsConnected)
                return isButtonHeld(name);
            return isKeyHeld(name);
#elif XBOX
            return isButtonHeld(name);
#endif
        }

        /// <summary>
        /// Returns true of the gamepad is connected.
        /// </summary>
        /// <returns></returns>
        public bool IsGamePadConnected()
        {
#if WINDOWS || XBOX
            return _curGamePadState.IsConnected;
#else
            return false;
#endif
        }

        #endregion

        //Keyboard handlers
        #region KEYBOARD
#if WINDOWS
        private void initKeyboard()
        {
            _curKeyboardState = Keyboard.GetState(_pIndex);
            _mappedKeys = new Dictionary<string, Keys>();
        }

        private void getKeyboardState()
        {
            _oldKeyboardState = _curKeyboardState;
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

        private void mapKey(string name, Keys key)
        {
            if (_mappedKeys.ContainsKey(name))
                _mappedKeys[name] = key;
            else
                _mappedKeys.Add(name, key);
        }

        private bool isKeyPressed(Keys key)
        {
            return _curKeyboardState.IsKeyDown(key);// && _oldKeyboardState.IsKeyUp(key);
        }

        private bool isKeyPressed(string key)
        {
            if(_mappedKeys.ContainsKey(key))
                return isKeyPressed(_mappedKeys[key]);
            return false;
        }

        private bool isKeyHeld(Keys key)
        {
            return _curKeyboardState.IsKeyDown(key) && _oldKeyboardState.IsKeyDown(key);
        }

        private bool isKeyHeld(string key)
        {
            if(_mappedKeys.ContainsKey(key))
                return isKeyHeld(_mappedKeys[key]);
            return false;
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
            _mappedButtons = new Dictionary<string, Buttons>();
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

        private void mapButton(string name, Buttons button)
        {
            if (_mappedButtons.ContainsKey(name))
                _mappedButtons[name] = button;
            else
                _mappedButtons.Add(name, button);
        }

        private bool isButtonPressed(Buttons button)
        {
            return _curGamePadState.IsButtonDown(button);
        }

        private bool isButtonPressed(string button)
        {
            return isButtonPressed(_mappedButtons[button]);
        }

        private bool isButtonHeld(Buttons button)
        {
            return _curGamePadState.IsButtonDown(button) && _oldGamePadState.IsButtonDown(button);
        }

        private bool isButtonHeld(string button)
        {
            return isButtonHeld(_mappedButtons[button]);
        }
#endif
        #endregion
    }
}
