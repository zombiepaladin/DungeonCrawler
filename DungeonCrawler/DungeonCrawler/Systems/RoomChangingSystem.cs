#region File Description
//-----------------------------------------------------------------------------
// RoomChangingSystem.cs
//
// Author: Nicholas Strub
//
// Modified By: Nicholas Strub - Smoothed out room transitions and added ability to spawn at specified spawn points (11/3/2012)
//
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence 
//-----------------------------------------------------------------------------
#endregion

#region Using Statements

using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using DungeonCrawler.Components;

#endregion

namespace DungeonCrawler.Systems
{
    /// <summary>
    /// A system for changing rooms
    /// </summary>
    public class RoomChangingSystem
    {

        #region Private Members

        /// <summary>
        /// The game this system belongs to
        /// </summary>
        private DungeonCrawlerGame game;

        /// <summary>
        /// The amount of time the room has been loading
        /// </summary>
        private float loadingTime;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new Room Changing System
        /// </summary>
        /// <param name="game">The Dungeon Crawler Game</param>
        public RoomChangingSystem(DungeonCrawlerGame game)
        {
            loadingTime = 0;
            this.game = game;
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Anything that needs to be cleaned up after switching rooms
        /// </summary>
        private void CleanupLastRoom(uint roomId)
        {
            //For now, we only need to clean up bullets. Maybe collectibles too.
            foreach (Bullet bullet in game.BulletComponent.All)
            {
                if (game.PositionComponent[bullet.EntityID].RoomID == roomId)
                {
                    game.GarbagemanSystem.ScheduleVisit(bullet.EntityID);
                }
            }
        }
   
        #endregion

        #region Public Methods

        /// <summary>
        /// Changes from one room to the room specified
        /// </summary>
        /// <param name="destinationRoom">Room to change to</param>
        public void ChangeRoom(Door door)
        {
            loadingTime = 0;
            game.GameState = GameState.RoomChange;

            ThreadStart threadStarter = delegate
            {
                // Delete all current doors in the game
                //foreach (Door door in game.DoorComponent.All)
                //{
                //    game.GarbagemanSystem.ScheduleVisit(door.EntityID);
                //}

                // Delete all the current rooms in the game
                //foreach (Room room in game.RoomComponent.All)
                //{
                //    game.GarbagemanSystem.ScheduleVisit(room.EntityID);
                //}

                // Load the destination room
                uint lastRoomEid = game.CurrentRoomEid;

                if (door.DestinationRoom == "TestDungeon1" && game.RoomComponent[lastRoomEid].roomName == "TestDungeon3" && game.QuestLogSystem.currentQuest.questName == QuestName.ReachNextRoom && game.QuestLogSystem.currentQuest.questStatus == QuestStatus.InProgress)
                {
                    game.QuestLogSystem.currentQuest.questStatus = QuestStatus.Finished;
                    game.QuestComponent[game.QuestLogSystem.currentQuest.EntityID] = game.QuestLogSystem.currentQuest;
                }

                DungeonCrawlerGame.LevelManager.LoadLevel(door.DestinationRoom);

                CleanupLastRoom(lastRoomEid);

                // Don't proceed until the room has finished loading
                // We wait for a certain time so that the screen doesn't flash black very quickly and then to the new room.
                while (loadingTime < 0.25f || DungeonCrawlerGame.LevelManager.Loading)
                {
                }

                // Move the player to a new position
                // TODO: Get this position from the spawn positions in the room
                foreach (Player player in game.PlayerComponent.All)
                {
                    Position position = game.PositionComponent[player.EntityID];
                    position.Center = DungeonCrawlerGame.LevelManager.getCurrentRoom().playerSpawns[door.DestinationSpawnName];
                    position.RoomID = DungeonCrawlerGame.LevelManager.getCurrentRoom().EntityID;
                    game.PositionComponent[player.EntityID] = position;
                }


                game.GameState = GameState.Gameplay;
            };

            Thread loadingThread = new Thread(threadStarter);
            loadingThread.Start();
        }

        public void Update(float elapsedTime)
        {
            loadingTime += elapsedTime;
        }

        #endregion
    }
}
