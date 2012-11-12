#region File Description
//-----------------------------------------------------------------------------
//ChargeComponent.cs
//
// Author: Andrew Bellinder
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

namespace DungeonCrawler.Components.EarthianSkillComponents
{
    /// <summary>
    /// A structure indicating the local nature of an entity
    /// </summary>
    public struct Charge
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
        /// Speed increase amount
        /// </summary>
        public int speedIncrease;

        /// <summary>
        /// Attack increase amount
        /// </summary>
        public int attackIncrease;

        /// <summary>
        /// Defense decrease amount
        /// </summary>
        public int defenseDecrease;

        /// <summary>
        /// Duration in seconds
        /// </summary>
        public int duration;

    }

    /// <summary>
    /// The player components for all entities in a game world
    /// </summary>
    public class ChargeComponent : GameComponent<Charge>
    {

    }
}
