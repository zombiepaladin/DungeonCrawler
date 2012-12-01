#region File Description
//-----------------------------------------------------------------------------
// StunComponent.cs 
//
// Author: Nicholas Boen
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
    public struct Enslave
    {
        public uint EntityID;
        public uint OwnerID;
        public uint TargetID;
    }

    public class EnslaveComponent : GameComponent<Enslave>
    {
    }
}
