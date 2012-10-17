using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonCrawlerWindowsLibrary
{
    /// <summary>
    /// A structure representing a tile in a tileset
    /// </summary>
    public struct Tile
    {
        /// <summary>
        /// The ID of the texture this tile is found in
        /// </summary>
        public int TextureID;

        /// <summary>
        /// The source rectangle of the tile within
        /// its texture
        /// </summary>
        public Rectangle Source;
    }
}
