using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonCrawler.Components
{
    public struct Cloak
    {
        public uint EntityID;

        public float StartingTime;

        public float TimeLeft;

        public uint TargetID;

        public int spriteHeight;

        public Cloak(uint eid, uint targetID, float time)
        {
            EntityID = eid;
            TimeLeft = StartingTime = time;
            spriteHeight = -1;
            TargetID = targetID;
        }
    }

    public class CloakComponent : GameComponent<Cloak>
    {
    }
}
