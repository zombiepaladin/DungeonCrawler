#region File Description
//-----------------------------------------------------------------------------
// CoolDownComponent.cs 
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
using DungeonCrawler.Systems;

namespace DungeonCrawler.Components
{

    public struct CoolDown
    {
        public uint EntityID;
        public uint UserID;

        public SkillType Type;
        public float MaxTime;
        public float TimeLeft;
    }

    public class CoolDownComponent : GameComponent<CoolDown>
    {
    }
}
