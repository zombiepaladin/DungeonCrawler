using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonCrawler.Components
{
    public struct Colliedable
    {
        public uint EntityID;

        public Bounds Bounds;
    }

    public class CollisionComponent : GameComponent<Colliedable>
    {

    }
}
