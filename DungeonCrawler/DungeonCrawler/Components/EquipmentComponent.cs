using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonCrawler.Components
{
    public struct Equipment
    {
        public uint EntityID;
        public uint PlayerID;
        public uint WeaponID;
    }

    public class EquipmentComponent : GameComponent<Equipment>
    {
    }
}
