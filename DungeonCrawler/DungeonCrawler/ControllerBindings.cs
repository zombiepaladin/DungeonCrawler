﻿#region File Description
//-----------------------------------------------------------------------------
// ControllerBindings.cs 
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
    public class ControllerBindings
    {
        #region Static Members

        private static Dictionary<PlayerIndex, ControllerBindings> _bindings = new Dictionary<PlayerIndex, ControllerBindings>(4);

        /// <summary>
        /// Load all the controller configuration.
        /// </summary>
        public static void LoadBindings()
        {
            //TODO have this load from files
            _bindings.Add(PlayerIndex.One, new ControllerBindings());
            _bindings.Add(PlayerIndex.Two, new ControllerBindings());
            _bindings.Add(PlayerIndex.Three, new ControllerBindings());
            _bindings.Add(PlayerIndex.Four, new ControllerBindings());
        }

        /// <summary>
        /// Gets the controller configuration for the given Player.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static ControllerBindings GetBindings(PlayerIndex index)
        {
            return _bindings[index];
        }

        #endregion

        public Buttons MainTrigger;
        public Buttons MainBumper;
        public Buttons AltTrigger;
        public Buttons AltBumper;
        public Buttons Up;
        public Buttons Right;
        public Buttons Left;
        public Buttons Down;
        public Buttons A;
        public Buttons B;
        public Buttons X;
        public Buttons Y;
        public Buttons Start;
        public Buttons Select;

        public ControllerBindings()
        {
            MainTrigger = Buttons.LeftTrigger;
            MainBumper = Buttons.LeftShoulder;
            AltBumper = Buttons.RightTrigger;
            AltTrigger = Buttons.RightShoulder;
            Up = Buttons.DPadUp;
            Right = Buttons.DPadRight;
            Left = Buttons.DPadLeft;
            Down = Buttons.DPadDown;
            A = Buttons.A;
            B = Buttons.B;
            X = Buttons.X;
            Y = Buttons.Y;
            Start = Buttons.Start;
            Select = Buttons.Back;
        }

        public ControllerBindings(string file)
        {
            throw new NotImplementedException();
        }

        public void Save(string file)
        {
            throw new NotImplementedException();
        }
    }
}
