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
        /// The current state of the player.
        /// </summary>
        public PlayerState State;
    }

    /// <summary>
    /// The player components for all entities in a game world
    /// </summary>
    public class PlayerComponent : GameComponent<Player>
    {

    }
}