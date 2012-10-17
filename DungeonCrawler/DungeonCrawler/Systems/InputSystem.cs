#region File Description
//-----------------------------------------------------------------------------
// MovementSystem.cs 
//
// Author: Nathan Bean
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

                // Cache last frame's input state
                oldKeyboardState = keyboardState;
                oldGamePadState[(int)player.PlayerIndex] = gamePadState;
            }
        }

        #endregion
    }
}
