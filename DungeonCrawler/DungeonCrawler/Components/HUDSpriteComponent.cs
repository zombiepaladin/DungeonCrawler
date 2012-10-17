#region File Description
//-----------------------------------------------------------------------------
// HUDSpriteComponent.cs 
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
#endregion

namespace DungeonCrawler.Components
{

    public struct HUDSprite
    {
        public uint EntityID;
        public Texture2D SpriteSheet;
        public Rectangle SpriteBounds;
        public bool isSeen;
    }
    public class HUDSpriteComponent : GameComponent<HUDSprite>
    {
    }
}
