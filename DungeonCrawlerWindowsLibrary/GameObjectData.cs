using Microsoft.Xna.Framework;

namespace DungeonCrawlerWindowsLibrary
{
    /// <summary>
    /// A structure representing the instance-specific
    /// data for a GameObject within a level
    /// </summary>
    public struct GameObjectData
    {
        /// <summary>
        /// The game object ID assigned to this instance
        /// </summary>
        public uint ID;

        /// <summary>
        /// The broad category of game object (Enemy, Player, etc).
        /// </summary>
        public string Category;

        /// <summary>
        /// The specific type of game object (Dart, Shrike, etc).
        /// </summary>
        public string Type;

        /// <summary>
        /// The position of the game object instance within the game world
        /// </summary>
        public Rectangle Position;
    }
}
