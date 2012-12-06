#region File Description
//-----------------------------------------------------------------------------
// AgroDropComponent.cs 
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
    public struct AgroDrop
    {
        public uint EntityID;
        public uint PlayerID;
        public uint EnemyID;
    }

    public class AgroDropComponent : GameComponent<AgroDrop>
    {
        new public void Add(uint entityID, AgroDrop component)
        {
            base.Add(entityID, component);

            DungeonCrawlerGame game = DungeonCrawlerGame.game;
            if (component.EnemyID > 0)
            {
                EnemyAI enemyAI = game.EnemyAIComponent[component.EnemyID];
                enemyAI.NoTargetList.Add(component.PlayerID);
                game.EnemyAIComponent[component.EnemyID] = enemyAI;
                game.EnemyAISystem.GetDifferentTarget(component.EnemyID);
            }
        }

        new public void Remove(uint entityID)
        {
            DungeonCrawlerGame game = DungeonCrawlerGame.game;
            AgroDrop component = game.AgroDropComponent[entityID];
            if (component.EnemyID > 0)
            {
                EnemyAI enemyAI = game.EnemyAIComponent[component.EnemyID];

                enemyAI.NoTargetList.Remove(component.PlayerID);

                game.EnemyAIComponent[component.EnemyID] = enemyAI;
                game.EnemyAISystem.GetClosestTarget(component.EnemyID);
            }
            base.Remove(entityID);
        }
    }
}
