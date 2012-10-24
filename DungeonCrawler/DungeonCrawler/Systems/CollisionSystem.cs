using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DungeonCrawler.Components;

namespace DungeonCrawler.Systems
{
    public class CollisionSystem
    {
        /// <summary>
        /// The parent game.
        /// </summary>
        private DungeonCrawlerGame _game;

        /// <summary>
        /// Creates a new collision system.
        /// </summary>
        /// <param name="game">The game this systems belongs to.</param>
        public CollisionSystem(DungeonCrawlerGame game)
        {
            _game = game;
        }

        /// <summary>
        /// Updates all the collisions currently in the game.
        /// </summary>
        /// <param name="elaspedTime">time since the last call to this method.</param>
        public void Update(float elaspedTime)
        {
            
        }
    }

    public class Collision
    {
        public uint Entity1ID;
        public uint Entity2ID;

        public Collision(uint e1, uint e2)
        {
            Entity1ID = e1;
            Entity2ID = e2;
        }

        public override bool Equals(object obj)
        {
            if (obj is Collision)
                return (obj as Collision).Entity1ID == this.Entity1ID && (obj as Collision).Entity2ID == this.Entity2ID;
            else
                return false;
        }
    }
}
