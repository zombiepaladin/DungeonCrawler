#region File Description
//-----------------------------------------------------------------------------
// SpriteComponent.cs
//
// Author: Nathan Bean
//
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence 
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace DungeonCrawler.Components
{
    /// <summary>
    /// A struct representing a sprite component
    /// </summary>
    public struct Sprite
    {
        /// <summary>
        /// The ID of the entity this sprite belongs to
        /// </summary>
        public uint EntityID;

        /// <summary>
        /// The texture containing this sprite
        /// </summary>
        public Texture2D SpriteSheet;

        /// <summary>
        /// The bounds of this sprite in its spritesheet
        /// </summary>
        public Rectangle SpriteBounds;

        /// <summary>
        /// What color to use when drawing the sprite.  Used to change the alpha channel
        /// </summary>
        public Color SpriteColor;

        /// <summary>
        /// Use SpriteColor as the color parameter instead of white
        /// </summary>
        public bool UseDifferentColor;
    }


    /// <summary>
    /// A component containing a simple sprite to render on-screen
    /// </summary>
    public class SpriteComponent : GameComponent<Sprite>
    {

    }
}
