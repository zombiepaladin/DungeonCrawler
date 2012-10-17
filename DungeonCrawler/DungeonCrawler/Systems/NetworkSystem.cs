#region File Description
//-----------------------------------------------------------------------------
// NetworkSystem.cs 
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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;
using DungeonCrawler.Components;
using Microsoft.Xna.Framework.Audio;
using System.ComponentModel;
#endregion

namespace DungeonCrawler.Systems
{
    /// <summary>
    /// This class embodies a system for managing network play,
    /// coordinating elements created on multiple consoles
    /// </summary>
    public class NetworkSystem
    {
        #region Private and Protected Members

        /// <summary>
        /// The enum for the network selection
        /// </summary>
        private enum NetworkMenuState
        {
            SelectMode = 0,
            JoinSession,
            CreateSession
        };

        /// <summary>
        /// The enum for the Join Session Options
        /// </summary>
        private enum SelectSessionState
        {
            Join = 0,
            Create
        };

        /// <summary>
        /// The enum for the Join Session Options
        /// </summary>
        private enum JoinSessionState
        {
            //SetName = 0,
            Start
        };

        /// <summary>
        /// The enum for the Create Session Options
        /// </summary>
        private enum CreateSessionState
        {
            SelectSession = 0,
            Start
        };

        /// <summary>
        /// The holder for the main enum values
        /// </summary>
        private NetworkMenuState menuState = 0;

        /// <summary>
        /// The menu select sound effect
        /// </summary>
        private SoundEffect menuSelectSound;

        /// <summary>
        /// The holder for the session enum values
        /// </summary>
        private int menuSessionState = 0;

        /// <summary>
        /// The timer for the searching available sessions text
        /// </summary>
        private float searchingtimer = 0;

        /// <summary>
        /// Background worker to find the available sessions
        /// </summary>
        private BackgroundWorker backgroundWorker;

        /// <summary>
        /// Bool explaining if we're searching or not.
        /// </summary>
        private bool searchingForAvailableSessions;

        /// <summary>
        /// The spritefont for the text to draw
        /// </summary>
        private SpriteFont spriteFont;

        /// <summary>
        /// The sprite for the option selector
        /// </summary>
        private Texture2D menuSprite;

        /// <summary>
        /// The location of the option selector
        /// </summary>
        private Vector2 menuSpriteLocation;

        /// <summary>
        /// Struct with bool values for pressed keys
        /// </summary>
        private struct PressedKeys
        {
            public bool up;
            public bool down;
            public bool left;
            public bool right;
            public bool space;
            public bool b;
        };

        /// <summary>
        /// Holder for pressed keys
        /// </summary>
        private PressedKeys pressedKeys;
        
        /// <summary>
        /// The sessions available for joining
        /// </summary>
        AvailableNetworkSessionCollection availableSessions;

        /// <summary>
        /// The selected session
        /// </summary>
        int selectedSession = 0;

        /// <summary>
        /// The name of the selected session
        /// </summary>
        private string currentSessionName;

        /// <summary>
        /// Spritebatch, need a different one instead?
        /// </summary>
        private SpriteBatch spriteBatch;

        /// <summary>
        /// Contains the locations of each text
        /// </summary>
        private Vector2[] TextLocations;

        /// <summary>
        /// The game this system belongs to
        /// </summary>
        DungeonCrawlerGame game;

        /// <summary>
        /// A network session used by this network system
        /// </summary>
        NetworkSession session;

        /// <summary>
        /// A packet writer used by this network session
        /// </summary>
        PacketWriter packetWriter = new PacketWriter();

        /// <summary>
        /// A packet reader used by this network session
        /// </summary>
        PacketReader packetReader = new PacketReader();

        #endregion

        #region Public Members

        /// <summary>
        /// True if this game is the current host
        /// </summary>
        public bool IsHost
        {
            get { return isHost; }
        }
        private bool isHost = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new NetworkSystem for a game
        /// </summary>
        /// <param name="game">The game this NetworkSystem belongs to</param>
        public NetworkSystem(DungeonCrawlerGame game)
        {
            this.game = game;

            menuSprite = game.Content.Load<Texture2D>("Spritesheets/menuSelect");
            spriteFont = game.Content.Load<SpriteFont>("Spritefonts/Pescadero");

            menuSelectSound = game.Content.Load<SoundEffect>("Sounds/MenuSelect");

            this.spriteBatch = new SpriteBatch(game.GraphicsDevice);

            searchingForAvailableSessions = false;

            currentSessionName = "No Sessions Found";

            TextLocations = new Vector2[] {
                new Vector2(600, 230), //Select Mode
                new Vector2(400, 330), //Create Session
                new Vector2(700, 330), //Join Session
                new Vector2(400, 530), //Create Go
                new Vector2(700, 430), //Session Name
                new Vector2(700, 530) //Join Go
            };

            menuSpriteLocation = TextLocations[1] - new Vector2(10, 0);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the NetworkSystem to 
        /// 1) Create and maintain the network session
        /// 2) update all networked element's components and 
        /// 3) send local element's component's updates to remote systems
        /// </summary>
        /// <param name="elapsedTime">
        /// The time elapsed between this and the previous frame
        /// </param>
        public void Update(float elapsedTime)
        {
            if (searchingForAvailableSessions)
            {
                searchingtimer += elapsedTime;
                searchingtimer %= 1;
                return;
            }

            if (game.GameState == GameState.NetworkSetup)
            {
                switch (menuState)
                {
                    case NetworkMenuState.SelectMode:
                        if (menuSessionState == 1)
                        {
                            if (Keyboard.GetState().IsKeyDown(Keys.Left) ||
                                GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X <= -1 ||
                                GamePad.GetState(PlayerIndex.Two).ThumbSticks.Left.X <= -1 ||
                                GamePad.GetState(PlayerIndex.Three).ThumbSticks.Left.X <= -1 ||
                                GamePad.GetState(PlayerIndex.Four).ThumbSticks.Left.X <= -1)
                            {
                                if (!pressedKeys.left)
                                {
                                    menuSessionState--;
                                    menuSpriteLocation = TextLocations[1] - new Vector2(10, 0);
                                    pressedKeys.left = true;


                                    menuSelectSound.Play();
                                    break;
                                }
                            }
                            else
                            {
                                pressedKeys.left = false;
                            }

                            if (Keyboard.GetState().IsKeyDown(Keys.Space) ||
                                GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A) ||
                                GamePad.GetState(PlayerIndex.Two).IsButtonDown(Buttons.A) ||
                                GamePad.GetState(PlayerIndex.Three).IsButtonDown(Buttons.A) ||
                                GamePad.GetState(PlayerIndex.Four).IsButtonDown(Buttons.A))
                            {
                                if (!pressedKeys.space)
                                {

                                    try
                                    {
                                        menuSelectSound.Play();

                                        backgroundWorker = new BackgroundWorker();

                                        backgroundWorker.DoWork += 
                                            new DoWorkEventHandler(findAvailableSessions);
                                        backgroundWorker.RunWorkerCompleted += 
                                            new RunWorkerCompletedEventHandler(foundAvailableSessions);

                                        backgroundWorker.RunWorkerAsync();

                                        searchingForAvailableSessions = true;

                                        pressedKeys.space = true;
                                    }
                                    catch (Exception e)
                                    {
                                        //Throw error
                                    }
                                    break;
                                }
                            }
                            else
                            {
                                pressedKeys.space = false;
                            }
                        }

                        if (menuSessionState == 0)
                        {
                            if (Keyboard.GetState().IsKeyDown(Keys.Right) ||
                                GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X >= 1 ||
                                GamePad.GetState(PlayerIndex.Two).ThumbSticks.Left.X >= 1 ||
                                GamePad.GetState(PlayerIndex.Three).ThumbSticks.Left.X >= 1 ||
                                GamePad.GetState(PlayerIndex.Four).ThumbSticks.Left.X >= 1)
                            {
                                if(!pressedKeys.right)
                                {
                                    menuSessionState++;
                                    menuSpriteLocation = TextLocations[2] - new Vector2(10, 0);
                                    pressedKeys.right = true;

                                    menuSelectSound.Play();

                                    break;
                                }
                            }
                            else
                            {
                                pressedKeys.right = false;
                            }

                            if (Keyboard.GetState().IsKeyDown(Keys.Space) ||
                                GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A) ||
                                GamePad.GetState(PlayerIndex.Two).IsButtonDown(Buttons.A) ||
                                GamePad.GetState(PlayerIndex.Three).IsButtonDown(Buttons.A) ||
                                GamePad.GetState(PlayerIndex.Four).IsButtonDown(Buttons.A))
                            {
                                if (!pressedKeys.space)
                                {
                                    menuSelectSound.Play();

                                    menuState = NetworkMenuState.CreateSession;
                                    menuSpriteLocation = TextLocations[3] - new Vector2(10, 0);
                                    pressedKeys.space = true;

                                    break;
                                }
                            }
                            else
                            {
                                pressedKeys.space = false;
                            }

                        }

                        break;
                    case NetworkMenuState.JoinSession:

                        if (menuSessionState == 0) //select game
                        {
                            if (Keyboard.GetState().IsKeyDown(Keys.Down) ||
                                GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y >= 1 ||
                                GamePad.GetState(PlayerIndex.Two).ThumbSticks.Left.Y >= 1 ||
                                GamePad.GetState(PlayerIndex.Three).ThumbSticks.Left.Y >= 1 ||
                                GamePad.GetState(PlayerIndex.Four).ThumbSticks.Left.Y >= 1)
                            {
                                if (!pressedKeys.down)
                                {
                                    menuSessionState++;
                                    menuSpriteLocation = TextLocations[5] - new Vector2(10, 0);
                                    pressedKeys.down = true;


                                    menuSelectSound.Play();
                                    break;
                                }
                            }
                            else
                            {
                                pressedKeys.down = false;
                            }

                            if (Keyboard.GetState().IsKeyDown(Keys.Left) ||
                                GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X <= -1 ||
                                GamePad.GetState(PlayerIndex.Two).ThumbSticks.Left.X <= -1 ||
                                GamePad.GetState(PlayerIndex.Three).ThumbSticks.Left.X <= -1 ||
                                GamePad.GetState(PlayerIndex.Four).ThumbSticks.Left.X <= -1)
                            {
                                if (!pressedKeys.left)
                                {
                                    if (availableSessions.Count > 0)
                                    {
                                        selectedSession--;
                                        selectedSession %= availableSessions.Count;
                                        currentSessionName = string.Format("Session %s : %d / %d", 
                                            availableSessions[selectedSession].HostGamertag,
                                            availableSessions[selectedSession].CurrentGamerCount,
                                            availableSessions[selectedSession].CurrentGamerCount 
                                                + availableSessions[selectedSession].OpenPublicGamerSlots);

                                        menuSelectSound.Play();
                                    }
                                    pressedKeys.left = true;

                                    break;
                                }
                            }
                            else
                            {
                                pressedKeys.left = false;
                            }

                            if (Keyboard.GetState().IsKeyDown(Keys.Right) ||
                                GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X >= 1 ||
                                GamePad.GetState(PlayerIndex.Two).ThumbSticks.Left.X >= 1 ||
                                GamePad.GetState(PlayerIndex.Three).ThumbSticks.Left.X >= 1 ||
                                GamePad.GetState(PlayerIndex.Four).ThumbSticks.Left.X >= 1)
                            {
                                if (!pressedKeys.right)
                                {
                                    if (availableSessions.Count > 0)
                                    {
                                        selectedSession++;
                                        selectedSession %= availableSessions.Count;
                                        currentSessionName = string.Format("Session %s : %d / %d",
                                            availableSessions[selectedSession].HostGamertag,
                                            availableSessions[selectedSession].CurrentGamerCount,
                                            availableSessions[selectedSession].CurrentGamerCount
                                                + availableSessions[selectedSession].OpenPublicGamerSlots);

                                        menuSelectSound.Play();
                                    }
                                    pressedKeys.right = true;

                                    break;
                                }
                            }
                            else
                            {
                                pressedKeys.right = false;
                            }
                        }
                        else if (menuSessionState == 1) //go
                        {
                            if (Keyboard.GetState().IsKeyDown(Keys.Up) ||
                                GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y <= -1 ||
                                GamePad.GetState(PlayerIndex.Two).ThumbSticks.Left.Y <= -1 ||
                                GamePad.GetState(PlayerIndex.Three).ThumbSticks.Left.Y <= -1 ||
                                GamePad.GetState(PlayerIndex.Four).ThumbSticks.Left.Y <= -1)
                            {
                                if (!pressedKeys.up)
                                {
                                    menuSessionState--;
                                    menuSpriteLocation = TextLocations[4] - new Vector2(10, 0);
                                    pressedKeys.up = true;

                                    menuSelectSound.Play();
                                    break;
                                }
                            }
                            else
                            {
                                pressedKeys.up = false;
                            }

                            if (Keyboard.GetState().IsKeyDown(Keys.Space) ||
                                GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A) ||
                                GamePad.GetState(PlayerIndex.Two).IsButtonDown(Buttons.A) ||
                                GamePad.GetState(PlayerIndex.Three).IsButtonDown(Buttons.A) ||
                                GamePad.GetState(PlayerIndex.Four).IsButtonDown(Buttons.A))
                            {
                                if (!pressedKeys.space)
                                {
                                    menuSelectSound.Play();

                                    JoinSession();

                                    break;
                                }
                            }
                            else
                            {
                                pressedKeys.space = false;
                            }
                        }


                        if (Keyboard.GetState().IsKeyDown(Keys.B) ||
                                GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.B) ||
                                GamePad.GetState(PlayerIndex.Two).IsButtonDown(Buttons.B) ||
                                GamePad.GetState(PlayerIndex.Three).IsButtonDown(Buttons.B) ||
                                GamePad.GetState(PlayerIndex.Four).IsButtonDown(Buttons.B))
                        {
                            if (!pressedKeys.b)
                            {
                                menuSessionState = 1;
                                menuState = NetworkMenuState.SelectMode;
                                menuSpriteLocation = TextLocations[2] - new Vector2(10, 0);
                                pressedKeys.b = true;

                                menuSelectSound.Play();

                                break;
                            }
                        }
                        else
                        {
                            pressedKeys.b = false;
                        }

                        break;
                    case NetworkMenuState.CreateSession:
                        if (Keyboard.GetState().IsKeyDown(Keys.Space) ||
                                GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A) ||
                                GamePad.GetState(PlayerIndex.Two).IsButtonDown(Buttons.A) ||
                                GamePad.GetState(PlayerIndex.Three).IsButtonDown(Buttons.A) ||
                                GamePad.GetState(PlayerIndex.Four).IsButtonDown(Buttons.A))
                        {
                            if (!pressedKeys.space)
                            {

                                menuSelectSound.Play();
                                CreateSession();

                                break;
                            }
                        }
                        else
                        {
                            pressedKeys.space = false;
                        }

                        if (Keyboard.GetState().IsKeyDown(Keys.B) ||
                                GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.B) ||
                                GamePad.GetState(PlayerIndex.Two).IsButtonDown(Buttons.B) ||
                                GamePad.GetState(PlayerIndex.Three).IsButtonDown(Buttons.B) ||
                                GamePad.GetState(PlayerIndex.Four).IsButtonDown(Buttons.B))
                        {
                            if (!pressedKeys.b)
                            {
                                menuSessionState = 0;
                                menuState = NetworkMenuState.SelectMode;
                                menuSpriteLocation = TextLocations[1] - new Vector2(10, 0);
                                pressedKeys.b = true;

                                menuSelectSound.Play();

                                break;
                            }
                        }
                        else
                        {
                            pressedKeys.b = false;
                        }
                        break;
                }
            }
            else
            {
                // send local object updates
                SendLocalEntityUpdates();
                
                // Pump the session 
                session.Update();

                // Check for session ending
                if (session == null) game.GameState = GameState.NetworkSetup;

                // Read packets for remote updates
                RecieveRemoteEntityUpdates();
            }
        }

        /// <summary>
        /// Renders the network menu and messages, when appropriate
        /// </summary>
        /// <param name="elapsedTime">The time between this and the previous frame</param>
        public void Draw(float elapsedTime)
        {
            if (game.GameState == GameState.NetworkSetup)
            {
                // TODO: Render network menu
                spriteBatch.Begin();

                Color color;

                spriteBatch.DrawString(spriteFont, "Select Mode", TextLocations[0], Color.Black);

                if (menuState == NetworkMenuState.SelectMode || menuState == NetworkMenuState.CreateSession)
                    color = Color.Black;
                else
                    color = Color.Gray;
                spriteBatch.DrawString(spriteFont, "Create Session", TextLocations[1], color);

                if (menuState == NetworkMenuState.SelectMode || menuState == NetworkMenuState.JoinSession)
                    color = Color.Black;
                else
                    color = Color.Gray;
                spriteBatch.DrawString(spriteFont, "Join Session", TextLocations[2], color);

                if(searchingForAvailableSessions && searchingtimer <= .5)
                    spriteBatch.DrawString(spriteFont, "searching for sessions", 
                        TextLocations[2] + new Vector2(0, 50), Color.Black);

                if (menuState == NetworkMenuState.CreateSession)
                    color = Color.Black;
                else
                    color = Color.Gray;
                spriteBatch.DrawString(spriteFont, "Go!", TextLocations[3], color);

                if (menuState == NetworkMenuState.JoinSession)
                    color = Color.Black;
                else
                    color = Color.Gray;
                spriteBatch.DrawString(spriteFont, currentSessionName, TextLocations[4], color);
                spriteBatch.DrawString(spriteFont, "Go!", TextLocations[5], color);


                //Draw the sprite
                spriteBatch.Draw(menuSprite, menuSpriteLocation, Color.White);

                spriteBatch.End();

            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Helper method for creating a hosted network session
        /// </summary>
        private void CreateSession()
        {
            try
            {
                // Create the hosted network session
                session = NetworkSession.Create(
                    NetworkSessionType.SystemLink,  // Session Type
                    4,                              // Max Local Gamers
                    16                              // Max Gamers
                );

                // Set ourselves as host
                isHost = true;

                // Enable migration and join-in-progress
                session.AllowHostMigration = true;
                session.AllowJoinInProgress = true;

                // Set up the session event handlers
                HookSessonEvents();

                // Update our game state
                game.GameState = GameState.Gameplay;
            }
            catch (Exception e)
            {
                // TODO: Report Errors
            }
        }


        /// <summary>
        /// Helper method for joining a hosted network session
        /// </summary>
        private void JoinSession()
        {
            try
            {
                //using (availableSessions =
                  //  NetworkSession.Find(
                  //      NetworkSessionType.SystemLink,  // Session Type
                 //       4,                              // Max local gamers
               //         null                            // Search Properties
                //        ))
              //  {
                    if (availableSessions.Count == 0)
                    {
                        // No sessions available
                        // TODO: Offer feedback
                        return;
                    }

                    // Join the first session found
                    session = NetworkSession.Join(availableSessions[selectedSession]);

                    // Set ourselves as guest
                    isHost = false;

                    // Set up the session event handlers
                    HookSessonEvents();

                    // Update our game state
                    game.GameState = GameState.Gameplay;
               // }
            }
            catch (Exception e)
            {
                // TODO: Report Errors
            }
        }


        /// <summary>
        /// Sends updates on all entities under local authority
        /// </summary>
        void SendLocalEntityUpdates()
        {
            foreach (Local local in game.LocalComponent.All)
            {
                // Send position
                if (game.PositionComponent.Contains(local.EntityID))
                {
                    Position position = game.PositionComponent[local.EntityID];
                    packetWriter.Write(position.EntityID);
                    packetWriter.Write((short)PacketTypes.Position);
                    packetWriter.Write(position.Center);
                    packetWriter.Write(position.Radius);
                    session.LocalGamers[0].SendData(packetWriter, SendDataOptions.InOrder);
                }

                // Send sprite
                if (game.SpriteComponent.Contains(local.EntityID))
                {
                    Sprite sprite = game.SpriteComponent[local.EntityID];
                    packetWriter.Write(sprite.EntityID);
                    packetWriter.Write((short)PacketTypes.Sprite);
                    packetWriter.Write(sprite.SpriteSheet.Name);
                    packetWriter.Write(sprite.SpriteBounds.X);
                    packetWriter.Write(sprite.SpriteBounds.Y);
                    packetWriter.Write(sprite.SpriteBounds.Width);
                    packetWriter.Write(sprite.SpriteBounds.Height);
                    session.LocalGamers[0].SendData(packetWriter, SendDataOptions.InOrder);
                }

                // Send movement sprite
                if (game.MovementSpriteComponent.Contains(local.EntityID))
                {
                    MovementSprite sprite = game.MovementSpriteComponent[local.EntityID];
                    packetWriter.Write(sprite.EntityID);
                    packetWriter.Write((short)PacketTypes.MovementSprite);
                    packetWriter.Write(sprite.SpriteSheet.Name);
                    packetWriter.Write(sprite.SpriteBounds.X);
                    packetWriter.Write(sprite.SpriteBounds.Y);
                    packetWriter.Write(sprite.SpriteBounds.Width);
                    packetWriter.Write(sprite.SpriteBounds.Height);
                    session.LocalGamers[0].SendData(packetWriter, SendDataOptions.InOrder);
                }
            }
        }


        /// <summary>
        /// Update remote entity components based on network input
        /// TODO: Reloading the texture every frame may quickly 
        /// become cost-prohibitive we may need a better option
        /// </summary>
        void RecieveRemoteEntityUpdates()
        {
            // Each local gamer recieves network messages - so process them all
            foreach(LocalNetworkGamer gamer in session.LocalGamers)
            {
                // Process all waiting packets
                while (gamer.IsDataAvailable)
                {
                    NetworkGamer sender;

                    // Read a single packet from the network
                    gamer.ReceiveData(packetReader, out sender);

                    // Discard local packets - we already know local state
                    if (sender.IsLocal) continue;


                    // Look up the entity associated with this network packet
                    uint entityID = game.RemoteComponent.FindRemoteEntity(sender.Id, packetReader.ReadUInt32());
                    string textureName;

                    // process the packet
                    PacketTypes packetType = (PacketTypes)packetReader.ReadInt16();
                    switch (packetType)
                    {
                        case PacketTypes.Position:
                            Position position = new Position()
                            {
                                EntityID = entityID,
                                Center = packetReader.ReadVector2(),
                                Radius = packetReader.ReadSingle(),
                            };
                            game.PositionComponent[entityID] = position;
                            break;

                        case PacketTypes.Sprite:
                            Sprite sprite = new Sprite();
                            textureName = packetReader.ReadString();
                            sprite.EntityID = entityID;
                            sprite.SpriteSheet = game.Content.Load<Texture2D>(textureName);
                            sprite.SpriteSheet.Name = textureName;
                            sprite.SpriteBounds.X = packetReader.ReadInt32();
                            sprite.SpriteBounds.Y = packetReader.ReadInt32();
                            sprite.SpriteBounds.Width = packetReader.ReadInt32();
                            sprite.SpriteBounds.Height = packetReader.ReadInt32();
                            game.SpriteComponent[entityID] = sprite;
                            break;

                        case PacketTypes.MovementSprite:
                            MovementSprite movementSprite = new MovementSprite();
                            textureName = packetReader.ReadString();
                            movementSprite.EntityID = entityID;
                            movementSprite.SpriteSheet = game.Content.Load<Texture2D>(textureName);
                            movementSprite.SpriteSheet.Name = textureName;
                            movementSprite.SpriteBounds.X = packetReader.ReadInt32();
                            movementSprite.SpriteBounds.Y = packetReader.ReadInt32();
                            movementSprite.SpriteBounds.Width = packetReader.ReadInt32();
                            movementSprite.SpriteBounds.Height = packetReader.ReadInt32();
                            game.MovementSpriteComponent[entityID] = movementSprite;
                            break;
                    }
                }
            }
        }


        /// <summary>
        /// Helper method for setting up network session event handlers
        /// </summary>
        void HookSessonEvents()
        {
            session.GamerJoined += GamerJoinedEventHandler;
            session.GamerLeft += GamerLeftEventHandler;
            session.GameStarted += GameStartedEventHandler;
            session.GameEnded += GameEndedEventHandler;
            session.SessionEnded += SessionEndedEventHandler;
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles a player joining the game.  The new gamer should be sent 
        /// all the entity components it will need to synchonize with this 
        /// peer (i.e. all the components this peer has authority over)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GamerJoinedEventHandler(object sender, GamerJoinedEventArgs e)
        {
            // We only need to update remote gamers - local ones will share
            // our local game state
            if (!e.Gamer.IsLocal)
            {
                // Send components for all entitys that we have authority over
                // (i.e. those with a local component)
                foreach (Local local in game.LocalComponent.All)
                {
                    // Send position
                    if (game.PositionComponent.Contains(local.EntityID))
                    {
                        Position position = game.PositionComponent[local.EntityID];
                        packetWriter.Write(position.EntityID);
                        packetWriter.Write((short)PacketTypes.Position);
                        packetWriter.Write(position.Center);
                        packetWriter.Write(position.Radius);
                    }

                    // Send sprite
                    if (game.SpriteComponent.Contains(local.EntityID))
                    {
                        Sprite sprite = game.SpriteComponent[local.EntityID];
                        packetWriter.Write(sprite.EntityID);
                        packetWriter.Write((short)PacketTypes.Sprite);
                        packetWriter.Write(sprite.SpriteSheet.Name);
                        packetWriter.Write(sprite.SpriteBounds.X);
                        packetWriter.Write(sprite.SpriteBounds.Y);
                        packetWriter.Write(sprite.SpriteBounds.Width);
                        packetWriter.Write(sprite.SpriteBounds.Height);
                    }

                    // Send movement sprite
                    if (game.MovementSpriteComponent.Contains(local.EntityID))
                    {
                        MovementSprite sprite = game.MovementSpriteComponent[local.EntityID];
                        packetWriter.Write(sprite.EntityID);
                        packetWriter.Write((short)PacketTypes.MovementSprite);
                        packetWriter.Write(sprite.SpriteSheet.Name);
                        packetWriter.Write(sprite.SpriteBounds.X);
                        packetWriter.Write(sprite.SpriteBounds.Y);
                        packetWriter.Write(sprite.SpriteBounds.Width);
                        packetWriter.Write(sprite.SpriteBounds.Height);
                    }

                    // Send the data
                    session.LocalGamers[0].SendData(packetWriter, SendDataOptions.InOrder, e.Gamer);
                }
            }
        }


        /// <summary>
        /// Handles a player leaving.  Any entities belongong to that player must
        /// have authority transferred to another player (this is the responsiblity
        /// of the host)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GamerLeftEventHandler(object sender, GamerLeftEventArgs e)
        {
            if (session.IsHost)
            {
                // TODO: Transfer authority for the remote player's entities
                // to another player
            }
        }


        /// <summary>
        /// Handles a game start event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GameStartedEventHandler(object sender, GameStartedEventArgs e)
        {
        }


        /// <summary>
        /// Handles a game ending event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GameEndedEventHandler(object sender, GameEndedEventArgs e)
        {
        }


        /// <summary>
        /// Handles a session ending event.  This should push the player
        /// back to the network screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SessionEndedEventHandler(object sender, NetworkSessionEndedEventArgs e)
        {
            // TODO: Report the session ending
            game.GameState = GameState.NetworkSetup;
        }

        /// <summary>
        /// Handles a change of host.  The new host becomes responsible for
        /// assigning authority over objects when the situation is ambigous,
        /// so if it is us, we need to know it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void HostChangedEventHandler(object sender, HostChangedEventArgs e)
        {
        }

        /// <summary>
        /// This event handler takes the background worker's work
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void findAvailableSessions(object sender, EventArgs e)
        {
            availableSessions =
                NetworkSession.Find(
                    NetworkSessionType.SystemLink,  // Session Type
                    4,                              // Max local gamers
                    null                            // Search Properties
                    );

        }

        /// <summary>
        /// This event handler takes the case when the available sessions are found
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void foundAvailableSessions(object sender, EventArgs e)
        {
            searchingForAvailableSessions = false;

            if (availableSessions.Count == 0)
                currentSessionName = "No Sessions Found";
            else
                currentSessionName = string.Format("Session %s : %d / %d",
                    availableSessions[0].HostGamertag,
                    availableSessions[0].CurrentGamerCount,
                    availableSessions[0].CurrentGamerCount
                        + availableSessions[0].OpenPublicGamerSlots);

            selectedSession = 0;

            menuState = NetworkMenuState.JoinSession;
            menuSessionState = 0;
            menuSpriteLocation = TextLocations[4] - new Vector2(10, 0);

        }

        #endregion
    }
}
