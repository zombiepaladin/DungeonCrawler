#region File Description
//-----------------------------------------------------------------------------
// SkillProjectileFactory.cs 
//
// Author: Matthew Hart
//
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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DungeonCrawler.Components;

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

        public uint CreateSkillProjectile(Skills skill, Vector2 direction, Position position)
        {
            SkillProjectile skillProjectile;
            Movement movement;
            Sprite sprite;
            Collideable collideable;
            uint eid = Entity.NextEntity();

            position.EntityID = eid;
            position.Center += direction * 70;

            switch (skill)
            {
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

        public uint CreateSkillAoE(Skills skill, Vector2 direction, Position position)
        {
            SkillAoE skillAoE;
            Movement movement;
            Sprite sprite;
            Collideable collideable;
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

        public uint CreateSkillDeployable(Skills skill, Vector2 direction, Position position)
        {
            SkillDeployable skillDeployable;
            Movement movement;
            Sprite sprite;
            Collideable collideable;
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
    }
    #endregion
}
