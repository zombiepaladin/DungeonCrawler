#region File Description
//-----------------------------------------------------------------------------
//HealComponent.cs
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

namespace DungeonCrawler.Components
{
    /// <summary>
    /// A structure indicating the local nature of an entity
    /// </summary>
    public struct Heal
    {
        /// <summary>
        /// The entity this Player component belongs to
        /// </summary>
        public uint EntityID;

        /// <summary>
        /// This will apply below number ever X secs
        /// Set at 0 if heal is not a heal over time.
        /// </summary>
        public int HealOverTimeSecs;

        /// <summary>
        /// X percent to heal
        /// </summary>
        public int HealPercent;

        /// <summary>
        /// The exact number of hit points to heal
        /// </summary>
        public int HealHP;

    }

    /// <summary>
    /// The player components for all entities in a game world
    /// </summary>
    public class HealComponent : GameComponent<Heal>
    {

    }
}

