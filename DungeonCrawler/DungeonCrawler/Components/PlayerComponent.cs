#region File Description
//-----------------------------------------------------------------------------
// LocalComponent.cs 
//
// Author: Nathan Bean
//
// Modified: Devin Kelly-Collins added PlayerInfo struct and component, 10/24/2012
//
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence 
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Xna.Framework;
using DungeonCrawler.Entities;
using DungeonCrawler.Systems;
#endregion

namespace DungeonCrawler.Components
{

    /// <summary>
    /// The states of a player. This can help us determine what needs to be rendered.
    /// </summary>
    public enum PlayerState
    {
        Default,
        Dead,
        Attacking,
        Inactive,
    }

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
        /// The race this player is playing as
        /// </summary>
        public Aggregate PlayerRace;

        /// <summary>
        /// A struct containing all the player's ability modifiers
        /// </summary>
        public AbilityModifiers abilityModifiers;
    }

    /// <summary>
    /// A struct containing modifiers based off of the stats
    /// added by Matthew Hart
    /// </summary>
    public struct AbilityModifiers
    {
        /// <summary>
        /// The reduces incoming melee damage
        /// </summary>
        public int meleeDamageReduction;

        /// <summary>
        /// The reduces incoming ranged damage
        /// </summary>
        public int rangedDamageReduction;

        /// <summary>
        /// Bonus to melee attack damage
        /// </summary>
        public int meleeAttackBonus;

        /// <summary>
        /// Bonus to ranged attack damage;
        /// </summary>
        public int RangedAttackBonus;

        /// <summary>
        /// Bonus to attack speed;
        /// </summary>
        public int MeleeAttackSpeed;

        /// <summary>
        /// Bonus to accuracy;
        /// </summary>
        public int Accuracy;

        /// <summary>
        /// Bonus to ranged spell effects
        /// </summary>
        public int SpellBonus;

        /// <summary>
        /// Bonus Health
        /// </summary>
        public int HealthBonus;

        /// <summary>
        /// Melee Defense Bonus
        /// </summary>
        public int MeleeDefenseBonus;

        /// <summary>
        /// Melee Defense Reduction
        /// </summary>
        public int MeleeDefenseReduction;
    }

    /// <summary>
    /// The player components for all entities in a game world
    /// </summary>
    public class PlayerComponent : GameComponent<Player>
    {
        public bool GetByIndex(PlayerIndex index, out Player player)
        {
            player = new Player();
            foreach (Player p in this.All)
            {
                if (p.PlayerIndex == index)
                {
                    player = p;
                    return true;
                }
            }
            return false; 
        }
    }

    ///<summary>
    ///Contains current information for the player.
    ///</summary>
    public struct PlayerInfo
    {
        ///<summary>
        ///The entity the player information belongs to.
        ///</summary>
        public uint EntityID;

        /// <summary>
        /// Current amount of health the player has.
        /// </summary>
        public float Health;

        /// <summary>
        /// Current amount of psi the player has.
        /// </summary>
        public int Psi;

        /// <summary>
        /// The current level of the player
        /// </summary>
        public int Level;

        /// <summary>
        /// The current experience of the player
        /// </summary>
        public int Experience;

        ///<summary>
        ///The current state of the player.
        ///</summary>
        public PlayerState State;

        /// <summary>
        /// The name of the game save file this player is associated with
        /// </summary>
        public string FileName;

        public int PoisonResistance;

        public int AttackMelee;

        public int AttackRanged;

        public int DefenseMelee;

        public int DefenseRanged;

        public int WeaponStrength;

        public int WeaponAccuracy;

        public int WeaponSpeed;

        public int AttackSpeed;

        public SkillType skill1;

        public SkillType skill2;

        public SkillType skill3;

        public SkillType skill4;

        public SkillType skill5;

        public SkillType skill6;

        public SkillType skill7;

        public SkillType skill8;

        public SkillType skill9;
    }

    ///<summary>
    ///The player information components.
    ///</summary>
    public class PlayerInfoComponent : GameComponent<PlayerInfo>
    {

    }

    [Serializable]
    public struct PlayerSkillInfo
    {
        public int Skill1Rank;

        public int Skill2Rank;

        public int Skill3Rank;

        public int Skill4Rank;

        public int Skill5Rank;

        public int Skill6Rank;

        public int Skill7Rank;

        public int Skill8Rank;

        public int Skill9Rank;
    }

    public class PlayerSkillInfoComponent : GameComponent<PlayerSkillInfo>
    {
    }

    public struct ActiveSkill
    {
        public SkillType activeSkill;
    }

    public class ActiveSkillComponent : GameComponent<ActiveSkill>
    {
    }
}