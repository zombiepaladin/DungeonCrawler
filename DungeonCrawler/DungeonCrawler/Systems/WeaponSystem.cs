#region File Description
//-----------------------------------------------------------------------------
// WeaponSystem.cs 
//
// Author: Devin Kelly-Collins
//
// Modified: Samuel Fike and Jiri Malina: Fixed errors due to removal of movementSprite for players
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

namespace DungeonCrawler.Systems
{
    public class WeaponSystem
    {
        //The parent game.
        private DungeonCrawlerGame _game;

        //The timer will be used to determine when to update and create sprites and objects.
        private float _timer = 0;
        private float _bulletTimer = 0;

        /// <summary>
        /// Creates this system.
        /// </summary>
        /// <param name="game"></param>
        public WeaponSystem(DungeonCrawlerGame game)
        {
            _game = game;
        }

        /// <summary>
        /// Updates weapons currently in the game.
        /// </summary>
        /// <param name="elapsedTime"></param>
        public void Update(float elapsedTime)
        {
            _timer += elapsedTime;
            bool spriteRemoved = false;

            foreach (Player player in _game.PlayerComponent.All)
            {
                Equipment equipment = _game.EquipmentComponent[player.EntityID];
                bool attacking = _game.PlayerInfoComponent[player.EntityID].State == PlayerState.Attacking;

                //Handle sprites
                if (_game.WeaponSpriteComponent.Contains(player.EntityID))
                {
                    //If the player has a weapon sprite update it
                    UpdateWeaponSprite(_game.WeaponSpriteComponent[player.EntityID]);
                }
                else if (attacking)
                {
                    //Otherwise create a new sprite.
                    CreateWeaponSprite(equipment);
                }   

                Weapon weapon = _game.WeaponComponent[equipment.WeaponID];
                if (attacking && weapon.AttackType == WeaponAttackType.Ranged)
                {
                    _bulletTimer += elapsedTime;
                    if (_bulletTimer >= weapon.Speed)
                    {
                        CreateBulletAndSprite(_game.EquipmentComponent[player.EntityID]);
                        _bulletTimer = 0;
                    }
                }
            }
        }

        //Handles Creating the WeaponSprite.
        private void CreateWeaponSprite(Equipment equipment)
        {
           
            WeaponType type = _game.WeaponComponent[equipment.WeaponID].Type;
            Position position = _game.PositionComponent[equipment.EntityID];
            int y = (int)_game.SpriteAnimationComponent[equipment.EntityID].CurrentAnimationRow * 64; //changed to get direction from spriteanimation instead of movementsprite, currentAnimationRow returns same values as facing for directions

            WeaponSprite sprite = new WeaponSprite()
            {
                EntityID = equipment.EntityID,
            };

            switch (type)
            {
                case WeaponType.WeakSword:
                case WeaponType.StandardSword:
                case WeaponType.StrongSword:
                    sprite.SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/StandardSword");
                    sprite.SpriteBounds = new Rectangle(0, y, 64, 64);
                    break;
                case WeaponType.StandardGun:
                    sprite.SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/StandardSword");
                    sprite.SpriteBounds = new Rectangle(0, y, 64, 64);
                    break;
            }
            _game.WeaponSpriteComponent.Add(sprite.EntityID, sprite);
        }

        //Handles creating the Bullet object and sprite.
        private void CreateBulletAndSprite(Equipment equipment)
        {
            Position position = _game.PositionComponent[equipment.EntityID];
            Vector2 direction = getDirectionFromFacing((Facing)_game.SpriteAnimationComponent[equipment.EntityID].CurrentAnimationRow); //changed to get direction from spriteanimation instead of movementsprite, currentAnimationRow returns same values as facing for directions

            switch (_game.WeaponComponent[equipment.WeaponID].Type)
            {
                case WeaponType.StandardGun:
                    _game.WeaponFactory.CreateBullet(BulletType.StandardBullet, direction, position);
                    break;
                default:
                    throw new Exception("Unknown weapon type.");
            }
        }

        //Handles updating already made weapon sprites. Returns if the sprite was removed.
        private bool UpdateWeaponSprite(WeaponSprite sprite)
        {
            bool removed = false;

            if (_timer >= .05f)
            {
                if (sprite.SpriteBounds.X < 192)
                {
                    sprite.SpriteBounds.X += 64;
                    _game.WeaponSpriteComponent[sprite.EntityID] = sprite;
                }
                else
                {
                    _game.WeaponSpriteComponent.Remove(sprite.EntityID);
                    removed = true;
                }

                _timer = 0;
            }

            return removed;
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
}
