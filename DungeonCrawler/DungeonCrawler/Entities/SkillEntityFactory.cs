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
using DungeonCrawler.Systems;

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
        public uint CreateSkillProjectile(SkillType skillP, Facing facing, Position position, int rankP, int speed)
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
                #region Vermis Projectiles
                case SkillType.ThrownBlades:
                    skillProjectile = new SkillProjectile()
                    {
                        EntityID = eid,
                        skill = skillP,
                        maxRange = 1,
                        rank=rankP,
                    };
                    movement = new Movement()
                    {
                        EntityID = eid,
                        Direction = direction,
                        Speed = speed,
                    };
                    sprite = new Sprite()
                    {
                        EntityID = eid,
                        SpriteSheet = game.Content.Load<Texture2D>("Spritesheets/skillPlaceHolder2"),
                        SpriteBounds = new Rectangle(0,250,50,50),
                    };
                    position.Radius = 10;
                    break;
                case SkillType.MaliciousParasite:
                    skillProjectile = new SkillProjectile()
                    {
                        EntityID = eid,
                        skill = skillP,
                        maxRange = 1,
                        rank=rankP,
                    };
                    movement = new Movement()
                    {
                        EntityID = eid,
                        Direction = direction,
                        Speed = speed,
                    };
                    sprite = new Sprite()
                    {
                        EntityID = eid,
                        SpriteSheet = game.Content.Load<Texture2D>("Spritesheets/skillPlaceHolder2"),
                        SpriteBounds = new Rectangle(300, 150, 50, 50),
                    };
                    position.Radius = 10;
                    break;
                case SkillType.MindlessParasites:
                    skillProjectile = new SkillProjectile()
                    {
                        EntityID = eid,
                        skill = skillP,
                        maxRange = 1,
                        rank=rankP,
                    };
                    movement = new Movement()
                    {
                        EntityID = eid,
                        Direction = direction,
                        Speed = speed,
                    };
                    sprite = new Sprite()
                    {
                        EntityID = eid,
                        SpriteSheet = game.Content.Load<Texture2D>("Spritesheets/skillPlaceHolder2"),
                        SpriteBounds = new Rectangle(250, 50, 50, 50),
                    };
                    position.Radius = 10;
                    break;
                case SkillType.BenignParasite:
                    skillProjectile = new SkillProjectile()
                    {
                        EntityID = eid,
                        skill = skillP,
                        maxRange = 1,
                        rank=rankP,
                    };
                    movement = new Movement()
                    {
                        EntityID = eid,
                        Direction = direction,
                        Speed = speed,
                    };
                    sprite = new Sprite()
                    {
                        EntityID = eid,
                        SpriteSheet = game.Content.Load<Texture2D>("Spritesheets/skillPlaceHolder2"),
                        SpriteBounds = new Rectangle(100, 0, 50, 50),
                    };
                    position.Radius = 10;
                    break;
                #endregion
                default:
                    throw new Exception("Not a projectile skill");
            }

            collideable = new Collideable()
            {
                EntityID = eid,
                RoomID = position.RoomID,
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
        public uint CreateSkillAoE(SkillType skill, Position position, int rankP, int radius)
        {
            SkillAoE skillAoE;
            Sprite sprite;
            Collideable collideable;
            uint eid = Entity.NextEntity();

            position.EntityID = eid;

            switch (skill)
            {
                case SkillType.Detnate:
                    skillAoE = new SkillAoE()
                    {
                        EntityID = eid,
                        radius = 1,
                        rank = rankP,
                    };
                    sprite = new Sprite()
                    {
                        EntityID = eid,
                        SpriteSheet = game.Content.Load<Texture2D>("Spritesheets/Weapons/Bullets/BlueBullet"),
                        SpriteBounds = new Rectangle(0, 0, 10, 10),
                    };
                    position.Radius = radius;
                    break;
                default:
                    throw new Exception("Not a AoE skill");
            }

            collideable = new Collideable()
            {
                EntityID = eid,
                RoomID = position.RoomID,
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
        public uint CreateSkillDeployable(SkillType skill, Position position, int rankP)
        {
            SkillDeployable skillDeployable;
            Sprite sprite;
            Collideable collideable;
            uint eid = Entity.NextEntity();

            position.EntityID = eid;

            switch (skill)
            {
                case SkillType.PortableShop:
                    skillDeployable = new SkillDeployable()
                    {
                        EntityID = eid,
                        duration = 1,
                        rank = rankP,
                    };
                    sprite = new Sprite()
                    {
                        EntityID = eid,
                        SpriteSheet = game.Content.Load<Texture2D>("Spritesheets/shop"),
                        SpriteBounds = new Rectangle(343, 50, 24, 25),
                    };
                    position.Radius = 5;
                    break;
                default:
                    throw new Exception("Not a Deployable skill");
            }

            collideable = new Collideable()
            {
                EntityID = eid,
                RoomID = position.RoomID,
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
