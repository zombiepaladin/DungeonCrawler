#region File Description
//-----------------------------------------------------------------------------
// ContinueNewGameScreen.cs 
//
// This is the base character selecting/loading screen.  It loads the user's
// saved files and lets them pick an existing file or start a new game.
// When new game is selected, it operates through the CharacterSelectionSreen.
//
// Author: Joseph Shaw
// Modified: Josh Zavala - Moved Equipment to AggregateFactory, Assignment 9
//
// Modified: Devin Kelly-Collins - Implemented UserInput (11/26/12)
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
    public class ContinueNewGameScreen
    {
        #region  Protected and Private Members

        /// <summary>
        /// The game this CharacterSelectionScreen belongs to
        /// </summary>
        protected DungeonCrawlerGame game;

        /// <summary>
        /// A SpriteBatch for rendering sprites
        /// </summary>
        protected SpriteBatch spriteBatch;

        public CharacterSelectionScreen CharacterSelectionScreen;
        public CharSelectPlayer currentPlayer;

        #endregion

        #region Private Members
        // Background and button image, button image is reused to initialize all of the buttons
        private Texture2D titleTexture;
        private Texture2D buttonTexture;

        // Background and button image/text sprites, again the button sprites are reused to initialize all of the buttons
        private ImageSprite titleImage;
        private ImageSprite buttonImage;
        private TextSprite buttonText;
        private Vector2 buttonPos;
        private Vector2 buttonTextOffset;

        // Image sprite for the controls
        private ImageSprite controlsImage;

        // The sprite font for the text sprite
        private SpriteFont spriteFont;

        // List of the image and text sprites and the character's aggregate
        private ImageSprite[,] buttons;
        private TextSprite[,] buttonTexts;
        private Aggregate[,] buttonAggregates;

        // Viewport and its center
        private Viewport viewport;
        private Vector2 center;
        private Vector2 gameSavePosition;

        // Colors for the character image for when it is highlighted or not
        private Color selected;
        private Color unselected;

        // Sound for when the player moves between the characters
        private SoundEffect soundEffect;

        // Variables for keeping track of the player cursor, selection, etc.
        private CharSelectPlayer playerOne;
        private CharSelectPlayer playerTwo;
        private CharSelectPlayer playerThree;
        private CharSelectPlayer playerFour;
        private List<CharSelectPlayer> players;
        private float controllerDelay;
        private float selectedCursorAlpha;
        private bool cursorMoved;
        private int currentPlayerIndex = 0;

        private Vector2 movementOffset = new Vector2(0, 100);
        private int selectedGameSave = 0;

        private List<CharSelectPreview> gameSaves;

        public bool isConnected = false;

        public bool isNewGame = false;

        private Vector2[] cursorPositions;
        #endregion

        /// <summary>
        /// The constructor for the CharacterSelectionScreen
        /// </summary>
        public ContinueNewGameScreen(GraphicsDeviceManager graphics, DungeonCrawlerGame game)
        {
            // Get the game reference
            this.game = game;

            // Set up the viewport vars
            viewport = graphics.GraphicsDevice.Viewport;
            center = new Vector2(viewport.Width / 2, viewport.Height / 2);
            gameSavePosition = center - new Vector2(viewport.Width / 4, 0);

            // Set the colors and cursor vars
            selected = Color.White;
            unselected = Color.White * (1f / 2);
            controllerDelay = 100f;
            selectedCursorAlpha = 0.7f;
            cursorMoved = false;
            players = new List<CharSelectPlayer>();

            cursorPositions = new Vector2[] { new Vector2(viewport.Width * 0.8f, viewport.Height * 0.2f),
                                              new Vector2(viewport.Width * 0.8f,viewport.Height * 0.4f),
                                              new Vector2(viewport.Width * 0.8f,viewport.Height * 0.6f),
                                              new Vector2(viewport.Width * 0.8f,viewport.Height * 0.8f), };

            // Initialize the sprite arrays
            buttons = new ImageSprite[2, 3];
            buttonTexts = new TextSprite[2, 3];
            buttonAggregates = new Aggregate[2, 3];

            // Initialize the sprite batch
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

            gameSaves = new List<CharSelectPreview>();

            CharacterSelectionScreen = new CharacterSelectionScreen(graphics, game);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public void LoadContent()
        {
            // Set up the background
            titleTexture = game.Content.Load<Texture2D>("Spritesheets/CharSelect/charSelectBackground");

            titleImage = new ImageSprite(titleTexture, viewport.Width / 2, viewport.Height / 2, Color.White);
            titleImage.Visible = true;
            titleImage.Scale = (float)(new Vector2(viewport.Width, viewport.Height).Length()) / (float)(new Vector2(titleTexture.Width, titleTexture.Height).Length());

            // Set the control images
            buttonTexture = game.Content.Load<Texture2D>("Spritesheets/CharSelect/charSelectControls1");
            buttonPos = new Vector2(center.X, center.Y - viewport.Height * 0.365f);
            controlsImage = new ImageSprite(buttonTexture, (int)buttonPos.X, (int)buttonPos.Y, Color.White * (1f / 1));
            controlsImage.Scale = 1;

            // Initialize the sprite font
            spriteFont = game.Content.Load<SpriteFont>("SpriteFonts/Pescadero");


            playerOne = new CharSelectPlayer(new ImageSprite(game.Content.Load<Texture2D>("Spritesheets/CharSelect/charSelectPlayerOneCursor"), (int)buttonPos.X,
                                            (int)buttonPos.Y, selected), true, PlayerIndex.One);
            playerTwo = new CharSelectPlayer(new ImageSprite(game.Content.Load<Texture2D>("Spritesheets/CharSelect/charSelectPlayerTwoCursor"), (int)buttonPos.X,
                                            (int)buttonPos.Y, unselected), true, PlayerIndex.Two);
            playerThree = new CharSelectPlayer(new ImageSprite(game.Content.Load<Texture2D>("Spritesheets/CharSelect/charSelectPlayerThreeCursor"), (int)buttonPos.X,
                                            (int)buttonPos.Y, unselected), true, PlayerIndex.Three);
            playerFour = new CharSelectPlayer(new ImageSprite(game.Content.Load<Texture2D>("Spritesheets/CharSelect/charSelectPlayerFourCursor"), (int)buttonPos.X,
                                            (int)buttonPos.Y, unselected), true, PlayerIndex.Four);
            players.Add(playerOne);
            currentPlayer = playerOne;
            // Initialized the sound effect
            soundEffect = game.Content.Load<SoundEffect>("Audio/BClick_Menu");
        }

        /// <summary>
        /// Allows the game to run logic such as player input and character selection,
        /// also changes visual representation of selected/unselected characters
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            if (isConnected)
            {
                if (!isNewGame)
                {
                    // Allows the game to exit
                    InputHelper state = InputHelper.GetInput(currentPlayer.PlayerIndex);
                    if (state.IsHeld(Inputs.ESCAPE) 
                        && currentPlayer.timer <= 0)
                    {
                        if (currentPlayerIndex == 0)
                            game.Exit();
                        else
                        {
                            currentPlayerIndex--;
                            currentPlayer = players.ElementAt(currentPlayerIndex);
                            currentPlayer.Timer = controllerDelay;
                            currentPlayer.Selected = false;
                        }
                    }
                    if (InputHelper.GetInput(PlayerIndex.One).IsHeld(Inputs.ENTER))
                    {
                        playerOne.Connected = true;
                        if(!players.Contains(playerOne))
                            players.Add(playerOne);
                    }
                    if (InputHelper.GetInput(PlayerIndex.Two).IsHeld(Inputs.ENTER))
                    {
                        playerTwo.Connected = true;
                        if(!players.Contains(playerTwo))
                            players.Add(playerTwo);
                    }
                    if (InputHelper.GetInput(PlayerIndex.Three).IsHeld(Inputs.ENTER))
                    {
                        playerThree.Connected = true;
                        if(!players.Contains(playerThree))
                            players.Add(playerThree);
                    }
                    if (InputHelper.GetInput(PlayerIndex.Four).IsHeld(Inputs.ENTER))
                    {
                        playerFour.Connected = true;
                        if(!players.Contains(playerFour))
                            players.Add(playerFour);
                    }

#if XBOX 
                // If windows we don't want to disconnect the keyboard, which is player one
                if (!UserInput.GetInput(PlayerIndex.One).IsGamePadConnected()) 
                {
                    players.Remove(playerOne);
                    playerOne.Connected = false;
                }
#endif

                    if (!InputHelper.GetInput(PlayerIndex.Two).IsGamePadConnected())
                    {
                        players.Remove(playerTwo);
                        playerTwo.Connected = false;
                    }
                    if (!InputHelper.GetInput(PlayerIndex.Three).IsGamePadConnected())
                    {
                        players.Remove(playerThree);
                        playerThree.Connected = false;
                    }
                    if (!InputHelper.GetInput(PlayerIndex.Four).IsGamePadConnected())
                    {
                        players.Remove(playerFour);
                        playerFour.Connected = false;
                    }

                    if (currentPlayer.Connected)
                    {
                        currentPlayer.Timer -= (float)gameTime.ElapsedGameTime.Milliseconds;
                        if (!currentPlayer.Selected && currentPlayer.Timer <= 0)
                        {
                            currentPlayer.timer = controllerDelay;
                            if (state.IsHeld(Inputs.DOWN))
                            {
                                selectedGameSave++;
                                if (selectedGameSave < gameSaves.Count)
                                {
                                    foreach (CharSelectPreview preview in gameSaves)
                                    {
                                        preview.MoveByOffset(-movementOffset);
                                        preview.Color = unselected;
                                    }
                                }
                                else
                                {
                                    selectedGameSave = 0;
                                    foreach (CharSelectPreview preview in gameSaves)
                                    {
                                        preview.MoveByOffset((movementOffset * (gameSaves.Count - 1)));
                                        preview.Color = unselected;
                                    }
                                }
                                cursorMoved = true;
                                gameSaves.ElementAt(selectedGameSave).Color = selected;
                            }
                            if (state.IsHeld(Inputs.UP))
                            {
                                selectedGameSave--;
                                if (selectedGameSave >= 0)
                                {
                                    foreach (CharSelectPreview preview in gameSaves)
                                    {
                                        preview.MoveByOffset((movementOffset));
                                        preview.Color = unselected;
                                    }
                                }
                                else
                                {
                                    selectedGameSave = gameSaves.Count - 1;
                                    foreach (CharSelectPreview preview in gameSaves)
                                    {
                                        preview.MoveByOffset(-(movementOffset * (gameSaves.Count - 1)));
                                        preview.Color = unselected;
                                    }
                                }
                                cursorMoved = true;
                                gameSaves.ElementAt(selectedGameSave).Color = selected;
                            }

                            if (state.IsHeld(Keys.Space, Buttons.A))
                            {
                                currentPlayer.Timer = controllerDelay;
                                if (selectedGameSave == 0)
                                {
                                    isNewGame = true;
                                    CharacterSelectionScreen.selectionDone = false;
                                    currentPlayer.GameSave = gameSaves.ElementAt(selectedGameSave);
                                    //currentPlayer.GameSave.FileNumber = gameSaves.Count;
                                    currentPlayer.GameSave.NewGame = true;
                                    CharacterSelectionScreen.LoadContent(currentPlayer);
                                }
                                else
                                {
                                    currentPlayer.GameSave = gameSaves.ElementAt(selectedGameSave);
                                    currentPlayer.Selected = true;
                                    currentPlayer.Cursor.Color = unselected;
                                    currentPlayerIndex++;
                                    // If we are on the last player in the wait list, go to the networking screen
                                    if (currentPlayerIndex == players.Count)
                                        GoToNetworking();
                                    else
                                    {
                                        gameSaves.Remove(currentPlayer.GameSave);
                                        currentPlayer = players.ElementAt(currentPlayerIndex);
                                        currentPlayer.Selected = false;
                                        currentPlayer.Cursor.Color = selected;
                                        ReloadGameSaves();
                                        currentPlayer.Timer = controllerDelay;
                                    }
                                }
                            }
                            if (state.IsHeld(Inputs.ESCAPE))
                            {
                                currentPlayer.Timer = controllerDelay;
                                if (selectedGameSave != 0)
                                {
                                    DoGameDelete(LoadGameSave(gameSaves.ElementAt(selectedGameSave)));
                                }
                            }
                        }
                    }

                    // If there has been a cursor movement, play the sound
                    if (cursorMoved)
                    {
                        soundEffect.Play();
                        cursorMoved = false;
                    }
                }
                else
                {
                    CharacterSelectionScreen.Update(gameTime);
                    if (CharacterSelectionScreen.selectionDone)
                    {
                        isNewGame = false;
                        CharacterSelectionScreen.selectionDone = false;
                        if (currentPlayer.Selected)
                        {
                            currentPlayer.Cursor.Color = unselected;
                            currentPlayerIndex++;
                            // If we are on the last player in the wait list, go to the networking screen
                            if (currentPlayerIndex == players.Count)
                                GoToNetworking();
                            else
                            {
                                gameSaves.Remove(currentPlayer.GameSave);
                                currentPlayer = players.ElementAt(currentPlayerIndex);
                                currentPlayer.Selected = false;
                                currentPlayer.Cursor.Color = selected;
                                ReloadGameSaves();
                                currentPlayer.Timer = controllerDelay;
                            }
                        }
                    }
                }
            }
        }

        public void loadGameSaves()
        {
            // Create the "New Game" slot
            CharSelectPreview charPreview = new CharSelectPreview(game.Content.Load<Texture2D>("Spritesheets/CharSelect/charSelectNewGame"), gameSavePosition, spriteFont, "New Game", " ", selected, "");
            gameSaves.Add(charPreview);

            IAsyncResult result = StorageDevice.BeginShowSelector(PlayerIndex.One, null, null);
            StorageDevice device = StorageDevice.EndShowSelector(result);

            DungeonCrawlerGame.MasterSaveFile masterSaveFile;
            List<DungeonCrawlerGame.CharacterSaveFilePreview> listCSF = new List<DungeonCrawlerGame.CharacterSaveFilePreview>();

            if (device != null && device.IsConnected)
            {
                masterSaveFile = DungeonCrawlerGame.GetMasterSaveFile(device);
                if (masterSaveFile.charFiles == null)
                {
                    charPreview.FileNumber = 1;
                    charPreview.FileName = "CharSave1";
                }
                else
                {
                    charPreview.FileNumber = masterSaveFile.charFiles.Count + 1;
                    charPreview.FileName = "charSave" + charPreview.FileNumber;
                    //masterSaveFile.charFiles.Clear();
                    //DungeonCrawlerGame.SaveMasterFile(device, masterSaveFile);

                    foreach (DungeonCrawlerGame.CharacterSaveFilePreview csf in masterSaveFile.charFiles)
                    {
                        if (csf.charSprite == "" || csf.charSprite == null)
                        {
                            masterSaveFile.charFiles.Remove(csf);
                            DungeonCrawlerGame.SaveMasterFile(device, masterSaveFile);
                            break;
                        }
                        else
                        {
                            charPreview = LoadPreview(csf);
                            gameSaves.Add(charPreview);
                        }
                    }
                }
            }
        }

        public CharSelectPreview LoadPreview(DungeonCrawlerGame.CharacterSaveFilePreview saveFilePreview)
        {
            CharSelectPreview charSelectPreview;
            Texture2D charSprite = game.Content.Load<Texture2D>(saveFilePreview.charSprite);
            string charSaveFile = saveFilePreview.CharacterSaveFile;
            string charType = saveFilePreview.characterType;
            string level = "Level: " + saveFilePreview.Level.ToString();
            Vector2 position = gameSavePosition + (movementOffset * gameSaves.Count);
            charSelectPreview = new CharSelectPreview(charSprite, position, spriteFont, charType, level, unselected, charSaveFile);
            return charSelectPreview;
        }

        public DungeonCrawlerGame.CharacterSaveFile LoadGameSave(CharSelectPreview preview)
        {
            IAsyncResult result = StorageDevice.BeginShowSelector(PlayerIndex.One, null, null);
            StorageDevice device = StorageDevice.EndShowSelector(result);

            if (device != null && device.IsConnected)
            {
                return DungeonCrawlerGame.DoLoadGame(device, preview.FileName);
            }
            return new DungeonCrawlerGame.CharacterSaveFile();
        }

        public void DoGameSave(DungeonCrawlerGame.CharacterSaveFile gameSave)
        {
            IAsyncResult result = StorageDevice.BeginShowSelector(PlayerIndex.One, null, null);
            StorageDevice device = StorageDevice.EndShowSelector(result);

            if (device != null && device.IsConnected)
            {
                DungeonCrawlerGame.DoSaveGame(device, gameSave, true);
            }
        }

        public void DoGameDelete(DungeonCrawlerGame.CharacterSaveFile gameSave)
        {
            IAsyncResult result = StorageDevice.BeginShowSelector(PlayerIndex.One, null, null);
            StorageDevice device = StorageDevice.EndShowSelector(result);
            CharSelectPreview preview;

            if (device != null && device.IsConnected)
            {
                DungeonCrawlerGame.DoDeleteGame(device, gameSave);
                gameSaves.Clear();
                loadGameSaves();
                if ((gameSaves.Count - 1) < selectedGameSave)
                    selectedGameSave = (gameSaves.Count - 1);
                for (int i = 0; i < gameSaves.Count; i++)
                {
                    preview = gameSaves.ElementAt(i);
                    preview.SetPosition(gameSavePosition + (movementOffset * (i - selectedGameSave)));
                    if (i == selectedGameSave)
                        preview.Color = selected;
                    else
                        preview.Color = unselected;
                }
            }
        }

        public void ReloadGameSaves()
        {
            CharSelectPreview preview;
            // Check to see if the previous player chose new game, if so, make a new "New Game" slot with an incremented file name
            CharSelectPlayer previousPlayer = players.ElementAt(currentPlayerIndex - 1);
            if (previousPlayer.GameSave.NewGame)
            {
                preview = new CharSelectPreview(game.Content.Load<Texture2D>("Spritesheets/CharSelect/charSelectNewGame"), gameSavePosition, spriteFont, "New Game", " ", selected, "");
                preview.FileNumber = previousPlayer.GameSave.FileNumber + 1;
                preview.FileName = "charSave" + preview.FileNumber;
                gameSaves.Insert(0, preview);
            }

            // Reload the game saves
            for (int i = 0; i < gameSaves.Count; i++)
            {
                preview = gameSaves.ElementAt(i);
                preview.SetPosition(gameSavePosition + (movementOffset * i));
                if (i == 0)
                    preview.Color = selected;
                else
                    preview.Color = unselected;
            }

            selectedGameSave = 0;
        }

        public void GoToNetworking()
        {
            foreach (CharSelectPlayer player in players)
            {
                DungeonCrawlerGame.CharacterSaveFile gameSave;
                uint entityID;
                if (player.GameSave.NewGame)
                {
                    entityID = game.AggregateFactory.CreateFromAggregate((Aggregate)player.GameSave.Aggregate, player.PlayerIndex, player.GameSave.FileName, out gameSave);
                    gameSave.characterType = player.GameSave.CharType.Text;
                    gameSave.charSprite = "Spritesheets/CharSelect/charSelect" + gameSave.characterType.Replace(" ", "");
                    DoGameSave(gameSave);
                }
                else
                {
                    gameSave = LoadGameSave(player.GameSave);
                    entityID = game.AggregateFactory.CreateFromGameSave(gameSave, currentPlayer.PlayerIndex);
                }
                //Author: Josh Zavala - Assignment 9
                //This has been moved to AggregateFactory.CreateFromAggregate.
                //This allows us to have different default weapons for each class
                //without having to write a lot of needless switch statements.
                //Equipment e = new Equipment()
                //{
                //    EntityID = entityID,
                //    WeaponID = game.WeaponFactory.CreateWeapon(WeaponType.StandardSword),
                //};
                //game.EquipmentComponent.Add(e.EntityID, e);
            }
            game.GameState = GameState.NetworkSetup;
        }

        /// <summary>
        /// Renders the character selection screen, when appropriate
        /// </summary>
        /// <param name="elapsedTime">The time between this and the previous frame</param>
        public void Draw(float elapsedTime)
        {
            if (game.GameState == GameState.CharacterSelection)
            {
                if (isNewGame)
                    CharacterSelectionScreen.Draw(elapsedTime);
                else
                {
                    spriteBatch.Begin(SpriteSortMode.FrontToBack, null, SamplerState.PointClamp, null, null, null);

                    // Draw the background
                    spriteBatch.Draw(titleImage.Image, titleImage.Position, new Rectangle?(), titleImage.Color, 0, titleImage.Origin, 1, SpriteEffects.None, 0);

                    // Draw the controls image
                    spriteBatch.Draw(controlsImage.Image, controlsImage.Position, new Rectangle?(), controlsImage.Color, 0, controlsImage.Origin, 1, SpriteEffects.None, 1);

                    // Draw the character images and texts
                    foreach (CharSelectPreview preview in gameSaves)
                    {
                        preview.Draw(spriteBatch);
                    }

                    // Draw the player cursors for the connected players
                    float scale;
                    for (int i = 0; i < players.Count; i++)
                    {
                        if (i == currentPlayerIndex) scale = 1.5f;
                        else scale = 1;
                        spriteBatch.Draw(players[i].Cursor.Image, cursorPositions[i], new Rectangle?(), players[i].Cursor.Color, 0, players[i].Cursor.Origin, scale, SpriteEffects.None, 1);
                    }

                    spriteBatch.End();
                }
            }
        }
    }
}
