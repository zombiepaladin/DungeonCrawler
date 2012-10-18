#region File Description
//-----------------------------------------------------------------------------
// EnemyAIComponent.cs 
//
// Author: Brett Barger
//
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence 
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using DungeonCrawler.Entities;
#endregion

namespace DungeonCrawler.Components
{
    public struct EnemyAIComponent
    {
        // <summary>
        /// The ID of the entity this AI belongs to
        /// </summary>
        public uint EntityID;

        // <summary>
        /// The ID of the Enemy this entity belongs to
        /// </summary>
        public byte EnemyID;
    }

    public class EnemyAIComponent : GameComponent<EnemyAI>
    {
        /// <summary>
        /// The game the Enemy belongs to
        /// </summary>
        DungeonCrawlerGame game;
        IEnumerable<Position> HitList;
        Position position;

        /// <summary>
        /// Creates a new AggregateFactory instance
        /// </summary>
        /// <param name="game"></param>
        public EnemyAIComponent(DungeonCrawlerGame game)
        {
            this.game = game;
        }

        public void Scan(Position position)
        {
            
        }

        public void Attack()
        {

        }
    }
}
