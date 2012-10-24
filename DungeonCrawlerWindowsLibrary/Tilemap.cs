using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace DungeonCrawlerWindowsLibrary
{
    /// <summary>
    /// A class representing a tilemap
    /// </summary>
    public class Tilemap
    {
        /// <summary>
        /// The name of the tilemap
        /// </summary>
        public string Name;

        /// <summary>
        /// The tilemap's width, in tiles
        /// </summary>
        public int Width;

        /// <summary>
        /// The tilemap's height, in tiles
        /// </summary>
        public int Height;

        /// <summary>
        /// The width of the tilemap's tiles
        /// </summary>
        public int TileWidth;

        /// <summary>
        /// The height of the tilemap's tiles
        /// </summary>
        public int TileHeight;

        /// <summary>
        /// The width of the walls, in tiles
        /// </summary>
        public int WallWidth;

        /// <summary>
        /// The texture names used in this tilemap
        /// </summary>
        public string[] ImagePaths;

        /// <summary>
        /// The tileset textures used in this tilemap
        /// </summary>
        [ContentSerializerIgnore]
        public Texture2D[] Textures;

        /// <summary>
        /// The total number of tiles in our tileset
        /// </summary>
        public int TileCount;

        /// <summary>
        /// The set of all tiles used by this tilemap
        /// </summary>
        public Tile[] Tiles;

        /// <summary>
        /// The total number of layers in our tilemap
        /// </summary>
        public int LayerCount;

        /// <summary>
        /// The layers in our tilemap
        /// </summary>
        public TilemapLayer[] Layers;

        /// <summary>
        /// The total number of game object groups in our tilemap
        /// </summary>
        public int GameObjectGroupCount;

        /// <summary>
        /// The game object groups in our tilemap
        /// </summary>
        public GameObjectGroup[] GameObjectGroups;

        /// <summary>
        /// The starting position of the player
        /// </summary>
        public Vector2 PlayerStart;

        /// <summary>
        /// The layer in which the player exists
        /// </summary>
        public int PlayerLayer;

        /// <summary>
        /// The music to play in this level
        /// </summary>
        public string MusicTitle;

        /// <summary>
        /// Loads the dependent content for the tilemap
        /// </summary>
        /// <param name="contentManager">The content manager to load the textures with</param>
        public void LoadContent(ContentManager contentManager)
        {
            Textures = new Texture2D[ImagePaths.Length];

            for (int i = 0; i < ImagePaths.Length; i++)
            {
                Textures[i] = contentManager.Load<Texture2D>(ImagePaths[i]);
            }
        }
    }
}
