using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DungeonCrawler.Components
{
    #region Weapon
    /// <summary>
    /// Type for the weapons.
    /// </summary>
    public enum WeaponType
    {
        Melee,
        Ranged,
    }

    /// <summary>
    /// Different effects weapons can have. These should be bitmasked so we can store multiple effects.
    /// </summary>
    public enum WeaponEffect
    {
        Stun = 0x1,

    }

    /// <summary>
    /// Weapon data.
    /// </summary>
    public struct Weapon
    {
        /// <summary>
        /// Entitiy ID.
        /// </summary>
        public uint EntitiyID;

        /// <summary>
        /// Damage this weapon does.
        /// </summary>
        public float Damage;

        /// <summary>
        /// How fast this weapon can attack.
        /// </summary>
        public float Speed;

        /// <summary>
        /// The range this weapon has. Ranged weapon should have a large range while Melee should have a very small range.
        /// </summary>
        public float Range;

        /// <summary>
        /// This weapon's type.
        /// </summary>
        public WeaponType Type;

        /// <summary>
        /// The effects this weapon has.
        /// </summary>
        public WeaponEffect Effect;
    }

    public class WeaponComponent : GameComponent<Weapon>
    {
    }
    #endregion

    #region Bullet
    /// <summary>
    /// Bullet data. Bullets should be created by Ranged weapons.
    /// </summary>
    public struct Bullet
    {
        /// <summary>
        /// Entity Id.
        /// </summary>
        public uint EntityID;

        /// <summary>
        /// Damage the bullet does. (Should be the same as the weapon that created it.)
        /// </summary>
        public float Damage;

        /// <summary>
        /// Direction the bullet is traveling. (Should be normalized.)
        /// </summary>
        public Vector2 Direction;

        /// <summary>
        /// The speed of the bullet.
        /// </summary>
        public float Speed;
    }

    public class BulletComponent : GameComponent<Bullet>
    {
    }
    #endregion
}
