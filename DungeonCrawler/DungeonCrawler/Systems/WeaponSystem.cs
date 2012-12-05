#region File Description
//-----------------------------------------------------------------------------
// WeaponSystem.cs 
//
// Author: Devin Kelly-Collins
//
// Modified: Samuel Fike and Jiri Malina: Fixed errors due to removal of movementSprite for players
// Modified: Devin Kelly-Collins; added weapon collisions and other required methods, added references to common coponents. Added logic to play weapon sounds. Moved sprite creation methods to WeaponFactory (11/15/12)
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
            _playerInfoComponent = _game.PlayerInfoComponent;
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
                    PlayWeaponSound(equipment.WeaponID);
                }   

                //Handle more weapon logic.
                if(attacking)
                {
                    if (weapon.AttackType == WeaponAttackType.Ranged)
                    {
                        _bulletTimer += elapsedTime;
                        if (_bulletTimer >= weapon.Speed)
                        {
                            CreateBulletAndSprite(equipment.EntityID, equipment.WeaponID);
                            _bulletTimer = 0;
                        }
                    }
                    else
                    {
                        if (collisionCreated)
                            UpdateWeaponCollision(equipment.EntityID, equipment.WeaponID);
                        else
                            CreateWeaponCollision(equipment.EntityID, equipment.WeaponID);
                    }
                }
                else if (collisionCreated) //Not attacking but the collision box is there.
                {
                    _collisionComponent.Remove(equipment.WeaponID);
                }
            }
        }

        /// <summary>
        /// Updates weapon's collision box to the players position.
        /// </summary>
        /// <param name="playerID">Player's EntityID</param>
        /// <param name="weaponID">Weapons's EntityID</param>
        private void UpdateWeaponCollision(uint playerID, uint weaponID)
        {
            Collideable collision = _collisionComponent[weaponID];
            Position playerPosition = _positionComponent[playerID];
            Facing facing = (Facing)_game.SpriteAnimationComponent[playerID].CurrentAnimationRow;

            if (collision.Bounds is CircleBounds)
            {
                ((CircleBounds)collision.Bounds).Center = playerPosition.Center;
            }
            else
            {
                Vector2 newPos = updatePositionByFacing(playerPosition, facing, 32);
                ((RectangleBounds)collision.Bounds).Rectangle.X = (int)newPos.X;
                ((RectangleBounds)collision.Bounds).Rectangle.Y = (int)newPos.Y;
            }

            _collisionComponent[weaponID] = collision;
        }

        /// <summary>
        /// Plays the weapon's sound effect if there is one.
        /// </summary>
        /// <param name="weaponID">Weapon's EntityID</param>
        private void PlayWeaponSound(uint weaponID)
        {
            if (_game.SoundComponent.Contains(weaponID))
                _game.SoundComponent[weaponID].SoundEffect.Play();
        }

        /// <summary>
        /// Handles creating the collision box for the weapon.
        /// </summary>
        /// <param name="playerID">Player's EntityID</param>
        /// <param name="weaponID">Weapon's EntityID</param>
        private void CreateWeaponCollision(uint playerID, uint weaponID)
        {
            uint roomID = _positionComponent[playerID].RoomID;
            Collideable weaponCollision = new Collideable { EntityID = weaponID, RoomID = roomID };
            Facing facing = (Facing)_game.SpriteAnimationComponent[playerID].CurrentAnimationRow;

            Vector2 position = updatePositionByFacing(_positionComponent[playerID], facing, 32);

            RectangleBounds rb = new RectangleBounds((int)position.X, (int)position.Y, 64, 64);
            weaponCollision.Bounds = rb;
            _collisionComponent.Add(weaponCollision.EntityID, weaponCollision);
        }

        /// <summary>
        /// Handles creating the Bullet object and sprite.
        /// </summary>
        /// <param name="playerID">Player's EntityID</param>
        /// <param name="weaponID">Weapon's EntityID</param>
        private void CreateBulletAndSprite(uint playerID, uint weaponID)
        {
            Position position = _positionComponent[playerID];
            Vector2 direction = getDirectionFromFacing((Facing)_game.SpriteAnimationComponent[playerID].CurrentAnimationRow); //changed to get direction from spriteanimation instead of movementsprite, currentAnimationRow returns same values as facing for directions
            _game.WeaponFactory.CreateBullet(_weaponComponent[weaponID].Type, direction, position);
        }

        /// <summary>
        /// Handles updating already made weapon sprites. Returns true if the sprite was removed.
        /// </summary>
        /// <param name="sprite">Weapon Sprite to update</param>
        /// <returns>True if sprite is removed. False otherwise.</returns>
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

        /// <summary>
        /// Get the direction the player is facing.
        /// </summary>
        /// <param name="facing">Direction player is facing.</param>
        /// <returns>Vecotr2 direction.</returns>
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

        /// <summary>
        /// Returns the position a rectangle should be depending on the direction the player is facing.
        /// </summary>
        /// <param name="position">Position of the player.</param>
        /// <param name="facing">The direction the player is facing.</param>
        /// <param name="offset">How much to offset the position by. Should be the player's redius generally.</param>
        /// <returns>New position for the rectangle</returns>
        private Vector2 updatePositionByFacing(Position position, Facing facing, int offset)
        {
            Vector2 retVal = position.Center;

            switch (facing)
            {
                case Facing.North:
                    retVal.Y -= 2*offset;
                    retVal.X -= offset;
                    break;
                case Facing.East:
                    retVal.Y -= offset;
                    break;
                case Facing.South:
                    retVal.X -= offset;
                    break;
                case Facing.West:
                    retVal.Y -= offset;
                    retVal.X -= 2*offset;
                    break;
            }

            return retVal;
        }
    }
}
