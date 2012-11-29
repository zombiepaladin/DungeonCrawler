﻿#region File Description
//-----------------------------------------------------------------------------
// RenderingSystem.cs 
//
// Author: Nathan Bean
//
// Modified: Devin Kelly-Collins - Added debugTexture and logic to draw collisions in Draw. Added logic to draw actorText in draw. (11/15/12)
// Modified: Devin Kelly-Collins - Replaced HUD component with HUDSystem. (11/29/12)
//
// Modified: Nick Stanley added HUDSpriteComponent, 10/15/2012
// Modified: Devin Kelly-Collins added WeaponSprite rendering, 10/24/2012
// Modified: Samuel Fike and Jiri Malina: Fixed errors due to removal of movementSprite for players
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence 
//-----------------------------------------------------------------------------

#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DungeonCrawler.Components;
#endregion

namespace DungeonCrawler.Systems
{
    /// <summary>
    /// This class embodies the rendering system for our dungeon crawler.
    /// Once per frame it renders everything visible in the game world.
    /// </summary>
    public class RenderingSystem
    {
        #region Private Members
        
        /// <summary>
        /// The game this system belongs to
        /// </summary>
        private DungeonCrawlerGame game;

        /// <summary>
        /// A SpriteBatch for rendering sprites
        /// </summary>
        private SpriteBatch spriteBatch;

        private SpriteFont _actorTextFont;

#if DEBUG
        private Texture2D _debugTexture;
#endif

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new RenderingSystem
        /// </summary>
        /// <param name="game">The game this system belongs to</param>
        public RenderingSystem(DungeonCrawlerGame game)
        {
            this.game = game;
            this.spriteBatch = new SpriteBatch(game.GraphicsDevice);
            this._actorTextFont = game.Content.Load<SpriteFont>("SpriteFonts/Pescadero");

#if DEBUG
            _debugTexture = new Texture2D(game.GraphicsDevice, 1, 1);

	        Color[] data = new Color[1];
	        //for (int i = 0; i < data.Length; ++i) { data[i] = Color.Red; data[i].A /= 2; }
	        //_debugTexture.SetData(data);
#endif
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Renders the game world
        /// </summary>
        /// <param name="elapsedTime">
        /// The time between this and the previous frame.
        /// </param>
        public void Draw(float elapsedTime)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, null, SamplerState.PointClamp, null, null, null);
            uint roomId;
            try
            {
                roomId = DungeonCrawlerGame.LevelManager.getCurrentRoom().EntityID;
            }
            catch
            {
                roomId = uint.MaxValue; //should change this no room should not have an id
            }

            // Draw all Sprites
            List<Sprite> sprites = new List<Sprite>();
            foreach(Sprite sprite in game.SpriteComponent.All) { sprites.Add(sprite); }
            foreach (Sprite sprite in sprites)
            {
            //foreach (Sprite sprite in game.SpriteComponent.All)
            //{
                Position position = game.PositionComponent[sprite.EntityID];
                if (position.RoomID == roomId)
                {

                    spriteBatch.Draw(sprite.SpriteSheet,
                                    position.Center,
                                    sprite.SpriteBounds,
                                    Color.White,
                                    0f,                                             // rotation
                                    new Vector2(position.Radius, position.Radius),  // origin
                                    1f,                                             // scale
                                    SpriteEffects.None,
                                    1);
                }
            }

            // Draw all MovementSprites
            foreach (MovementSprite sprite in game.MovementSpriteComponent.All)
            {
                Position position = game.PositionComponent[sprite.EntityID];
                if (position.RoomID == roomId)
                {
                    spriteBatch.Draw(sprite.SpriteSheet,
                                    position.Center,
                                    sprite.SpriteBounds,
                                    Color.White,
                                    0f,                                             // rotation
                                    new Vector2(position.Radius, position.Radius),  // origin
                                    1f,                                             // scale
                                    SpriteEffects.None,
                                    .8f);
                }
            }

            //Draw actor text
            foreach (ActorText actorText in game.ActorTextComponent.All)
            {
                Position position = game.PositionComponent[actorText.EntityID];
                if(position.RoomID == roomId)
                {
                    position.Center.Y -= actorText.Offset;
                    spriteBatch.DrawString(_actorTextFont, actorText.Text, position.Center, Color.White);
                }
            }

            //Draw Weapon animations
            foreach (WeaponSprite sprite in game.WeaponSpriteComponent.All)
            {
                Position position = game.PositionComponent[sprite.EntityID];
                if (position.RoomID == roomId)
                {
                    Facing facing = (Facing)game.SpriteAnimationComponent[sprite.EntityID].CurrentAnimationRow; //changed to get direction from spriteanimation instead of movementsprite, currentAnimationRow returns same values as facing for directions

                    position.Center = applyFacingOffset(facing, position.Center);
                    spriteBatch.Draw(sprite.SpriteSheet,
                                    position.Center,
                                    sprite.SpriteBounds,
                                    Color.White,
                                    0f,
                                    new Vector2(position.Radius),
                                    1f,
                                    SpriteEffects.None,
                                    (facing == Facing.North) ? .9f : .7f);
                }
            }

            //Draw HUD - Moving this to the new HUDSystem.
            /*foreach (HUDSprite sprite in game.HUDSpriteComponent.All)
            {
                Color playerColor;
                PlayerIndex playerDex = sprite.PlayerIndex;
                switch (playerDex)
                {
                    case PlayerIndex.One:
                        playerColor = Color.Red;
                        break;
                    case PlayerIndex.Two:
                        playerColor = Color.Blue;
                        break;
                    case PlayerIndex.Three:
                        playerColor = Color.Green;
                        break;
                    case PlayerIndex.Four:
                        playerColor = Color.Magenta;
                        break;
                    default:
                        playerColor = Color.White;
                        break;
                }
                
                if (sprite.isSeen) //A,B,X,Y, and Dpad are temp on screen
                {
                    
                    Position position = game.PositionComponent[sprite.EntityID];
                    spriteBatch.Draw(sprite.SpriteSheet,
                                    position.Center,
                                    sprite.SpriteBounds,
                                    playerColor,
                                    0f,                                             // rotation
                                    new Vector2(position.Radius,position.Radius),  // origin
                                    1f,                                             // scale
                                    SpriteEffects.None,
                                    0.6f);
                }
            }*/

            game.HUDSystem.Draw(elapsedTime, spriteBatch);

            foreach (InventorySprite sprite in game.InventorySpriteComponent.All)
            {
                if (sprite.isSeen)
                {
                    Position position = game.PositionComponent[sprite.EntityID];
                    spriteBatch.Draw(sprite.SpriteSheet,
                                    position.Center,
                                    sprite.SpriteBounds,
                                    Color.White,
                                    0f,                                             // rotation
                                    new Vector2(position.Radius, position.Radius),  // origin
                                    1f,                                             // scale
                                    SpriteEffects.None,
                                    0.5f);
                }
            }

#if DEBUG
            //If we are in debug, draw the collision bounds.
            uint roomID = game.CurrentRoomEid;

            foreach (Collideable collision in game.CollisionComponent.InRoom(roomID))
            {
                if (collision.Bounds is RectangleBounds)
                    spriteBatch.Draw(_debugTexture, ((RectangleBounds)collision.Bounds).Rectangle, Color.Red);
                else
                {
                    //Might want to change this to a circle.
                    CircleBounds bounds = (CircleBounds)collision.Bounds;
                    spriteBatch.Draw(_debugTexture, new Rectangle((int)(bounds.Center.X - bounds.Radius), (int)(bounds.Center.Y - bounds.Radius),
                        (int)(bounds.Radius * 2), (int)(bounds.Radius * 2)), Color.Red);
                }
            }
#endif

            spriteBatch.End();
        }
        
        #endregion

        private Vector2 applyFacingOffset(Facing facing, Vector2 center)
        {
            int offset = 32;

            switch (facing)
            {
                case Facing.North:
                    center.Y -= offset;
                    break;
                case Facing.East:
                    center.X += offset;
                    break;
                case Facing.South:
                    center.Y += offset;
                    break;
                case Facing.West:
                    center.X -= offset;
                    break;
            }

            return center;
        }
    }
}
