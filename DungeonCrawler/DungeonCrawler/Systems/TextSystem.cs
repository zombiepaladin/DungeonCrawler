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

        private DungeonCrawlerGame _game;
        private ActorTextComponent _actorTextComponent;
        private PlayerComponent _playerComponent;
        private EnemyComponent _enemyComponent;

        public TextSystem(DungeonCrawlerGame game)
        {
            _game = game;
            _actorTextComponent = _game.ActorTextComponent;
            _playerComponent = _game.PlayerComponent;
            _enemyComponent = _game.EnemyComponent;
        }

        public void Update(float elapsedTime)
        {
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
        }
    }
}
