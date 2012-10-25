//-----------------------------------------------------------------------------
//Based on Nathan Bean's file from Scrolling Shooter Game(Copyright (C) CIS 580 Fall 2012 Class).
// Author: Jiri Malina
//
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence 
//-----------------------------------------------------------------------------
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace DungeonCrawlerWindowsLibrary
{
    /// <summary>
    /// A structure representing the instance-specific
    /// data for a GameObject within a level
    /// </summary>
    public struct GameObjectData
    {
        /// <summary>
        /// The game object ID assigned to this instance
        /// </summary>
        public uint ID;

        /// <summary>
        /// The broad category of game object (Enemy, Player, etc).
        /// </summary>
        public string Category;

        /// <summary>
        /// The specific type of game object (Dart, Shrike, etc).
        /// </summary>
        public string Type;

        /// <summary>
        /// The position of the game object instance within the game world
        /// </summary>
        public Rectangle Position;

        public Dictionary<string, string> properties;
    }
}
