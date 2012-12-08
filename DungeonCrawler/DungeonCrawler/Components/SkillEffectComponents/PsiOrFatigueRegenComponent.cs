#region File Description
//-----------------------------------------------------------------------------
// HealOverTimeComponent.cs 
//
// Author: Nicholas Boen, Michael Fountain
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
    public struct PsiOrFatigueRegen
    {
        public uint EntityID;
        public uint TargetID;

        public float AmountPerTick;
        public float TickTime;
        public float CurrentTime;
    }

    public class PsiOrFatigueRegenComponent : GameComponent<PsiOrFatigueRegen>
    {
    }
}
