#region File Description
//-----------------------------------------------------------------------------
// GarbagemanSystem.cs 
//
// Author: Matthew McHaney
//
// Modified By: Nicholas Strub - Added handing of doors and room 10/31/12
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
using Microsoft.Xna.Framework.Input;
using DungeonCrawler.Components;
using System.Collections.Generic;
#endregion

namespace DungeonCrawler.Systems
{
    /// <summary>
    /// This class embodies a system that simplifies deleting entities
    /// from the components list. Ideally, you'll tell the garbageman that
    /// entity ID #YOURNUMBERHERE needs to be taken out of the game. So then
    /// when he updates, he'll take the old data (garbage) out of the
    /// components (driveway). Foolproof.
    /// </summary>
    public class GarbagemanSystem
    {
        #region Private Members

        /// <summary>
        /// The game this system belongs to
        /// </summary>
        private DungeonCrawlerGame game;

        public enum ComponentType
        {
            Player,
            Enemy,
            Bullet,
            Collectible,
            Door,
            Room,
            //Needs more in the future!
        }

        private Stack<KeyValuePair<uint, ComponentType>> garbageSchedule;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new MovementSystem
        /// </summary>
        /// <param name="game">The game this system belongs to</param>
        public GarbagemanSystem(DungeonCrawlerGame game)
        {
            this.game = game;
            garbageSchedule = new Stack<KeyValuePair<uint, ComponentType>>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Pulls out any necessary eids
        /// </summary>
        /// <param name="elapsedTime">
        /// The time between this and the previous frame.
        /// </param>
        public void Update(float elapsedTime)
        {
            KeyValuePair<uint, ComponentType> keyValue;

            // Update all entities that have a movement component
            while(garbageSchedule.Count > 0)
            {
                keyValue = garbageSchedule.Pop();

                switch (keyValue.Value)
                {
                    case ComponentType.Player:
                        game.PositionComponent.Remove(keyValue.Key);
                        game.MovementComponent.Remove(keyValue.Key);
                        game.MovementSpriteComponent.Remove(keyValue.Key);
                        game.LocalComponent.Remove(keyValue.Key); //?? What does this do? Will this break something?
                        game.StatsComponent.Remove(keyValue.Key);
                        game.PlayerComponent.Remove(keyValue.Key);
                        game.PlayerInfoComponent.Remove(keyValue.Key);
                        game.CollisionComponent.Remove(keyValue.Key);
                        break;
                    case ComponentType.Enemy:
                        //No enemies yet, will need to be added
                        break;
                    case ComponentType.Bullet:
                        game.PositionComponent.Remove(keyValue.Key);
                        game.BulletComponent.Remove(keyValue.Key);
                        game.SpriteComponent.Remove(keyValue.Key);
                        game.MovementComponent.Remove(keyValue.Key);
                        game.CollisionComponent.Remove(keyValue.Key);
                        break;
                    case ComponentType.Collectible:
                        game.PositionComponent.Remove(keyValue.Key);
                        game.CollectibleComponent.Remove(keyValue.Key);
                        game.SpriteComponent.Remove(keyValue.Key);
                        game.CollisionComponent.Remove(keyValue.Key);
                        //game.MovementComponent.Remove(keyValue.Key);
                        break;
                    case ComponentType.Room:
                        game.PositionComponent.Remove(keyValue.Key);
                        game.LocalComponent.Remove(keyValue.Key);
                        game.RoomComponent.Remove(keyValue.Key);
                        break;
                    case ComponentType.Door:
                        game.PositionComponent.Remove(keyValue.Key);
                        game.CollisionComponent.Remove(keyValue.Key);
                        game.LocalComponent.Remove(keyValue.Key);
                        game.DoorComponent.Remove(keyValue.Key);
                        //game.SpriteComponent.Remove(keyValue.Key);
                        break;
                }
            }
        }

        public void ScheduleVisit(uint eid)
        {
            ComponentType cType;
            if (game.PlayerInfoComponent.Contains(eid))
                cType = ComponentType.Player;
            //else if (game.EnemyComponent.Contains(eid)) Not Implemented
            //    cType = ComponentType.Enemy;
            else if (game.BulletComponent.Contains(eid))
                cType = ComponentType.Bullet;
            else if (game.CollectibleComponent.Contains(eid))
                cType = ComponentType.Collectible;
            else if (game.DoorComponent.Contains(eid))
                cType = ComponentType.Door;
            else if (game.RoomComponent.Contains(eid))
                cType = ComponentType.Room;
            else
                return; //It's something not supported

            garbageSchedule.Push(new KeyValuePair<uint, ComponentType>(eid, cType));
        }

        public void ScheduleVisit(uint eid, ComponentType type)
        {
            garbageSchedule.Push(new KeyValuePair<uint, ComponentType>(eid, type));
        }
        #endregion
    }
}