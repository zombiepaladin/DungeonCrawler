﻿#region File Description
//-----------------------------------------------------------------------------
// EnemyAIComponent.cs 
//
// Author: Brett Barger
//
// Modified: Nick Boen - Added the Target ID and a NoTargetList (or CantTargetList), 
//                       figured this would be useful later and it's necessary for agro gain and drop
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
    public enum AIBehaviorType
    {
        None,
        DefaultRanged,
        DefaultMelee,
        Alien,
    }

    public struct EnemyAI
    {
        // <summary>
        /// The ID of the entity this AI belongs to
        /// </summary>
        public uint EntityID;
        public AIBehaviorType AIBehaviorType;
        public bool HasTarget;
        public uint TargetID;
        public List<uint> NoTargetList;
    }

    public class EnemyAIComponent : GameComponent<EnemyAI>
    {
        
    }
}