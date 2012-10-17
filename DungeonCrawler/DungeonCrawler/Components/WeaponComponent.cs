using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonCrawler.Components
{
    public enum WeaponType
    {
        Melee,
        Ranged,
    }

    public enum WeaponEffect
    {
    }

    public struct Weapon
    {
        public uint EntitiyID;
        public int Damage;
        public WeaponType Type;
        public WeaponEffect Effect;
    }

    class WeaponComponent
    {
    }
}
