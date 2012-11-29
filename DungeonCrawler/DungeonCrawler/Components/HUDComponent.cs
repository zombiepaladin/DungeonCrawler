﻿
#region File Description
//-----------------------------------------------------------------------------
// HUDAggregateFactory.cs 
//
// Author: Nick Stanley
//
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence 
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DungeonCrawler.Components;
#endregion


namespace DungeonCrawler.Components
{
    //Note: Use HUDSystem instead.

    public struct HUD
    {
        public uint EntityID;
        public uint AButtonSpriteID;
        public uint BButtonSpriteID;
        public uint XButtonSpriteID;
        public uint YButtonSpriteID;
        public uint DPadSpriteID;
        public uint HealthPsiStatusSpriteID;
        public uint AvatarSpriteID;
    }
    public class HUDComponent : GameComponent<HUD>
    {
    }
}
