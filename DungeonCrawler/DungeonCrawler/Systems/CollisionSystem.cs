#region File Description
//-----------------------------------------------------------------------------
// CollisionSystem.cs 
//
// Author: Devin Kelly-Collins, Matthew McHaney
//
// Modified: Nicholas Strub - Added Room Transitioning ability based on player/door collisions 10/31/2012
// Modified: Nicholas Strub - Updated Player/Door Collisions (11/3/2012)
// Modified: Devin Kelly-Collins - Added Weapon, PlayerWeapon, EnemyWeapon to CollisionType. Update switch statement in Update. Added PlayerWeaponCollision and EnemyWeaponCollision methods. Added DoDamage methods. (11/15/12)
// Modified: Nick Boen - Added GetClosestEnemy() method to retrieve the closest enemy to a given position
//                       I included it here because I felt that it best fit, though there may be a better place for it...
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
using DungeonCrawler.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DungeonCrawler.Systems;

namespace DungeonCrawler.Systems
{
    public struct DistanceToTarget
    {
        public uint eid;
        public float distance;
    }

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
            NPC = 0x200,
            ExplodingDroid = 0x1000,
            HealingStation = 0x400,
            PortableShield = 0x800,
            MatchingPuzzlePiece = 0x1200,

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

            NPCStatic = 0x201,
            NPCPlayer = 0x202,
            NPCEnemy = 0x204,
            NPCDoor = 0x220,
            
            DroidEnemy=0x1004,
            StationPlayer=0x402,
            ShieldBullet=0x808,
            ShieldEnemy=0x804,


            
           
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

            List<Player> players = new List<Player>();
            foreach (Player player in _game.PlayerComponent.All) { players.Add(player); }
            for (int p = 0; p < players.Count; p++)
            {
                Player player = players[p];
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
                            case CollisionType.ShieldEnemy:
                            case CollisionType.EnemyStatic:
                                EnemyStaticCollision(collideablesInRoom[i].EntityID, collideablesInRoom[j].EntityID);
                                break;
                            case CollisionType.EnemyDoor:
                                EnemyDoorCollision(collideablesInRoom[i].EntityID, collideablesInRoom[j].EntityID);
                                break;
                            case CollisionType.EnemyWeapon:
                                EnemyWeaponCollision(collideablesInRoom[i].EntityID, collideablesInRoom[j].EntityID);
                                break;
                            case CollisionType.ShieldBullet:
                            case CollisionType.BulletStatic:
                                BulletStaticCollision(collideablesInRoom[i].EntityID, collideablesInRoom[j].EntityID);
                                break;
                            case CollisionType.BulletDoor:
                                BulletDoorCollision(collideablesInRoom[i].EntityID, collideablesInRoom[j].EntityID);
                                break;
                            case CollisionType.SkillBullet:
                                break;
                            case CollisionType.SkillCollectible:
                                break;
                                
                            case CollisionType.SkillEnemy:
                                //If can hit enemy
                                if (_game.SkillProjectileComponent.Contains(collideablesInRoom[i].EntityID) &&
                                    !_game.SkillProjectileComponent[collideablesInRoom[i].EntityID].CanHitEnemies
                                    || _game.SkillProjectileComponent.Contains(collideablesInRoom[j].EntityID) &&
                                    !_game.SkillProjectileComponent[collideablesInRoom[j].EntityID].CanHitEnemies)
                                {
                                    continue;
                                }
                                SkillCollision(collideablesInRoom[i].EntityID, collideablesInRoom[j].EntityID,false);
                                break;
                            case CollisionType.SkillDoor:
                            case CollisionType.SkillStatic:
                                SkillStaticCollision(collideablesInRoom[i].EntityID, collideablesInRoom[j].EntityID);
                                break;
                            case CollisionType.SkillPlayer:
                                //If can hit player
                                if (_game.SkillProjectileComponent.Contains(collideablesInRoom[i].EntityID) &&
                                    !_game.SkillProjectileComponent[collideablesInRoom[i].EntityID].CanHitPlayers
                                    || _game.SkillProjectileComponent.Contains(collideablesInRoom[j].EntityID) &&
                                    !_game.SkillProjectileComponent[collideablesInRoom[j].EntityID].CanHitPlayers)
                                {
                                    continue;
                                }
                                SkillCollision(collideablesInRoom[i].EntityID, collideablesInRoom[j].EntityID, true);
                                break;

                            case CollisionType.NPCPlayer:
                                NPCPlayerCollision(collideablesInRoom[i].EntityID, collideablesInRoom[j].EntityID);                                
                                break;
                            case CollisionType.NPCStatic:
                                NPCStaticCollision(collideablesInRoom[i].EntityID, collideablesInRoom[j].EntityID);
                                break;
                            case CollisionType.NPCDoor:
                                NPCDoorCollision(collideablesInRoom[i].EntityID, collideablesInRoom[j].EntityID);
                                break;

                            case CollisionType.NPCEnemy:
                                NPCEnemyCollision(collideablesInRoom[i].EntityID, collideablesInRoom[j].EntityID);
                                break;
                                
                            case CollisionType.StationPlayer:
                                HealingStationCollision(collideablesInRoom[i].EntityID, collideablesInRoom[j].EntityID);
                                break;

                          
                            case CollisionType.DroidEnemy:
                                ExplodingDroidCollision(collideablesInRoom[i].EntityID, collideablesInRoom[j].EntityID);
                                break;

                            case CollisionType.MatchingPuzzlePiece:
                                PlayerMatchingPuzzlePieceCollision(collideablesInRoom[i].EntityID, collideablesInRoom[j].EntityID);
                                break;

                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns the Enemy closest to the given position
        /// </summary>
        /// <param name="currentPosition">The position to use as a base for finding the closest enemy</param>
        /// <returns></returns>
        public uint GetClosestEnemy(Position currentPosition)
        {
            return GetClosestEnemy(currentPosition, null);
        }

        public uint GetClosestEnemy(Position currentPosition, List<uint> doNotIncludeList, float maxDistance)
        {
            uint currentBestEnemyID = uint.MaxValue;
            float currentBestDistance = float.MaxValue;
            float distance;
            Position enemyPosition;

            foreach (Enemy enemy in _game.EnemyComponent.All)
            {
                //Make sure that we don't look at enemies that we want to skip
                if (doNotIncludeList != null && doNotIncludeList.Contains(enemy.EntityID)) continue;

                //Sanity Check - make sure that the enemy even has a position component
                if (!_game.PositionComponent.Contains(enemy.EntityID)) continue;

                //Get the Position of the enemy
                enemyPosition = _game.PositionComponent[enemy.EntityID];

                //Sanity Check - make sure that the two positions are actually in the same room
                if (enemyPosition.RoomID != currentPosition.RoomID) continue;

                //Get the distance between the two points (its ok that it's squared since we are just comparing)
                distance = Vector2.DistanceSquared(currentPosition.Center, enemyPosition.Center);

                //Check to see if the distance for this enemy is better than the best we've found so far
                //If so, then keep track of it, otherwise keep going.
                //Also with the added check for the maximum distance
                if (distance < currentBestDistance && distance < maxDistance)
                {
                    currentBestDistance = distance;
                    currentBestEnemyID = enemy.EntityID;
                }
            }

            //return our results
            return currentBestEnemyID;
        }

        /// <summary>
        /// Returns the Enemy closest to the given position while ignoring specific enemies
        /// </summary>
        /// <param name="currentPosition">The position to use as a base for finding the closest enemy</param>
        /// <param name="doNotIncludeList">The list of enemies to ignore</param>
        /// <returns></returns>
        public uint GetClosestEnemy(Position currentPosition, List<uint> doNotIncludeList)
        {
            uint currentBestEnemyID = 0;
            float currentBestDistance = float.MaxValue;
            float distance;
            Position enemyPosition;

            foreach (Enemy enemy in _game.EnemyComponent.All)
            {
                //Make sure that we don't look at enemies that we want to skip
                if (doNotIncludeList != null && doNotIncludeList.Contains(enemy.EntityID)) continue;

                //Sanity Check - make sure that the enemy even has a position component
                if (!_game.PositionComponent.Contains(enemy.EntityID)) continue;

                //Get the Position of the enemy
                enemyPosition = _game.PositionComponent[enemy.EntityID];

                //Sanity Check - make sure that the two positions are actually in the same room
                if (enemyPosition.RoomID != currentPosition.RoomID) continue;

                //Get the distance between the two points (its ok that it's squared since we are just comparing)
                distance = Vector2.DistanceSquared(currentPosition.Center, enemyPosition.Center);

                //Check to see if the distance for this enemy is better than the best we've found so far
                //If so, then keep track of it, otherwise keep going.
                if (distance < currentBestDistance)
                {
                    currentBestDistance = distance;
                    currentBestEnemyID = enemy.EntityID;
                }
            }

            //return our results
            return currentBestEnemyID;
        }

        /// <summary>
        /// Gets a list of the enemie's ids within a certain range of a given position
        /// </summary>
        /// <param name="pos">The center of the check</param>
        /// <param name="range">The radius away from the center to check</param>
        /// <returns>A list of enemie's ids within that given range</returns>
        public List<uint> GetEnemiesInRange(Position pos, int range)
        {
            List<uint> enemies = new List<uint>();
            Position enemyPos;

            foreach(Enemy enemy in _game.EnemyComponent.All)
            {
                enemyPos = _game.PositionComponent[enemy.EntityID];
                if (enemyPos.RoomID == pos.RoomID)
                {
                    if (Vector2.DistanceSquared(enemyPos.Center, pos.Center) <= range)
                        enemies.Add(enemy.EntityID);
                }
            }

            return enemies;
        }

        /// <summary>
        /// Gets a list of the enemie's ids within a certain range of a given position
        /// </summary>
        /// <param name="pos">The center of the check</param>
        /// <param name="range">The radius away from the center to check</param>
        /// <param name="count">The number of enemies to return</param>
        /// <returns>A list of enemie's ids within that given range</returns>
        public List<uint> GetEnemiesInRange(Position pos, int range, int count)
        {
            List<DistanceToTarget> distances = new List<DistanceToTarget>(); //this list will store each distance/eid pair that will go in our list that we return
            float furthestDistance = 0; //the furthest distance an enemy is to the pos and is still in the list
            int furthestEnemy = 0; //the enemy in the list that is the furthest distance from pos
            float farness; //how far away the enemy is from the pos
            List<uint> enemies = new List<uint>(); //the list of enemies to return
            Position enemyPos; //the position of each enemy

            //first, loop through each enemy in the enemy component
            foreach (Enemy enemy in _game.EnemyComponent.All)
            {
                //we need to know there position of the enemy as we go through the list
                enemyPos = _game.PositionComponent[enemy.EntityID];
                //no need to check them if they are not in the same room as our position
                if (enemyPos.RoomID == pos.RoomID)
                {
                    //using this nifty function, we'll find the distance between the enemy and the pos
                    farness = Vector2.DistanceSquared(enemyPos.Center, pos.Center);
                    //check to see if the enemy is in range
                    if (farness <= range)
                    {
                        //first, has the necessary amount of enemies been counted yet?
                        if (distances.Count < count)
                        {
                            //if so, then is the current enemy the furthest one away?
                            if (farness > furthestDistance)
                            {
                                //if yes, then change the furthestDistance variable and indicate where that person is in the list
                                //I do this before they are added to the list so that furthestEnemy will be their index in the list 
                                //without subtracting.  Genius, I know :)
                                furthestDistance = farness;
                                furthestEnemy = distances.Count;
                            }
                            //create the enemy/distance pair and add it to the list
                            DistanceToTarget newDistance = new DistanceToTarget()
                            {
                                eid = enemy.EntityID,
                                distance = farness
                            };
                            distances.Add(newDistance);
                            
                        }
                        //now is where it gets good, this means the list is full so we have to be selective
                        else
                        {
                            //is the new enemy closer than the furthest enemy in the list?
                            if (farness < furthestDistance)
                            {
                                //if they are then this enemy needs to be added to our list, first we need to create him
                                DistanceToTarget newDistance = new DistanceToTarget()
                                {
                                    eid = enemy.EntityID,
                                    distance = farness
                                };
                                //now add him to the list instead of the guy who is further out
                                distances[furthestEnemy] = newDistance;

                                //we don't know for sure that the new guy is the furthest one out in the list, so first we have to check the list
                                for(int i = 0; i < distances.Count(); i++)
                                {
                                    //if we do find someone that is further away, we will change the variables to indicate so
                                    if (distances[i].distance > furthestDistance)
                                    {
                                        furthestDistance = distances[i].distance;
                                        furthestEnemy = i;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //finally, get the closest targets and store just their ids in the enemies list
            foreach (DistanceToTarget enemy in distances)
            {
                enemies.Add(enemy.eid);
            }
            //and return that list
            return enemies;
        }

        /// <summary>
        /// Handles NPC/Player collisions.
        /// </summary>
        /// <param name="p">First entityID</param>
        /// <param name="p_2">Second entityID</param>
        private void NPCPlayerCollision(uint p, uint p_2)
        {
            HandleDynamicDynamic(p, p_2);
        }

        /// <summary>
        /// Handles NPC/Player collisions.
        /// </summary>
        /// <param name="p">First entityID</param>
        /// <param name="p_2">Second entityID</param>
        private void NPCEnemyCollision(uint p, uint p_2)
        {
            HandleDynamicDynamic(p, p_2);
        }

        /// <summary>
        /// Handles NPC/Static collisions.
        /// </summary>
        /// <param name="p">First entityID</param>
        /// <param name="p_2">Second entityID</param>
        private void NPCStaticCollision(uint p, uint p_2)
        {
            //Stop player movement.

            //Get closest point on the rectangle, angle between player and point, and push back radius amount
            uint staticId, npcId;
            if (_game.NPCComponent.Contains(p))
            {
                npcId = p;
                staticId = p_2;
            }
            else
            {
                staticId = p;
                npcId = p_2;
            }

            HandleStaticDynamic(staticId, npcId);

        }

        /// <summary>
        /// Handles NPC/Static collisions.
        /// </summary>
        /// <param name="p">First entityID</param>
        /// <param name="p_2">Second entityID</param>
        private void NPCDoorCollision(uint p, uint p_2)
        {
            //Stop player movement.

            //Get closest point on the rectangle, angle between player and point, and push back radius amount
            uint doorId, npcId;
            if (_game.NPCComponent.Contains(p))
            {
                npcId = p;
                doorId = p_2;
            }
            else
            {
                doorId = p;
                npcId = p_2;
            }

            HandleStaticDynamic(doorId, npcId);

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
            //_game.EnemyComponent[enemy.EntityID] = enemy;

            //Remove collision immediately.
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

            HandleStaticDynamic(doorId, enemyId);
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

            HandleStaticDynamic(staticId, enemyId);
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
            //Make them collide with eachother or else they will stack

            HandleDynamicDynamic(p, p_2);
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


            if (_game.DoorComponent[doorId].Locked)
            {
                HandleStaticDynamic(doorId, playerId);
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

            HandleStaticDynamic(staticId, playerId);

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
            
            return; //TODO: remove

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
                HandleStaticDynamic(enemyId, playerId);
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

                HandleDynamicDynamic(enemyId, playerId);
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

            HandleDynamicDynamic(p, p_2);
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

        private void SkillStaticCollision(uint p, uint p_2)
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

        private void SkillCollision(uint p, uint p_2, bool friendly)
        {
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
            SkillProjectile skill;
            if (_game.SkillProjectileComponent.Contains(skillId)) skill = _game.SkillProjectileComponent[skillId];
            else return;

            _game.SkillSystem.TriggerEffect(skill.skill, skill.rank, friendly, oId, skill.OwnerID);

            if (_game.SkillProjectileComponent.Contains(skillId))
            {
                _game.GarbagemanSystem.ScheduleVisit(skillId, GarbagemanSystem.ComponentType.Skill);
            }
        }

        private void SkillAoECollision(uint p, uint p_2, bool friendly)
        {
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
            SkillProjectile skill = _game.SkillProjectileComponent[skillId];
            _game.SkillSystem.TriggerEffect(skill.skill, skill.rank, friendly, oId,skill.OwnerID);
        }
        /// <summary>
        /// Handles Enemy/ExplodingDroid collisions.
        /// </summary>
        /// <param name="p">First entityID</param>
        /// <param name="p_2">Second entityID</param>
        private void ExplodingDroidCollision(uint p, uint p_2)
        {
            uint droidId, enemyId;
            if (_game.ExplodingDroidComponent.Contains(p))
            {
                droidId = p;
                enemyId = p_2;
            }
            else
            {
                enemyId = p;
                droidId = p_2;
            }

            //Factor in damage
            Enemy enemy = _game.EnemyComponent[enemyId];
            Sprite sprite;
            TimedEffect timedEffect;
            DoDamage(enemy, 3);

            uint eid = Entity.NextEntity();

            timedEffect = new TimedEffect()
            {
                EntityID = eid,
                TotalDuration = 1,
                TimeLeft = 0.3f,
            };
            _game.TimedEffectComponent.Add(eid, timedEffect);

            sprite = new Sprite()
            {
                EntityID = eid,
                SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/EngineeringOffense"),
                SpriteBounds = new Rectangle(68, 8, 40, 37),
            };
            _game.SpriteComponent.Add(eid, sprite);

            _game.PositionComponent.Add(eid, _game.PositionComponent[droidId]);

            _game.GarbagemanSystem.ScheduleVisit(droidId, GarbagemanSystem.ComponentType.Effect);

            if (_game.MovementComponent.Contains(enemyId))
            {
                Vector2 directionOfKnockback = (_game.PositionComponent[droidId].Center -
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
        /// Handles Player/HealingStation collisions.
        /// </summary>
        /// <param name="p">First entityID</param>
        /// <param name="p_2">Second entityID</param>
        private void HealingStationCollision(uint p, uint p_2)
        {
            uint stationId, playerId;
            if (_game.HealingStationComponent.Contains(p))
            {
                stationId = p;
                playerId = p_2;
            }
            else
            {
                playerId = p;
                stationId = p_2;
            }

            Player player = _game.PlayerComponent[playerId];
            HealingStation healingStation = _game.HealingStationComponent[stationId];
            Buff buffEffect;
            TimedEffect timedEffect;
            Sprite sprite;
            Position position;
            uint eid;

            if (healingStation.healthAvailable > 0)
            {
                float playerHealth = _game.PlayerInfoComponent[player.EntityID].Health;

                if (playerHealth < 100 && player.PlayerRace != Aggregate.EarthianPlayer)
                {
                    int healthBonus = 100 - (int)playerHealth;
                    if (healingStation.healthAvailable >= healthBonus)
                    {

                        healingStation.healthAvailable -= healthBonus;
                        eid = Entity.NextEntity();
                        buffEffect = new Buff()
                        {
                            EntityID = eid,
                            TargetID = player.EntityID,
                            Health = healthBonus,
                        };
                        _game.BuffComponent.Add(eid, buffEffect);

                    }

                    else
                    {
                        eid = Entity.NextEntity();
                        buffEffect = new Buff()
                        {
                            EntityID = eid,
                            TargetID = player.EntityID,
                            Health = healingStation.healthAvailable,
                        };
                        _game.BuffComponent.Add(eid, buffEffect);

                        healingStation.healthAvailable = 0;
                        _game.GarbagemanSystem.ScheduleVisit(healingStation.EntityID, GarbagemanSystem.ComponentType.Effect);
                    }


                    timedEffect = new TimedEffect()
                    {
                        EntityID = eid,
                        TotalDuration = 1,
                        TimeLeft = 1
                    };
                    _game.TimedEffectComponent.Add(eid, timedEffect);

                    sprite = new Sprite()
                    {
                        EntityID = eid,
                        SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/red_cross_logo"),
                        SpriteBounds = new Rectangle(0, 0, 72, 72),
                    };
                    _game.SpriteComponent.Add(eid, sprite);
                    position = _game.PositionComponent[healingStation.EntityID];

                    _game.PositionComponent.Add(eid, position);

                    _game.PlayerComponent[player.EntityID] = player;
                    _game.HealingStationComponent[healingStation.EntityID] = healingStation;
                }
            }

            //Healing Station is empty
            else
            {
                _game.GarbagemanSystem.ScheduleVisit(healingStation.EntityID, GarbagemanSystem.ComponentType.Effect);
            }

        }

        private void PlayerMatchingPuzzlePieceCollision(uint p, uint p_2)
        {
            uint MatchingPuzzlePieceId, playerId;
            if (_game.PlayerComponent.Contains(p))
            {
                playerId = p;
                MatchingPuzzlePieceId = p_2;
            }
            else
            {
                MatchingPuzzlePieceId = p;
                playerId = p_2;
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
            else if (_game.SkillProjectileComponent.Contains(p) || _game.SkillAoEComponent.Contains(p) || _game.SkillDeployableComponent.Contains(p))
                obj1 = CollisionType.Skill;
            else if (_game.WeaponComponent.Contains(p))
                obj1 = CollisionType.Weapon;
            else if (_game.ExplodingDroidComponent.Contains(p))
                obj1 = CollisionType.ExplodingDroid;
            else if (_game.NPCComponent.Contains(p))
                obj1 = CollisionType.NPC;
            else if (_game.HealingStationComponent.Contains(p))
                obj1 = CollisionType.HealingStation;
            else if (_game.PortableShieldComponent.Contains(p))
                obj1 = CollisionType.PortableShield;
            else if (_game.MatchingPuzzleComponent.Contains(p))
                obj1 = CollisionType.MatchingPuzzlePiece;
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
            else if (_game.SkillProjectileComponent.Contains(p_2) || _game.SkillAoEComponent.Contains(p_2) || _game.SkillDeployableComponent.Contains(p_2))
                obj2 = CollisionType.Skill;
            else if (_game.WeaponComponent.Contains(p_2))
                obj2 = CollisionType.Weapon;
            else if (_game.ExplodingDroidComponent.Contains(p_2))
                obj2 = CollisionType.ExplodingDroid;
            else if (_game.NPCComponent.Contains(p_2))
                obj2 = CollisionType.NPC;
            else if (_game.HealingStationComponent.Contains(p_2))
                obj2 = CollisionType.HealingStation;
            else if (_game.PortableShieldComponent.Contains(p_2))
                obj2 = CollisionType.PortableShield;
            else if (_game.MatchingPuzzleComponent.Contains(p_2))
                obj2 = CollisionType.MatchingPuzzlePiece;
            else //Static
                obj2 = CollisionType.Static;

            return obj1 | obj2;
        }

        private void HandleStaticDynamic(uint staticID, uint dynamicID)
        {
            Bounds b = _game.CollisionComponent[staticID].Bounds;
            Position dynamicPos = _game.PositionComponent[dynamicID];

            if (b.GetType() == typeof(RectangleBounds))
            {
                //Get the closest point on the rectangle
                Vector2 closestPos = ((RectangleBounds)b).GetClosestPoint(dynamicPos.Center);
                double angle = Math.Atan2(closestPos.Y - dynamicPos.Center.Y, closestPos.X - dynamicPos.Center.X);

                double x = closestPos.X - (Math.Cos(angle) * (dynamicPos.Radius));
                double y = closestPos.Y - (Math.Sin(angle) * (dynamicPos.Radius));

                dynamicPos.Center = new Vector2((float)x, (float)y);

                _game.PositionComponent[dynamicID] = dynamicPos;
            }

            else //is circle
            {
                CircleBounds circle = ((CircleBounds)b);
                double angle = Math.Atan2(circle.Center.Y - dynamicPos.Center.Y,
                    circle.Center.X - dynamicPos.Center.X);

                double x = circle.Center.X - (Math.Cos(angle) * (dynamicPos.Radius + circle.Radius));
                double y = circle.Center.Y - (Math.Sin(angle) * (dynamicPos.Radius + circle.Radius));

                dynamicPos.Center = new Vector2((float)x, (float)y);

                _game.PositionComponent[dynamicID] = dynamicPos;
            }
        }

        private void HandleDynamicDynamic(uint dID1, uint dID2)
        {
            //Need the position for position, the collideable for the bounds type,
            // and the movement to analyze the amount of pushing that should be done
            Position pos1 = _game.PositionComponent[dID1];
            Position pos2 = _game.PositionComponent[dID2];

            Collideable col1 = _game.CollisionComponent[dID1];
            Collideable col2 = _game.CollisionComponent[dID2];

            Movement mov1 = _game.MovementComponent[dID1];
            Movement mov2 = _game.MovementComponent[dID2];

            //We'll need different equations for different types of bounds.

            if (col1.Bounds.GetType() == typeof(CircleBounds) && col2.Bounds.GetType() == typeof(CircleBounds))
            {
                CircleBounds c1 = (CircleBounds)col1.Bounds;
                CircleBounds c2 = (CircleBounds)col2.Bounds;

                //Angle from c1 to c2
                double angle = Math.Atan2(c2.Center.Y - c1.Center.Y,
                    c2.Center.X - c1.Center.X);

                double distance = Math.Abs((c2.Center.Y - c1.Center.Y) / Math.Sin(angle));
                double ddistance = c1.Radius + c2.Radius - distance;

                double x = pos1.Center.X - (Math.Cos(angle) * ddistance);
                double y = pos1.Center.Y - (Math.Sin(angle) * ddistance);

                pos1.Center = new Vector2((float)x, (float)y);


                x = pos2.Center.X + (Math.Cos(angle) * ddistance);
                y = pos2.Center.Y + (Math.Sin(angle) * ddistance);

                pos2.Center = new Vector2((float)x, (float)y);

                //Now determine the pushback from the movement
                /*Vector2 dMove = (mov1.Direction * mov1.Speed + mov2.Direction * mov2.Speed) / 2;
                double moveAngle = Math.Atan2(dMove.Y, dMove.X);

                double dAngle = moveAngle - angle;

                double moveDistance = Math.Cos(dAngle) * dMove.Length();

                dMove.Normalize();

                pos1.Center += dMove * (float)moveDistance;
                pos2.Center += dMove * (float)moveDistance;
                */
                _game.PositionComponent[dID1] = pos1;
                _game.PositionComponent[dID2] = pos2;

                c1.Center = pos1.Center;
                c2.Center = pos2.Center;

                col1.Bounds = c1;
                col2.Bounds = c2;

                _game.CollisionComponent[dID1] = col1;
                _game.CollisionComponent[dID2] = col2;
            }

            if (col1.Bounds.GetType() != col2.Bounds.GetType())
            {
                bool col1IsRect = (col1.Bounds.GetType() == typeof(CircleBounds));
                Position rectPos = (col1IsRect ? pos1 : pos2);
                Collideable rectCol = (col1IsRect ? col1 : col2);
                Movement rectMov = (col1IsRect ? mov1 : mov2);
                RectangleBounds rect = (RectangleBounds)rectCol.Bounds;

                Position circPos = (col1IsRect ? pos2 : pos1);
                Collideable circCol = (col1IsRect ? col2 : col1);
                Movement circMov = (col1IsRect ? mov2 : mov1);
                CircleBounds circ = (CircleBounds)rectCol.Bounds;

                //Do your thing here

                throw new NotImplementedException();
            }

            if (col1.Bounds.GetType() == typeof(RectangleBounds) && col2.Bounds.GetType() == typeof(RectangleBounds))
            {
                //Can this happen?
                throw new NotImplementedException();
            }
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
