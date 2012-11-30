#region File Description
//-----------------------------------------------------------------------------
// SkillProjectileComponent.cs 
//
// Author: Matthew Hart
//
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence 
//-----------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DungeonCrawler.Components;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using DungeonCrawler.Systems;

namespace DungeonCrawler.Components
{
    #region SkillProjectiles
    public struct SkillProjectile
    {
        /// <summary>
        /// Entity Id.
        /// </summary>
        public uint EntityID;
        
        /// <summary>
        /// enum for the associated skill
        /// </summary>
        public SkillType skill;

        /// <summary>
        /// Max range of the projectile
        /// </summary>
        public int maxRange;

        /// <summary>
        /// rank of the skill
        /// </summary>
        public int rank;

        public bool canHitPlayers;

        public bool canHitEnemies;
    }
    
    public class SkillProjectileComponent : GameComponent<SkillProjectile>
    {
    }
    #endregion

    #region SkillAoE
    public struct SkillAoE
    {
        /// <summary>
        /// Entity Id.
        /// </summary>
        public uint EntityID;

        /// <summary>
        /// radius of the AoE
        /// </summary>
        public int radius;

        /// <summary>
        /// rank of the skill
        /// </summary>
        public int rank;
    }

    public class SkillAoEComponent : GameComponent<SkillAoE>
    {
    }
    #endregion

    #region SkillDeployable
    public struct SkillDeployable
    {
        /// <summary>
        /// Entity Id.
        /// </summary>
        public uint EntityID;

        /// <summary>
        /// Duration of the AoE
        /// </summary>
        public int duration;

        /// <summary>
        /// rank of the skill
        /// </summary>
        public int rank;
    }

    public class SkillDeployableComponent : GameComponent<SkillDeployable>
    {
    }
    #endregion 
}
