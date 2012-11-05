#region File Description
//-----------------------------------------------------------------------------
// DungeonCrawlerGame.cs 
//
// Author: Nathan Bean
//
// Modified: Nick Stanley added HUDSpriteComponent, 10/15/2012
// Modified: Devin Kelly-Collins added Weapon Components and Systems, 10/24/2012
// Modified: Joseph Shaw added Game Saving Region and Methods/Structs, 10/31/2012
// Modified: Nicholas Strub added RoomChange Game State, 10/31/2012
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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;
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
        RoomChange,
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
        public ContinueNewGameScreen ContinueNewGameScreen;

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
        public TriggerComponent TriggerComponent;
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
        public RoomChangingSystem RoomChangingSystem;

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
            CharacterSelectionScreen = new CharacterSelectionScreen(graphics, this);
            ContinueNewGameScreen = new ContinueNewGameScreen(graphics, this);
            EquipmentComponent = new EquipmentComponent();
            WeaponComponent = new WeaponComponent();
            BulletComponent = new BulletComponent();
            PlayerInfoComponent = new PlayerInfoComponent();
            WeaponSpriteComponent = new WeaponSpriteComponent();
            StatsComponent = new StatsComponent();
            EnemyAIComponent = new EnemyAIComponent();
            CollectibleComponent = new CollectibleComponent();
            CollisionComponent = new CollisionComponent();
            TriggerComponent = new TriggerComponent();
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
            RoomChangingSystem = new RoomChangingSystem(this);

            CharacterSelectionScreen.LoadContent();
            ContinueNewGameScreen.LoadContent();
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
                    //if (!ContinueNewGameScreen.isConnected)
                    //{
                    //    ContinueNewGameScreen.isConnected = true;
                    //    ContinueNewGameScreen.loadGameSaves();
                    //}
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
                        //if (!ContinueNewGameScreen.isConnected)
                        //{
                        //    ContinueNewGameScreen.isConnected = true;
                        //    ContinueNewGameScreen.loadGameSaves();
                        //}
                    }
                    break;

                case GameState.CharacterSelection:
                    // TODO: Update character selection screen
                    CharacterSelectionScreen.Update(gameTime);
                    //ContinueNewGameScreen.Update(gameTime);
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

                case GameState.RoomChange:
                    NetworkSystem.Update(elapsedTime);
                    RoomChangingSystem.Update(elapsedTime);
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
            GraphicsDevice.Clear(Color.Black);

            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            CharacterSelectionScreen.Draw(elapsedTime);
            //ContinueNewGameScreen.Draw(elapsedTime);
            if (GameState != GameState.CharacterSelection && GameState != GameState.RoomChange)
            {
                LevelManager.Draw(elapsedTime);
                NetworkSystem.Draw(elapsedTime);
                RenderingSystem.Draw(elapsedTime);
            }

            base.Draw(gameTime);
        }

        #region Game Saving
        /// <summary>
        /// The filename of the master save file
        /// </summary>
        public const string masterFileName = "master.sav";
        public IAsyncResult result;
        public Object stateobj;

        /// <summary>
        /// Serializable for the master save file, which holds the previews for the actual saved files
        /// </summary>
        [Serializable]
        public struct MasterSaveFile
        {
            public List<CharacterSaveFilePreview> charFiles;
        }

        /// <summary>
        /// A preview of the actual saved file, including the real filename, character sprite, and level
        /// </summary>
        [Serializable]
        public struct CharacterSaveFilePreview
        {
            public string CharacterSaveFile;
            public string characterType;
            public string charSprite;
            public int Level;
        }

        /// <summary>
        /// The actual saved character file, which stores the character type, level, stats, skills, and items
        /// </summary>
        [Serializable]
        public struct CharacterSaveFile
        {
            public string fileName;
            public string charSprite;
            public string characterType;
            public int Level;
            // Other skills/stats
        }

        /// <summary>
        /// This method serializes a data object into
        /// the StorageContainer for this game.
        /// </summary>
        /// <param name="device"></param>
        public static void DoSaveGame(StorageDevice device, CharacterSaveFile gameData)
        {
            // Open a storage container.
            IAsyncResult result = device.BeginOpenContainer("DungeonCrawler", null, null);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = device.EndOpenContainer(result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();

            // Create the BinaryFormatter here in-case we have to create a new save
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            Stream stream;
            // Use this to tell us if this is a new save
            bool fileExists = false;
            // Check to see whether the save exists.
            if (container.FileExists(gameData.fileName))
            {
                // Delete it so that we can create one fresh.
                container.DeleteFile(gameData.fileName);
                fileExists = true;
            }

            // Create/Update the charPreview to reflect the current player's level
            CharacterSaveFilePreview charPreview;
            MasterSaveFile masterSaveFile = GetMasterSaveFile(device);
            if (fileExists)
            {
                charPreview = masterSaveFile.charFiles.Find(charFile => charFile.CharacterSaveFile == gameData.fileName);
                masterSaveFile.charFiles.Remove(charPreview);
            }
            else
            {
                charPreview = new CharacterSaveFilePreview();
                charPreview.CharacterSaveFile = gameData.fileName;
                charPreview.charSprite = gameData.charSprite;
                charPreview.characterType = gameData.characterType;
            }
            charPreview.Level = gameData.Level;

            masterSaveFile.charFiles.Add(charPreview);

            // Create the file.
            stream = container.CreateFile(gameData.fileName);

            // Convert the file to binary and save it
            binaryFormatter.Serialize(stream, gameData);

            // Close the file.
            stream.Close();

            // Dispose the container, to commit changes.
            container.Dispose();
        }

        /// <summary>
        /// This method loads a serialized data object
        /// from the StorageContainer for this game.
        /// </summary>
        /// <param name="device"></param>
        public static CharacterSaveFile DoLoadGame(StorageDevice device, string fileName)
        {
            // Open a storage container.
            IAsyncResult result = device.BeginOpenContainer("DungeonCrawler", null, null);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = device.EndOpenContainer(result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();

            // Check to see whether the save exists.
            if (!container.FileExists(fileName))
            {
                // If not, dispose of the container and return.
                container.Dispose();
                return new CharacterSaveFile();
            }

            // Open the file.
            Stream stream = container.OpenFile(fileName, FileMode.Open);

            // Read the data from the file.
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            CharacterSaveFile characterSaveData = (CharacterSaveFile)binaryFormatter.Deserialize(stream);

            // Close the file.
            stream.Close();

            // Dispose the container.
            container.Dispose();

            return characterSaveData;
        }

        /// <summary>
        /// This method loads a serialized data object
        /// from the StorageContainer for this game.
        /// </summary>
        /// <param name="device"></param>
        public static MasterSaveFile GetMasterSaveFile(StorageDevice device)
        {
            // Open a storage container.
            IAsyncResult result = device.BeginOpenContainer("DungeonCrawler", null, null);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = device.EndOpenContainer(result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();

            // Check to see whether the save exists.
            if (!container.FileExists(masterFileName))
            {
                // If not, dispose of the container and return a new MasterSaveFile.
                container.Dispose();
                return new MasterSaveFile();
            }

            // Open the file.
            Stream stream = container.OpenFile(masterFileName, FileMode.Open);

            // Read the data from the file.
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MasterSaveFile masterSaveFile = (MasterSaveFile)binaryFormatter.Deserialize(stream);

            // Close the file.
            stream.Close();

            // Dispose the container.
            container.Dispose();

            return masterSaveFile;
        }

        /// <summary>
        /// This method serializes a data object into
        /// the StorageContainer for this game.
        /// </summary>
        /// <param name="device"></param>
        public static void SaveMasterFile(StorageDevice device, MasterSaveFile masterSaveFile)
        {
            // Open a storage container.
            IAsyncResult result = device.BeginOpenContainer("DungeonCrawler", null, null);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = device.EndOpenContainer(result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();

            // Check to see whether the save exists.
            if (container.FileExists(masterFileName))
            {
                // Delete it so that we can create one fresh.
                container.DeleteFile(masterFileName);
            }

            // Create the file.
            Stream stream = container.CreateFile(masterFileName);

            // Create the BinaryFormatter 
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            // Convert the file to binary and save it
            binaryFormatter.Serialize(stream, masterSaveFile);

            // Close the file.
            stream.Close();

            // Dispose the container, to commit changes.
            container.Dispose();
        }


        public void GetDevice(IAsyncResult result, StorageDevice device)
        {
            device = StorageDevice.EndShowSelector(result);
        }
        #endregion
    }
}
