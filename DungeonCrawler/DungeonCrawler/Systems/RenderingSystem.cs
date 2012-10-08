#region File Description
//-----------------------------------------------------------------------------
// RenderingSystem.cs 
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
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DungeonCrawler.Components;
#endregion

namespace DungeonCrawler.Systems
{
    /// <summary>
    /// This class embodies the rendering system for our dungeon crawler.
    /// Once per frame it renders everything visible in the game world.
    /// </summary>
    public class RenderingSystem
    {
        #region Private Members
        
        /// <summary>
        /// The game this system belongs to
        /// </summary>
        private DungeonCrawlerGame game;

        /// <summary>
        /// A SpriteBatch for rendering sprites
        /// </summary>
        private SpriteBatch spriteBatch;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new RenderingSystem
        /// </summary>
        /// <param name="game">The game this system belongs to</param>
        public RenderingSystem(DungeonCrawlerGame game)
        {
            this.game = game;
            this.spriteBatch = new SpriteBatch(game.GraphicsDevice);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Renders the game world
        /// </summary>
        /// <param name="elapsedTime">
        /// The time between this and the previous frame.
        /// </param>
        public void Draw(float elapsedTime)
        {
            spriteBatch.Begin();

            // Draw all Sprites
            foreach (Sprite sprite in game.SpriteComponent.All)
            {
                Position position = game.PositionComponent[sprite.EntityID];
                spriteBatch.Draw(sprite.SpriteSheet, 
                                position.Center, 
                                sprite.SpriteBounds, 
                                Color.White, 
                                0f,                                             // rotation
                                new Vector2(position.Radius, position.Radius),  // origin
                                1f,                                             // scale
                                SpriteEffects.None,
                                0); 
            }

            // Draw all MovementSprites
            foreach (MovementSprite sprite in game.MovementSpriteComponent.All)
            {
                Position position = game.PositionComponent[sprite.EntityID];
                spriteBatch.Draw(sprite.SpriteSheet,
                                position.Center,
                                sprite.SpriteBounds,
                                Color.White,
                                0f,                                             // rotation
                                new Vector2(position.Radius, position.Radius),  // origin
                                1f,                                             // scale
                                SpriteEffects.None,
                                0);
            }

            spriteBatch.End();
        }

        #endregion
    }
}
