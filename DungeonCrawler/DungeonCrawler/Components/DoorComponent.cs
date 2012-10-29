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

//Samuel Fike and Jiri Malina: Implemented HandleTrigger method

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
        /// The ID of the room the door leads to
        /// </summary>
        public string DestinationRoom;

        /// <summary>
        /// The name of the spawn in the destination room to place the player
        /// </summary>
        public string DestinationSpawnName;

        /// <summary>
        /// Indicates whether the door is (un)locked
        /// </summary>
        public bool Locked;

        /// <summary>
        /// Indicates whether the door is closed/opened
        /// </summary>
        public bool Closed;
    }

    public class DoorComponent : GameComponent<Door>
    {
        public override void HandleTrigger(uint entityID, string type)
        {

        }

    }
}
