#region File Description
//-----------------------------------------------------------------------------
// EnemyAISystem.cs 
//
// Author: Brett Barger
//
// Modified: Nick Boen - added the Get and Set target methods, unimplemented for now
//           Brett Barger - corrected functionality of enemy moving towards the player it is targeting.
//
// Modified Samuel Fike, Jiri Malina, Brett Barger: Reorganized, added behaviors and stuff, also 
//              
//
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence 
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DungeonCrawler.Components;
#endregion

namespace DungeonCrawler.Systems
{

    public class EnemyAISystem
    {
        #region Private Members

        /// <summary>
        /// The game this system belongs to
        /// </summary>
        private DungeonCrawlerGame game;

        private Random rand;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new MovementSystem
        /// </summary>
        /// <param name="game">The game this system belongs to</param>
        public EnemyAISystem(DungeonCrawlerGame game)
        {
            this.game = game;
            this.rand = new Random();
        }

        #endregion
        
        #region Public Methods

        /// <summary>
        /// Updates all Enemies in the game
        /// </summary>
        /// <param name="elapsedTime">
        /// The time between this and the previous frame.
        /// </param>
        public void Update(float elapsedTime)
        {
            List<uint> keyList = game.EnemyAIComponent.Keys.ToList<uint>();

            foreach(uint id in keyList)
            {
                EnemyAI enemyAI = game.EnemyAIComponent[id];
                Enemy enemy = game.EnemyComponent[id];
                AIBehaviorType AIBehavior = enemyAI.AIBehaviorType;
                Position pos = game.PositionComponent[id];
                Movement movement;

                if (pos.RoomID != game.CurrentRoomEid)
                {
                    continue;
                }

                if (game.EnemyComponent[id].Health <= 0)
                {
                    switch(enemy.Type)
                    {
                        case EnemyType.Sludge5:
                            Position pos2 = pos;
                            pos2.Center.X += 70;
                            game.EnemyFactory.CreateEnemy(EnemyType.Sludge4, pos);
                            game.EnemyFactory.CreateEnemy(EnemyType.Sludge4, pos2);
                            game.GarbagemanSystem.ScheduleVisit(id, GarbagemanSystem.ComponentType.Enemy);
                            break;
                        case EnemyType.Sludge4:
                            pos2 = pos;
                            pos2.Center.X += 70;
                            game.EnemyFactory.CreateEnemy(EnemyType.Sludge3, pos);
                            game.EnemyFactory.CreateEnemy(EnemyType.Sludge3, pos2);
                            game.GarbagemanSystem.ScheduleVisit(id, GarbagemanSystem.ComponentType.Enemy);
                            break;
                        case EnemyType.Sludge3:
                            game.EnemyFactory.CreateEnemy(EnemyType.Sludge2, pos);
                            game.EnemyFactory.CreateEnemy(EnemyType.Sludge2, pos);
                            game.EnemyFactory.CreateEnemy(EnemyType.Sludge2, pos);
                            game.GarbagemanSystem.ScheduleVisit(id, GarbagemanSystem.ComponentType.Enemy);
                            break;
                        case EnemyType.Sludge2:
                            game.EnemyFactory.CreateEnemy(EnemyType.Sludge1, pos);
                            game.EnemyFactory.CreateEnemy(EnemyType.Sludge1, pos);
                            game.EnemyFactory.CreateEnemy(EnemyType.Sludge1, pos);
                            game.EnemyFactory.CreateEnemy(EnemyType.Sludge1, pos);
                            game.GarbagemanSystem.ScheduleVisit(id, GarbagemanSystem.ComponentType.Enemy);
                            break;
                        case EnemyType.Sludge1:
                            game.GarbagemanSystem.ScheduleVisit(id, GarbagemanSystem.ComponentType.Enemy);
                            break;
                        default:
                            game.GarbagemanSystem.ScheduleVisit(id, GarbagemanSystem.ComponentType.Enemy);
                            break;
                    }
                }

                uint targetID;
                float dist;

                switch(AIBehavior)
                {
                    case AIBehaviorType.DefaultMelee:
                        updateTargeting(id);
                        MoveTowardTarget(id);
                        break;

                    case AIBehaviorType.DefaultRanged:
                         updateTargeting(id);

                        if (!enemyAI.HasTarget)
                        {
                            ManageAnimation(id);
                            continue;
                        }

                        targetID = enemyAI.TargetID;
                        dist = Vector2.Distance(pos.Center, game.PositionComponent[targetID].Center);
                            
                        movement = game.MovementComponent[id];

                        if (dist > 300)
                            MoveTowardTarget(id);
                        else
                        {
                            movement.Direction = Vector2.Zero;
                            game.MovementComponent[id] = movement;
                            //Put spritesheets here
                            string spriteSheet;

                            switch (enemy.Type)
                            {
                                default:
                                    spriteSheet = "Spritesheets/Skills/Effects/AlienOrb";
                                    break;
                            }

                            game.SkillSystem.EnemyUseBasicRanged(id, targetID, 1, 1f, spriteSheet, new Rectangle(0, 0, 20, 20));
                        }
                        
                        ManageAnimation(id);

                        break;

                    case AIBehaviorType.CloakingRanged:
                        updateTargeting(id);

                        if (!enemyAI.HasTarget)
                        {
                            ManageAnimation(id);
                            continue;
                        }

                        targetID = enemyAI.TargetID;
                        dist = Vector2.Distance(pos.Center, game.PositionComponent[targetID].Center);

                        bool isUsingCloak = false;

                        foreach (Cloak cloak in game.CloakComponent.All)
                        {
                            if (cloak.TargetID == id)
                            {
                                if (cloak.TimeLeft > 2 && cloak.StartingTime - cloak.TimeLeft > 2)
                                    isUsingCloak = true;
                                break;
                            }
                        }
                            
                        movement = game.MovementComponent[id];

                        if (!isUsingCloak)
                        {
                            if (dist > 300)
                                MoveTowardTarget(id);
                            else
                            {
                                movement.Direction = Vector2.Zero;
                                game.MovementComponent[id] = movement;
                                //game.SkillSystem.EnemyUseSkill(SkillType.SniperShot, id, targetID);
                                game.SkillSystem.EnemyUseBasicRanged(id, targetID, 1, 1f, "Spritesheets/Skills/Effects/AlienOrb", new Rectangle(0, 0, 20, 20));
                                game.SkillSystem.EnemyUseSkill(SkillType.Cloak, id, id);
                            }
                        }
                        else if (movement.Direction.Equals(Vector2.Zero))
                        {
                            movement.Direction = new Vector2(rand.Next(20) - 10, rand.Next(20) - 10);
                            movement.Direction = Vector2.Normalize(movement.Direction);
                            game.MovementComponent[id] = movement;
                        }
                        
                        ManageAnimation(id);

                        break;

                    case AIBehaviorType.Spider:
                        updateTargeting(id);
                        MoveTowardTarget(id);
                        
                        targetID = enemyAI.TargetID;
                        dist = Vector2.Distance(pos.Center, game.PositionComponent[targetID].Center);

                        if (dist < 50)
                        {
                            game.SkillSystem.EnemyUseSkill(SkillType.DamagingPull, id, targetID);
                            game.SkillSystem.EnemyUseBasicMelee(id, targetID, 1, 1);
                        }

                        ManageAnimation(id);
                        break;

                    default:
                        break;
                }
            }
        }

        private void updateTargeting(uint entityID)
        {
            Position pos = game.PositionComponent[entityID];
            EnemyAI ai = game.EnemyAIComponent[entityID];

            if (ai.HasTarget == false)
            {
                IEnumerable<Position> HitList = game.PositionComponent.InRegion(pos.Center, 500);

                foreach (Position thing in HitList)
                {
                    if (game.PlayerComponent.Contains(thing.EntityID))
                    {
                        ai.TargetID = thing.EntityID;
                        ai.HasTarget = true;
                        break;
                    }
                }
            }
            else if (ai.HasTarget == true && game.PlayerInfoComponent[ai.TargetID].Health <= 0)
            {
                ai.HasTarget = false;
            }

            if (ai.HasTarget && game.AgroDropComponent.Count() > 0)
            {
                foreach (AgroDrop agro in game.AgroDropComponent.All)
                {
                    if (agro.PlayerID == ai.TargetID)
                        ai.HasTarget = false;
                }
            }

            game.EnemyAIComponent[ai.EntityID] = ai;
        }

        private void MoveTowardTarget(uint entityID)
        {
            EnemyAI enemy = game.EnemyAIComponent[entityID];

            if (!enemy.HasTarget)
                return;

            Position enemyPosition = game.PositionComponent[entityID];
            Position targetPosition = game.PositionComponent[enemy.TargetID];

            Vector2 toTarget = targetPosition.Center - enemyPosition.Center;
            toTarget.Normalize();

            Movement movement = game.MovementComponent[entityID];
            movement.Direction = toTarget;

            game.MovementComponent[movement.EntityID] = movement;
        }

        /// <summary>
        /// Changes the current animation playing for an enemy based on movement direction.
        /// Default uses the 4 direction animations. Add your enemy to the switch statement for custom animation behavior.
        /// Samuel Fike & Jiri Malina
        /// </summary>
        /// <param name="key"></param>
        private void ManageAnimation(uint id)
        {

            Enemy enemy = game.EnemyComponent[id];
            SpriteAnimation spriteAnimation = game.SpriteAnimationComponent[id];
            Movement movement = game.MovementComponent[id];

            switch (enemy.Type)
            {
                default:
                    if (movement.Direction.Length() == 0) //If not moving, stop animation
                    {
                        spriteAnimation.IsPlaying = false;
                    }
                    else
                    {
                        spriteAnimation.IsPlaying = true;
                        spriteAnimation.CurrentAnimationRow = (int)game.MovementComponent.GetFacingFromID(id);
                    }
                    break;
            }

            game.SpriteAnimationComponent[id] = spriteAnimation;

        }

        public void GetDifferentTarget(uint enemyKey)
        {

        }

        public void GetClosestTarget(uint enemyKey)
        {

        }

        public void SetTarget(uint enemyKey, uint targetKey)
        {

        }
        
        #endregion
    }
}
