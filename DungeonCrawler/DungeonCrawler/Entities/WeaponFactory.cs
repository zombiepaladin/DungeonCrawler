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
        StandardSword,
        StandardGun,
    }

    public enum BulletType
    {
        StandardBullet,
    }

    public class WeaponFactory
    {
        #region Base Weapons
        //Base weapon objects. Everything but the entity id will be set here.

        private static Weapon _standardSword = new Weapon()
        {
            Critical = .1f,
            Damage = 1,
            Effect = WeaponEffect.None,
            Range = 1,
            Speed = 1f,
            Type = WeaponAttackType.Melee,
        };

        private static Weapon _standardGun = new Weapon()
        {
            Critical = .1f,
            Damage = 1,
            Effect = WeaponEffect.None,
            Range = 1,
            Speed = 1f,
            Type = WeaponAttackType.Ranged,
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
            Sprite sprite;
            uint eid = Entity.NextEntity();

            switch (type)
            {
                case WeaponType.StandardSword:
                    //Weapon Component
                    _standardSword.EntitiyID = eid;
                    _game.WeaponComponent.Add(eid, _standardSword);

                    //Sprite Component (Consider making some of this static)
                    sprite = new Sprite()
                    {
                        EntityID = eid,
                        SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/StandardSword"),
                        SpriteBounds = new Rectangle(0, 0, 64, 64),
                    };
                    _game.SpriteComponent.Add(eid, sprite);

                    //Position Component (Just make the placehold, this will be updated later)
                    _game.PositionComponent.Add(eid, new Position());
                    break;

                case WeaponType.StandardGun:
                    //Weapon Component
                    _standardGun.EntitiyID = eid;
                    _game.WeaponComponent.Add(eid, _standardSword);

                    //Sprite Component (Consider making some of this static)
                    sprite = new Sprite()
                    {
                        EntityID = eid,
                        SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/StandardGun"),
                        SpriteBounds = new Rectangle(0, 0, 64, 64),
                    };
                    _game.SpriteComponent.Add(eid, sprite);

                    //Position Component (Just make the placehold, this will be updated later)
                    _game.PositionComponent.Add(eid, new Position());
                    break;

                default:
                    throw new Exception("Unknown WeaponType");
            }

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
