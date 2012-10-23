#region File Description
//-----------------------------------------------------------------------------
// LocalComponent.cs 
//
// Author: Nathan Bean
//
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence 
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System.Collections.Generic;
using Microsoft.Xna.Framework;
#endregion

namespace DungeonCrawler.Components
{
    /// <summary>
    /// A structure indicating the local nature of an entity
    /// </summary>
    public struct Player
    {
        /// <summary>
        /// The entity this Player component belongs to
        /// </summary>
        public uint EntityID;

        /// <summary>
        /// The PlayerIndex of this entity's player
        /// </summary>
        public PlayerIndex PlayerIndex;

        /// <summary>
        /// This player's strength stat
        /// </summary>
        public int Strength;

        /// <summary>
        /// This player's stamina stat
        /// </summary>
        public int Stamina;

        /// <summary>
        /// This player's agility stat
        /// </summary>
        public int Agility;

        /// <summary>
        /// This player's intelligence stat
        /// </summary>
        public int Intelligence;

        /// <summary>
        /// This player's defense stat
        /// </summary>
        public int Defense;
    }

    /// <summary>
    /// The player components for all entities in a game world
    /// </summary>
    public class PlayerComponent : GameComponent<Player>
    {

    }
}