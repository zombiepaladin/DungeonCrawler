#region File Description
//-----------------------------------------------------------------------------
// EnemyFactory.cs 
//
// Author: Matthew McHaney
//
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
            Enemy enemy;
            Sprite sprite;
            Collideable collideable;

            switch (type)
            {
                case EnemyType.StationaryTarget:
                    enemy .HurtOnTouch = false;
                    enemy.Health = 1;

                    sprite = new Sprite()
                    {
                        EntityID = eid,
                        SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/target2"),
                        SpriteBounds = new Rectangle(0, 0, 64, 64),
                    };
                    break;
                default:
                    throw new Exception("Unknown EnemyType");
            }

            enemy.Type = type;
            enemy.EntityID = eid;
            position.EntityID = eid;

            collideable = new Collideable()
            {
                EntityID = eid,
                Bounds = new CircleBounds(position.Center, position.Radius)
            };
            _game.CollisionComponent[eid] = collideable;

            _game.EnemyComponent.Add(eid, enemy);
            //_game.MovementComponent.Add(eid, movement);
            _game.PositionComponent.Add(eid, position);
            _game.SpriteComponent.Add(eid, sprite);
            return eid;
        }
    }
}
