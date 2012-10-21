using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonCrawler.Components
{
    public struct Equipment
    {
        public uint EntityID;

        public uint WeaponID;
        public uint AltWeaponID;

        public uint Skill1ID;
        public uint Skill2ID;
        public uint Skill3ID;
        public uint Skill4ID;

        public uint Item1ID;
        public uint Item2ID;
        public uint Item3ID;
        public uint Item4ID;
    }

    public class EquipmentComponent : GameComponent<Equipment>
    {
    }
}
