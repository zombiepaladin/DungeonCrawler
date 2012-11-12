#region File Description
//-----------------------------------------------------------------------------
// NPCComponent.cs 
//
// Author: Michael Fountain
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
    public enum NPCType
    {
        Trollph,
    }

    /// <summary>
    /// A struct representing an npc Component
    /// </summary>
    public struct NPC
    {
        /// <summary>
        /// The ID of the entity this room belongs to
        /// </summary>
        public uint EntityID;

        /// <summary>
        /// The type of the npc
        /// </summary>
        public NPCType Type;

    }

    public class NPCComponent : GameComponent<NPC>
    {

    }
}
