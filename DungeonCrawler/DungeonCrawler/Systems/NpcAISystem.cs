#region File Description
//-----------------------------------------------------------------------------
// NPCAISystem.cs 
//
// Author: Michael Fountain
//
//
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence 
//-----------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DungeonCrawler.Components;

namespace DungeonCrawler.Systems
{
   
    
    public class NpcAISystem
    {

        #region Private Members

        /// <summary>
        /// The game this system belongs to
        /// </summary>
        private DungeonCrawlerGame game;
        private Random rand;
        private int direction;
        private Movement movement;
        private SpriteAnimation spriteAnimation;
        private float timer;
        private bool resetTimer;


        private enum Direction
        {
            Up,
            Right,
            Down,
            Left
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new MovementSystem
        /// </summary>
        /// <param name="game">The game this system belongs to</param>
        public NpcAISystem(DungeonCrawlerGame game)
        {
            this.game = game;
            rand = new Random();
            resetTimer = false;
        }

        #endregion

         #region Public Methods

        /// <summary>
        /// Updates all moving entities in the game
        /// </summary>
        /// <param name="elapsedTime">
        /// The time between this and the previous frame.
        /// </param>
        public void Update(float elapsedTime)
        {
            timer += elapsedTime;
            foreach (NpcAI ai in game.NpcAIComponent.All)
            {
                direction = rand.Next(0, 8);
               
                Movement movement = game.MovementComponent[ai.EntityID];
                SpriteAnimation spriteAnimation = game.SpriteAnimationComponent[ai.EntityID];

                if (timer > 1)
                {
                    switch (direction)
                    {
                        case (int)Direction.Up:
                            movement.Direction = new Vector2(0, -1);
                            movement.Speed = 200f;

                            spriteAnimation.IsPlaying = true;
                            spriteAnimation.CurrentAnimationRow = (int)AnimationMovementDirection.Up;
                            break;
                        case (int)Direction.Right:
                            movement.Direction = new Vector2(1, 0);
                            movement.Speed = 200f;

                            spriteAnimation.IsPlaying = true;
                            spriteAnimation.CurrentAnimationRow = (int)AnimationMovementDirection.Right;
                            break;
                        case (int)Direction.Down:
                            movement.Direction = new Vector2(0, 1);
                            movement.Speed = 200f;

                            spriteAnimation.IsPlaying = true;
                            spriteAnimation.CurrentAnimationRow = (int)AnimationMovementDirection.Down;
                            break;
                        case (int)Direction.Left:
                            movement.Direction = new Vector2(-1, 0);
                            movement.Speed = 200f;

                            spriteAnimation.IsPlaying = true;
                            spriteAnimation.CurrentAnimationRow = (int)AnimationMovementDirection.Left;
                            break;
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        default:
                            movement.Direction = new Vector2(0, 0);
                            movement.Speed = 0f;

                            spriteAnimation.IsPlaying = false;
                            break;
                    }
                    game.MovementComponent[ai.EntityID] = movement;
                    game.SpriteAnimationComponent[spriteAnimation.EntityID] = spriteAnimation;
                    resetTimer = true;
                }
            }
            if (resetTimer)
            {
                timer = 0;
                resetTimer = false;
            }
            
        }

        #endregion
    }
}
