//-----------------------------------------------------------------------------
//Based on Nathan Bean's file from Scrolling Shooter Game(Copyright (C) CIS 580 Fall 2012 Class).
// Author: Jiri Malina
//
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence 
//-----------------------------------------------------------------------------
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonCrawlerWindowsLibrary
{
    /// <summary>
    /// A structure representing a tile in a tileset
    /// </summary>
    public struct Tile
    {
        /// <summary>
        /// The ID of the texture this tile is found in
        /// </summary>
        public int TextureID;

        /// <summary>
        /// The source rectangle of the tile within
        /// its texture
        /// </summary>
        public Rectangle Source;
    }
}
