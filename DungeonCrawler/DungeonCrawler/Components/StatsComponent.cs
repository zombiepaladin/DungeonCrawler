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
    public struct Stats
    {
        /// <summary>
        /// The entity this Player component belongs to
        /// </summary>
        public uint EntityID;

        /// <summary>
        /// This aggregate's strength stat
        /// </summary>
        public int Strength;

        /// <summary>
        /// This aggregate's stamina stat
        /// </summary>
        public int Stamina;

        /// <summary>
        /// This aggregate's agility stat
        /// </summary>
        public int Agility;

        /// <summary>
        /// This aggregate's intelligence stat
        /// </summary>
        public int Intelligence;

        /// <summary>
        /// This aggregate's defense stat
        /// </summary>
        public int Defense;

        public int PoisonResistanceBase;

        public int AttackMeleeBase;

        public int AttackRangedBase;

        public int DefenseMeleeBase;

        public int DefenseRangedBase;

        public int WeaponStrengthBase;

        public int WeaponAccuracyBase;

        public int WeaponSpeedBase;

        public int AttackSpeedBase;

        public int HealthBase;

        public int PsiBase;

        public int FatigueBase;
    }

    /// <summary>
    /// The player components for all entities in a game world
    /// </summary>
    public class StatsComponent : GameComponent<Stats>
    {

    }
}