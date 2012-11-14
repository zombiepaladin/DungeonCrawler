#region File Description
//-----------------------------------------------------------------------------
// SkillProjectileFactory.cs 
//
// Author: Matthew Hart
//
//getDirectionFromFacing(): taken from weapon systems
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
using DungeonCrawler.Entities;

namespace DungeonCrawler.Entities
{
    
    public class SkillEntityFactory
    {
        /// <summary>
        /// The game this SkillProjectileFactory belongs to
        /// </summary>
        DungeonCrawlerGame game;

        /// <summary>
        /// Constructors
        /// </summary>
        /// <param name="game"></param>
        public SkillEntityFactory(DungeonCrawlerGame game)
        {
            this.game = game;
        }


        #region SkillProjectile
        public uint CreateSkillProjectile(Skills skillP, Facing facing, Position position)
        {
            SkillProjectile skillProjectile;
            Movement movement;
            Sprite sprite;
            Collideable collideable;
            uint eid = Entity.NextEntity();
            Vector2 direction = getDirectionFromFacing(facing);

            position.EntityID = eid;
            position.Center += direction * 70;

            switch (skillP)
            {
                case Skills.benignParasite:
                    skillProjectile = new SkillProjectile()
                    {
                        EntityID = eid,
                        skill = skillP,
                        maxRange = 1,
                    };
                    movement = new Movement()
                    {
                        EntityID = eid,
                        Direction = direction,
                        Speed = 300,
                    };
                    sprite = new Sprite()
                    {
                        EntityID = eid,
                        SpriteSheet = game.Content.Load<Texture2D>("Spritesheets/BlueBullet"),
                        SpriteBounds = new Rectangle(0, 0, 10, 10),
                    };
                    position.Radius = 5;
                    break;
                default:
                    throw new Exception("Not a projectile skill");
            }

            collideable = new Collideable()
            {
                EntityID = eid,
                Bounds = new CircleBounds(position.Center, position.Radius),
            };

            game.SkillProjectileComponent.Add(eid, skillProjectile);
            game.MovementComponent.Add(eid, movement);
            game.PositionComponent.Add(eid, position);
            game.SpriteComponent.Add(eid, sprite);
            game.CollisionComponent.Add(eid, collideable);
            return eid;
        }
#endregion

        #region SkillAoE
        public uint CreateSkillAoE(Skills skill, Position position)
        {
            SkillAoE skillAoE;
            Sprite sprite;
            Collideable collideable;
            uint eid = Entity.NextEntity();

            position.EntityID = eid;

            switch (skill)
            {
                case Skills.detonate:
                    skillAoE = new SkillAoE()
                    {
                        EntityID = eid,
                        radius = 1,
                    };
                    sprite = new Sprite()
                    {
                        EntityID = eid,
                        SpriteSheet = game.Content.Load<Texture2D>("Spritesheets/BlueBullet"),
                        SpriteBounds = new Rectangle(0, 0, 10, 10),
                    };
                    position.Radius = 5;
                    break;
                default:
                    throw new Exception("Not a AoE skill");
            }

            collideable = new Collideable()
            {
                EntityID = eid,
                Bounds = new CircleBounds(position.Center, position.Radius),
            };

            game.SkillAoEComponent.Add(eid, skillAoE);
            game.PositionComponent.Add(eid, position);
            game.SpriteComponent.Add(eid, sprite);
            game.CollisionComponent.Add(eid, collideable);
            return eid;
        }
        #endregion

        #region SkillDeployable
        public uint CreateSkillDeployable(Skills skill, Position position)
        {
            SkillDeployable skillDeployable;
            Sprite sprite;
            Collideable collideable;
            uint eid = Entity.NextEntity();

            position.EntityID = eid;

            switch (skill)
            {
                case Skills.healingStation:
                    skillDeployable = new SkillDeployable()
                    {
                        EntityID = eid,
                        duration = 1,
                    };
                    sprite = new Sprite()
                    {
                        EntityID = eid,
                        SpriteSheet = game.Content.Load<Texture2D>("Spritesheets/BlueBullet"),
                        SpriteBounds = new Rectangle(0, 0, 10, 10),
                    };
                    position.Radius = 5;
                    break;
                default:
                    throw new Exception("Not a Deployable skill");
            }

            collideable = new Collideable()
            {
                EntityID = eid,
                Bounds = new CircleBounds(position.Center, position.Radius),
            };

            game.SkillDeployableComponent.Add(eid, skillDeployable);
            game.PositionComponent.Add(eid, position);
            game.SpriteComponent.Add(eid, sprite);
            game.CollisionComponent.Add(eid, collideable);
            return eid;
        }
        #endregion

        #region getDriectionHelper
        private Vector2 getDirectionFromFacing(Facing facing)
        {
            Vector2 direction = new Vector2(0);

            switch (facing)
            {
                case Facing.North:
                    direction.Y = -1;
                    break;
                case Facing.East:
                    direction.X = 1;
                    break;
                case Facing.South:
                    direction.Y = 1;
                    break;
                case Facing.West:
                    direction.X = -1;
                    break;
            }
            return direction;
        }
        #endregion

    }
}
