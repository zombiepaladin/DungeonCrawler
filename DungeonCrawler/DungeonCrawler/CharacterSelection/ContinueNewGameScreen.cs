#region File Description
//-----------------------------------------------------------------------------
// ContinueNewGameScreen.cs 
//
// Author: Joseph Shaw
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

        private Vector2 movementOffset = new Vector2(0,100);
        private int selectedGameSave = 0;

        private List<CharSelectPreview> gameSaves;

        public bool isConnected = false;

        public bool isNewGame = false;
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

            // Initialize the sprite arrays
            buttons = new ImageSprite[2, 3];
            buttonTexts = new TextSprite[2, 3];
            buttonAggregates = new Aggregate[2, 3];

            // Initialize the sprite batch
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

            gameSaves = new List<CharSelectPreview>();
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public void LoadContent()
        {
            // Set up the background
            titleTexture = game.Content.Load<Texture2D>("Spritesheets/charSelectBackground");

            titleImage = new ImageSprite(titleTexture, viewport.Width / 2, viewport.Height / 2, Color.White);
            titleImage.Visible = true;
            titleImage.Scale = (float)(new Vector2(viewport.Width, viewport.Height).Length()) / (float)(new Vector2(titleTexture.Width, titleTexture.Height).Length());

            // Set the control images
            buttonTexture = game.Content.Load<Texture2D>("Spritesheets/charSelectControls1");
            buttonPos = new Vector2(center.X, center.Y - viewport.Height * 0.365f);
            controlsImage = new ImageSprite(buttonTexture, (int)buttonPos.X, (int)buttonPos.Y, Color.White * (1f / 1));
            controlsImage.Scale = 1;

            // Initialize the sprite font
            spriteFont = game.Content.Load<SpriteFont>("SpriteFonts/Pescadero");


            playerOne = new CharSelectPlayer(new ImageSprite(game.Content.Load<Texture2D>("Spritesheets/charSelectPlayerOneCursor"), (int)buttonPos.X,
                                            (int)buttonPos.Y, Color.White), true, PlayerIndex.One);
            playerTwo = new CharSelectPlayer(new ImageSprite(game.Content.Load<Texture2D>("Spritesheets/charSelectPlayerOneCursor"), (int)buttonPos.X,
                                            (int)buttonPos.Y, Color.White), true, PlayerIndex.Two);
            playerThree = new CharSelectPlayer(new ImageSprite(game.Content.Load<Texture2D>("Spritesheets/charSelectPlayerOneCursor"), (int)buttonPos.X,
                                            (int)buttonPos.Y, Color.White), true, PlayerIndex.Three);
            playerFour = new CharSelectPlayer(new ImageSprite(game.Content.Load<Texture2D>("Spritesheets/charSelectPlayerOneCursor"), (int)buttonPos.X,
                                            (int)buttonPos.Y, Color.White), true, PlayerIndex.Four);
            players.Add(playerOne);
            players.Add(playerTwo);
            players.Add(playerThree);
            players.Add(playerFour);
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                game.GameState = GameState.SignIn;

            if (isConnected)
            {
                if ((GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter)) && !playerOne.connected)
                {
                    playerOne.connected = true;
                    playerOne.yPos = 0;
                    playerOne.xPos = 0;
                    playerOne.cursor.Position = buttons[playerOne.yPos, playerOne.xPos].Position;
                }
                if (GamePad.GetState(PlayerIndex.Two).Buttons.Start == ButtonState.Pressed && !playerTwo.connected)
                {
                    playerTwo.connected = true;
                    playerTwo.yPos = 0;
                    playerTwo.xPos = 0;
                    playerTwo.cursor.Position = buttons[playerOne.yPos, playerOne.xPos].Position;
                }
                if (GamePad.GetState(PlayerIndex.Three).Buttons.Start == ButtonState.Pressed && !playerThree.connected)
                {
                    playerThree.connected = true;
                    playerThree.yPos = 0;
                    playerThree.xPos = 0;
                    playerThree.cursor.Position = buttons[playerOne.yPos, playerOne.xPos].Position;
                }
                if (GamePad.GetState(PlayerIndex.Four).Buttons.Start == ButtonState.Pressed && !playerFour.connected)
                {
                    playerFour.connected = true;
                    playerFour.yPos = 0;
                    playerFour.xPos = 0;
                    playerFour.cursor.Position = buttons[playerOne.yPos, playerOne.xPos].Position;
                }

#if XBOX 
            // If windows we don't want to disconnect the keyboard, which is player one
            if (!GamePad.GetState(PlayerIndex.One).IsConnected) playerOne.connected = false;
#endif

                if (!GamePad.GetState(PlayerIndex.Two).IsConnected) playerTwo.connected = false;
                if (!GamePad.GetState(PlayerIndex.Three).IsConnected) playerThree.connected = false;
                if (!GamePad.GetState(PlayerIndex.Four).IsConnected) playerFour.connected = false;

                foreach (CharSelectPlayer player in players)
                {

                    if (player.connected)
                    {
                        player.timer -= (float)gameTime.ElapsedGameTime.Milliseconds;
                        if (!player.selected && player.timer <= 0)
                        {
                            player.timer = controllerDelay;
                            if (GamePad.GetState(player.playerIndex).DPad.Down == ButtonState.Pressed ||
                               (player.playerIndex == PlayerIndex.One && Keyboard.GetState().IsKeyDown(Keys.Down)))
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
                            if (GamePad.GetState(player.playerIndex).DPad.Up == ButtonState.Pressed ||
                               (player.playerIndex == PlayerIndex.One && Keyboard.GetState().IsKeyDown(Keys.Up)))
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
                        }
                        if (GamePad.GetState(player.playerIndex).Buttons.A == ButtonState.Pressed
                                || (player.playerIndex == PlayerIndex.One && (Keyboard.GetState().IsKeyDown(Keys.A))))
                        {
                            player.selected = true;
                            player.cursor.Color = Color.White * selectedCursorAlpha;
                        }
                        if (GamePad.GetState(player.playerIndex).Buttons.B == ButtonState.Pressed
                                || (player.playerIndex == PlayerIndex.One && (Keyboard.GetState().IsKeyDown(Keys.B))))
                        {
                            player.selected = false;
                            player.cursor.Color = Color.White;
                        }

                        if (GamePad.GetState(player.playerIndex).Buttons.Start == ButtonState.Pressed
                                || (player.playerIndex == PlayerIndex.One && (Keyboard.GetState().IsKeyDown(Keys.Enter))))
                        {
                            // Check to see if all of the connected players (at least one) have selected a character and if so, create the selected players and move to the network setup stage
                            if ((!playerOne.connected || playerOne.selected) && (!playerTwo.connected || playerTwo.selected) &&
                                (!playerThree.connected || playerThree.selected) && (!playerFour.connected || playerFour.selected) &&
                                (playerOne.selected || playerTwo.selected || playerThree.selected || playerFour.selected))
                            {
                                GoToNetworking();
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
        }

        public void loadGameSaves()
        {
            CharSelectPreview charPreview = new CharSelectPreview(game.Content.Load<Texture2D>("Spritesheets/charSelectCyborg"), gameSavePosition, spriteFont, "New Game", " ", selected, "");
            gameSaves.Add(charPreview);
            IAsyncResult result = StorageDevice.BeginShowSelector(PlayerIndex.One, null, null); ;

            //Object stateobj = (Object)"GetDevice for Player One";
            //StorageDevice.BeginShowSelector(PlayerIndex.One, game.GetDevice, stateobj);
            StorageDevice device = StorageDevice.EndShowSelector(result);
            DungeonCrawlerGame.MasterSaveFile masterSaveFile;
            List<DungeonCrawlerGame.CharacterSaveFilePreview> listCSF = new List<DungeonCrawlerGame.CharacterSaveFilePreview>();

            if (device != null && device.IsConnected)
            {
                masterSaveFile = DungeonCrawlerGame.GetMasterSaveFile(device);
                if (masterSaveFile.charFiles == null)
                    charPreview.fileName = "CharSave1";
                else
                {
                    charPreview.fileName = "charSave" + masterSaveFile.charFiles.Count;
                    foreach (DungeonCrawlerGame.CharacterSaveFilePreview csf in masterSaveFile.charFiles)
                    {
                        charPreview = LoadPreview(csf);
                        gameSaves.Add(charPreview);
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

        public void GoToNetworking()
        {
            foreach (CharSelectPlayer player in players)
            {
                if (player.connected && player.selected)
                    game.AggregateFactory.CreateFromAggregate(buttonAggregates[player.yPos, player.xPos], player.playerIndex);
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
                foreach (CharSelectPlayer player in players)
                {
                    if (player.connected)
                        spriteBatch.Draw(player.cursor.Image, player.cursor.Position, new Rectangle?(), player.cursor.Color, 0, player.cursor.Origin, 1, SpriteEffects.None, 1);
                }

                spriteBatch.End();
            }
        }
    }
}
