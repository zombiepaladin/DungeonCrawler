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
    #region Healing Station

    public struct HealingStation
    {
        /// <summary>
        /// Entity Id.
        /// </summary>
        public uint EntityID;

        /// <summary>
        /// Position of Healing station.
        /// </summary>
        public Position position;

        /// <summary>
        /// Health 
        /// </summary>
        public int healthAvailable;
    };

    public class HealingStationComponent : GameComponent<HealingStation>
    {

    }
    #endregion


    #region Portable Shield

    public struct PortableShield
    {
        /// <summary>
        /// Entity Id.
        /// </summary>
        public uint EntityID;

        /// <summary>
        /// Position of Healing station.
        /// </summary>
        public Position position;
    };

    public class PortableShieldComponent : GameComponent<PortableShield>
    {

    }
    #endregion


    #region Portable Store

    public struct PortableStore
    {

    };

    public class PortableStoreComponent : GameComponent<PortableStore>
    {

    }
    #endregion

}
