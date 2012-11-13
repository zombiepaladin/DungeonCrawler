#region File Description
//-----------------------------------------------------------------------------
// AgroGainComponent.cs 
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
using Microsoft.Xna.Framework;

namespace DungeonCrawler.Components
{
    public struct AgroGain
    {
        public uint EntityID;
        public uint PlayerID;
        public uint EnemyID;

        public bool GainAll;
    }

    public class AgroGainComponent : GameComponent<AgroGain>
    {

        new public void Add(uint entityID, AgroGain component)
        {
            base.Add(entityID, component);
            DungeonCrawlerGame game = DungeonCrawlerGame.game;

            game.EnemyAISystem.SetTarget(component.EnemyID, component.PlayerID);

        }

        new public void Remove(uint entityID)
        {
            AgroGain component = DungeonCrawlerGame.game.AgroGainComponent[entityID];

            DungeonCrawlerGame.game.EnemyAISystem.GetClosestTarget(component.EnemyID);

            base.Remove(entityID);
        }

    }
}
