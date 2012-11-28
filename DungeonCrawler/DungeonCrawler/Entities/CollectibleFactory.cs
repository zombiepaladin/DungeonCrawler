#region File Description
//-----------------------------------------------------------------------------
// CollectibleFactory.cs 
//
// Author: Matthew McHaney
//
// Note  : If you want to test this, add this code right before 
//         Loading is set to false in LoadLevel in LevelManager.
/*              game.CollectableFactory.CreateCollectible(
                    Entities.CollectibleType.money1, new Position
                    {
                        Center = new Vector2(300, 300),
                        Radius = 16,
                        Collideable = true,
                        RoomID = room.EntityID,
                        EntityID = 0
                    });*/
//
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence 
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DungeonCrawler.Components;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
#endregion

namespace DungeonCrawler.Entities
{
    /// <summary>
    /// Type of collectible to create.
    /// </summary>
    public enum CollectibleType
    {
        money1,
        money5,
        money10, //etc.
        health1,
        pog,
    }

    /// <summary>
    /// Handles creating collectibles and adding them to the game.
    /// </summary>
    public class CollectibleFactory
    {
        /// <summary>
        /// Parent game.
        /// </summary>
        private DungeonCrawlerGame _game;

        /// <summary>
        /// Creates a new Factory.
        /// </summary>
        /// <param name="game"></param>
        public CollectibleFactory(DungeonCrawlerGame game)
        {
            _game = game;
        }

        /// <summary>
        /// Creates a new weapon and adds it to the game. (No other components created)
        /// </summary>
        /// <param name="type">The type of weapon to create.</param>
        public uint CreateCollectible(CollectibleType type, Position position)
        {
            uint eid = Entity.NextEntity();
            Collectible collectible;
            Sprite sprite;
            Collideable collideable;

            /*
            Locations for the sprites:
            gold = 136,46,32,32
            silver = 170, 46, 32, 32
            bronze = 204, 46, 32, 32
            heart = 156, 360, 24, 24
             */

            switch (type)
            {
                case CollectibleType.health1:
                    collectible.CollectibleType = Components.CollectibleType.health;
                    collectible.CollectibleValue = 1;
                    sprite = new Sprite()
                    {
                        EntityID = eid,
                        SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/TestMiscIcons1"),
                        SpriteBounds = new Rectangle(156, 360, 24, 24),
                    };
                    break;

                case CollectibleType.money1:
                    collectible.CollectibleType = Components.CollectibleType.money;
                    collectible.CollectibleValue = 1;
                    sprite = new Sprite()
                    {
                        EntityID = eid,
                        SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/MiscIcons1"),
                        SpriteBounds = new Rectangle(204, 46, 32, 32),
                    };
                    break;

                case CollectibleType.money5:
                    collectible.CollectibleType = Components.CollectibleType.money;
                    collectible.CollectibleValue = 5;
                    sprite = new Sprite()
                    {
                        EntityID = eid,
                        SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/TestMiscIcons1"),
                        SpriteBounds = new Rectangle(170, 46, 32, 32),
                    };
                    break;

                case CollectibleType.money10:
                    collectible.CollectibleType = Components.CollectibleType.money;
                    collectible.CollectibleValue = 10;
                    sprite = new Sprite()
                    {
                        EntityID = eid,
                        SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/TestMiscIcons1"),
                        SpriteBounds = new Rectangle(136, 46, 32, 32),
                    };
                    break;

                case CollectibleType.pog:
                    collectible.CollectibleType = Components.CollectibleType.pog;
                    collectible.CollectibleValue = 0; //use random generator here, or more complex if you want
                    sprite = new Sprite();
                    /*sprite = new Sprite()
                    {
                        EntityID = eid,
                        SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/TestMiscIcons1"),
                        SpriteBounds = new Rectangle(136, 46, 32, 32),
                    };*/
                    break;

                default:
                    throw new Exception("Unknown CollectibleType");
            }

            collectible.EntityID = eid;
            position.EntityID = eid;

            collideable = new Collideable()
            {
                EntityID = eid,
                RoomID = position.RoomID,
                Bounds = new CircleBounds(position.Center, position.Radius)
            };
            _game.CollisionComponent[eid] = collideable;
            
            _game.CollectibleComponent.Add(eid, collectible);
            //_game.MovementComponent.Add(eid, movement); //Eventually do a sin wave y emulation to do the wave effect
            _game.PositionComponent.Add(eid, position);
            _game.SpriteComponent.Add(eid, sprite);
            return eid;
        }
    }
}
