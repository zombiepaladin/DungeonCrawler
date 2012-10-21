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
        public void CreateWeapon(WeaponType type)
        {
            Sprite sprite;

            switch (type)
            {
                case WeaponType.StandardSword:
                    //Weapon Component
                    _standardSword.EntitiyID = Entity.NextEntity();
                    _game.WeaponComponent.Add(_standardSword.EntitiyID, _standardSword);

                    //Sprite Component (Consider making some of this static)
                    sprite = new Sprite()
                    {
                        EntityID = _standardSword.EntitiyID,
                        SpriteSheet = _game.Content.Load<Texture2D>("Content/standardSword"),
                        SpriteBounds = new Rectangle(0, 0, 64, 96),
                    };
                    _game.SpriteComponent.Add(_standardSword.EntitiyID, sprite);

                    //Position Component (Just make the placehold, this will be updated later)
                    _game.PositionComponent.Add(_standardGun.EntitiyID, new Position());
                    break;

                case WeaponType.StandardGun:
                    //Weapon Component
                    _standardGun.EntitiyID = Entity.NextEntity();
                    _game.WeaponComponent.Add(_standardGun.EntitiyID, _standardSword);

                    //Sprite Component (Consider making some of this static)
                    sprite = new Sprite()
                    {
                        EntityID = _standardGun.EntitiyID,
                        SpriteSheet = _game.Content.Load<Texture2D>("Content/standardGun"),
                        SpriteBounds = new Rectangle(0, 0, 64, 96),
                    };
                    _game.SpriteComponent.Add(_standardGun.EntitiyID, sprite);

                    //Position Component (Just make the placehold, this will be updated later)
                    _game.PositionComponent.Add(_standardGun.EntitiyID, new Position());
                    break;

                default:
                    throw new Exception("Unknown WeaponType");
            }
        }

        /// <summary>
        /// Creates a new bullet and adds it to the game.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="position"></param>
        /// <param name="direction"></param>
        public void CreateBullet(BulletType type, Position position, Vector2 direction)
        {
            Movement movement;

            switch (type)
            {
                case BulletType.StandardBullet:
                    //Bullet Component
                    _standardBullet.EntityID = Entity.NextEntity();

                    //Position Component
                    position.EntityID = _standardBullet.EntityID;
                    _game.PositionComponent.Add(_standardBullet.EntityID, position);

                    //Movement Component
                    direction.Normalize();
                    movement = new Movement()
                    {
                        EntityID = _standardBullet.EntityID,
                        Direction = direction,
                        Speed = 1,
                    };
                    _game.MovementComponent.Add(_standardBullet.EntityID, movement);

                    break;

                default:
                    throw new Exception("Unknown BulletType");
            }
        }
    }
}
