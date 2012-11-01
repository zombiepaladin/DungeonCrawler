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

//Samuel Fike and Jiri Malina: Added idMap and targetTypeMap fields

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

        /// <summary>
        /// The tilemap's width, in tiles
        /// </summary>
        public int Width;

        /// <summary>
        /// The tilemap's height, in tiles
        /// </summary>
        public int Height;

        /// <summary>
        /// The width of the tilemap's tiles
        /// </summary>
        public int TileWidth;

        /// <summary>
        /// The height of the tilemap's tiles
        /// </summary>
        public int TileHeight;

        /// <summary>
        /// The width of the walls, in tiles
        /// </summary>
        public int WallWidth;
    }

    public class RoomComponent : GameComponent<Room>
    {

    }
}
