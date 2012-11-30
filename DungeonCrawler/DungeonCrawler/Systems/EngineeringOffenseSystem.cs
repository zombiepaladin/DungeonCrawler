#region File Description
//-----------------------------------------------------------------------------
// TurretSystem.cs 
//
// Author: Andrew Bellinder
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
using DungeonCrawler.Entities;
using DungeonCrawler.Components;

namespace DungeonCrawler.Systems
{
    public class EngineeringOffenseSystem
    {
        DungeonCrawlerGame _game;
        float turretTimer;
        float trapTimer;

        public EngineeringOffenseSystem(DungeonCrawlerGame game)
        {
            _game = game;
            turretTimer = 0;
            trapTimer = 0;
        }

        public void Update(float elapsedTime)
        {
            #region Turret

            foreach (Turret turret in _game.TurretComponent.All)
            {
                turretTimer += elapsedTime;
                foreach (Enemy enemy in _game.EnemyComponent.All)
                {
                    Vector2 toEnemy = _game.PositionComponent[enemy.EntityID].Center -  turret.position.Center;
                    float distance = toEnemy.Length();
                    toEnemy.Normalize();
                    if (distance <= turret.range)
                    {
                        if (turretTimer > 0.2f)
                        {
                        _game.WeaponFactory.CreateBullet(BulletType.TurretBullet, toEnemy, turret.position);
                        turretTimer = 0;
                        }
                        break;
                    }
                }
            }
            #endregion

            #region Trap
            trapTimer += elapsedTime;
            List<Trap> traps = new List<Trap>();
            foreach(Trap trap in _game.TrapComponent.All) { traps.Add(trap); }
            for (int i = 0; i < traps.Count; i++)
            {
                Trap trap = traps[i];
                if (!(trap.isSet))
                {
                    foreach (Enemy enemy in _game.EnemyComponent.All)
                    {
                        Vector2 toEnemy = _game.PositionComponent[enemy.EntityID].Center - trap.position.Center;
                        float check = toEnemy.Length();
                        if (check < trap.range)
                        {
                            trap.trappedEnemy = enemy;
                            trap.isSet = true;
                            _game.TrapComponent[trap.EntityID] = trap;
                            _game.PositionComponent[trap.trappedEnemy.EntityID] = trap.position;
                            break;
                        }
                    }
                }

                else
                {
                    _game.PositionComponent[trap.trappedEnemy.EntityID] = trap.position;

                }

            }

            #endregion

            #region ExplodingDroid

            List<ExplodingDroid> droids = new List<ExplodingDroid>();
            foreach(ExplodingDroid droid in _game.ExplodingDroidComponent.All) { droids.Add(droid); }
            for (int i = 0; i < droids.Count; i++)
            {
                ExplodingDroid droid = droids[i];
                if (!(droid.hasEnemy))
                {
                    droid.enemyToAttack = _game.EnemyComponent.All.First();
                    droid.hasEnemy = true;
                    _game.ExplodingDroidComponent[droid.EntityID] = droid;

                }

                else
                {
                    if (_game.MovementComponent.Contains(droid.EntityID))
                    {

                        Vector2 toEnemy = _game.PositionComponent[droid.enemyToAttack.EntityID].Center - droid.position.Center;
                        toEnemy.Normalize();

                        Movement move = _game.MovementComponent[droid.EntityID];
                        move.Direction = toEnemy;
                        _game.MovementComponent[droid.EntityID] = move;
                    }

                    else
                    {
                        droid.hasEnemy = false;
                    }
                }
            }

            #endregion

        }
    }
}
