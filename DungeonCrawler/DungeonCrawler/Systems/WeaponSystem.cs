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
            foreach (Player player in _game.PlayerComponent.All)
            {
                if (true) //Need to figure out attacking flag
                    continue;

                Equipment e = _game.EquipmentComponent[player.EntityID];
                
            }
        }

        //Handles Creating the WeaponSprite.
        private void CreateWeaponSprite(Equipment equipment)
        {
            WeaponType type = _game.WeaponComponent[equipment.WeaponID].Type;
            Position position = _game.PositionComponent[equipment.EntityID];

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
                    sprite.SpriteBounds = new Rectangle(0, 0, 64, 64);
                    break;
                case WeaponType.StandardGun:
                    sprite.SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/StandardGun");
                    sprite.SpriteBounds = new Rectangle(0, 0, 64, 64);
                    break;
            }
            _game.SpriteComponent.Add(sprite.EntityID, sprite);

            position.EntityID = sprite.EntityID;
            _game.PositionComponent.Add(position.EntityID, position);
        }

        //Handles creating the Bullet object and sprite.
        private void CreateBulletAndSprite(uint weaponID)
        {

        }

        //Handles updating already made weapon sprites.
        private void UpdateWeaponSprite(uint weaponID)
        {
            Sprite sprite = _game.SpriteComponent[weaponID];

            if (ring(.5f))
            {
                if (sprite.SpriteBounds.X < 192)
                    sprite.SpriteBounds.X += 64;
                else
                    _game.SpriteComponent.Remove(weaponID);
            }
        }

        /// <summary>
        /// Use this to determine if timer has elasped.
        /// </summary>
        /// <param name="alarm"></param>
        /// <returns></returns>
        private bool ring(float alarm)
        {
            return (_timer % alarm) < .0001;
        }
    }
}
