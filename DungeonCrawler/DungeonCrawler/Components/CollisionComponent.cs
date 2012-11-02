#region File Description
//-----------------------------------------------------------------------------
// CollisionComponent.cs 
//
// Author: Devin Kelly-Collins & Matthew McHaney
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

namespace DungeonCrawler.Components
{
    public struct Collideable
    {
        public uint EntityID;

        public Bounds Bounds;
    }

    public class CollisionComponent : GameComponent<Collideable>
    {

    }
}
