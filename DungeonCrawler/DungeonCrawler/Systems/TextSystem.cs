#region File Description
//-----------------------------------------------------------------------------
// TextSystem.cs 
//
// Author: Devin Kelly-Collins
//
// Modified: Nick Stanley added HUDSpriteComponent, 10/15/2012
// Modified: Devin Kelly-Collins added WeaponSprite rendering, 10/24/2012
// Modified: Samuel Fike and Jiri Malina: Fixed errors due to removal of movementSprite for players
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence 
//-----------------------------------------------------------------------------

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DungeonCrawler.Components;

namespace DungeonCrawler.Systems
{
    public class TextSystem
    {
        private const int MAX_OFFSET = 100;
        private const int INC_OFFSET = 25;
        private const float UPDATE_TIME = .1f;

        private DungeonCrawlerGame _game;
        private ActorTextComponent _actorTextComponent;
        private PlayerComponent _playerComponent;
        private EnemyComponent _enemyComponent;

        private float _timer;

        public TextSystem(DungeonCrawlerGame game)
        {
            _game = game;
            _actorTextComponent = _game.ActorTextComponent;
            _playerComponent = _game.PlayerComponent;
            _enemyComponent = _game.EnemyComponent;
        }

        public void Update(float elapsedTime)
        {
            _timer += elapsedTime;

            if (_timer < UPDATE_TIME)
                return;

            foreach(Player player in _playerComponent.All)
            {
                if (!_actorTextComponent.Contains(player.EntityID))
                    continue;

                uint playerID = player.EntityID;
                ActorText actorText = _actorTextComponent[playerID];
                actorText.Offset += INC_OFFSET;
                if (actorText.Offset >= MAX_OFFSET)
                    _actorTextComponent.Remove(playerID);
                else
                    _actorTextComponent[playerID] = actorText;
            }

            foreach (Enemy enemy in _enemyComponent.All)
            {
                if (!_actorTextComponent.Contains(enemy.EntityID))
                    continue;

                uint enemyID = enemy.EntityID;
                ActorText actorText = _actorTextComponent[enemyID];
                actorText.Offset += INC_OFFSET;
                if (actorText.Offset >= MAX_OFFSET)
                    _actorTextComponent.Remove(enemyID);
                else
                    _actorTextComponent[enemyID] = actorText;
            }

            _timer = 0;
        }
    }
}
