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
            if (game.GameState == GameState.NetworkSetup)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.A) ||
                    GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A) ||
                    GamePad.GetState(PlayerIndex.Two).IsButtonDown(Buttons.A) ||
                    GamePad.GetState(PlayerIndex.Three).IsButtonDown(Buttons.A) ||
                    GamePad.GetState(PlayerIndex.Four).IsButtonDown(Buttons.A))
                {
                    CreateSession();
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.B) ||
                    GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.B) ||
                    GamePad.GetState(PlayerIndex.Two).IsButtonDown(Buttons.B) ||
                    GamePad.GetState(PlayerIndex.Three).IsButtonDown(Buttons.B) ||
                    GamePad.GetState(PlayerIndex.Four).IsButtonDown(Buttons.B))
                {
                    JoinSession();
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
                using (AvailableNetworkSessionCollection availableSessions =
                    NetworkSession.Find(
                        NetworkSessionType.SystemLink,  // Session Type
                        4,                              // Max local gamers
                        null                            // Search Properties
                        ))
                {
                    if (availableSessions.Count == 0)
                    {
                        // No sessions available
                        // TODO: Offer feedback
                        return;
                    }

                    // Join the first session found
                    session = NetworkSession.Join(availableSessions[0]);

                    // Set ourselves as guest
                    isHost = false;

                    // Set up the session event handlers
                    HookSessonEvents();

                    // Update our game state
                    game.GameState = GameState.Gameplay;
                }
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

        #endregion
    }
}
