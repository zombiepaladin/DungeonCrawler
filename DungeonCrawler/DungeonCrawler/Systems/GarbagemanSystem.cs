﻿#region File Description
//-----------------------------------------------------------------------------
// GarbagemanSystem.cs 
//
// Author: Matthew McHaney
//
// Modified By: Nicholas Strub - Added handing of doors and room 10/31/12
// Modified by Samuel Fike, Jiri Malina, Brett Barger: Added handling of SpriteAnimationComponents and EnemyComponent
// Modified By: Nick Boen - Added compatability with Effect Components
//
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence 
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using DungeonCrawler.Components;
using System.Collections.Generic;
#endregion

namespace DungeonCrawler.Systems
{
    /// <summary>
    /// This class embodies a system that simplifies deleting entities
    /// from the components list. Ideally, you'll tell the garbageman that
    /// entity ID #YOURNUMBERHERE needs to be taken out of the game. So then
    /// when he updates, he'll take the old data (garbage) out of the
    /// components (driveway). Foolproof.
    /// </summary>
    public class GarbagemanSystem
    {
        #region Private Members

        /// <summary>
        /// The game this system belongs to
        /// </summary>
        private DungeonCrawlerGame game;

        public enum ComponentType
        {
            Player,
            Enemy,
            Bullet,
            Collectible,
            Door,
            Room,
            Effect,
            //Needs more in the future!
            Skill,
            Turret,
        }

        private Stack<KeyValuePair<uint, ComponentType>> garbageSchedule;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new MovementSystem
        /// </summary>
        /// <param name="game">The game this system belongs to</param>
        public GarbagemanSystem(DungeonCrawlerGame game)
        {
            this.game = game;
            garbageSchedule = new Stack<KeyValuePair<uint, ComponentType>>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Pulls out any necessary eids
        /// </summary>
        /// <param name="elapsedTime">
        /// The time between this and the previous frame.
        /// </param>
        public void Update(float elapsedTime)
        {
            KeyValuePair<uint, ComponentType> keyValue;

            // Update all entities that have a movement component
            while(garbageSchedule.Count > 0)
            {
                keyValue = garbageSchedule.Pop();

                switch (keyValue.Value)
                {
                    case ComponentType.Player:
                        game.PositionComponent.Remove(keyValue.Key);
                        game.MovementComponent.Remove(keyValue.Key);
                        game.MovementSpriteComponent.Remove(keyValue.Key);
                        game.LocalComponent.Remove(keyValue.Key); //?? What does this do? Will this break something?
                        game.StatsComponent.Remove(keyValue.Key);
                        game.PlayerComponent.Remove(keyValue.Key);
                        game.PlayerInfoComponent.Remove(keyValue.Key);
                        game.CollisionComponent.Remove(keyValue.Key);
                        break;
                    case ComponentType.Enemy:
                        game.SpriteAnimationComponent.Remove(keyValue.Key);
                        game.EnemyComponent.Remove(keyValue.Key);
                        game.CollisionComponent.Remove(keyValue.Key);
                        game.PositionComponent.Remove(keyValue.Key);
                        if (game.EnemyAIComponent.Contains(keyValue.Key))
                            game.EnemyAIComponent.Remove(keyValue.Key);
                        if (game.MovementComponent.Contains(keyValue.Key))
                            game.MovementComponent.Remove(keyValue.Key);
                        if (game.MovementSpriteComponent.Contains(keyValue.Key))
                            game.MovementSpriteComponent.Remove(keyValue.Key);
                        if (game.SpriteComponent.Contains(keyValue.Key))
                            game.SpriteComponent.Remove(keyValue.Key);
                        if (game.ActorTextComponent.Contains(keyValue.Key))
                            game.ActorTextComponent.Remove(keyValue.Key);
                        break;
                    case ComponentType.Bullet:
                        game.PositionComponent.Remove(keyValue.Key);
                        game.BulletComponent.Remove(keyValue.Key);
                        game.SpriteComponent.Remove(keyValue.Key);
                        game.MovementComponent.Remove(keyValue.Key);
                        game.CollisionComponent.Remove(keyValue.Key);
                        break;
                    case ComponentType.Collectible:
                        game.PositionComponent.Remove(keyValue.Key);
                        game.CollectibleComponent.Remove(keyValue.Key);
                        game.SpriteComponent.Remove(keyValue.Key);
                        game.CollisionComponent.Remove(keyValue.Key);
                        //game.MovementComponent.Remove(keyValue.Key);
                        break;
                    case ComponentType.Room:
                        game.PositionComponent.Remove(keyValue.Key);
                        game.LocalComponent.Remove(keyValue.Key);
                        game.RoomComponent.Remove(keyValue.Key);
                        break;
                    case ComponentType.Door:
                        game.PositionComponent.Remove(keyValue.Key);
                        game.CollisionComponent.Remove(keyValue.Key);
                        game.LocalComponent.Remove(keyValue.Key);
                        game.DoorComponent.Remove(keyValue.Key);
                        //game.SpriteComponent.Remove(keyValue.Key);
                        break;
                    case ComponentType.Effect:
                        if (game.AgroDropComponent.Contains(keyValue.Key))          game.AgroDropComponent.Remove(keyValue.Key);
                        if (game.AgroGainComponent.Contains(keyValue.Key))          game.AgroGainComponent.Remove(keyValue.Key);
                        if (game.BuffComponent.Contains(keyValue.Key))              game.BuffComponent.Remove(keyValue.Key);
                        if (game.ChanceToSucceedComponent.Contains(keyValue.Key))   game.ChanceToSucceedComponent.Remove(keyValue.Key);
                        if (game.ChangeVisibilityComponent.Contains(keyValue.Key))  game.ChangeVisibilityComponent.Remove(keyValue.Key);
                        if (game.CoolDownComponent.Contains(keyValue.Key))          game.CoolDownComponent.Remove(keyValue.Key);
                        if (game.DamageOverTimeComponent.Contains(keyValue.Key))    game.DamageOverTimeComponent.Remove(keyValue.Key);
                        if (game.DirectDamageComponent.Contains(keyValue.Key))      game.DirectDamageComponent.Remove(keyValue.Key);
                        if (game.DirectHealComponent.Contains(keyValue.Key))        game.DirectHealComponent.Remove(keyValue.Key);
                        if (game.FearComponent.Contains(keyValue.Key))              game.FearComponent.Remove(keyValue.Key);
                        if (game.HealOverTimeComponent.Contains(keyValue.Key))      game.HealOverTimeComponent.Remove(keyValue.Key);
                        if (game.PsiOrFatigueRegenComponent.Contains(keyValue.Key)) game.PsiOrFatigueRegenComponent.Remove(keyValue.Key);
                        if (game.InstantEffectComponent.Contains(keyValue.Key))     game.InstantEffectComponent.Remove(keyValue.Key);
                        if (game.KnockBackComponent.Contains(keyValue.Key))         game.KnockBackComponent.Remove(keyValue.Key);
                        if (game.ReduceAgroRangeComponent.Contains(keyValue.Key))   game.ReduceAgroRangeComponent.Remove(keyValue.Key);
                        if (game.ResurrectComponent.Contains(keyValue.Key))         game.ResurrectComponent.Remove(keyValue.Key);
                        if (game.StunComponent.Contains(keyValue.Key))              game.StunComponent.Remove(keyValue.Key);
                        if (game.TimedEffectComponent.Contains(keyValue.Key))       game.TimedEffectComponent.Remove(keyValue.Key);
                        if (game.CloakComponent.Contains(keyValue.Key))             game.CloakComponent.Remove(keyValue.Key);
                        if (game.TargetedKnockBackComponent.Contains(keyValue.Key)) game.TargetedKnockBackComponent.Remove(keyValue.Key);
                        if (game.TurretComponent.Contains(keyValue.Key))          game.TurretComponent.Remove(keyValue.Key);
                        if (game.TrapComponent.Contains(keyValue.Key))            game.TrapComponent.Remove(keyValue.Key);
                        if (game.ExplodingDroidComponent.Contains(keyValue.Key))  game.ExplodingDroidComponent.Remove(keyValue.Key);
                        if (game.SpriteComponent.Contains(keyValue.Key))          game.SpriteComponent.Remove(keyValue.Key);
                        if (game.MovementComponent.Contains(keyValue.Key))        game.MovementComponent.Remove(keyValue.Key);
                        if (game.CollisionComponent.Contains(keyValue.Key))       game.CollisionComponent.Remove(keyValue.Key);
                        if (game.HealingStationComponent.Contains(keyValue.Key))  game.HealingStationComponent.Remove(keyValue.Key);
                        if (game.PortableShieldComponent.Contains(keyValue.Key))  game.PortableShieldComponent.Remove(keyValue.Key); 
                        break;


                    case ComponentType.Skill:
                        game.PositionComponent.Remove(keyValue.Key);
                        game.SkillProjectileComponent.Remove(keyValue.Key);
                        game.SpriteComponent.Remove(keyValue.Key);
                        game.MovementComponent.Remove(keyValue.Key);
                        game.CollisionComponent.Remove(keyValue.Key);
                        break;

                }
            }
        }

        public void ScheduleVisit(uint eid)
        {
            ComponentType cType;
            if (game.PlayerInfoComponent.Contains(eid))
                cType = ComponentType.Player;
            //else if (game.EnemyComponent.Contains(eid)) 
              //  cType = ComponentType.Enemy;
            else if (game.BulletComponent.Contains(eid))
                cType = ComponentType.Bullet;
            else if (game.CollectibleComponent.Contains(eid))
                cType = ComponentType.Collectible;
            else if (game.DoorComponent.Contains(eid))
                cType = ComponentType.Door;
            else if (game.RoomComponent.Contains(eid))
                cType = ComponentType.Room;
            else
                return; //It's something not supported

            garbageSchedule.Push(new KeyValuePair<uint, ComponentType>(eid, cType));
        }

        public void ScheduleVisit(uint eid, ComponentType type)
        {
            garbageSchedule.Push(new KeyValuePair<uint, ComponentType>(eid, type));
        }
        #endregion
    }
}