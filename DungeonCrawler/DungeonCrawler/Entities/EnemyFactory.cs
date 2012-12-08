#region File Description
//-----------------------------------------------------------------------------
// EnemyFactory.cs 
//
// Author: Matthew McHaney
// Modified Samuel Fike and Jiri Malina: Added Alien and organized
// Modified Samuel Fike and Jiri Malina: Made spiders, changed aliens to robots, added support for default enemies
// Modified Samuel Fike and Jiri Malina: added sludge boss/enemies
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
            Rectangle spriteBounds = new Rectangle(0, 0, 64, 64);
            Color spriteColor = Color.White;

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

                case EnemyType.Spider:
                    enemy.HurtOnTouch = false;
                    enemy.Health = 20;
                    spritesheet = "Spritesheets/Enemies/spider";
                    spriteBounds = new Rectangle(0, 0, 120/3, 141/4);
                    aiBehaviorType = AIBehaviorType.Spider;
                    moveSpeed = 115;
                    spriteAnimation.FramesPerSecond = 14;
                    break;

                case EnemyType.CloakingRobot:
                    enemy.HurtOnTouch = false;
                    enemy.Health = 150;
                    spritesheet = "Spritesheets/Enemies/CloakingRobot";
                    spriteColor = Color.LightCyan;
                    spriteBounds = new Rectangle(0, 0, 65, 75);
                    spriteAnimation.FramesPerSecond = 5;
                    aiBehaviorType = AIBehaviorType.CloakingRanged;
                    position.Radius = 37;
                    break;
                case EnemyType.BasicShootingRobot:
                    enemy.HurtOnTouch = false;
                    enemy.Health = 150;
                    spritesheet = "Spritesheets/Enemies/BasicShootingRobot";
                    spriteBounds = new Rectangle(0, 0, 65, 75);
                    spriteAnimation.FramesPerSecond = 5;
                    aiBehaviorType = AIBehaviorType.DefaultRanged;
                    position.Radius = 32;
                    break;
                case EnemyType.Sludge1:
                    enemy.HurtOnTouch = false;
                    enemy.Health = 150;
                    spritesheet = "Spritesheets/Enemies/Sludge1";
                    spriteBounds = new Rectangle(0, 0, 32, 32);
                    spriteAnimation.FramesPerSecond = 10;
                    aiBehaviorType = AIBehaviorType.DefaultRanged;
                    position.Radius = 1;
                    break;
                case EnemyType.Sludge2:
                    enemy.HurtOnTouch = false;
                    enemy.Health = 150;
                    spritesheet = "Spritesheets/Enemies/Sludge2";
                    spriteBounds = new Rectangle(0, 0, 64, 64);
                    spriteAnimation.FramesPerSecond = 10;
                    aiBehaviorType = AIBehaviorType.DefaultRanged;
                    position.Radius = 1;
                    break;
                case EnemyType.Sludge3:
                    enemy.HurtOnTouch = false;
                    enemy.Health = 150;
                    spritesheet = "Spritesheets/Enemies/Sludge3";
                    spriteBounds = new Rectangle(0, 0, 144, 144);
                    spriteAnimation.FramesPerSecond = 10;
                    aiBehaviorType = AIBehaviorType.DefaultRanged;
                    position.Radius = 1;
                    break;
                case EnemyType.Sludge4:
                    enemy.HurtOnTouch = false;
                    enemy.Health = 150;
                    spritesheet = "Spritesheets/Enemies/Sludge4";
                    spriteBounds = new Rectangle(0, 0, 192, 192);
                    spriteAnimation.FramesPerSecond = 10;
                    aiBehaviorType = AIBehaviorType.DefaultRanged;
                    position.Radius = 1;
                    break;
                case EnemyType.Sludge5:
                    enemy.HurtOnTouch = false;
                    enemy.Health = 150;
                    spritesheet = "Spritesheets/Enemies/Sludge5";
                    spriteBounds = new Rectangle(0, 0, 256, 256);
                    spriteAnimation.FramesPerSecond = 10;
                    aiBehaviorType = AIBehaviorType.DefaultRanged;
                    position.Radius = 1;
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
                SpriteColor = spriteColor,
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
