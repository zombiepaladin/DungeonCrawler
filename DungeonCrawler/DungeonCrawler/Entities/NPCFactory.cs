#region File Description
//-----------------------------------------------------------------------------
// NPCFactory.cs 
//
// Author: Michael Fountain
//
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
    /*public enum NPCFactoryType
    {
        Quest,
        Shopkeeper,
        Information,
        Background
    }*/

    /// <summary>
    /// Handles creating npcs and adding them to the game.
    /// </summary>
    public class NPCFactory
    {
        /// <summary>
        /// Parent game.
        /// </summary>
        private DungeonCrawlerGame _game;

        /// <summary>
        /// Creates a new Factory.
        /// </summary>
        /// <param name="game"></param>
        public NPCFactory(DungeonCrawlerGame game)
        {
            _game = game;
        }

        /// <summary>
        /// Creates a new npc and adds it to the game. (No other components created)
        /// </summary>
        /// <param name="type">The type of npc to create.</param>
        public uint CreateNPC(NPCType type, Position position)
        {
            uint eid = Entity.NextEntity();
            NPC npc;
            Sprite sprite;
            Collideable collideable;

            switch (type)
            {
                case NPCType.Trollph:
                     sprite = new Sprite()
                        {
                            EntityID = eid,
                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/trollph"),
                            SpriteBounds = new Rectangle(0, 0, 64, 64),
                        };                       
                        break;

                    default:
                        throw new Exception("Unknown NPCType");
                }
            
                npc.EntityID = eid;
                npc.Type = type;
                position.EntityID = eid;

                collideable = new Collideable()
                {
                    EntityID = eid,
                    Bounds = new CircleBounds(position.Center, position.Radius)
                };

                Movement move = new Movement()
                {
                    EntityID = eid,
                };

                _game.MovementComponent.Add(eid, move);
                _game.CollisionComponent[eid] = collideable;
                _game.NPCComponent.Add(eid, npc);    
                _game.PositionComponent.Add(eid, position);
                _game.SpriteComponent.Add(eid, sprite);
                return eid;
        }
    }
}
