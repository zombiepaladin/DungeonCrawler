#region File Description
//-----------------------------------------------------------------------------
// DoorComponent.cs 
//
// Author: Nicholas Strub
//
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence 
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
#endregion

namespace DungeonCrawler.Components
{
    /// <summary>
    /// A struct representing a Door Component
    /// </summary>
    public struct Door
    {
        /// <summary>
        /// The ID of the entity this sprite belongs to
        /// </summary>
        public uint EntityID;

        /// <summary>
        /// The ID of the first room the door is attached to
        /// </summary>
        public uint Room1;

        /// <summary>
        /// The ID of the second room the door is attached to
        /// </summary>
        public uint Room2;
    }

    public class DoorComponent : GameComponent<Door>
    {

    }
}
