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

            WeaponSprite[] weaponSprites = _game.WeaponSpriteComponent.All.ToArray();
            for (int i = 0; i < weaponSprites.Length; i++)
            {
                UpdateWeaponSprite(weaponSprites[i]);
            }

            foreach (Player player in _game.PlayerComponent.All)
            {
                if (_game.PlayerInfoComponent[player.EntityID].State != PlayerState.Attacking)
                    continue;
                CreateWeaponSprite(_game.EquipmentComponent[player.EntityID]);
                PlayerInfo info = _game.PlayerInfoComponent[player.EntityID];
                info.State = PlayerState.Default;
                _game.PlayerInfoComponent[player.EntityID] = info;
            }
        }

        //Handles Creating the WeaponSprite.
        private void CreateWeaponSprite(Equipment equipment)
        {
            WeaponType type = _game.WeaponComponent[equipment.WeaponID].Type;
            Position position = _game.PositionComponent[equipment.EntityID];
            int y = (int)_game.MovementSpriteComponent[equipment.EntityID].Facing * 64;

            WeaponSprite sprite = new WeaponSprite()
            {
                EntityID = Entity.NextEntity(),
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
                    sprite.SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/StandardGun");
                    sprite.SpriteBounds = new Rectangle(0, y, 64, 64);
                    break;
            }
            _game.WeaponSpriteComponent.Add(sprite.EntityID, sprite);

            position.EntityID = sprite.EntityID;
            _game.PositionComponent.Add(position.EntityID, position);
        }

        //Handles creating the Bullet object and sprite.
        private void CreateBulletAndSprite(Equipment equipment)
        {
            Position position = _game.PositionComponent[equipment.EntityID];
            Vector2 direction = _game.MovementComponent[equipment.EntityID].Direction;
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
    }
}
