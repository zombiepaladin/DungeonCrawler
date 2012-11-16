#region File Description
//-----------------------------------------------------------------------------
// CollisionSystem.cs 
//
// Author: Devin Kelly-Collins, Matthew McHaney
//
// Modified: Nicholas Strub - Added Room Transitioning ability based on player/door collisions 10/31/2012
// Modified: Nicholas Strub - Updated Player/Door Collisions (11/3/2012)
// Modified: Devin Kelly-Collins - Added Weapon, PlayerWeapon, EnemyWeapon to CollisionType. Update switch statement in Update. Added PlayerWeaponCollision and EnemyWeaponCollision methods. Added DoDamage methods. (11/15/12)
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
using Microsoft.Xna.Framework.Graphics;

namespace DungeonCrawler.Systems
{
    public class CollisionSystem
    {
        /* To add in collision logic:
         * To add collision logic between two components you only need to fill in the correct method. So if you wanted to implement the Player on Static collision you would fill in the PlayerStaticCollision method.
         * Each of these methods take in the two colliding entity IDs. There is no guarantee on the type of the entity IDs, so you will need to determine this in your method.
         * If you created a new object type to collide with you will need to add the proper types in CollisionType, update the switch statement in Update, and add the type to getCollisionType.
         */

        /// <summary>
        /// Different collisions. The first group are individual components. We use bitmask to determine what the actual collision is in the end.
        /// For example Player on Static collision == Player | Static or PlayerStatic.
        /// </summary>
        private enum CollisionType
        {
            None = 0x0,
            Static = 0x1,
            Player = 0x2,
            Enemy = 0x4,
            Bullet = 0x8,
            Collectible = 0x10,
            Door =  0x20,
            Trigger = 0x40,
            Weapon = 0x80,
            Skill = 0x100,

            PlayerEnemy = 0x6,
            PlayerBullet = 0xA,
            PlayerStatic = 0x3,
            PlayerCollectible = 0x12,
            PlayerDoor = 0x22,
            PlayerTrigger = 0x42,
            PlayerWeapon = 0x82,

            EnemyBullet = 0xC,
            EnemyStatic = 0x5,
            EnemyDoor   = 0x24,
            EnemyWeapon = 0x84,

            BulletStatic = 0x9,
            BulletDoor = 0x28,
            
            SkillStatic = 0x101,
            SkillPlayer = 0x102,
            SkillEnemy =0x104,
            SkillBullet =0x108,
            SkillCollectible=0x110,
            SkillDoor=0x120,
            
           
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
                //Get all collisions in the room.
                roomID = _game.PositionComponent[player.EntityID].RoomID;
                List<Collideable> collideablesInRoom = _game.CollisionComponent.InRoom(roomID);

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
                            case CollisionType.PlayerDoor:
                                PlayerDoorCollision(collideablesInRoom[i].EntityID, collideablesInRoom[j].EntityID);
                                break;
                            case CollisionType.PlayerCollectible:
                                PlayerCollectibleCollision(collideablesInRoom[i].EntityID, collideablesInRoom[j].EntityID);
                                break;
                            case CollisionType.PlayerTrigger:
                                PlayerTriggerCollision(collideablesInRoom[i].EntityID, collideablesInRoom[j].EntityID);
                                break;
                            case CollisionType.PlayerWeapon:
                                PlayerWeaponCollision(collideablesInRoom[i].EntityID, collideablesInRoom[j].EntityID);
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
                            case CollisionType.EnemyDoor:
                                EnemyDoorCollision(collideablesInRoom[i].EntityID, collideablesInRoom[j].EntityID);
                                break;
                            case CollisionType.EnemyWeapon:
                                EnemyWeaponCollision(collideablesInRoom[i].EntityID, collideablesInRoom[j].EntityID);
                                break;
                            case CollisionType.BulletStatic:
                                BulletStaticCollision(collideablesInRoom[i].EntityID, collideablesInRoom[j].EntityID);
                                break;
                            case CollisionType.BulletDoor:
                                BulletDoorCollision(collideablesInRoom[i].EntityID, collideablesInRoom[j].EntityID);
                                break;
                            case CollisionType.SkillBullet:
                            case CollisionType.SkillCollectible:
                            case CollisionType.SkillDoor:
                            case CollisionType.SkillEnemy:
                            case CollisionType.SkillStatic:
                                SkillCollision(collideablesInRoom[i].EntityID, collideablesInRoom[j].EntityID);
                                break;
                            case CollisionType.SkillPlayer:
                                break;
                                

                        }
                    }
                }
            }
        }

        private void PlayerWeaponCollision(uint p, uint p_2)
        {
            //Do nothing for now.
        }

        private void EnemyWeaponCollision(uint p, uint p_2)
        {
            Enemy enemy;
            Weapon weapon;
            if (_game.WeaponComponent.Contains(p))
            {
                weapon = _game.WeaponComponent[p];
                enemy = _game.EnemyComponent[p_2];
            }
            else
            {
                weapon = _game.WeaponComponent[p_2];
                enemy = _game.EnemyComponent[p];
            }

            //Process attack.
            DoDamage(enemy, weapon.Damage);

            //Update enemy info.
            _game.EnemyComponent[enemy.EntityID] = enemy;

            //Remove collision imiediatly.
            _game.CollisionComponent.Remove(weapon.EntitiyID);
        }

        /// <summary>
        /// Handles Player on Collectible collisions.
        /// </summary>
        /// <param name="p">First entityID</param>
        /// <param name="p_2">Second entityID</param>
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

        /// <summary>
        /// Handles Bullet/Door collisions.
        /// </summary>
        /// <param name="p">First entityID</param>
        /// <param name="p_2">Second entityID</param>
        private void BulletDoorCollision(uint p, uint p_2)
        {
            //Destroy the bullet if door is closed

            uint bulletId, doorId;
            if (_game.BulletComponent.Contains(p))
            {
                bulletId = p;
                doorId = p_2;
            }
            else
            {
                doorId = p;
                bulletId = p_2;
            }
            if(_game.DoorComponent[doorId].Locked || _game.DoorComponent[doorId].Closed)
                _game.GarbagemanSystem.ScheduleVisit(bulletId, GarbagemanSystem.ComponentType.Bullet);
        }

        /// <summary>
        /// Handles Bullet/Static collisions.
        /// </summary>
        /// <param name="p">First entityID</param>
        /// <param name="p_2">Second entityID</param>
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

        /// <summary>
        /// Handles Enemy/Door collisions.
        /// </summary>
        /// <param name="p">First entityID</param>
        /// <param name="p_2">Second entityID</param>
        private void EnemyDoorCollision(uint p, uint p_2)
        {
            //Just block them
            //Stop player movement.

            //Get closest point on the rectangle, angle between player and point, and push back radius amount
            uint doorId, enemyId;
            if (_game.DoorComponent.Contains(p))
            {
                doorId = p;
                enemyId = p_2;
            }
            else
            {
                enemyId = p;
                doorId = p_2;
            }

            Bounds b = _game.CollisionComponent[doorId].Bounds;
            Position enemyPos = _game.PositionComponent[enemyId];
            //Assuming the door is always a rectangle

            Vector2 closestPos = ((RectangleBounds)b).GetClosestPoint(enemyPos.Center);
            double angle = Math.Atan2(closestPos.Y - enemyPos.Center.Y, closestPos.X - enemyPos.Center.X);

            double x = closestPos.X - (Math.Cos(angle) * (enemyPos.Radius));
            double y = closestPos.Y - (Math.Sin(angle) * (enemyPos.Radius));

            enemyPos.Center = new Vector2((float)x, (float)y);

            _game.PositionComponent[enemyId] = enemyPos;
        }

        /// <summary>
        /// Handles Enemy/Static collisions.
        /// </summary>
        /// <param name="p">First entityID</param>
        /// <param name="p_2">Second entityID</param>
        private void EnemyStaticCollision(uint p, uint p_2)
        {
            //Get closest point on the rectangle, angle between player and point, and push back radius amount
            uint staticId, enemyId;
            if(_game.EnemyComponent.Contains(p))
            {
                enemyId = p;
                staticId = p_2;
            }
            else
            {
                staticId = p;
                enemyId = p_2;
            }

            Bounds b = _game.CollisionComponent[staticId].Bounds;
            Position enemyPos = _game.PositionComponent[enemyId];

            if (b.GetType() == typeof(RectangleBounds))
            {
                //Get the closest point on the rectangle
                Vector2 closestPos = ((RectangleBounds)b).GetClosestPoint(enemyPos.Center);
                double angle = Math.Atan2(closestPos.Y - enemyPos.Center.Y, closestPos.X - enemyPos.Center.X);

                double x = closestPos.X - (Math.Cos(angle) * (enemyPos.Radius));
                double y = closestPos.Y - (Math.Sin(angle) * (enemyPos.Radius));

                enemyPos.Center = new Vector2((float)x, (float)y);

                _game.PositionComponent[enemyId] = enemyPos;
            }
            else //is circle
            {
                //static won't move, so just place enemy out there

                CircleBounds circle = ((CircleBounds)b);
                double angle = Math.Atan2(circle.Center.Y - enemyPos.Center.Y,
                    circle.Center.X - enemyPos.Center.X);

                double x = circle.Center.X - (Math.Cos(angle) * (enemyPos.Radius + circle.Radius));
                double y = circle.Center.Y - (Math.Sin(angle) * (enemyPos.Radius + circle.Radius));

                enemyPos.Center = new Vector2((float)x, (float)y);

                _game.PositionComponent[enemyId] = enemyPos;
            }
        }

        /// <summary>
        /// Handles Enemy/Bullet collisions.
        /// </summary>
        /// <param name="p">First entityID</param>
        /// <param name="p_2">Second entityID</param>
        private void EnemyBulletCollision(uint p, uint p_2)
        {
            //Delete bullet, calculate damage.

            uint bulletId, enemyId;
            if (_game.BulletComponent.Contains(p))
            {
                bulletId = p;
                enemyId = p_2;
            }
            else
            {
                enemyId = p;
                bulletId = p_2;
            }

            _game.GarbagemanSystem.ScheduleVisit(bulletId, GarbagemanSystem.ComponentType.Bullet);

            //Factor in damage
            Enemy enemy = _game.EnemyComponent[enemyId];
            DoDamage(enemy, _game.BulletComponent[bulletId].Damage);

            if (_game.MovementComponent.Contains(enemyId))
            {
                Vector2 directionOfKnockback = (_game.PositionComponent[bulletId].Center -
                    _game.PositionComponent[enemyId].Center);
                if (directionOfKnockback == Vector2.Zero) directionOfKnockback = new Vector2(1, 0);

                directionOfKnockback.Normalize();

                //knockback
                Position newLocation = _game.PositionComponent[enemyId];
                newLocation.Center -= directionOfKnockback * 10;

                _game.PositionComponent[enemyId] = newLocation;
            }
        }

        /// <summary>
        /// Handles Enemy collisions.
        /// </summary>
        /// <param name="p">First entityID</param>
        /// <param name="p_2">Second entityID</param>
        private void EnemyEnemyCollision(uint p, uint p_2)
        {
            //Set enemies against each other

            
            
            throw new NotImplementedException();
        }

        /// <summary>
        /// Handles Player/Door collisions.
        /// </summary>
        /// <param name="p">First entityID</param>
        /// <param name="p_2">Second entityID</param>
        private void PlayerDoorCollision(uint p, uint p_2)
        {
            //Stop player movement.

            //Get closest point on the rectangle, angle between player and point, and push back radius amount
            uint doorId, playerId;
            if (_game.PlayerComponent.Contains(p))
            {
                playerId = p;
                doorId = p_2;
            }
            else
            {
                doorId = p;
                playerId = p_2;
            }

            if(_game.DoorComponent[doorId].Locked)
            {
                Bounds b = _game.CollisionComponent[doorId].Bounds;
                Position playerPos = _game.PositionComponent[playerId];
                //Assuming the door is always a rectangle

                Vector2 closestPos = ((RectangleBounds)b).GetClosestPoint(playerPos.Center);
                double angle = Math.Atan2(closestPos.Y - playerPos.Center.Y, closestPos.X - playerPos.Center.X);

                double x = closestPos.X - (Math.Cos(angle) * (playerPos.Radius));
                double y = closestPos.Y - (Math.Sin(angle) * (playerPos.Radius));

                playerPos.Center = new Vector2((float)x, (float)y);

                _game.PositionComponent[playerId] = playerPos;
            }
            else //is unlocked
            {
                _game.RoomChangingSystem.ChangeRoom(_game.DoorComponent[doorId]);
            }

        }

        /// <summary>
        /// Handles Player/Static collisions.
        /// </summary>
        /// <param name="p">First entityID</param>
        /// <param name="p_2">Second entityID</param>
        private void PlayerStaticCollision(uint p, uint p_2)
        {
            //Stop player movement.

            //Get closest point on the rectangle, angle between player and point, and push back radius amount
            uint staticId, playerId;
            if (_game.PlayerComponent.Contains(p))
            {
                playerId = p;
                staticId = p_2;
            }
            else
            {
                staticId = p;
                playerId = p_2;
            }

            Bounds b = _game.CollisionComponent[staticId].Bounds;
            Position playerPos = _game.PositionComponent[playerId];

            if (b.GetType() == typeof(RectangleBounds))
            {
                //Get the closest point on the rectangle
                Vector2 closestPos = ((RectangleBounds)b).GetClosestPoint(playerPos.Center);
                double angle = Math.Atan2(closestPos.Y - playerPos.Center.Y, closestPos.X - playerPos.Center.X);

                double x = closestPos.X - (Math.Cos(angle) * (playerPos.Radius));
                double y = closestPos.Y - (Math.Sin(angle) * (playerPos.Radius));

                playerPos.Center = new Vector2((float)x, (float)y);

                _game.PositionComponent[playerId] = playerPos;
            }
            else //is circle
            {
                //static won't move, so just place player out there

                CircleBounds circle = ((CircleBounds)b);
                double angle = Math.Atan2(circle.Center.Y - playerPos.Center.Y, 
                    circle.Center.X - playerPos.Center.X);

                double x = circle.Center.X - (Math.Cos(angle) * (playerPos.Radius + circle.Radius));
                double y = circle.Center.Y - (Math.Sin(angle) * (playerPos.Radius + circle.Radius));

                playerPos.Center = new Vector2((float)x, (float)y);

                _game.PositionComponent[playerId] = playerPos;
            }


        }

        /// <summary>
        /// Handles Player/Bullet collisions.
        /// </summary>
        /// <param name="p">First entityID</param>
        /// <param name="p_2">Second entityID</param>
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

            DoDamage(_game.PlayerComponent[playerId], _game.BulletComponent[bulletId].Damage);

            Vector2 directionOfKnockback = (_game.PositionComponent[bulletId].Center -
                _game.PositionComponent[playerId].Center);
            if (directionOfKnockback == Vector2.Zero) directionOfKnockback = new Vector2(1, 0);

            directionOfKnockback.Normalize();


            //knockback
            Position newLocation = _game.PositionComponent[playerId];
            newLocation.Center -= directionOfKnockback * 10;

            _game.PositionComponent[playerId] = newLocation; 
        }

        /// <summary>
        /// Handles Player/Enemy collisions.
        /// </summary>
        /// <param name="p">First entityID</param>
        /// <param name="p_2">Second entityID</param>
        private void PlayerEnemyCollision(uint p, uint p_2)
        {
            //Need to separate into 4 types: stationary&painless, stationary&painful, moving&painless, moving&painful

            uint enemyId, playerId;
            if (_game.EnemyComponent.Contains(p))
            {
                enemyId = p;
                playerId = p_2;
            }
            else
            {
                playerId = p;
                enemyId = p_2;
            }

            Enemy enemy = _game.EnemyComponent[enemyId];

            bool moving = _game.MovementComponent.Contains(enemyId);

            if (!enemy.HurtOnTouch && !moving)
            {
                //Stationary & Painless
                //Act like a static

                Bounds b = _game.CollisionComponent[enemyId].Bounds;
                Position playerPos = _game.PositionComponent[playerId];

                if (b.GetType() == typeof(RectangleBounds))
                {
                    //Get the closest point on the rectangle
                    Vector2 closestPos = ((RectangleBounds)b).GetClosestPoint(playerPos.Center);
                    double angle = Math.Atan2(closestPos.Y - playerPos.Center.Y, closestPos.X - playerPos.Center.X);

                    double x = closestPos.X - (Math.Cos(angle) * (playerPos.Radius));
                    double y = closestPos.Y - (Math.Sin(angle) * (playerPos.Radius));

                    playerPos.Center = new Vector2((float)x, (float)y);

                    _game.PositionComponent[playerId] = playerPos;
                }
                else //is circle
                {
                    //static won't move, so just place player out there

                    CircleBounds circle = ((CircleBounds)b);
                    double angle = Math.Atan2(circle.Center.Y - playerPos.Center.Y,
                        circle.Center.X - playerPos.Center.X);

                    double x = circle.Center.X - (Math.Cos(angle) * (playerPos.Radius + circle.Radius));
                    double y = circle.Center.Y - (Math.Sin(angle) * (playerPos.Radius + circle.Radius));

                    playerPos.Center = new Vector2((float)x, (float)y);

                    _game.PositionComponent[playerId] = playerPos;
                }
            }
            else if (enemy.HurtOnTouch && !moving)
            {
                //Stationary & Painful
                DoDamage(_game.PlayerComponent[playerId], 10); //Replace 10 with something dynamic.
            }
            else if (!enemy.HurtOnTouch && moving)
            {
                //For now, let's have it just push the person around. But we need to make sure eventually that
                //complicated nonviolent collisions is handled. That way pushing blocks/people walking into each other
                //will be done properly.

                //Act like a static

                Bounds b = _game.CollisionComponent[enemyId].Bounds;
                Position playerPos = _game.PositionComponent[playerId];

                if (b.GetType() == typeof(RectangleBounds))
                {
                    //Get the closest point on the rectangle
                    Vector2 closestPos = ((RectangleBounds)b).GetClosestPoint(playerPos.Center);
                    double angle = Math.Atan2(closestPos.Y - playerPos.Center.Y, closestPos.X - playerPos.Center.X);

                    double x = closestPos.X - (Math.Cos(angle) * (playerPos.Radius));
                    double y = closestPos.Y - (Math.Sin(angle) * (playerPos.Radius));

                    playerPos.Center = new Vector2((float)x, (float)y);

                    _game.PositionComponent[playerId] = playerPos;
                }
                else //is circle
                {
                    //static won't move, so just place player out there

                    CircleBounds circle = ((CircleBounds)b);
                    double angle = Math.Atan2(circle.Center.Y - playerPos.Center.Y,
                        circle.Center.X - playerPos.Center.X);

                    double x = circle.Center.X - (Math.Cos(angle) * (playerPos.Radius + circle.Radius));
                    double y = circle.Center.Y - (Math.Sin(angle) * (playerPos.Radius + circle.Radius));

                    playerPos.Center = new Vector2((float)x, (float)y);

                    _game.PositionComponent[playerId] = playerPos;
                }
            }
            else
            {
                //Moving & Painful
                DoDamage(_game.PlayerComponent[playerId], 10); //Replace 10 with something dynamic.
            }

        }

        
        private void PlayerPlayerCollision(uint p, uint p_2)
        {
            //Put up against each other

            //Find the direction, set to 1,0 if nothing, Normalize,
            // Find the radii - distance, divide by 2, set away

            

            throw new NotImplementedException();
        }

        /// <summary>
        /// Handles Player/Trigger collisions.
        /// </summary>
        /// <param name="p">First entityID</param>
        /// <param name="p_2">Second entityID</param>
        private void PlayerTriggerCollision(uint p, uint p_2)
        {
            uint playerId, triggerId;
            if (_game.TriggerComponent.Contains(p))
            {
                triggerId = p;
                playerId = p_2;
            }
            else
            {
                playerId = p;
                triggerId = p_2;
            }
            
            //Put your code here

            throw new NotImplementedException();
        }

        private void SkillCollision(uint p, uint p_2)
        {  
            //TODO: actuall skill collision
            uint skillId, oId;
            if (_game.SkillProjectileComponent.Contains(p))
            {
                skillId = p;
                oId = p_2;
            }
            else
            {
                skillId = p_2;
                oId = p;
            }
            if (!(_game.SkillAoEComponent.Contains(skillId) || _game.SkillDeployableComponent.Contains(skillId)) )
            {
                _game.GarbagemanSystem.ScheduleVisit(skillId, GarbagemanSystem.ComponentType.Skill);
            }
        }

        /// <summary>
        /// Retrives the collision type between the two eids
        /// </summary>
        /// <param name="p">First eid</param>
        /// <param name="p_2">Second eid</param>
        /// <returns>The collision type.</returns>
        private CollisionType getCollisionType(uint p, uint p_2)
        {
            CollisionType obj1 = CollisionType.None;
            if (_game.PlayerComponent.Contains(p))
                obj1 = CollisionType.Player;
            else if (_game.EnemyComponent.Contains(p))
                obj1 = CollisionType.Enemy;
            else if (_game.BulletComponent.Contains(p))
                obj1 = CollisionType.Bullet;
            else if (_game.CollectibleComponent.Contains(p))
                obj1 = CollisionType.Collectible;
            else if (_game.DoorComponent.Contains(p))
                obj1 = CollisionType.Door;
            else if (_game.TriggerComponent.Contains(p))
                obj1 = CollisionType.Trigger;
            else if (_game.SkillProjectileComponent.Contains(p) || _game.SkillAoEComponent.Contains(p)||_game.SkillDeployableComponent.Contains(p))
                obj1 = CollisionType.Skill;
            else if (_game.WeaponComponent.Contains(p))
                obj1 = CollisionType.Weapon;
            else //Static
                obj1 = CollisionType.Static;

            CollisionType obj2 = CollisionType.None;
            if (_game.PlayerComponent.Contains(p_2))
                obj2 = CollisionType.Player;
            else if (_game.EnemyComponent.Contains(p_2))
                obj2 = CollisionType.Enemy;
            else if (_game.BulletComponent.Contains(p_2))
                obj2 = CollisionType.Bullet;
            else if (_game.CollectibleComponent.Contains(p_2))
                obj2 = CollisionType.Collectible;
            else if (_game.DoorComponent.Contains(p_2))
                obj2 = CollisionType.Door;
            else if (_game.TriggerComponent.Contains(p_2))
                obj2 = CollisionType.Trigger;
            else if(_game.SkillProjectileComponent.Contains(p_2) || _game.SkillAoEComponent.Contains(p_2)||_game.SkillDeployableComponent.Contains(p_2))
                obj2 = CollisionType.Skill;
            else if (_game.WeaponComponent.Contains(p_2))
                obj2 = CollisionType.Weapon;
            else //Static
                obj2 = CollisionType.Static;

            return obj1 | obj2;  
        }

        /// <summary>
        /// Applies damage and creates an actorText to show it.
        /// </summary>
        /// <param name="player">Player to deal damage to</param>
        /// <param name="damage">Amount of damage to do.</param>
        private void DoDamage(Player player, float damage)
        {
            PlayerInfo info = _game.PlayerInfoComponent[player.EntityID];
            info.Health -= (int)damage;
            _game.PlayerInfoComponent[player.EntityID] = info;

            _game.ActorTextComponent.Add(player.EntityID, damage.ToString());
        }

        /// <summary>
        /// Applies damage and creates an actorText to show it.
        /// </summary>
        /// <param name="enemy">Enemy to apply damge to.</param>
        /// <param name="damage">Damage to apply</param>
        private void DoDamage(Enemy enemy, float damage)
        {
            enemy.Health -= damage;
            _game.EnemyComponent[enemy.EntityID] = enemy;

            _game.ActorTextComponent.Add(enemy.EntityID, damage.ToString());
        }
    }
}
