using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using DungeonCrawler.Entities;

namespace DungeonCrawler.Components
{
    #region Weapon
    /// <summary>
    /// The type of attack the weapon does.
    /// </summary>
    public enum WeaponAttackType
    {
        Melee,
        Ranged,
    }

    /// <summary>
    /// Different effects weapons can have. These should be bitmasked so we can store multiple effects.
    /// </summary>
    public enum WeaponEffect
    {
        None = 0x0,
        Stun = 0x1,
        One_Hit_KO = 0x2,
        Slow = 0x4,
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
        /// Chance for a critical hit.
        /// </summary>
        public float Critical;

        /// <summary>
        /// This weapon's attack type.
        /// </summary>
        public WeaponAttackType AttackType;

        /// <summary>
        /// What the weapon actually is. We will need this when creating sprites.
        /// </summary>
        public WeaponType Type;

        /// <summary>
        /// The effects this weapon has.
        /// </summary>
        public WeaponEffect Effect;
    }

    /// <summary>
    /// Manages the weapons currently in the game.
    /// </summary>
    public class WeaponComponent : GameComponent<Weapon>
    {
        /// <summary>
        /// Returns true if the weapon has the given effect set.
        /// </summary>
        /// <param name="weapon">Weapon struct</param>
        /// <param name="effect">The WeaponEffect to check</param>
        /// <returns>True or false.</returns>
        public bool HasEffect(Weapon weapon, WeaponEffect effect)
        {
            return (weapon.Effect & effect) != WeaponEffect.None;
        }

        public bool HasEffect(uint id, WeaponEffect effect)
        {
            return HasEffect(this[id], effect);
        }

        /// <summary>
        /// Adds the given effect to the given Weapon.
        /// </summary>
        /// <param name="weapon">Weapon struct</param>
        /// <param name="effect">The WeaponEffect to add.</param>
        public void SetEffect(Weapon weapon, WeaponEffect effect)
        {
            weapon.Effect |= effect;
        }

        public void SetEffect(uint id, WeaponEffect effect)
        {
            SetEffect(this[id], effect);
        }

        /// <summary>
        /// REmoves the given effect from the given Weapon.
        /// </summary>
        /// <param name="weapon">Weapon struct</param>
        /// <param name="effect">The WeaponEffecft to remove.</param>
        public void RemoveEffect(Weapon weapon, WeaponEffect effect)
        {
            weapon.Effect &= ~effect;
        }

        public void RemoveEffect(uint id, WeaponEffect effect)
        {
            RemoveEffect(this[id], effect);
        }
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
    }

    public class BulletComponent : GameComponent<Bullet>
    {
    }
    #endregion
}
