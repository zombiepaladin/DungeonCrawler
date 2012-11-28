#region File Description
//-----------------------------------------------------------------------------
// EnemyAISystem.cs 
//
// Author: Brett Barger
//
// Modified: Nick Boen - added the Get and Set target methods, unimplemented for now
//           Brett Barger - corrected functionalit of enemy moving towards the player it is targeting.
//
// Modified Samuel Fike and Jiri Malina: Reorganized, added behaviors and stuff
//
// TODO: 1. Should probably refactor the Update to use the TargetID from the EnemyAI Component
//              rather than a local instance.
//       2. May want to check if the AI should get a different target if its current one isn't allowed
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

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new MovementSystem
        /// </summary>
        /// <param name="game">The game this system belongs to</param>
        public EnemyAISystem(DungeonCrawlerGame game)
        {
            this.game = game;
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
                AIBehaviorType AIBehavior = enemyAI.AIBehaviorType;
                Position pos = game.PositionComponent[id];

                if (pos.RoomID != game.CurrentRoomEid)
                {
                    continue;
                }

                if (game.EnemyComponent[id].Health <= 0)
                {
                    game.GarbagemanSystem.ScheduleVisit(id, GarbagemanSystem.ComponentType.Enemy);
                }
                switch(AIBehavior)
                {
                    case AIBehaviorType.DefaultMelee:
                        updateTargeting(id);
                        MoveTowardTarget(id);
                        break;

                    case AIBehaviorType.DefaultRanged:
                        updateTargeting(id);
                        MoveTowardTarget(id);
                        break;

                    case AIBehaviorType.Alien:
                        updateTargeting(id);
                        MoveTowardTarget(id);
                        
                        uint targetID = enemyAI.TargetID;
                        float dist = Vector2.Distance(pos.Center, game.PositionComponent[targetID].Center);
                        
                        if(dist < 35)
                            game.SkillSystem.EnemyUseSkill(SkillType.DamagingPull, id, targetID);

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
                        spriteAnimation.CurrentAnimationRow = (int)game.MovementComponent.GetFacingFromDirection(id);
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
