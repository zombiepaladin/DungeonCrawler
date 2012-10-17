#region File Description
//-----------------------------------------------------------------------------
// DungeonCrawlerGame.cs 
//
// Author: Nathan Bean
//
// Modified: Nick Stanley added HUDSpriteComponent, 10/15/2012
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
using DungeonCrawler.Entities;
#endregion

namespace DungeonCrawler
{
    public enum GameState
    {
        SplashScreen,
        SignIn,
        CharacterSelection,
        NetworkSetup,
        Gameplay,
        GameMenu,
        Credits,
    }

    /// <summary>
    /// A DungeonCrawler game
    /// </summary>
    public class DungeonCrawlerGame : Microsoft.Xna.Framework.Game
    {
        #region  Protected and Private Members

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        #endregion

        #region Public Members

        /// <summary>
        /// The current GameState
        /// TODO: Change initial state to SplashScreen
        /// </summary>
        public GameState GameState = GameState.SignIn;

        public static LevelManager LevelManager;
        /// <summary>
        /// An AggregateFactory for creating entities quickly
        /// from pre-defined aggregations of components
        /// </summary>
        public AggregateFactory AggregateFactory;

        public CharacterSelectionScreen CharacterSelectionScreen;

        #endregion

        #region Game Components

        // Game Components
        public PlayerComponent PlayerComponent;
        public LocalComponent LocalComponent;
        public RemoteComponent RemoteComponent;
        public PositionComponent PositionComponent;
        public MovementComponent MovementComponent;
        public MovementSpriteComponent MovementSpriteComponent;
        public SpriteComponent SpriteComponent;
        public RoomComponent RoomComponent;
        public DoorComponent DoorComponent;
		public HUDSpriteComponent HUDSpriteComponent;
        public HUDComponent HUDComponent;

        #endregion

        #region Game Systems

        // Game Systems
        InputSystem InputSystem;
        NetworkSystem NetworkSystem;
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
            Components.Add(new GamerServicesComponent(this));
        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            AggregateFactory = new AggregateFactory(this);

            // Initialize Components
            PlayerComponent = new PlayerComponent();
            LocalComponent = new LocalComponent();
            RemoteComponent = new RemoteComponent();
            PositionComponent = new PositionComponent();
            MovementComponent = new MovementComponent();
            MovementSpriteComponent = new MovementSpriteComponent();
            SpriteComponent = new SpriteComponent();
            DoorComponent = new DoorComponent();
            RoomComponent = new RoomComponent();
			HUDSpriteComponent = new HUDSpriteComponent();
            HUDComponent = new HUDComponent();

            CharacterSelectionScreen = new CharacterSelectionScreen(graphics, this);

            LevelManager = new LevelManager(this);

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
            InputSystem = new InputSystem(this);
            NetworkSystem = new NetworkSystem(this);
            RenderingSystem = new RenderingSystem(this);
            MovementSystem = new MovementSystem(this);

            CharacterSelectionScreen.LoadContent();
            // Testing code
            //AggregateFactory.CreateFromAggregate(Aggregate.ZombiePlayer, PlayerIndex.One);
            LevelManager.LoadContent();
            LevelManager.LoadLevel("TestDungeon3");

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

            if (GameState == DungeonCrawler.GameState.SignIn)
            {
                // Requires at least one player to sign in
                if (Gamer.SignedInGamers.Count == 0)
                {
                    if (IsActive) Guide.ShowSignIn(4, false);
                }
                else
                {
                    GameState = GameState.CharacterSelection;
                }
            }

            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            switch (GameState)
            {
                case GameState.SplashScreen:
                    // TODO: Update splash screens
                    break;

                case GameState.SignIn:
                    // Requires at least one player to sign in
                    if (Gamer.SignedInGamers.Count == 0)
                    {
                        if (IsActive) Guide.ShowSignIn(4, false);
                    }
                    else
                    {
                        GameState = GameState.CharacterSelection;
                    }
                    break;

                case GameState.CharacterSelection:
                    // TODO: Update character selection screen
                    CharacterSelectionScreen.Update(gameTime);
                    break;

                case GameState.NetworkSetup:
                    NetworkSystem.Update(elapsedTime);
                    break;

                case GameState.GameMenu:
                    // TODO: Update game menu screen

                    // Game Menu does not pause game!
                    // Update game systems
                    NetworkSystem.Update(elapsedTime);
                    MovementSystem.Update(elapsedTime);
                    break;

                case GameState.Gameplay:
                    // Update game systems
                    InputSystem.Update(elapsedTime);
                    NetworkSystem.Update(elapsedTime);
                    MovementSystem.Update(elapsedTime);
                    LevelManager.Update(elapsedTime);
                    break;

                case GameState.Credits:
                    // TODO: Update credits
                    break;
            }


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
            CharacterSelectionScreen.Draw(elapsedTime);
            if (GameState != GameState.CharacterSelection)
            {
                LevelManager.Draw(elapsedTime);
                NetworkSystem.Draw(elapsedTime);
                RenderingSystem.Draw(elapsedTime);
            }
           
            base.Draw(gameTime);
        }
    }
}
