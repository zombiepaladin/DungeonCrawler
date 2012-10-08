﻿#region File Description
//-----------------------------------------------------------------------------
// MovementSystem.cs 
//
// Author: Nathan Bean
//
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence 
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DungeonCrawler.Components;
#endregion

namespace DungeonCrawler.Systems
{
    /// <summary>
    /// This class embodies a system for updating all moving
    /// entities within the game world
    /// </summary>
    public class MovementSystem
    {
        #region Private Members

        /// <summary>
        /// The game this system belongs to
        /// </summary>
        private DungeonCrawlerGame game;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new MovementSystem
        /// </summary>
        /// <param name="game">The game this system belongs to</param>
        public MovementSystem(DungeonCrawlerGame game)
        {
            this.game = game;
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
            // Update all entities that have a movement component
            foreach (Movement movement in game.MovementComponent.All)
            {
                // Update the entity's position in the world
                Position position = game.PositionComponent[movement.EntityID];
                position.Center += elapsedTime * movement.Speed * movement.Direction;
                game.PositionComponent[movement.EntityID] = position;
                
                // Update the entity's movement sprite
                if(game.MovementSpriteComponent.Contains(movement.EntityID))
                {
                    MovementSprite movementSprite = game.MovementSpriteComponent[movement.EntityID];
                    
                    // Set the direction of movement
                    float angle = (float)Math.Atan2(movement.Direction.Y, movement.Direction.X);
                    if (angle > -MathHelper.PiOver4 && angle < MathHelper.PiOver4)
                        movementSprite.Facing = Facing.East;
                    else if (angle >= MathHelper.PiOver4 && angle <= 3f * MathHelper.PiOver4)
                        movementSprite.Facing = Facing.South;
                    else if (angle >= -3 * MathHelper.PiOver4 && angle <= -MathHelper.PiOver4)
                        movementSprite.Facing = Facing.North;
                    else 
                        movementSprite.Facing = Facing.West;

                    // Update the timing
                    movementSprite.Timer += elapsedTime;
                    if (movementSprite.Timer > 0.1f)
                    {
                        movementSprite.Frame += 1;
                        if (movementSprite.Frame > 2) movementSprite.Frame = 0;
                        movementSprite.Timer -= 0.1f;
                    }

                    // Update the sprite bounds
                    movementSprite.SpriteBounds.X = 64 * movementSprite.Frame;
                    movementSprite.SpriteBounds.Y = 64 * (int)movementSprite.Facing;

                    // Apply our updates
                    game.MovementSpriteComponent[movement.EntityID] = movementSprite;
                }
            }
        }

        #endregion
    }
}
