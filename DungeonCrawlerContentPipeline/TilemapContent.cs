//-----------------------------------------------------------------------------
//Based on Nathan Bean's file from Scrolling Shooter Game(Copyright (C) CIS 580 Fall 2012 Class).
// Author: Jiri Malina
//
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence 
//-----------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

using DungeonCrawlerWindowsLibrary;

namespace DungeonCrawlerContentPipeline
{
    /// <summary>
    /// The content pipeline equivalent of Tilemap
    /// </summary>
    [ContentSerializerRuntimeType("DungeonCrawlerWindowsLibrary.Tilemap, DungeonCrawlerWindowsLibrary")]
    public class TilemapContent
    {
        /// <summary>
        /// The name of the tilemap
        /// </summary>
        public string Name;

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
        
        /// <summary>
        /// A list of paths to all images used by this tilemap.
        /// </summary>
        public string[] ImagePaths;

        /// <summary>
        /// The total number of unique tiles used in our tilemap
        /// </summary>
        public int TileCount;

        /// <summary>
        /// The set of all unique tiles used by this tilemap
        /// </summary>
        public Tile[] Tiles;

        /// <summary>
        /// The total number of layers in our tileset
        /// </summary>
        public int LayerCount;

        /// <summary>
        /// The layers in our tileset
        /// </summary>
        public TilemapLayerContent[] Layers;

        /// <summary>
        /// The total number of game object groups in our tilemap
        /// </summary>
        public int GameObjectGroupCount;

        /// <summary>
        /// The game object groups in our tilemap
        /// </summary>
        public GameObjectGroupContent[] GameObjectGroups;

        /// <summary>
        /// The player's starting position
        /// </summary>
        public Vector2 PlayerStart;

        /// <summary>
        /// The layer in which the player exists
        /// </summary>
        public int PlayerLayer;

        /// <summary>
        /// The music to play in this level
        /// </summary>
        public string MusicTitle;

        /// <summary>
        /// The properties defined on this tilemap.  These are loaded
        /// from the tmx file, and should be converted to more efficient 
        /// and meaningful variables in the TilemapProcessor 
        /// </summary>
        [ContentSerializerIgnore]
        public Dictionary<string, string> Properties = new Dictionary<string, string>();

    }
}
