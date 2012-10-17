using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonCrawler.Components
{
    #region Melee
    /// <summary>
    /// Contains information for drawing a melee weapon and it's animations.
    /// </summary>
    public struct MeleeWeaponSprite
    {
        public uint EntityID;
    }

    public class MeleeWeaponSpriteComponent : GameComponent<MeleeWeaponSprite>
    {

    }
    #endregion

    #region Ranged
    /// <summary>
    /// Contains information for drawing a ranged weapon and it's animations.
    /// </summary>
    public struct RangedWeaponSprite
    {
    }

    public class RangedWeaponSpriteComponent : GameComponent<RangedWeaponSprite>
    {
    }

    /// <summary>
    /// Contains information for drawing a bullet.
    /// </summary>
    public struct BulletSprite
    {
    }

    public class BulletSpriteCompoenet : GameComponent<BulletSprite>
    {
    }
    #endregion
}
