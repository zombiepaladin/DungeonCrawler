#region File Description
//-----------------------------------------------------------------------------
// TurretComponent.cs 
//
// Author: Andrew Bellinder
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
using Microsoft.Xna.Framework;
using DungeonCrawler.Entities;

namespace DungeonCrawler.Components
{

    public struct Turret
    {
        /// <summary>
        /// Entity Id.
        /// </summary>
        public uint EntityID;

        /// <summary>
        /// Position of Turret.
        /// </summary>
        public Position position;

        /// <summary>
        /// Range within turret will fire at enemy.
        /// </summary>
        public int range;
    }

    public class TurretComponent : GameComponent<Turret>
    {

    }



    public struct Trap
    {
        /// <summary>
        /// Entity Id.
        /// </summary>
        public uint EntityID;

        /// <summary>
        /// Position of Trap.
        /// </summary>
        public Position position;

        /// <summary>
        /// Range within trap will catch enemy.
        /// </summary>
        public int range;

        /// <summary>
        /// Number of seconds trap exists for.
        /// </summary>
        public float duration;

        /// <summary>
        /// Number of seconds trap exists for.
        /// </summary>
        public Enemy trappedEnemy;

        /// <summary>
        /// Number of seconds trap exists for.
        /// </summary>
        public bool isSet;
    }

    public class TrapComponent : GameComponent<Trap>
    {

    }

    public struct ExplodingDroid
    {
        /// <summary>
        /// Entity Id.
        /// </summary>
        public uint EntityID;

        /// <summary>
        /// Position of Droid.
        /// </summary>
        public Position position;

        /// <summary>
        /// Enemy to attack.
        /// </summary>
        public Enemy enemyToAttack;

        /// <summary>
        /// Droid has an enemy to attack
        /// </summary>
        public bool hasEnemy;
    }

    public class ExplodingDroidComponent : GameComponent<ExplodingDroid>
    {

    }
}
