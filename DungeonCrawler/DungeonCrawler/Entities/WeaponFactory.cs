using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DungeonCrawler.Components;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DungeonCrawler.Entities
{
    /// <summary>
    /// Type of weapon to create.
    /// </summary>
    public enum WeaponType
    {
        WeakSword,
        StandardSword,
        StrongSword,
        StandardGun,
    }

    /// <summary>
    /// Type of bullet to create.
    /// </summary>
    public enum BulletType
    {
        StandardBullet,
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
            Weapon weapon;

            switch (type)
            {
                case WeaponType.WeakSword:
                    weapon = _standardSword;
                    weapon.Critical *= .5f;
                    weapon.Damage *= .5f;
                    break;

                case WeaponType.StandardSword:
                    weapon = _standardSword;
                    break;

                case WeaponType.StrongSword:
                    weapon = _standardSword;
                    weapon.Critical *= 2;
                    weapon.Damage *= 2;
                    break;

                case WeaponType.StandardGun:
                    weapon = _standardGun;
                    break;

                default:
                    throw new Exception("Unknown WeaponType");
            }

            weapon.EntitiyID = eid;
            weapon.Type = type;
            _game.WeaponComponent.Add(eid, weapon);
            return eid;
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
            uint eid = Entity.NextEntity();

            position.EntityID = eid;
            position.Center.X += 32;
            position.Center.Y += 32;

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
                        SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/BlueBullet"),
                        SpriteBounds = new Rectangle(0, 0, 10, 10),
                    };
                    break;
                default:
                    throw new Exception("Unknown BulletType");
            }

            _game.BulletComponent.Add(eid, bullet);
            _game.MovementComponent.Add(eid, movement);
            _game.PositionComponent.Add(eid, position);
            _game.SpriteComponent.Add(eid, sprite);
            return eid;
        }
    }
}
