using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonCrawler.Components
{
    public struct Inventory
    {
        public uint EntityID;
        public uint BackgroundSpriteID;
        public uint SelectorSpriteID;
    }


    /// <summary>
    /// A component containing a simple sprite to render on-screen
    /// </summary>
    public class InventoryComponent : GameComponent<Inventory>
    {

    }
}
