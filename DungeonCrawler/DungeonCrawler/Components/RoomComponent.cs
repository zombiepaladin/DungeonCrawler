#region File Description
//-----------------------------------------------------------------------------
// RoomComponent.cs 
//
// Author: Nicholas Strub
//
// Modified By: Nicholas Strub - Added dictionary of player spawn points (11/3/2012)
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
        public Dictionary<string, string> targetTypeMap;

        /// <summary>
        /// Stores all of the playerSpawns within the rooms. Keys are the SpawnNames defined in the properties of the object on the tilemap and the values are the spawn positions in the form of a Vector2.
        /// </summary>
        public Dictionary<string, Vector2> playerSpawns;

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
        /// <summary>
        /// Finds a room that has the specified name.
        /// </summary>
        /// <param name="elements">The components to query</param>
        /// <param name="name">The name of the room</param>
        /// <returns>The room if it exists, or null if doesn't exist.</returns>
        public Room FindRoom(string name)
        {
            foreach (KeyValuePair<uint, Room> room in elements)
            {
                if (room.Value.Tilemap == name)
                {
                    return room.Value;
                }
            }

            return new Room() { Tilemap = "null" };
        }
    }

    public static class RoomExtensions
    {
        /// <summary>
        /// Finds a room that has the specified name.
        /// </summary>
        /// <param name="elements">The components to query</param>
        /// <param name="name">The name of the room</param>
        /// <returns>The room if it exists, or null if doesn't exist.</returns>
        public static Room FindRoom(this IEnumerable<Room> elements, string name)
        {
            foreach (Room room in elements)
            {
                if (room.Tilemap == name)
                {
                    return room;
                }
            }

            return new Room() { Tilemap = "null" };
        }
    }
}
