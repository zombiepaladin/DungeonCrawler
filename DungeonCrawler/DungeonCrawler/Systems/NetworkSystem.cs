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
                else if (Keyboard.GetState().IsKeyDown(Keys.A) ||
                    GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A) ||
                    GamePad.GetState(PlayerIndex.Two).IsButtonDown(Buttons.A) ||
                    GamePad.GetState(PlayerIndex.Three).IsButtonDown(Buttons.A) ||
                    GamePad.GetState(PlayerIndex.Four).IsButtonDown(Buttons.A))
                {
                    JoinSession();
                }
            }
            else
            {
                // send local object updates
                foreach (Network network in game.NetworkComponent.Local)
                {
                    // TODO: send a custom packet for every updated component on local network entities
                }
                
                // Pump the session 
                session.Update();

                // Check for session ending
                if (session == null) game.GameState = GameState.NetworkSetup;

                // Read packets for remote updates
                // TODO: read custom packets for updated entities, and apply them to 
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
        /// Handles a player joining the game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GamerJoinedEventHandler(object sender, GamerJoinedEventArgs e)
        {            
        }


        /// <summary>
        /// Handles a player leaving.  Any elements belongong to that player must
        /// have authority transferred to another player (this is the responsiblity
        /// of the host)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GamerLeftEventHandler(object sender, GamerLeftEventArgs e)
        {
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
