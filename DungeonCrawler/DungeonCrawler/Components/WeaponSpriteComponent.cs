#region File Description
//-----------------------------------------------------------------------------
// WeaponSpriteCompoenent.cs 
//
// Author: Devin Kelly-Collins
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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DungeonCrawler.Components
{
    public struct WeaponSprite
    {
        public uint EntityID;
        public Texture2D SpriteSheet;
        public Rectangle SpriteBounds;
    }

    public class WeaponSpriteComponent : GameComponent<WeaponSprite>
    {
        
    }
}
