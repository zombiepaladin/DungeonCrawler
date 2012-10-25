#region File Description
//-----------------------------------------------------------------------------
// EnemyAISystem.cs 
//
// Author: Brett Barger
//
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence 
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DungeonCrawler.Components;
#endregion

namespace DungeonCrawler.Systems
{
    public class EnemyAISystem
    {
        #region Private Members

        /// <summary>
        /// The game this system belongs to
        /// </summary>
        private DungeonCrawlerGame game;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new MovementSystem
        /// </summary>
        /// <param name="game">The game this system belongs to</param>
        public EnemyAISystem(DungeonCrawlerGame game)
        {
            this.game = game;
        }

        #endregion
        #region Public Methods

        /// <summary>
        /// Updates all Enemies in the game
        /// </summary>
        /// <param name="elapsedTime">
        /// The time between this and the previous frame.
        /// </param>
        public void Update(float elapsedTime)
        {
            foreach(EnemyAI ai in game.EnemyAIComponent.All)
            {
                Position pos = game.PositionComponent[ai.EntityID];
                IEnumerable<Position> HitList = game.PositionComponent.InRegion(pos.Center, 500);

                foreach (Position thing in HitList)
                {
                    if (game.PlayerComponent.Contains(thing.EntityID))
                    {
                        Vector2 toPlayer = thing.Center - pos.Center;
                        toPlayer.Normalize();
                        pos.Center += toPlayer * elapsedTime * 50;
                    }
                }
            }
        }

        #endregion
    }
}
