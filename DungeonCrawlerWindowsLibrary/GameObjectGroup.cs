using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace DungeonCrawlerWindowsLibrary
{
    /// <summary>
    /// Represents a single group of GameObjects, at a 
    /// specific elevation within the game world
    /// </summary>
    public struct GameObjectGroup
    {
        ///// <summary>
        ///// The current scrolling offset in the y axis
        ///// </summary>
        //public float ScrollOffset;

        ///// <summary>
        ///// The speed at which this layer scrolls
        ///// (needed for paralax scrolling)
        ///// </summary>
        //public float ScrollingSpeed;

        /// <summary>
        /// The depth of the objects in this layer
        /// (0 is foremost, and 1 is rearmost)
        /// </summary>
        public float LayerDepth;

        /// <summary>
        /// The tile data of this layer
        /// </summary>
        public GameObjectData[] GameObjectData;
    }
}
