#region File Description
//-----------------------------------------------------------------------------
// CollectibleComponent.cs 
//
// Author: Matthew McHaney
//
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence bla bla bla
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
    /// An enum representing a the types of collectibles
    /// </summary>
    public enum CollectibleType
    {
        money,
        health,
        pog
    };

    /// <summary>
    /// A struct representing a Collectible Component
    /// </summary>
    public struct Collectible
    {
        /// <summary>
        /// The ID of the entity this sprite belongs to
        /// </summary>
        public uint EntityID;

        /// <summary>
        /// The type of collectible that this is
        /// </summary>
        public CollectibleType CollectibleType;

        /// <summary>
        /// The value of the collectible (money value, health value, pog id#, etc.)
        /// </summary>
        public int CollectibleValue;
    }

    public class CollectibleComponent : GameComponent<Collectible>
    {

    }
}
