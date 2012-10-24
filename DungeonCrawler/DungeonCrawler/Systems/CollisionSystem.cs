using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DungeonCrawler.Components;
using Microsoft.Xna.Framework;

namespace DungeonCrawler.Systems
{
    public class CollisionSystem
    {
        /// <summary>
        /// The parent game.
        /// </summary>
        private DungeonCrawlerGame _game;

        private List<Collision> _collisions;

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
            uint roomID;

            foreach (Player player in _game.PlayerComponent.All)
            {
                roomID = _game.PositionComponent[player.EntityID].RoomID;
                IEnumerable<Position> positionsInRoom = _game.PositionComponent.InRoom(roomID);

                foreach (Position position in positionsInRoom)
                {
                    IEnumerable<Position> collisions = positionsInRoom.InRegion(position.Center, position.Radius);
                    if(collisions.Count() > 0)
                    {
                        foreach(Position collidingPosition in collisions)
                        {
                            if(_game.PlayerComponent.Contains(collidingPosition.EntityID) &&
                                _game.PlayerComponent.Contains(position.EntityID))
                            {
                                //Player on Player collision
                            }
                            //etc
                        }
                    }
                }
            }
        }

        private void findCollisions(IEnumerable<Position> positions, Vector2 center, float radius)
        {
            IEnumerable<Position> positionsInRegion = positions.InRegion(center, radius);
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
