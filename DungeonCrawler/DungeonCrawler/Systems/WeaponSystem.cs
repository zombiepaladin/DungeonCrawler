using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DungeonCrawler.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
            foreach (Weapon weapon in _game.WeaponComponent.WeaponsInUse)
            {
                if (_game.SpriteComponent.Contains(weapon.EntitiyID))
                    UpdateWeaponSprite(weapon.EntitiyID);
                else
                {
                    CreateWeaponSprite(weapon.EntitiyID);
                    if (weapon.Type == WeaponAttackType.Ranged) //TODO implement timer.
                        CreateBulletAndSprite(weapon.EntitiyID);
                }
            }
        }

        //Handles Creating the WeaponSprite.
        private void CreateWeaponSprite(uint weaponID)
        {
            Sprite sprite = new Sprite()
            {
                EntityID = weaponID,
                SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/StandardSword"),
                SpriteBounds = new Rectangle(0, 0, 64, 64),
            };
            _game.SpriteComponent.Add(weaponID, sprite);

            Position position = new Position()
            {
                EntityID = weaponID,
                Center = new Vector2(25),
                Radius = 32,
            };
            _game.PositionComponent.Add(weaponID, position);
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

        private bool ring(float alarm)
        {
            return (_timer % alarm) < .0001;
        }
    }
}
