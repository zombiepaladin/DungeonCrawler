#region File Description
//-----------------------------------------------------------------------------
// FearComponent.cs 
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
    public struct Fear
    {
        public uint EntityID;
        public uint TargetID;
        public uint SourceID;
    }

    public class FearComponent : GameComponent<Fear>
    {
        new public void Add(uint entityID, Fear component)
        {
            DungeonCrawlerGame game = DungeonCrawlerGame.game;

            if (game.EnemyComponent.Contains(component.TargetID))
            {
                Enemy enemy = game.EnemyComponent[component.TargetID];

                enemy.State = enemy.State | EnemyState.Scared;
            }
        }

        new public void Remove(uint entityID)
        {

        }
    }
}
