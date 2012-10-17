using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

using DungeonCrawlerWindowsLibrary;


namespace DungeonCrawlerContentPipeline
{
    /// <summary>
    /// The content pipeline equivalent of a TilemapLayer
    /// </summary>
    [ContentSerializerRuntimeType("DungeonCrawlerWindowsLibrary.TilemapLayer, DungeonCrawlerWindowsLibrary")]
    public struct TilemapLayerContent
    {
        /// <summary>
        /// The current scrolling offset in the y axis
        /// </summary>
        //public float ScrollOffset;

        /// <summary>
        /// The speed at which this layer scrolls
        /// (needed for paralax scrolling)
        /// </summary>
        //public float ScrollingSpeed;

        /// <summary>
        /// The depth of the tiles in this layer
        /// (0 is foremost, and 1 is rearmost)
        /// </summary>
        public float LayerDepth;

        /// <summary>
        /// The tile data of this layer
        /// </summary>
        public TileData[] TileData;

        /// <summary>
        /// A list of properties to apply to this TilemapLayer.  These
        /// should be converted to more meaningful and efficient
        /// variable representations in our TilemapProcessor
        /// </summary>
        [ContentSerializerIgnore]
        public Dictionary<string, string> Properties;
    }
}
