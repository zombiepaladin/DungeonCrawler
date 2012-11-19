//-----------------------------------------------------------------------------
// SpriteAnimationComponent.cs 
//
// Author: Samuel Fike, Jiri Malina
//
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence 
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonCrawler.Components
{
    public enum AnimationMovementDirection
    {
        Down = 0,
        Left = 1,
        Right = 2,
        Up = 3,
    }
    /*
     * Frames of spritesheet must be set up like the character spritesheets.
     * Each set of animation frames should be on a seperate row with the same frame size and # of frames.
     */
    public struct SpriteAnimation
    {
        /// <summary>
        /// The ID of the entity this MovementSprite belongs to
        /// </summary>
        public uint EntityID;

        /// <summary>
        /// An animation timer
        /// </summary>
        public float TimePassed;

        /// <summary>
        /// The displayed frame of this MovementSprite
        /// </summary>
        public int CurrentFrame;

        /// <summary>
        /// The number of frames to play per second
        /// </summary>
        public double FramesPerSecond;

        /// <summary>
        /// The row of the spritesheet
        /// </summary>
        public int CurrentAnimationRow;

        /// <summary>
        /// Whether or not the animation should play.
        /// </summary>
        public bool IsPlaying;

        /// <summary>
        /// Whether or not the animation should loop. If false, it will freeze on the last frame of the animation row.
        /// </summary>
        public bool IsLooping;

        public SpriteAnimation(uint id)
        {
            EntityID = id;
            TimePassed = 0;
            CurrentFrame = 0;
            FramesPerSecond = 10;
            CurrentAnimationRow = 0;
            IsPlaying = true;
            IsLooping = true;
        }
    }

    public class SpriteAnimationComponent : GameComponent<SpriteAnimation>
    {

    }
}
