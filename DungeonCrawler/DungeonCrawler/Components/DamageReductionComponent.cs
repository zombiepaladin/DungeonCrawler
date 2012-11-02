#region File Description
//-----------------------------------------------------------------------------
//DamageReductionComponent.cs
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
    /// an enumeratior for what kinds of damage to reduce
    /// </summary>
    public enum DamageReductionType
    {
        Melee,
        Psi,
        Ranged,
        All,
    }

    /// <summary>
    /// A structure indicating the local nature of an entity
    /// </summary>
    public struct DamageReduction
    {
        /// <summary>
        /// The entity this Player component belongs to
        /// </summary>
        public uint EntityID;

        /// <summary>
        /// The type of damage to reduce
        /// </summary>
        public DamageReductionType ReduceType;

    }

    /// <summary>
    /// The player components for all entities in a game world
    /// </summary>
    public class DamageReductionComponent : GameComponent<DamageReduction>
    {

    }
}

