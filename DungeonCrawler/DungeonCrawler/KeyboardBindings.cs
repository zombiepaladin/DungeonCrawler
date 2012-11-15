﻿#region File Description
//-----------------------------------------------------------------------------
// KeyboardBindings.cs 
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
    public class KeyboardBindings
    {
        #region Static Members

        private static Dictionary<PlayerIndex, KeyboardBindings> _bindings = new Dictionary<PlayerIndex, KeyboardBindings>(4);

        /// <summary>
        /// Load all the controller configuration.
        /// </summary>
        public static void LoadBindings()
        {
            //TODO have this load from files
            _bindings.Add(PlayerIndex.One, new KeyboardBindings());
            _bindings.Add(PlayerIndex.Two, new KeyboardBindings());
            _bindings.Add(PlayerIndex.Three, new KeyboardBindings());
            _bindings.Add(PlayerIndex.Four, new KeyboardBindings());
        }

        /// <summary>
        /// Gets the controller configuration for the given Player.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static KeyboardBindings GetBindings(PlayerIndex index)
        {
            return _bindings[index];
        }

        #endregion

        public Keys MainTrigger;
        public Keys MainBumper;
        public Keys AltTrigger;
        public Keys AltBumper;
        public Keys MoveUp;
        public Keys MoveRight;
        public Keys MoveLeft;
        public Keys MoveDown;
        public Keys Up;
        public Keys Right;
        public Keys Left;
        public Keys Down;
        public Keys A;
        public Keys B;
        public Keys X;
        public Keys Y;
        public Keys Start;
        public Keys Select;

        public KeyboardBindings()
        {
            MainTrigger = Keys.Enter;
            MainBumper = Keys.RightShift;
            AltTrigger = Keys.OemComma;
            AltBumper = Keys.OemBackslash;
            MoveUp = Keys.W;
            MoveRight = Keys.D;
            MoveLeft = Keys.A;
            MoveDown = Keys.S;
            Up = Keys.Up;
            Right = Keys.Right;
            Left = Keys.Left;
            Down = Keys.Down;
            A = Keys.NumPad8;
            B = Keys.NumPad6;
            X = Keys.NumPad2;
            Y = Keys.NumPad4;
            Start = Keys.Space;
            Select = Keys.P;
        }

        public KeyboardBindings(string file)
        {
            throw new NotImplementedException();
        }

        public void Save(string file)
        {
            throw new NotImplementedException();
        }
    }
}
