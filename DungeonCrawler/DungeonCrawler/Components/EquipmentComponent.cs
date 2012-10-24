using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonCrawler.Components
{
    /// <summary>
    /// Contains id of items players has equiped.
    /// </summary>
    public struct Equipment
    {
        /// <summary>
        /// ID of this component. Should be the same as playerID.
        /// </summary>
        public uint EntityID;

        /// <summary>
        /// The ID of the main weapon equiped.
        /// </summary>
        public uint WeaponID;

        /// <summary>
        /// ID of the Alternative Weapon.
        /// </summary>
        public uint AltWeaponID;

        /// <summary>
        /// ID of the skill equiped to the 1st skill slot.
        /// </summary>
        public uint Skill1ID;

        /// <summary>
        /// ID of the skill equiped to the 2nd skill slot.
        /// </summary>
        public uint Skill2ID;

        /// <summary>
        /// ID of the skill equiped to the 3rd skill slot.
        /// </summary>
        public uint Skill3ID;

        /// <summary>
        /// ID of the skill equiped to the 4th skill slot.
        /// </summary>
        public uint Skill4ID;

        /// <summary>
        /// ID of the item equiped to the 1st item slot.
        /// </summary>
        public uint Item1ID;

        /// <summary>
        /// ID of the item equiped to the 2nd item slot.
        /// </summary>
        public uint Item2ID;

        /// <summary>
        /// ID of the item equiped to the 3rd item slot.
        /// </summary>
        public uint Item3ID;

        /// <summary>
        /// ID of the item equiped to the 4th item slot.
        /// </summary>
        public uint Item4ID;
    }

    public class EquipmentComponent : GameComponent<Equipment>
    {
    }
}
