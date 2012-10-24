#region File Description
//-----------------------------------------------------------------------------
// RoomComponent.cs 
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
    /// A struct representing a Room Component
    /// </summary>
    public struct Room
    {
        /// <summary>
        /// The ID of the entity this room belongs to
        /// </summary>
        public uint EntityID;

        /// <summary>
        /// The name of the tilemap for the room
        /// </summary>
        public string Tilemap;

        /// <summary>
        /// EntityIDs of objects, keys are strings indicated on the map editor, stored in trigger's targetID.
        /// </summary>
        public Dictionary<string, uint> idMap;

        /// <summary>
        /// Type of object of the target, keys are strings indicated on the map editor, stored in trigger's targetID.
        /// </summary>
        public Dictionary<string, string> targetTypeMap ;
    }

    public class RoomComponent : GameComponent<Room>
    {

    }
}
