using Microsoft.Xna.Framework.Graphics;

namespace DungeonCrawlerWindowsLibrary
{
    /// <summary>
    /// A structure representing a tile in the game world
    /// </summary>
    public struct TileData
    {
        /// <summary>
        /// The Tile's ID
        /// </summary>
        public uint TileID;

        /// <summary>
        /// Indicates the sprite's orientation
        /// </summary>
        public SpriteEffects SpriteEffects;
    }
}
