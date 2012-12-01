#region File Description
//-----------------------------------------------------------------------------
// EnemyComponent.cs 
//
// Author: Matthew McHaney
//
// Modified: Nick Boen - Added the EnemyState enumeration, 11/12/2012
//
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence 
//-----------------------------------------------------------------------------

#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
#endregion

namespace DungeonCrawler.Components
{
    /// <summary>
    /// An enum representing all the types of enemies
    /// </summary>
    public enum EnemyType
    {
        Target,
        StationaryTarget,
        MovingTarget,
        Spider,
        Alien,
    }

    public enum EnemyState
    {
        Dead = 0x0,
        Stunned = 0x1,
        Scared = 0x2, //for fear
        Poisoned = 0x3,
    }

    public enum EnemyIntelligence
    {
        Mindless,
        SemiIntelligent,
        Intelligent,
    }

    /// <summary>
    /// A struct representing an Enemy Component
    /// </summary>
    public struct Enemy
    {
        /// <summary>
        /// The ID of the entity this room belongs to
        /// </summary>
        public uint EntityID;

        /// <summary>
        /// The type of the enemy
        /// </summary>
        public EnemyType Type;

        /// <summary>
        /// The state of the enemy
        /// <//summary>
        public EnemyState State;

        /// <summary>
        /// Boolean for whether touching the objects hurts the player or not
        /// </summary>
        public bool HurtOnTouch;
        
        /// <summary>
        /// Integer for health
        /// </summary>
        public float Health;

    }

    public class EnemyComponent : GameComponent<Enemy>
    {

    }
}
