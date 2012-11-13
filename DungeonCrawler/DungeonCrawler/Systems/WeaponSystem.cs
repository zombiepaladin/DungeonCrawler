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
        //Components this Systems uses often.
        private CollisionComponent _collisionComponent;
        private EquipmentComponent _equipmentComponent;
        private PlayerInfoComponent _playerInfoComponent;
        private WeaponComponent _weaponComponent;
        private WeaponSpriteComponent _weaponSpriteComponent;

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
            _collisionComponent = _game.CollisionComponent;
            _equipmentComponent = _game.EquipmentComponent;
            _playerInfoComponent = _game.PlayerInfoComponent;
            _weaponComponent = _game.WeaponComponent;
            _weaponSpriteComponent = _game.WeaponSpriteComponent;
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
                Equipment equipment = _equipmentComponent[player.EntityID];
                bool attacking = _playerInfoComponent[player.EntityID].State == PlayerState.Attacking;
                Weapon weapon = _weaponComponent[equipment.WeaponID];

                //Handle sprites
                if (_weaponSpriteComponent.Contains(player.EntityID))
                {
                    //If the player has a weapon sprite update it
                    spriteRemoved = UpdateWeaponSprite(_weaponSpriteComponent[player.EntityID]);
                }
                else if (attacking)
                {
                    //Otherwise create a new sprite and sound.
                    _game.WeaponFactory.CreateWeaponSprite(equipment.WeaponID, equipment.EntityID);
                    PlayWeaponSound(equipment);
                }   

                //Handle more weapon logic.
                if(attacking)
                {
                    if (weapon.AttackType == WeaponAttackType.Ranged)
                    {
                        _bulletTimer += elapsedTime;
                        if (_bulletTimer >= weapon.Speed)
                        {
                            CreateBulletAndSprite(equipment);
                            _bulletTimer = 0;
                        }
                    }
                    else
                    {
                        CreateWeaponCollision(equipment);
                    }
                }
                if (spriteRemoved && _collisionComponent.Contains(equipment.WeaponID))
                    _collisionComponent.Remove(equipment.WeaponID);
            }
        }

        private void PlayWeaponSound(Equipment equipment)
        {
            if (_game.SoundComponent.Contains(equipment.WeaponID))
                _game.SoundComponent[equipment.WeaponID].SoundEffect.Play();
        }

        private void CreateWeaponCollision(Equipment equipment)
        {
            if (_collisionComponent.Contains(equipment.WeaponID))
                return;

            Collideable weaponCollision;
            weaponCollision.EntityID = equipment.WeaponID;
            Vector2 position = _game.PositionComponent[equipment.EntityID].Center;
            Facing facing = (Facing)_game.SpriteAnimationComponent[equipment.EntityID].CurrentAnimationRow;

            switch (facing)
            {
                case Facing.North:
                    position.Y -= 64;
                    break;
                case Facing.East:
                    position.X += 64;
                    break;
                case Facing.South:
                    position.Y += 64;
                    break;
                case Facing.West:
                    position.X -= 64;
                    break;
            }

            RectangleBounds rb = new RectangleBounds((int)position.X, (int)position.Y, 64, 64);
            weaponCollision.Bounds = rb;
            _collisionComponent.Add(weaponCollision.EntityID, weaponCollision);
        }

        //Handles creating the Bullet object and sprite.
        private void CreateBulletAndSprite(Equipment equipment)
        {
            Position position = _game.PositionComponent[equipment.EntityID];
            Vector2 direction = getDirectionFromFacing((Facing)_game.SpriteAnimationComponent[equipment.EntityID].CurrentAnimationRow); //changed to get direction from spriteanimation instead of movementsprite, currentAnimationRow returns same values as facing for directions
            _game.WeaponFactory.CreateBullet(_weaponComponent[equipment.WeaponID].Type, direction, position);
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
