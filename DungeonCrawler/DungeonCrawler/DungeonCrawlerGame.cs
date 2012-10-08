#region File Description
//-----------------------------------------------------------------------------
// DungeonCrawlerGame.cs 
//
// Author: Nathan Bean
//
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence 
//-----------------------------------------------------------------------------
#endregion

#region Using Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using DungeonCrawler.Components;
using DungeonCrawler.Systems;
#endregion

namespace DungeonCrawler
{
    /// <summary>
    /// A DungeonCrawler game
    /// </summary>
    public class DungeonCrawlerGame : Microsoft.Xna.Framework.Game
    {
        #region  Protected and Private Members

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        #endregion

        #region Game Components

        // Game Components
        public PositionComponent PositionComponent;
        public MovementComponent MovementComponent;
        public MovementSpriteComponent MovementSpriteComponent;
        public SpriteComponent SpriteComponent;

        #endregion

        #region Game Systems

        // Game Systems
        RenderingSystem RenderingSystem;
        MovementSystem MovementSystem;

        #endregion


        /// <summary>
        /// Constructs a new DungeonCrawler game instance
        /// </summary>
        public DungeonCrawlerGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            Content.RootDirectory = "Content";
        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Initialize Components
            PositionComponent = new PositionComponent();
            MovementComponent = new MovementComponent();
            MovementSpriteComponent = new MovementSpriteComponent();
            SpriteComponent = new SpriteComponent();
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Create game systems
            RenderingSystem = new RenderingSystem(this);
            MovementSystem = new MovementSystem(this);

            // Testing code
            Position position = new Position()
            {
                EntityID = 0,
                Center = new Vector2(400, 50),
                Radius = 32f,
            };
            PositionComponent.Add(0, position);
            Movement movement = new Movement() { 
                EntityID = 0,
                Direction = new Vector2(0, 1),
                Speed = 200f,
            };
            MovementComponent.Add(0, movement);
            MovementSprite movementSprite = new MovementSprite() {
                EntityID = 0,
                Facing = Facing.South,
                SpriteSheet = Content.Load<Texture2D>("Spritesheets/wind_fae"),
                SpriteBounds = new Rectangle(0, 0, 64, 64),
                Timer = 0f,
            };
            MovementSpriteComponent.Add(0, movementSprite);


        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            MovementSystem.Update(elapsedTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            RenderingSystem.Draw(elapsedTime);

            base.Draw(gameTime);
        }
    }
}
