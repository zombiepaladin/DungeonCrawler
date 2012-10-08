#region File Description
//-----------------------------------------------------------------------------
// MovementSpriteComponent.cs 
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
    /// The direction a sprite is facing
    /// </summary>
    public enum Facing
    {
        South = 0,
        West = 1,
        East = 2,
        North = 3,
    }

    /// <summary>
    /// A Struct representing an animated "walking" Sprite
    /// </summary>
    public struct MovementSprite
    {
        /// <summary>
        /// The ID of the entity this MovementSprite belongs to
        /// </summary>
        public uint EntityID;

        /// <summary>
        /// The texture containing this MovementSprite
        /// </summary>
        public Texture2D SpriteSheet;

        /// <summary>
        /// The bounds of this MovementSprite in its spritesheet
        /// </summary>
        public Rectangle SpriteBounds;

        /// <summary>
        /// An animation timer
        /// </summary>
        public float Timer;

        /// <summary>
        /// The displayed frame of this MovementSprite
        /// </summary>
        public int Frame;

        /// <summary>
        /// The displayed facing direction for this MovementSprite
        /// </summary>
        public Facing Facing;
    }

    /// <summary>
    /// A GameComponent representing entities' MovementSprites
    /// </summary>
    public class MovementSpriteComponent : GameComponent<MovementSprite>
    {

    }
}
