using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonCrawler
{
    public interface Bounds
    {
        bool Intersect(Bounds obj);
    }
}
