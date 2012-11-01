
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonCrawler.Entities
{
    /// <summary>
    /// An enumeration of all possible aggregates
    /// </summary>
    public enum Aggregate
    {
        FairyPlayer,
        CultistPlayer = 1,
        EarthianPlayer = 2,
        CyborgPlayer = 3,
        GargranianPlayer = 4,
        SpacePiratePlayer = 5,
        ZombiePlayer = 6,
    }
}
