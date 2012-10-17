#region File Description
//-----------------------------------------------------------------------------
// MovementSystem.cs 
//
// Author: Nathan Bean
//
// Modified: Nick Stanley added Hud Controls, 10/15/2012
//
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence 
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using DungeonCrawler.Components;
#endregion

namespace DungeonCrawler.Systems
{
    /// <summary>
    /// This class embodies a system for updating player-controlled
    /// entities within the game world
    /// </summary>
    public class InputSystem
    {
        #region Private Members

        /// <summary>
        /// The game this system belongs to
        /// </summary>
        private DungeonCrawlerGame game;

        private KeyboardState oldKeyboardState;

        private GamePadState[] oldGamePadState;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new MovementSystem
        /// </summary>
        /// <param name="game">The game this system belongs to</param>
        public InputSystem(DungeonCrawlerGame game)
        {
            this.game = game;
            oldGamePadState = new GamePadState[4];
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates all moving entities in the game
        /// </summary>
        /// <param name="elapsedTime">
        /// The time between this and the previous frame.
        /// </param>
        public void Update(float elapsedTime)
        {
            
            // Update all entities that have a movement component
            foreach (Player player in game.PlayerComponent.All)
            {
                // Grab input for the player
                KeyboardState keyboardState = Keyboard.GetState();
                GamePadState gamePadState = GamePad.GetState(player.PlayerIndex);

                // Update the player's movement component
                Movement movement = game.MovementComponent[player.EntityID];
                movement.Direction = gamePadState.ThumbSticks.Left;
                if (keyboardState.IsKeyDown(Keys.W))
                    movement.Direction.Y = -1;
                if (keyboardState.IsKeyDown(Keys.A))
                    movement.Direction.X = -1;
                if (keyboardState.IsKeyDown(Keys.S))
                    movement.Direction.Y = 1;
                if (keyboardState.IsKeyDown(Keys.D))
                    movement.Direction.X = 1;
                if(movement.Direction != Vector2.Zero)
                    movement.Direction.Normalize();
                game.MovementComponent[player.EntityID] = movement;

                #region HUD Displays
                // Show HUD (A,B,X,Y, or Dpad Item)
                HUD hud = game.HUDComponent[player.EntityID];
                HUDSprite hs;
                #region key/Button DOWN
                if (gamePadState.IsButtonDown(Buttons.A) || keyboardState.IsKeyDown(Keys.D1))
                {
                    hs = game.HUDSpriteComponent[hud.AButtonSpriteID];
                    hs.isSeen = true;
                    game.HUDSpriteComponent[hud.AButtonSpriteID] = hs;
                    //TODO: Set skill
                    //      Show skill being set
                }
                if (gamePadState.IsButtonDown(Buttons.B) || keyboardState.IsKeyDown(Keys.D2))
                {
                    hs = game.HUDSpriteComponent[hud.BButtonSpriteID];
                    hs.isSeen = true;
                    game.HUDSpriteComponent[hud.BButtonSpriteID] = hs;
                    //TODO: Set skill
                }
                if (gamePadState.IsButtonDown(Buttons.X) || keyboardState.IsKeyDown(Keys.D3))
                {
                    hs = game.HUDSpriteComponent[hud.XButtonSpriteID];
                    hs.isSeen = true;
                    game.HUDSpriteComponent[hud.XButtonSpriteID] = hs;
                    //TODO: Set skill
                }
                if (gamePadState.IsButtonDown(Buttons.Y) || keyboardState.IsKeyDown(Keys.D4))
                {
                    hs = game.HUDSpriteComponent[hud.YButtonSpriteID];
                    hs.isSeen = true;
                    game.HUDSpriteComponent[hud.YButtonSpriteID] = hs;
                    //TODO: Set skill
                }
                if (gamePadState.IsButtonDown(Buttons.DPadUp) || keyboardState.IsKeyDown(Keys.Up))
                {
                    hs = game.HUDSpriteComponent[hud.DPadSpriteID];
                    hs.isSeen = true;
                    game.HUDSpriteComponent[hud.DPadSpriteID] = hs;
                    //TODO: Set item
                }
                if (gamePadState.IsButtonDown(Buttons.DPadDown) || keyboardState.IsKeyDown(Keys.Down))
                {
                    hs = game.HUDSpriteComponent[hud.DPadSpriteID];
                    hs.isSeen = true;
                    game.HUDSpriteComponent[hud.DPadSpriteID] = hs;
                    //TODO: Set item
                }
                if (gamePadState.IsButtonDown(Buttons.DPadLeft) || keyboardState.IsKeyDown(Keys.Left))
                {
                    hs = game.HUDSpriteComponent[hud.DPadSpriteID];
                    hs.isSeen = true;
                    game.HUDSpriteComponent[hud.DPadSpriteID] = hs;
                    //TODO: Set item
                }
                if (gamePadState.IsButtonDown(Buttons.DPadRight) || keyboardState.IsKeyDown(Keys.Right))
                {
                    hs = game.HUDSpriteComponent[hud.DPadSpriteID];
                    hs.isSeen = true;
                    game.HUDSpriteComponent[hud.DPadSpriteID] = hs;
                    //TODO: Set item
                }

                #endregion // end key down
                #region key/Button UP
                if (gamePadState.IsButtonUp(Buttons.A) && keyboardState.IsKeyUp(Keys.D1))
                {
                    hs = game.HUDSpriteComponent[hud.AButtonSpriteID];
                    hs.isSeen = false;
                    game.HUDSpriteComponent[hud.AButtonSpriteID] = hs;
                }
                if (gamePadState.IsButtonUp(Buttons.B) && keyboardState.IsKeyUp(Keys.D2))
                {
                    hs = game.HUDSpriteComponent[hud.BButtonSpriteID];
                    hs.isSeen = false;
                    game.HUDSpriteComponent[hud.BButtonSpriteID] = hs;
                }
                if (gamePadState.IsButtonUp(Buttons.X) && keyboardState.IsKeyUp(Keys.D3))
                {
                    hs = game.HUDSpriteComponent[hud.XButtonSpriteID];
                    hs.isSeen = false;
                    game.HUDSpriteComponent[hud.XButtonSpriteID] = hs;
                }
                if (gamePadState.IsButtonUp(Buttons.Y) && keyboardState.IsKeyUp(Keys.D4))
                {
                    hs = game.HUDSpriteComponent[hud.YButtonSpriteID];
                    hs.isSeen = false;
                    game.HUDSpriteComponent[hud.YButtonSpriteID] = hs;
                }
                //A temporary solution...
                if (gamePadState.IsButtonUp(Buttons.DPadRight) && keyboardState.IsKeyUp(Keys.Right)
                    && gamePadState.IsButtonUp(Buttons.DPadLeft) && keyboardState.IsKeyUp(Keys.Left)
                    && gamePadState.IsButtonUp(Buttons.DPadUp) && keyboardState.IsKeyUp(Keys.Up)
                    && gamePadState.IsButtonUp(Buttons.DPadDown) && keyboardState.IsKeyUp(Keys.Down))
                {
                    hs = game.HUDSpriteComponent[hud.DPadSpriteID];
                    hs.isSeen = false;
                    game.HUDSpriteComponent[hud.DPadSpriteID] = hs;
                } 
                /*
                if (gamePadState.IsButtonUp(Buttons.DPadLeft) && keyboardState.IsKeyUp(Keys.Left))
                {
                    hs = game.HUDSpriteComponent[hud.DPadSpriteID];
                    hs.isSeen = false;
                    game.HUDSpriteComponent[hud.DPadSpriteID] = hs;
                }
                if (gamePadState.IsButtonUp(Buttons.DPadUp) && keyboardState.IsKeyUp(Keys.Up))
                {
                    hs = game.HUDSpriteComponent[hud.DPadSpriteID];
                    hs.isSeen = false;
                    game.HUDSpriteComponent[hud.DPadSpriteID] = hs;
                }
                if (gamePadState.IsButtonUp(Buttons.DPadDown) || keyboardState.IsKeyUp(Keys.Down))
                {
                    hs = game.HUDSpriteComponent[hud.DPadSpriteID];
                    hs.isSeen = false;
                    game.HUDSpriteComponent[hud.DPadSpriteID] = hs;
                }*/
                #endregion //end key up
                #endregion //end hud control
                // Cache last frame's input state
                oldKeyboardState = keyboardState;
                oldGamePadState[(int)player.PlayerIndex] = gamePadState;
            }
        }

        #endregion
    }
}
