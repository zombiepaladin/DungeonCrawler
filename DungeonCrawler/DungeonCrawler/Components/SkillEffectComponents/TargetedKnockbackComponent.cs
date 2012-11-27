//Samuel Fike & Jiri Malina

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DungeonCrawler.Components
{
    public struct TargetedKnockBack
    {
        public uint TargetID;

        public Vector2 Origin;

        public float Distance;
    }

    public class TargetedKnockBackComponent : GameComponent<TargetedKnockBack>
    {
    }
}
