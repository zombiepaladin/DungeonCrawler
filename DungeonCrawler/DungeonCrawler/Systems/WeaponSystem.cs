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
        private PositionComponent _positionComponent;

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

            //To simplfy some things, I'm keeping a reference to components I often use.
            _collisionComponent = _game.CollisionComponent;
            _equipmentComponent = _game.EquipmentComponent;
            _playerInfoComponent = _game.PlayerInfoComponent;
            _weaponComponent = _game.WeaponComponent;
            _weaponSpriteComponent = _game.WeaponSpriteComponent;
            _positionComponent = _game.PositionComponent;
        }

        /// <summary>
        /// Updates weapons currently in the game.
        /// </summary>
        /// <param name="elapsedTime"></param>
        public void Update(float elapsedTime)
        {
            _timer += elapsedTime;
            bool spriteRemoved = false;
            Equipment equipment;
            Weapon weapon;
            bool attacking;
            bool collisionCreated;


            foreach (Player player in _game.PlayerComponent.All)
            {
                equipment = _equipmentComponent[player.EntityID];
                attacking = _playerInfoComponent[player.EntityID].State == PlayerState.Attacking;
                weapon = _weaponComponent[equipment.WeaponID];
                collisionCreated = _collisionComponent.Contains(equipment.WeaponID);

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
                        if (collisionCreated)
                            UpdateWeaponCollision(equipment);
                        else
                            CreateWeaponCollision(equipment);
                    }
                }
                else if (collisionCreated) //Not attacking but the collision box is there.
                {
                    _collisionComponent.Remove(equipment.WeaponID);
                }
            }
        }

        //Updates weapon's collision box to the players position.
        private void UpdateWeaponCollision(Equipment equipment)
        {
            Collideable collision = _collisionComponent[equipment.WeaponID];
            Position playerPosition = _positionComponent[equipment.EntityID];

            if (collision.Bounds is CircleBounds)
            {
                ((CircleBounds)collision.Bounds).Center = playerPosition.Center;
            }
            else
            {
                ((RectangleBounds)collision.Bounds).Rectangle.X = (int)playerPosition.Center.X;
                ((RectangleBounds)collision.Bounds).Rectangle.Y = (int)playerPosition.Center.Y;
            }

            _collisionComponent[equipment.WeaponID] = collision;
        }

        //Plays the weapon's sound effect if there is one.
        private void PlayWeaponSound(Equipment equipment)
        {
            if (_game.SoundComponent.Contains(equipment.WeaponID))
                _game.SoundComponent[equipment.WeaponID].SoundEffect.Play();
        }

        //Handles creating the collision box for the weapon.
        private void CreateWeaponCollision(Equipment equipment)
        {
            uint weaponID = equipment.WeaponID;
            uint roomID = _positionComponent[equipment.EntityID].RoomID;
            Collideable weaponCollision = new Collideable { EntityID = weaponID, RoomID = roomID };
            Vector2 position = _positionComponent[equipment.EntityID].Center;
            Facing facing = (Facing)_game.SpriteAnimationComponent[equipment.EntityID].CurrentAnimationRow;

            switch (facing)
            {
                case Facing.North:
                    position.Y += 64;
                    break;
                case Facing.East:
                    position.X -= 64;
                    break;
                case Facing.South:
                    position.Y -= 64;
                    break;
                case Facing.West:
                    position.X += 64;
                    break;
            }

            RectangleBounds rb = new RectangleBounds((int)position.X, (int)position.Y, 64, 64);
            weaponCollision.Bounds = rb;
            _collisionComponent.Add(weaponCollision.EntityID, weaponCollision);
        }

        //Handles creating the Bullet object and sprite.
        private void CreateBulletAndSprite(Equipment equipment)
        {
            Position position = _positionComponent[equipment.EntityID];
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

        //Get the direction the player is facing.
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
