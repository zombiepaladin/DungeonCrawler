#region File Description
//-----------------------------------------------------------------------------
//NanobotsComponent.cs
//
// Author: Adam Clark
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

namespace DungeonCrawler.Components.CyborgSkills
{
    /// <summary>
    /// A structure indicating the local nature of an entity
    /// </summary>
    public struct Nanobots
    {
        /// <summary>
        /// The entity this Player component belongs to
        /// </summary>
        public uint EntityID;

        /// <summary>
        /// The rank of the skill
        /// </summary>
        public int rank;

        /// <summary>
        /// Percentage to heal
        /// </summary>
        public int PercentageHeal;

        /// <summary>
        /// amount of damage to deal
        /// </summary>
        public int DamageDealt;

    }

    /// <summary>
    /// The player components for all entities in a game world
    /// </summary>
    public class NanobotsComponent : GameComponent<Nanobots>
    {

    }
}

