using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DungeonCrawler.Components;

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

        }

        //Handles creating the Bullet object and sprite.
        private void CreateBulletAndSprite(uint weaponID)
        {

        }

        //Handles updating already made weapon sprites.
        private void UpdateWeaponSprite(uint weaponID)
        {

        }
    }
}
