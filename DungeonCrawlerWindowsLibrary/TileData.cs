//-----------------------------------------------------------------------------
//Based on Nathan Bean's file from Scrolling Shooter Game(Copyright (C) CIS 580 Fall 2012 Class).
// Author: Jiri Malina
//
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence 
//-----------------------------------------------------------------------------
using Microsoft.Xna.Framework.Graphics;

namespace DungeonCrawlerWindowsLibrary
{
    /// <summary>
    /// A structure representing a tile in the game world
    /// </summary>
    public struct TileData
    {
        /// <summary>
        /// The Tile's ID
        /// </summary>
        public uint TileID;

        /// <summary>
        /// Indicates the sprite's orientation
        /// </summary>
        public SpriteEffects SpriteEffects;
    }
}
