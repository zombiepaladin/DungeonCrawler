#region File Description
//-----------------------------------------------------------------------------
// MovementSystem.cs 
//
// Author: Nathan Bean
//
// Modified By: Nicholas Strub - Added Player Clamping (Assignment 7)
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
                // Player clamping based on the size of the walls, the tile sizes, and the room dimensions.
                Room currentRoom = DungeonCrawlerGame.LevelManager.getCurrentRoom();

                bool clamped = false;
                if (position.Center.X - position.Radius < 0)
                {
                    position.Center.X = position.Radius;
                    clamped = true;
                }
                if (position.Center.Y - position.Radius < 0)
                {
                    position.Center.Y = position.Radius;
                    clamped = true;
                }
                if (position.Center.X + position.Radius > currentRoom.Width * currentRoom.TileWidth)
                {
                    position.Center.X = (currentRoom.Width * currentRoom.TileWidth) - position.Radius;
                    clamped = true;
                }
                if (position.Center.Y + position.Radius > currentRoom.Height * currentRoom.TileHeight)
                {
                    position.Center.Y = (currentRoom.Height * currentRoom.TileHeight) - position.Radius;
                    clamped = true;
                }
                //Remove if it's a bullet. (Took out for collision test demonstration)
                if (clamped && game.BulletComponent.Contains(position.EntityID))
                    game.GarbagemanSystem.ScheduleVisit(position.EntityID, GarbagemanSystem.ComponentType.Bullet);

                game.PositionComponent[movement.EntityID] = position;
                
                // Update the entity's movement sprite
                if(game.MovementSpriteComponent.Contains(movement.EntityID))
                {
                    MovementSprite movementSprite = game.MovementSpriteComponent[movement.EntityID];
                    
                    // Set the direction of movement
                    float angle = (float)Math.Atan2(movement.Direction.Y, movement.Direction.X);
                    if (movement.Direction.X == 0 && movement.Direction.Y == 0)
                    {
                        // He's not moving, so update the sprite bounds with the idle animation
                        movementSprite.SpriteBounds.X = 64;
                        movementSprite.SpriteBounds.Y = 64 * (int)movementSprite.Facing;
                    }
                    else
                    {
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
                    }
                    // Apply our updates
                    game.MovementSpriteComponent[movement.EntityID] = movementSprite;
                }

                //Update the collision bounds if it has one
                if (game.CollisionComponent.Contains(movement.EntityID))
                {
                    game.CollisionComponent[movement.EntityID].Bounds.UpdatePosition(
                        game.PositionComponent[movement.EntityID].Center);
                }
            }
        }

        #endregion
    }
}
