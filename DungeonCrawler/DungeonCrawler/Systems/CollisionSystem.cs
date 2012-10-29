#region File Description
//-----------------------------------------------------------------------------
// CollisionSystem.cs 
//
// Author: Devin Kelly-Collins, Matthew McHaney
//
// Modified: 
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
using DungeonCrawler.Components;
using Microsoft.Xna.Framework;

namespace DungeonCrawler.Systems
{
    public class CollisionSystem
    {
        private enum CollisionType
        {
            None = 0x0,
            Static = 0x1,
            Player = 0x2,
            Enemy = 0x4,
            Bullet = 0x8,
            Collectible = 0x10,
            //Door =  0x20, To Be Implemented
            //Wall = 0x40, To Be Implemented
            PlayerEnemy = 0x6,
            PlayerBullet = 0xA,
            PlayerStatic = 0x3,
            PlayerCollectible = 0x12,
            EnemyBullet = 0xC,
            EnemyStatic = 0x5,
            BulletStatic = 0x9,
        }

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
            uint roomID;

            foreach (Player player in _game.PlayerComponent.All)
            {
                //Get all positions in the room.
                roomID = _game.PositionComponent[player.EntityID].RoomID;
                IEnumerable<Position> positionsInRoom = _game.PositionComponent.InRoom(roomID);

                List<Collideable> collideablesInRoom = new List<Collideable>();

                foreach (Position position in positionsInRoom)
                {
                    if (_game.CollisionComponent.Contains(position.EntityID))
                        collideablesInRoom.Add(_game.CollisionComponent[position.EntityID]);
                }

                for (int i = 0; i < collideablesInRoom.Count; i++)
                {
                    for (int j = i + 1; j < collideablesInRoom.Count; j++)
                    {
                        //Uh...... Optimize later

                        if (!collideablesInRoom[i].Bounds.Intersect(collideablesInRoom[j].Bounds))
                            continue;

                        CollisionType type = getCollisionType(collideablesInRoom[i].EntityID, collideablesInRoom[j].EntityID);
                        switch (type)
                        {
                            case CollisionType.Player:
                                PlayerPlayerCollision(collideablesInRoom[i].EntityID, collideablesInRoom[j].EntityID);
                                break;
                            case CollisionType.PlayerEnemy:
                                PlayerEnemyCollision(collideablesInRoom[i].EntityID, collideablesInRoom[j].EntityID);
                                break;
                            case CollisionType.PlayerBullet:
                                PlayerBulletCollision(collideablesInRoom[i].EntityID, collideablesInRoom[j].EntityID);
                                break;
                            case CollisionType.PlayerStatic:
                                PlayerStaticCollision(collideablesInRoom[i].EntityID, collideablesInRoom[j].EntityID);
                                break;
                            case CollisionType.PlayerCollectible:
                                PlayerCollectibleCollision(collideablesInRoom[i].EntityID, collideablesInRoom[j].EntityID);
                                break;
                            case CollisionType.Enemy:
                                EnemyEnemyCollision(collideablesInRoom[i].EntityID, collideablesInRoom[j].EntityID);
                                break;
                            case CollisionType.EnemyBullet:
                                EnemyBulletCollision(collideablesInRoom[i].EntityID, collideablesInRoom[j].EntityID);
                                break;
                            case CollisionType.EnemyStatic:
                                EnemyStaticCollision(collideablesInRoom[i].EntityID, collideablesInRoom[j].EntityID);
                                break;
                            case CollisionType.BulletStatic:
                                BulletStaticCollision(collideablesInRoom[i].EntityID, collideablesInRoom[j].EntityID);
                                break;
                        }
                    }
                }
            }
        }

        private void PlayerCollectibleCollision(uint p, uint p_2)
        {
            //pick up the collectible, kill it
            uint playerID, collectibleID;
            if (_game.PlayerComponent.Contains(p))
            {
                playerID = p;
                collectibleID = p_2;
            }
            else
            {
                collectibleID = p;
                playerID = p_2;
            }

            //Handle the collectible adding type

            _game.GarbagemanSystem.ScheduleVisit(collectibleID, GarbagemanSystem.ComponentType.Collectible);
        }

        private void BulletStaticCollision(uint p, uint p_2)
        {
            //Remove the bullet

            uint bulletId;
            if (_game.BulletComponent.Contains(p))
                bulletId = p;
            else
                bulletId = p_2;

            _game.GarbagemanSystem.ScheduleVisit(bulletId, GarbagemanSystem.ComponentType.Bullet);
        }

        private void EnemyStaticCollision(uint p, uint p_2)
        {
            //Set enemy against object

            throw new NotImplementedException();
        }

        private void EnemyBulletCollision(uint p, uint p_2)
        {
            //Remove the bullet, calculate damage/knockback?

            throw new NotImplementedException();
        }

        private void EnemyEnemyCollision(uint p, uint p_2)
        {
            //Set enemies against each other

            throw new NotImplementedException();
        }

        private void PlayerStaticCollision(uint p, uint p_2)
        {
            //Stop player movement.

            throw new NotImplementedException();
        }

        private void PlayerBulletCollision(uint p, uint p_2)
        {
            //Delete bullet, calculate damage.

            uint bulletId, playerId;
            if (_game.BulletComponent.Contains(p))
            {
                bulletId = p;
                playerId = p_2;
            }
            else
            {
                playerId = p;
                bulletId = p_2;
            }

            _game.GarbagemanSystem.ScheduleVisit(bulletId, GarbagemanSystem.ComponentType.Bullet);

            Vector2 directionOfKnockback = (_game.PositionComponent[bulletId].Center -
                _game.PositionComponent[playerId].Center);
            if (directionOfKnockback == Vector2.Zero) directionOfKnockback = new Vector2(1, 0);

            directionOfKnockback.Normalize();


            //knockback
            Position newLocation = _game.PositionComponent[playerId];
            newLocation.Center -= directionOfKnockback * 10;

            _game.PositionComponent[playerId] = newLocation; 
        }

        private void PlayerEnemyCollision(uint p, uint p_2)
        {
            //Damage player, knockback?

            throw new NotImplementedException();
        }

        private void PlayerPlayerCollision(uint p, uint p_2)
        {
            //Put up against each other

            //Find the direction, set to 1,0 if nothing, Normalize,
            // Find the radii - distance, divide by 2, set away

            

            throw new NotImplementedException();
        }

        private CollisionType getCollisionType(uint p, uint p_2)
        {
            CollisionType obj1 = CollisionType.None;
            if (_game.PlayerComponent.Contains(p))
                obj1 = CollisionType.Player;
            else if (false) //Enemy
                obj1 = CollisionType.Enemy;
            else if (_game.BulletComponent.Contains(p))
                obj1 = CollisionType.Bullet;
            else if (_game.CollectibleComponent.Contains(p))
                obj1 = CollisionType.Collectible;
            else if (false) //Static
                obj1 = CollisionType.Static;

            CollisionType obj2 = CollisionType.None;
            if (_game.PlayerComponent.Contains(p_2))
                obj2 = CollisionType.Player;
            else if (false) //Enemy
                obj2 = CollisionType.Enemy;
            else if (_game.BulletComponent.Contains(p_2))
                obj2 = CollisionType.Bullet;
            else if (_game.CollectibleComponent.Contains(p_2))
                obj2 = CollisionType.Collectible;
            else if (false) //Static
                obj2 = CollisionType.Static;

            return obj1 | obj2;  
        }
    }
}
