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
        /// <param name="name">The type of npc to create.</param>
        public uint CreateNPC(NPCName name, Position position)
        {
            uint eid = Entity.NextEntity();
            NPC npc;
            Sprite sprite;
            Collideable collideable;
            Texture2D spriteSheet;
            Movement movement;
            SpriteAnimation spriteAnimation;
            NpcAI ai;
            
            switch (name)
            {
                case NPCName.Trollph:
                     sprite = new Sprite()
                        {
                            EntityID = eid,
                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/trollph"),
                            SpriteBounds = new Rectangle(0, 0, 64, 64),
                        };

                        ai = new NpcAI()
                        {
                            EntityID = eid,
                            Type = NPCType.Background,
                        };
                        _game.NpcAIComponent.Add(eid, ai);
                                               
                        break;
                case NPCName.DarkRalph:

                        sprite = new Sprite()
                        {
                            EntityID = eid,
                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/darkralph"),
                            SpriteBounds = new Rectangle(0, 0, 64, 64),
                        };
                        

                         ai = new NpcAI()
                         {
                            EntityID = eid,
                            Type = NPCType.Background,
                         };
                        _game.NpcAIComponent.Add(eid, ai);

                        break;

                    default:
                        throw new Exception("Unknown NPC");
                }
            
                npc.EntityID = eid;
                npc.Type = name;
                position.EntityID = eid;

                spriteAnimation = new SpriteAnimation()
                {
                    EntityID = eid,
                    FramesPerSecond = 10,
                    IsLooping = true,
                    IsPlaying = false,
                    TimePassed = 0f,
                    CurrentFrame = 0,
                    CurrentAnimationRow = 0

                };
                _game.SpriteAnimationComponent[eid] = spriteAnimation;

                movement = new Movement()
                {
                    EntityID = eid,
                };

                _game.MovementComponent[eid] = movement;

                collideable = new Collideable()
                {
                    EntityID = eid,
                    RoomID = position.RoomID,
                    Bounds = new CircleBounds(position.Center, position.Radius)
                };
                _game.CollisionComponent[eid] = collideable;

                _game.NPCComponent.Add(eid, npc);    
                _game.PositionComponent.Add(eid, position);
                _game.SpriteComponent.Add(eid, sprite);
                return eid;
        }
    }
}
