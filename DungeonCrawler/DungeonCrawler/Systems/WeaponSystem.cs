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
            //Implement logic here.
        }

        //Handles Creating the WeaponSprite.
        private void CreateWeaponSprite(Equipment equipment)
        {
            WeaponType type = _game.WeaponComponent[equipment.WeaponID].Type;
            Position position = _game.PositionComponent[equipment.EntityID];
            int y = (int)_game.MovementSpriteComponent[equipment.EntityID].Facing * 64;

            Sprite sprite = new Sprite()
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
            _game.SpriteComponent.Add(sprite.EntityID, sprite);

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

        //Handles updating already made weapon sprites.
        private void UpdateWeaponSprite(uint spriteID)
        {
            Sprite sprite = _game.SpriteComponent[spriteID];

            if (_timer >= .5f)
            {
                if (sprite.SpriteBounds.X < 192)
                    sprite.SpriteBounds.X += 64;
                else
                    _game.SpriteComponent.Remove(spriteID);

                _timer = 0;
            }
        }
    }
}
