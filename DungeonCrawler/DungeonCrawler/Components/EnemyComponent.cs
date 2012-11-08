#region File Description
//-----------------------------------------------------------------------------
// EnemyComponent.cs 
//
// Author: Matthew McHaney
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
        /// Boolean for whether touching the objects hurts the player or not
        /// </summary>
        public bool HurtOnTouch;
        
        /// <summary>
        /// Integer for health
        /// </summary>
        public int Health;

    }

    public class EnemyComponent : GameComponent<Enemy>
    {

    }
}
