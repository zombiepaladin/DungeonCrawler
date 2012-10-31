#region File Description
//-----------------------------------------------------------------------------
// DungeonCrawlerGame.cs 
//
// Author: Nathan Bean
//
// Modified: Nick Stanley added HUDSpriteComponent, 10/15/2012
// Modified: Devin Kelly-Collins added Weapon Components and Systems, 10/24/2012
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

        public static DungeonCrawlerGame game;

        public static LevelManager LevelManager;

        /// <summary>
        /// An AggregateFactory for creating entities quickly
        /// from pre-defined aggregations of components
        /// </summary>
        public AggregateFactory AggregateFactory;

        /// <summary>
        /// A factory for creating weapons and bullets.
        /// </summary>
        public WeaponFactory WeaponFactory;
        
        /// <summary>
        /// A DoorFactory for creating doors
        /// </summary>
        public DoorFactory DoorFactory;

        /// <summary>
        /// A RoomFactory for creating walls
        /// </summary>
        public WallFactory WallFactory;

        /// <summary>
        /// A RoomFactory for creating rooms
        /// </summary>
        public RoomFactory RoomFactory;

        /// <summary>
        /// A CollectibleFactory for creating (surprise) collectibles
        /// </summary>
        public CollectibleFactory CollectableFactory;

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
        public InventoryComponent InventoryComponent;
        public InventorySpriteComponent InventorySpriteComponent;
        public EquipmentComponent EquipmentComponent;
        public WeaponComponent WeaponComponent;
        public BulletComponent BulletComponent;
        public PlayerInfoComponent PlayerInfoComponent;
        public EnemyAIComponent EnemyAIComponent;
        public WeaponSpriteComponent WeaponSpriteComponent;
        public StatsComponent StatsComponent;
        public CollectibleComponent CollectibleComponent;
        public CollisionComponent CollisionComponent;
        #endregion

        #region Game Systems

        // Game Systems
        InputSystem InputSystem;
        NetworkSystem NetworkSystem;
        RenderingSystem RenderingSystem;
        MovementSystem MovementSystem;
        WeaponSystem WeaponSystem;
        EnemyAISystem EnemyAISystem;
        CollisionSystem CollisionSystem;

        public GarbagemanSystem GarbagemanSystem;

        #endregion


        /// <summary>
        /// Constructs a new DungeonCrawler game instance
        /// </summary>
        public DungeonCrawlerGame()
        {
            game = this;
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
            WeaponFactory = new WeaponFactory(this);
            DoorFactory = new DoorFactory(this);
            RoomFactory = new RoomFactory(this);
            CollectableFactory = new CollectibleFactory(this);
            WallFactory = new WallFactory(this);

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
            InventoryComponent = new InventoryComponent();
            InventorySpriteComponent = new InventorySpriteComponent();
            EquipmentComponent = new EquipmentComponent();
            WeaponComponent = new WeaponComponent();
            BulletComponent = new BulletComponent();
            PlayerInfoComponent = new PlayerInfoComponent();
            WeaponSpriteComponent = new WeaponSpriteComponent();
            StatsComponent = new StatsComponent();
            EnemyAIComponent = new EnemyAIComponent();
            CollectibleComponent = new CollectibleComponent();
            CollisionComponent = new CollisionComponent();

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
            WeaponSystem = new WeaponSystem(this);
            EnemyAISystem = new EnemyAISystem(this);
            GarbagemanSystem = new GarbagemanSystem(this);
            CollisionSystem = new Systems.CollisionSystem(this);

            CharacterSelectionScreen.LoadContent();
            // Testing code.
            LevelManager.LoadContent();
            LevelManager.LoadLevel("TestDungeon3");
            //End Testing Code

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
                    WeaponSystem.Update(elapsedTime);
                    LevelManager.Update(elapsedTime);
                    CollisionSystem.Update(elapsedTime);
                    GarbagemanSystem.Update(elapsedTime);
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
