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
    #region SkillProjectileFactory
    public class SkillProjectileFactory
    {
        /// <summary>
        /// The game this SkillProjectileFactory belongs to
        /// </summary>
        DungeonCrawlerGame game;

        /// <summary>
        /// Constructors
        /// </summary>
        /// <param name="game"></param>
        public SkillProjectileFactory(DungeonCrawlerGame game)
        {
            this.game = game;
        }

        public uint CreateSkillProjectile(Skills skillP, Facing facing, Position position)
        {
            SkillProjectile skillProjectile;
            Movement movement;
            Sprite sprite;
            Collideable collideable;
            uint eid = Entity.NextEntity();
            Vector2 direction = getDirectionFromFacing(facing);

            position.EntityID = eid;
            //position.Center += direction * 70;

            switch (skillP)
            {
                case Skills.benignParasite:
                    skillProjectile = new SkillProjectile()
                    {
                        EntityID=eid,
                        skill=skillP,
                        maxRange=1,                       
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
    }
    #endregion

    #region SkillAoEFactory
    public class SkillAoEFactory
    {
        /// <summary>
        /// The game this SkillProjectileFactory belongs to
        /// </summary>
        DungeonCrawlerGame game;

        /// <summary>
        /// Constructors
        /// </summary>
        /// <param name="game"></param>
        public SkillAoEFactory(DungeonCrawlerGame game)
        {
            this.game = game;
        }

        public uint CreateSkillAoE(Skills skill, Facing facing, Position position)
        {
            SkillAoE skillAoE;
            Movement movement;
            Sprite sprite;
            Collideable collideable;
            Vector2 direction = getDirectionFromFacing(facing);
            uint eid = Entity.NextEntity();

            position.EntityID = eid;
            position.Center += direction * 70;

            switch (skill)
            {
                default:
                    throw new Exception("Not a AoE skill");
            }

            collideable = new Collideable()
            {
                EntityID = eid,
                Bounds = new CircleBounds(position.Center, position.Radius),
            };

            game.SkillAoEComponent.Add(eid, skillAoE);
            game.MovementComponent.Add(eid, movement);
            game.PositionComponent.Add(eid, position);
            game.SpriteComponent.Add(eid, sprite);
            game.CollisionComponent.Add(eid, collideable);
            return eid;
        }


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
    }
    #endregion

    #region SkillDeployableFactory
    public class SkillDeployableFactory
    {
        /// <summary>
        /// The game this SkillProjectileFactory belongs to
        /// </summary>
        DungeonCrawlerGame game;

        /// <summary>
        /// Constructors
        /// </summary>
        /// <param name="game"></param>
        public SkillDeployableFactory(DungeonCrawlerGame game)
        {
            this.game = game;
        }

        public uint CreateSkillDeployable(Skills skill, Facing facing, Position position)
        {
            SkillDeployable skillDeployable;
            Movement movement;
            Sprite sprite;
            Collideable collideable;
            Vector2 direction = getDirectionFromFacing(facing);
            uint eid = Entity.NextEntity();

            position.EntityID = eid;
            position.Center += direction * 70;

            switch (skill)
            {
                default:
                    throw new Exception("Not a Deployableweqqw skill");
            }

            collideable = new Collideable()
            {
                EntityID = eid,
                Bounds = new CircleBounds(position.Center, position.Radius),
            };

            game.SkillDeployableComponent.Add(eid, skillDeployable);
            game.MovementComponent.Add(eid, movement);
            game.PositionComponent.Add(eid, position);
            game.SpriteComponent.Add(eid, sprite);
            game.CollisionComponent.Add(eid, collideable);
            return eid;

        }

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
    }
    #endregion

  
}
