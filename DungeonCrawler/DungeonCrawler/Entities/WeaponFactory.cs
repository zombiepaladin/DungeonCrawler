using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonCrawler.Entities
{
    public enum WeaponType
    {
        StandardSword,
        StandardGun,
    }

    public class WeaponFactory
    {
        private DungeonCrawlerGame _game;

        public WeaponFactory(DungeonCrawlerGame game)
        {
            _game = game;
        }

        public void CreateWeapon(WeaponType type)
        {
            switch (type)
            {
                case WeaponType.StandardSword:
                    break;

                case WeaponType.StandardGun:
                    break;

                default:
                //Create the default sword.
                    break;
            }
        }
    }
}
