#region File Description
//-----------------------------------------------------------------------------
// WeaponFactory.cs 
//
// Author: Devin Kelly-Collins
//
// Modified: Devin Kelly-Collins - Moved Sprite creation method from WeaponSystem to here. (11/15/12)
//           Josh Zavala - Added sprites for a basic animation for each class, Assignment 9
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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace DungeonCrawler.Entities
{
    /// <summary>
    /// Type of weapon to create.
    /// </summary>
    public enum WeaponType
    {
        DeadHand,
        PsychicStun,
        ShockRod,
        StandardSword,
        StrongSword,
        StandardGun,
        StolenCutlass,
        TreeBranch,
        WeakSword,
    }

    /// <summary>
    /// Type of bullet to create.
    /// </summary>
    public enum BulletType
    {
        StandardBullet,
        TurretBullet,
    }

    /// <summary>
    /// Handles creating weapons and bullets and adding them to the game.
    /// </summary>
    public class WeaponFactory
    {
        #region Base Weapons
        //Base weapon objects. Everything but the entity id will be set here. We can use if we want to create variations of weapons (like a strong, standard, weak variation of the sword)

        private static Weapon _standardSword = new Weapon()
        {
            Critical = .1f,
            Damage = 1,
            Effect = WeaponEffect.None,
            Range = 1,
            Speed = 1f,
            AttackType = WeaponAttackType.Melee,
        };

        private static Weapon _standardGun = new Weapon()
        {
            Critical = .1f,
            Damage = 1,
            Effect = WeaponEffect.None,
            Range = 1,
            Speed = .1f,
            AttackType = WeaponAttackType.Ranged,
        };

        private static Bullet _standardBullet = new Bullet()
        {
            Damage = 1,
        };

        #endregion

        /// <summary>
        /// Parent game.
        /// </summary>
        private DungeonCrawlerGame _game;

        /// <summary>
        /// Creates a new Factory.
        /// </summary>
        /// <param name="game"></param>
        public WeaponFactory(DungeonCrawlerGame game)
        {
            _game = game;
        }

        /// <summary>
        /// Creates a new weapon and adds it to the game. (No other components created)
        /// </summary>
        /// <param name="type">The type of weapon to create.</param>
        public uint CreateWeapon(WeaponType type)
        {
            uint eid = Entity.NextEntity();
            Weapon weapon = new Weapon { EntitiyID = eid };
            Sound? sound = null;

            switch (type)
            {
                case WeaponType.ShockRod:
                    weapon = _standardSword;
                    sound = new Sound { SoundEffect = _game.Content.Load<SoundEffect>("Sounds/sword sound") };
                    break;

                case WeaponType.PsychicStun:
                    weapon = _standardSword;
                    sound = new Sound { SoundEffect = _game.Content.Load<SoundEffect>("Sounds/sword sound") };
                    break;

                case WeaponType.DeadHand:
                    weapon = _standardSword;
                    sound = new Sound { SoundEffect = _game.Content.Load<SoundEffect>("Sounds/sword sound") };
                    break;

                case WeaponType.WeakSword:
                    weapon = _standardSword;
                    weapon.Critical *= .5f;
                    weapon.Damage *= .5f;
                    sound = new Sound { SoundEffect = _game.Content.Load<SoundEffect>("Sounds/sword sound") };
                    break;

                case WeaponType.StandardSword:
                    weapon = _standardSword;
                    sound = new Sound { SoundEffect = _game.Content.Load<SoundEffect>("Sounds/sword sound") };
                    break;

                case WeaponType.StrongSword:
                    weapon = _standardSword;
                    weapon.Critical *= 2;
                    weapon.Damage *= 2;
                    sound = new Sound { SoundEffect = _game.Content.Load<SoundEffect>("Sounds/sword sound") };
                    break;

                case WeaponType.StandardGun:
                    weapon = _standardGun;
                    break;

                case WeaponType.StolenCutlass:
                    weapon = _standardSword;
                    sound = new Sound { SoundEffect = _game.Content.Load<SoundEffect>("Sounds/sword sound") };
                    break;

                case WeaponType.TreeBranch:
                    weapon = _standardSword;
                    sound = new Sound { SoundEffect = _game.Content.Load<SoundEffect>("Sounds/sword sound") };
                    break;

                default:
                    throw new Exception("Unknown WeaponType");
            }

            weapon.Type = type;
            _game.WeaponComponent.Add(eid, weapon);
            if(sound != null)
                _game.SoundComponent.Add(eid, (Sound)sound);

            return eid;
        }

        public void CreateWeaponSprite(uint weaponID, uint playerID)
        {
            WeaponType type = _game.WeaponComponent[weaponID].Type;
            int y = (int)_game.SpriteAnimationComponent[playerID].CurrentAnimationRow * 64; //changed to get direction from spriteanimation instead of movementsprite, currentAnimationRow returns same values as facing for directions

            WeaponSprite sprite = new WeaponSprite()
            {
                EntityID = playerID,
            };

            switch (type)
            {
                case WeaponType.ShockRod:
                    sprite.SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Weapons/ShockRod");
                    sprite.SpriteBounds = new Rectangle(0, y, 64, 64);
                    break;
                case WeaponType.DeadHand:
                    sprite.SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Weapons/DeadHand");
                    sprite.SpriteBounds = new Rectangle(0, y, 64, 64);
                    break;
                case WeaponType.PsychicStun:
                    sprite.SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Weapons/PsychicStun");
                    sprite.SpriteBounds = new Rectangle(0, y, 64, 64);
                    break;
                case WeaponType.WeakSword:
                case WeaponType.StandardSword:
                case WeaponType.StrongSword:
                    sprite.SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Weapons/StandardSword");
                    sprite.SpriteBounds = new Rectangle(0, y, 64, 64);
                    break;
                case WeaponType.StandardGun:
                    sprite.SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Weapons/StandardSword");
                    sprite.SpriteBounds = new Rectangle(0, y, 64, 64);
                    break;
                case WeaponType.StolenCutlass:
                    sprite.SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Weapons/StolenCutlass");
                    sprite.SpriteBounds = new Rectangle(0, y, 64, 64);
                    break;
                case WeaponType.TreeBranch:
                    sprite.SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Weapons/TreeBranch");
                    sprite.SpriteBounds = new Rectangle(0, y, 64, 64);
                    break;
            }
            _game.WeaponSpriteComponent.Add(sprite.EntityID, sprite);
        }

        public uint CreateBullet(WeaponType weaponType, Vector2 direction, Position position)
        {
            uint bulletId;
            switch (weaponType)
            {
                case WeaponType.StandardGun:
                    bulletId = CreateBullet(BulletType.StandardBullet, direction, position);
                    break;
                default:
                    throw new Exception("Unknown weapon type.");
            }
            return bulletId;
        }

        /// <summary>
        /// Creates a new bullet and adds it to the game. (Will also create the position, movement, and sprite components)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="position"></param>
        /// <param name="direction"></param>
        public uint CreateBullet(BulletType type, Vector2 direction, Position position)
        {
            Bullet bullet;
            Movement movement;
            Sprite sprite;
            Collideable collideable;
            uint eid = Entity.NextEntity();

            position.EntityID = eid;
            position.Center += direction * 70;


            switch (type)
            {
                case BulletType.StandardBullet:
                    bullet = _standardBullet;
                    bullet.EntityID = eid;
                    movement = new Movement()
                    {
                        EntityID = eid,
                        Direction = direction,
                        Speed = 300,
                    };
                    sprite = new Sprite()
                    {
                        EntityID = eid,
                        SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Weapons/Bullets/BlueBullet"),
                        SpriteBounds = new Rectangle(0, 0, 10, 10),
                    };
                    position.Radius = 5;
                    break;

                case BulletType.TurretBullet:
                    position.Center -= direction * 63;
                    bullet = _standardBullet;
                    bullet.EntityID = eid;
                    movement = new Movement()
                    {
                        EntityID = eid,
                        Direction = direction,
                        Speed = 300,
                    };
                    sprite = new Sprite()
                    {
                        EntityID = eid,
                        SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Weapons/Bullets/BlueBullet"),
                        SpriteBounds = new Rectangle(0, 0, 10, 10),
                    };
                    position.Radius = 5;
                    break;

                default:
                    throw new Exception("Unknown BulletType");
            }

            collideable = new Collideable()
            {
                EntityID = eid,
                RoomID = position.RoomID,
                Bounds = new CircleBounds(position.Center, position.Radius),
            };

            
            _game.BulletComponent.Add(eid, bullet);
            _game.MovementComponent.Add(eid, movement);
            _game.PositionComponent.Add(eid, position);
            _game.SpriteComponent.Add(eid, sprite);
            _game.CollisionComponent.Add(eid, collideable);
            return eid;
        }
    }
}
