#region File Description
//-----------------------------------------------------------------------------
// CharacterSelectionScreen.cs 
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
    public class CharacterSelectionScreen
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

        // The sprite font for the text sprite
        private SpriteFont spriteFont;

        // List of the image and text sprites and the character's aggregate
        private ImageSprite[,] buttons;
        private TextSprite[,] buttonTexts;
        private Aggregate[,] buttonAggregates;

        // Viewport and its center
        private Viewport viewport;
        private Vector2 center;

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
        #endregion

        /// <summary>
        /// The constructor for the CharacterSelectionScreen
        /// </summary>
        public CharacterSelectionScreen(GraphicsDeviceManager graphics, DungeonCrawlerGame game)
        {
            // Get the game reference
            this.game = game;

            // Set up the viewport vars
            viewport = graphics.GraphicsDevice.Viewport;
            center = new Vector2(viewport.Width / 2, viewport.Height / 2);

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

            // Initialize the sprite font
            spriteFont = game.Content.Load<SpriteFont>("SpriteFonts/Pescadero");

            // Initialize the character buttons and the text for each button and the character's aggregate
            // The Cultist Character
            buttonTexture = game.Content.Load<Texture2D>("Spritesheets/charSelectGargranian");
            buttonPos = new Vector2(center.X - viewport.Width / 4, center.Y - viewport.Height / 6);
            buttonImage = new ImageSprite(buttonTexture, (int)buttonPos.X, (int)buttonPos.Y, Color.White * (1f / 1));
            buttonImage.Scale = 1;
            buttons[0, 0] = buttonImage;

            buttonTextOffset = new Vector2(0, buttonImage.Image.Height);
            buttonText = new TextSprite(spriteFont, "Cultist", buttonPos + buttonTextOffset, Color.White);
            buttonTexts[0, 0] = buttonText;

            buttonAggregates[0, 0] = Aggregate.CultistPlayer;

            // The Earthian Character
            buttonTexture = game.Content.Load<Texture2D>("Spritesheets/charSelectGargranian");
            buttonPos = new Vector2(center.X, center.Y - viewport.Height / 6);
            buttonImage = new ImageSprite(buttonTexture, (int)buttonPos.X, (int)buttonPos.Y, Color.White * (1f / 1));
            buttonImage.Scale = 1;
            buttons[0, 1] = buttonImage;

            buttonText = new TextSprite(spriteFont, "Earthian", buttonPos + buttonTextOffset, Color.White);
            buttonTexts[0, 1] = buttonText;

            buttonAggregates[0, 1] = Aggregate.EarthianPlayer;

            // The Cyborg Character
            buttonTexture = game.Content.Load<Texture2D>("Spritesheets/charSelectGargranian");
            buttonPos = new Vector2(center.X + viewport.Width / 4, center.Y - viewport.Height / 6);
            buttonImage = new ImageSprite(buttonTexture, (int)buttonPos.X, (int)buttonPos.Y, Color.White * (1f / 1));
            buttonImage.Scale = 1;
            buttons[0, 2] = buttonImage;

            buttonText = new TextSprite(spriteFont, "Cyborg", buttonPos + buttonTextOffset, Color.White);
            buttonTexts[0, 2] = buttonText;

            buttonAggregates[0, 2] = Aggregate.CyborgPlayer;

            // The Gargranian Character
            buttonTexture = game.Content.Load<Texture2D>("Spritesheets/charSelectGargranian");
            buttonPos = new Vector2(center.X - viewport.Width / 4, center.Y + viewport.Height / 6);
            buttonImage = new ImageSprite(buttonTexture, (int)buttonPos.X, (int)buttonPos.Y, Color.White * (1f / 1));
            buttonImage.Scale = 1;
            buttons[1, 0] = buttonImage;

            buttonText = new TextSprite(spriteFont, "Gargranian", buttonPos + buttonTextOffset, Color.White);
            buttonTexts[1, 0] = buttonText;

            buttonAggregates[1, 0] = Aggregate.GargranianPlayer;

            // The Space Pirate Character
            buttonTexture = game.Content.Load<Texture2D>("Spritesheets/charSelectGargranian");
            buttonPos = new Vector2(center.X, center.Y + viewport.Height / 6);
            buttonImage = new ImageSprite(buttonTexture, (int)buttonPos.X, (int)buttonPos.Y, Color.White * (1f / 1));
            buttonImage.Scale = 1;
            buttons[1, 1] = buttonImage;

            buttonText = new TextSprite(spriteFont, "Space Pirate", buttonPos + buttonTextOffset, Color.White);
            buttonTexts[1, 1] = buttonText;

            buttonAggregates[1, 1] = Aggregate.SpacePiratePlayer;

            // The Zombie Character
            buttonTexture = game.Content.Load<Texture2D>("Spritesheets/charSelectGargranian");
            buttonPos = new Vector2(center.X + viewport.Width / 4, center.Y + viewport.Height / 6);
            buttonImage = new ImageSprite(buttonTexture, (int)buttonPos.X, (int)buttonPos.Y, Color.White * (1f / 1));
            buttonImage.Scale = 1;
            buttons[1, 2] = buttonImage;


            buttonText = new TextSprite(spriteFont, "Zombie", buttonPos + buttonTextOffset, Color.White);
            buttonTexts[1, 2] = buttonText;

            buttonAggregates[1, 2] = Aggregate.ZombiePlayer;

            playerOne = new CharSelectPlayer(new ImageSprite(game.Content.Load<Texture2D>("Spritesheets/charSelectPlayerOneCursor"), (int)buttons[0, 0].Position.X,
                                            (int)buttons[0, 0].Position.Y, Color.White), true, PlayerIndex.One);
            playerTwo = new CharSelectPlayer(new ImageSprite(game.Content.Load<Texture2D>("Spritesheets/charSelectPlayerTwoCursor"), (int)buttons[0, 0].Position.X,
                                            (int)buttons[0, 0].Position.Y, Color.White), false, PlayerIndex.Two);
            playerThree = new CharSelectPlayer(new ImageSprite(game.Content.Load<Texture2D>("Spritesheets/charSelectPlayerThreeCursor"), (int)buttons[0, 0].Position.X,
                                            (int)buttons[0, 0].Position.Y, Color.White), false, PlayerIndex.Three);
            playerFour = new CharSelectPlayer(new ImageSprite(game.Content.Load<Texture2D>("Spritesheets/charSelectPlayerFourCursor"), (int)buttons[0, 0].Position.X,
                                            (int)buttons[0, 0].Position.Y, Color.White), false, PlayerIndex.Four);
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
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    game.Exit();

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
                        if (GamePad.GetState(player.playerIndex).DPad.Down == ButtonState.Pressed || GamePad.GetState(player.playerIndex).DPad.Up == ButtonState.Pressed
                            || (player.playerIndex == PlayerIndex.One && (Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.Up))))
                        {
                            player.MoveUpDown();
                            player.cursor.Position = buttons[player.yPos, player.xPos].Position;
                            player.timer = controllerDelay;
                            cursorMoved = true;
                        }
                        if (GamePad.GetState(player.playerIndex).DPad.Left == ButtonState.Pressed
                            || (player.playerIndex == PlayerIndex.One && (Keyboard.GetState().IsKeyDown(Keys.Left))))
                        {
                            player.MoveLeft();
                            player.cursor.Position = buttons[player.yPos, player.xPos].Position;
                            player.timer = controllerDelay;
                            cursorMoved = true;
                        }
                        if (GamePad.GetState(player.playerIndex).DPad.Right == ButtonState.Pressed
                            || (player.playerIndex == PlayerIndex.One && (Keyboard.GetState().IsKeyDown(Keys.Right))))
                        {
                            player.MoveRight();
                            player.cursor.Position = buttons[player.yPos, player.xPos].Position;
                            player.timer = controllerDelay;
                            cursorMoved = true;
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
            
            // Update the color of the character images if the are/are not selected
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if ((playerOne.yPos == i && playerOne.xPos == j && playerOne.connected) || (playerTwo.yPos == i && playerTwo.xPos == j && playerTwo.connected) ||
                        (playerThree.yPos == i && playerThree.xPos == j && playerThree.connected) || (playerFour.yPos == i && playerFour.xPos == j && playerFour.connected))
                        buttons[i, j].Color = selected;
                    else
                        buttons[i, j].Color = unselected;
                }
            }

            // If there has been a cursor movement, play the sound
            if (cursorMoved)
            {
                soundEffect.Play();
                cursorMoved = false;
            }
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

                // Draw the character images and texts
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        spriteBatch.Draw(buttons[i, j].Image, buttons[i, j].Position, new Rectangle?(), buttons[i, j].Color, 0, buttons[i,j].Origin, 1,SpriteEffects.None, 1);
                        spriteBatch.DrawString(buttonTexts[i, j].Font, buttonTexts[i, j].Text, buttonTexts[i, j].Position, buttonTexts[i, j].Color,0, buttonTexts[i,j].Origin,1,SpriteEffects.None, 1);
                    }
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
