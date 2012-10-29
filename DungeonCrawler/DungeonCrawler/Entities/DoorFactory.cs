﻿#region File Description
//-----------------------------------------------------------------------------
// DoorComponent.cs 
//
// Author: Nicholas Strub (Assignment 6)
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
    /// A class for creating new Doors
    /// </summary>
    public class DoorFactory
    {
        /// <summary>
        /// The game this AggregateFactory belongs to
        /// </summary>
        DungeonCrawlerGame game;

        /// <summary>
        /// Creates a new RoomFactory instance
        /// </summary>
        /// <param name="game"></param>
        public DoorFactory(DungeonCrawlerGame game)
        {
            this.game = game;

            
        }

        public uint CreateDoor(string destinationRoom, string destinationSpawn)
        {
            uint entityID = Entity.NextEntity();

            /*Texture2D spriteSheet = game.Content.Load<Texture2D>("");
            spriteSheet.Name = "";*/

            Position position = new Position()
            {
                EntityID = entityID,
                // Center and Radius TBD Later
                Center = new Vector2(0, 0),
                Radius = 32f,
            };
            game.PositionComponent[entityID] = position;

            Local local = new Local()
            {
                EntityID = entityID,
            };
            game.LocalComponent[entityID] = local;

            Sprite sprite = new Sprite()
            {
                EntityID = entityID,
                // TODO: SpriteSheet and SpriteBounds need a sprite and its bounds. I'm
                // waiting to see what the dungeons look like before choosing a sprite
                // so that the door sprite matches the dungeon theme.
                SpriteSheet = null,
                SpriteBounds = new Rectangle(0, 0, 0, 0),
            };
            game.SpriteComponent[entityID] = sprite;

            Door door = new Door()
            {
                EntityID = entityID,
                DestinationRoom = destinationRoom,
                DestinationSpawnName = destinationSpawn,
            };
            game.DoorComponent[entityID] = door;

            return entityID;
        }
    }
}
