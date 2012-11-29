#region File Description
//-----------------------------------------------------------------------------
// EnemyFactory.cs 
//
// Author: Matthew McHaney
// Modified Samuel Fike and Jiri Malina: Added Alien and organized
//
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence 
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DungeonCrawler.Components;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
#endregion

namespace DungeonCrawler.Entities
{
    /// <summary>
    /// Handles creating enemies and adding them to the game.
    /// </summary>
    public class EnemyFactory
    {
        /// <summary>
        /// Parent game.
        /// </summary>
        private DungeonCrawlerGame _game;

        /// <summary>
        /// Creates a new Factory.
        /// </summary>
        /// <param name="game"></param>
        public EnemyFactory(DungeonCrawlerGame game)
        {
            _game = game;
        }

        /// <summary>
        /// Creates a new enemy and adds it to the game. (No other components created)
        /// </summary>
        /// <param name="type">The type of enemy to create.</param>
        public uint CreateEnemy(EnemyType type, Position position)
        {
            uint eid = Entity.NextEntity();
            Enemy enemy = new Enemy();
            Sprite sprite;
            SpriteAnimation spriteAnimation = new SpriteAnimation(eid);

            Collideable collideable;
            EnemyAI ai;
            AIBehaviorType aiBehaviorType = AIBehaviorType.None;
            float moveSpeed = 100;

            String spritesheet;
            Rectangle spriteBounds = new Rectangle(0, 0, 64, 64); ;

            switch (type)
            {
                case EnemyType.StationaryTarget:
                    enemy.HurtOnTouch = false;
                    enemy.Health = 1;
                    spritesheet = "Spritesheets/Enemies/target2";
                    break;

                case EnemyType.MovingTarget:
                    enemy.HurtOnTouch = false;
                    enemy.Health = 1;
                    spritesheet = "Spritesheets/Enemies/target2";
                    break;

                case EnemyType.Alien:
                    enemy.HurtOnTouch = false;
                    enemy.Health = 10;
                    spritesheet = "Spritesheets/Enemies/alien";
                    spriteBounds = new Rectangle(0, 0, 32, 32);
                    aiBehaviorType = AIBehaviorType.Alien;
                    break;

                default:
                    throw new Exception("Unknown EnemyType");
            }

            ai = new EnemyAI()
            {
                EntityID = eid,
                AIBehaviorType = aiBehaviorType,
            };
            _game.EnemyAIComponent.Add(eid, ai);

            enemy.Type = type;
            enemy.EntityID = eid;
            position.EntityID = eid;

            sprite = new Sprite()
            {
                EntityID = eid,
                SpriteSheet = _game.Content.Load<Texture2D>(spritesheet),
                SpriteBounds = spriteBounds
            };

            collideable = new Collideable()
            {
                EntityID = eid,
                RoomID = position.RoomID,
                Bounds = new CircleBounds(position.Center, position.Radius)
            };

            Movement move = new Movement()
            {
                EntityID = eid,
                Speed = moveSpeed,
            };

            _game.SpriteAnimationComponent[eid] = spriteAnimation;
            _game.CollisionComponent[eid] = collideable;
            _game.MovementComponent.Add(eid, move);
            _game.EnemyComponent.Add(eid, enemy);
            _game.PositionComponent.Add(eid, position);
            _game.SpriteComponent.Add(eid, sprite);
           
            return eid;
        }
    }
}
