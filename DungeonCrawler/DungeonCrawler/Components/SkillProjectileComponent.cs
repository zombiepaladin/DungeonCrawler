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

namespace DungeonCrawler.Components
{
    /// <summary>
    /// enum for skills with 
    /// </summary>
    public enum skills
    {
        energyShield,
        defribrillate,
        nanobots,
        targetingUpgrade,
        repulserArm,
        energyShot,
        alloyBody,
        cyberneticSlam,
        thrusterRush,
        mindLock,
        invisibility,
        possess,
        psionicSpear,
        push,
        detonate,
        mentalBarrier,
        wormOfGargranian,
        soothe,
        enslave,
        fear,
        sacrifice,
        taint,
        rot,
        push,
        lightning,
        malice,
        throwBlades,
        frenziedAttack,
        causticWeapons,
        meatShield,
        hardenedBody,
        graspingBlade,
        benignParasite,
        malicousParasite,
        mindlessParasite,
        trap,
        explodingDroids,
        turret,
        healingStation,
        portableShop,
        portableShield,
        charge,
        fallBack,
        motivate,
        agilityBerserker,
        dualWielding,
        heavyDrinker,
        powerShot,
        eagleShot,
        trickShot,
        mug,
        lockpicking,
        steal
    }

    public struct SkillProjectile
    {
        /// <summary>
        /// Entity Id.
        /// </summary>
        public uint EntityID;

        /// <summary>
        /// enum for the associated skill
        /// </summary>
        public skills skill;

        /// <summary>
        /// Max range of the projectile
        /// </summary>
        public int maxRange;
    }
    
    public class SkillProjectileComponent : GameComponent<SkillProjectile>
    {
    }


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
    }

    public class SkillAoEComponent : GameComponent<SkillAoEComponent>
    {
    }

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
    }

    public class SkillDeployableComponent : GameComponent<SkillDeployable>
    {
    }
}
