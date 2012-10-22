using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DungeonCrawler.Components;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DungeonCrawler.Entities
{
    public enum WeaponType
    {
        WeakSword,
        StandardSword,
        StrongSword,
        StandardGun,
    }

    public enum BulletType
    {
        StandardBullet,
    }

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
            Speed = 1f,
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
        /// Creates a new weapon and adds it to the game.
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
        /// Creates a new bullet and adds it to the game.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="position"></param>
        /// <param name="direction"></param>
        public uint CreateBullet(BulletType type, Position position, Vector2 direction)
        {
            Movement movement;
            uint eid = Entity.NextEntity();

            switch (type)
            {
                case BulletType.StandardBullet:
                    //Bullet Component
                    _standardBullet.EntityID = eid;

                    //Position Component
                    position.EntityID = eid;
                    _game.PositionComponent.Add(eid, position);

                    //Movement Component
                    direction.Normalize();
                    movement = new Movement()
                    {
                        EntityID = eid,
                        Direction = direction,
                        Speed = 1,
                    };
                    _game.MovementComponent.Add(eid, movement);
                    break;

                default:
                    throw new Exception("Unknown BulletType");
            }

            return eid;
        }
    }
}
