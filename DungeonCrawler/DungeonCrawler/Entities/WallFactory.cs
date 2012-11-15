#region File Description
//-----------------------------------------------------------------------------
// WallFactory.cs 
//
// Author: Matthew McHaney
//
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence 
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using DungeonCrawler.Components;
#endregion

namespace DungeonCrawler.Entities
{
    /// <summary>
    /// A class for creating new Wall
    /// </summary>
    public class WallFactory
    {
        /// <summary>
        /// The game this WallFactory belongs to
        /// </summary>
        DungeonCrawlerGame game;

        /// <summary>
        /// Creates a new RoomFactory instance
        /// </summary>
        /// <param name="game"></param>
        public WallFactory(DungeonCrawlerGame game)
        {
            this.game = game;

            
        }

        public uint CreateWall(uint roomId, Rectangle bounds)
        {
            uint entityID = Entity.NextEntity();

            /*Texture2D spriteSheet = game.Content.Load<Texture2D>("");
            spriteSheet.Name = "";*/

            //It's assumed in collisions that anything in position/collideable but not in other
            // components is a static object (like a wall)

            Position position = new Position()
            {
                EntityID = entityID,
                // Center and Radius TBD Later
                Center = new Vector2(bounds.Left, bounds.Top),
                Radius = 1,
                RoomID = roomId,
            };
            game.PositionComponent[entityID] = position;

            Collideable collideable = new Collideable()
            {
                EntityID = entityID,
                RoomID = position.RoomID,
                Bounds = new RectangleBounds(bounds.Left,bounds.Top, bounds.Width, bounds.Height),
                // Center and Radius TBD Later
            };
            game.CollisionComponent[entityID] = collideable;

            Local local = new Local()
            {
                EntityID = entityID,
            };
            game.LocalComponent[entityID] = local;

            return entityID;
        }
    }
}
