#region File Description
//-----------------------------------------------------------------------------
// AgroDropComponent.cs 
//
// Author: Michael Fountain
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
using Microsoft.Xna.Framework.Graphics;

namespace DungeonCrawler.Components
{
    public struct ChangeVisibility
    {
        public uint EntityID;
        public uint TargetID;
        public Color newColor;
    }

    public class ChangeVisibilityComponent : GameComponent<ChangeVisibility>
    {
        new public void Add(uint entityID, ChangeVisibility component)
        {
            base.Add(entityID, component);

            DungeonCrawlerGame game = DungeonCrawlerGame.game;
            Sprite playerSprite = game.SpriteComponent[component.TargetID];
            playerSprite.SpriteColor = component.newColor;
            playerSprite.UseDifferentColor = true;
            game.SpriteComponent[component.TargetID] = playerSprite;
        }

        new public void Remove(uint entityID)
        {
            DungeonCrawlerGame game = DungeonCrawlerGame.game;
            ChangeVisibility component = game.ChangeVisibilityComponent[entityID];

            Sprite playerSprite = game.SpriteComponent[component.TargetID];
            playerSprite.SpriteColor = Color.White;
            playerSprite.UseDifferentColor = false;
            game.SpriteComponent[component.TargetID] = playerSprite;

            base.Remove(entityID);
        }
    }
}
