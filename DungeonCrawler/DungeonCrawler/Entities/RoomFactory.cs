using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DungeonCrawler.Components;

namespace DungeonCrawler.Entities
{
    /// <summary>
    /// A class for creating new rooms
    /// </summary>
    public class RoomFactory
    {
        /// <summary>
        /// The game this AggregateFactory belongs to
        /// </summary>
        DungeonCrawlerGame game;

        /// <summary>
        /// Creates a new RoomFactory instance
        /// </summary>
        /// <param name="game"></param>
        public RoomFactory(DungeonCrawlerGame game)
        {
            this.game = game;
        }

        public uint CreateRoom(string TilemapName, int width, int height, int tileWidth, int tileHeight, int wallWidth)
        {
            uint entityID = Entity.NextEntity();

            Position position = new Position()
            {
                EntityID = entityID,
                // Center and Radius TBD Later
                Center = new Vector2(0, 0),
                Radius = 32f,
                Collideable = false,
            };
            game.PositionComponent[entityID] = position;

            Local local = new Local()
            {
                EntityID = entityID,
            };
            game.LocalComponent[entityID] = local;

            Room room = new Room()
            {
                EntityID = entityID,
                Tilemap = TilemapName,
                Width = width,
                Height = height,
                TileWidth = tileWidth,
                TileHeight = tileHeight,
                WallWidth = wallWidth,
            };
            room.idMap = new Dictionary<string,uint>();
            room.targetTypeMap= new Dictionary<string, string>();
            game.RoomComponent[entityID] = room;

            return entityID;
        }
    }
}
