#region File Description
//-----------------------------------------------------------------------------
// MovementSystem.cs 
//
// Author: Nathan Bean
//
// Modified: Nick Stanley added Hud Controls, 10/15/2012
// Modified: Daniel Rymph added Inventory Controls, 10/17/2012
// Modified: Devin Kelly-Collins added Attack buttons in update method, 10/24/2012
// Modified by Samuel Fike and Jiri Malina: Added support for SpriteAnimationComponent
// Modified: Nick Boen - Added a test control for using a skill (buffs speed)
// Modified: Devin Kelly-Collins - Implemented UserInput (11/26/12)
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
        private DungeonCrawlerGame _game;

        private int getRank(uint eid, SkillType skill)
        {
            PlayerSkillInfo sInfo = _game.PlayerSkillInfoComponent[eid];
            PlayerInfo pInfo = _game.PlayerInfoComponent[eid];

            if (pInfo.skill1 == skill)
            {
                return sInfo.Skill1Rank;
            }

            if (pInfo.skill2 == skill)
            {
                return sInfo.Skill2Rank;
            }

            if (pInfo.skill3 == skill)
            {
                return sInfo.Skill3Rank;
            }

            if (pInfo.skill4 == skill)
            {
                return sInfo.Skill4Rank;
            }

            if (pInfo.skill5 == skill)
            {
                return sInfo.Skill5Rank;
            }

            if (pInfo.skill6 == skill)
            {
                return sInfo.Skill6Rank;
            }

            if (pInfo.skill7 == skill)
            {
                return sInfo.Skill7Rank;
            }

            if (pInfo.skill8 == skill)
            {
                return sInfo.Skill8Rank;
            }

            return sInfo.Skill9Rank;
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new MovementSystem
        /// </summary>
        /// <param name="game">The game this system belongs to</param>
        public InputSystem(DungeonCrawlerGame game)
        {
            this._game = game;
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
            List<Player> players = new List<Player>();
            foreach(Player player in _game.PlayerComponent.All) { players.Add(player); }
            foreach (Player player in players)
            {
                // Grab input for the player
                InputHelper state = InputHelper.GetInput(player.PlayerIndex);

                // Update the player's movement component
                Movement movement = _game.MovementComponent[player.EntityID];
                movement.Direction = state.GetLeftDirection();

                SpriteAnimation spriteAnimation = _game.SpriteAnimationComponent[player.EntityID];

                if (movement.Direction.Y < 0)
                {
                    spriteAnimation.CurrentAnimationRow = (int) AnimationMovementDirection.Up;
                }
                if (movement.Direction.Y > 0)
                {
                    spriteAnimation.CurrentAnimationRow = (int)AnimationMovementDirection.Down;
                }
                if (movement.Direction.X < 0)
                {
                    spriteAnimation.CurrentAnimationRow = (int)AnimationMovementDirection.Left;
                }
                if (movement.Direction.X > 0)
                {
                    spriteAnimation.CurrentAnimationRow = (int)AnimationMovementDirection.Right;
                }
                
                if (movement.Direction != Vector2.Zero)
                {
                    movement.Direction.Normalize();
                    spriteAnimation.IsPlaying = true;
                }
                else
                {
                    spriteAnimation.IsPlaying = false;
                    //spriteAnimation.CurrentFrame = 1;
                    //TODO: idle frame
                }

                _game.SpriteAnimationComponent[spriteAnimation.EntityID] = spriteAnimation;
                    
                _game.MovementComponent[player.EntityID] = movement;

               PlayerInfo info = _game.PlayerInfoComponent[player.EntityID];
              

               if (state.IsPressed(Keys.Enter, Buttons.LeftTrigger))
               {
                   info.State = PlayerState.Attacking;
               }
               else
               {
                    info.State = PlayerState.Default;
               }

                _game.PlayerInfoComponent[player.EntityID] = info;

                if (state.IsPressed(Keys.L, Buttons.RightStick) && !state.IsHeld(Keys.L, Buttons.RightStick)) _game.QuestLogSystem.displayLog = !_game.QuestLogSystem.displayLog;

                //set up a system to switch skills by using the 1-9 keys
                if (state.IsPressed(Keys.D1, Buttons.A))
                    ChangeActiveSkill(_game.PlayerInfoComponent[player.EntityID].skill1, player.EntityID);
                else if (state.IsPressed(Keys.D2, Buttons.B))
                    ChangeActiveSkill(_game.PlayerInfoComponent[player.EntityID].skill2, player.EntityID);
                else if (state.IsPressed(Keys.D3, Buttons.X))
                    ChangeActiveSkill(_game.PlayerInfoComponent[player.EntityID].skill3, player.EntityID);
                else if (state.IsPressed(Keys.D4, Buttons.Y))
                    ChangeActiveSkill(_game.PlayerInfoComponent[player.EntityID].skill4, player.EntityID);
                else if (state.IsPressed(Keys.D5, Buttons.DPadUp))
                    ChangeActiveSkill(_game.PlayerInfoComponent[player.EntityID].skill5, player.EntityID);
                else if (state.IsPressed(Keys.D6, Buttons.DPadDown))
                    ChangeActiveSkill(_game.PlayerInfoComponent[player.EntityID].skill6, player.EntityID);
                else if (state.IsPressed(Keys.D7, Buttons.DPadLeft))
                    ChangeActiveSkill(_game.PlayerInfoComponent[player.EntityID].skill7, player.EntityID);
                else if (state.IsPressed(Keys.D8, Buttons.DPadRight))
                    ChangeActiveSkill(_game.PlayerInfoComponent[player.EntityID].skill8, player.EntityID);
                else if (state.IsPressed(Keys.D9))
                    ChangeActiveSkill(_game.PlayerInfoComponent[player.EntityID].skill9, player.EntityID);


                if (state.IsPressed(Keys.Space, Buttons.RightTrigger))
                {
                    uint thisPlayerKey = 0;
                    foreach(Player p in _game.PlayerComponent.All)
                    {
                        if(p.PlayerIndex == PlayerIndex.One)
                            thisPlayerKey = p.EntityID;
                    }
  
                    SkillType activeSkill = _game.ActiveSkillComponent[player.EntityID].activeSkill;
                    _game.SkillSystem.UseSkill(player.PlayerRace,activeSkill,getRank(player.EntityID,activeSkill),player.EntityID);
                    
                    //game.SkillSystem.UseSkill(player.PlayerRace, SkillType.Invisibility, 1, thisPlayerKey);

                    //game.SkillSystem.UseSkill(player.PlayerRace, SkillType.ThrusterRush, 1, thisPlayerKey);
                }


               
                
                
                #region HUD Displays
                // Show HUD (A,B,X,Y, or Dpad Item)
               // HUD hud = game.HUDComponent[player.EntityID];
              //  HUDSprite hs;
                Inventory inv = _game.InventoryComponent[player.EntityID];
                InventorySprite isb;
                InventorySprite iss;
              
                #endregion //end hud control
                
            }
        }

        private void ChangeActiveSkill(SkillType activatedSkill, uint playerId)
        {
            ActiveSkill skill = _game.ActiveSkillComponent[playerId];
            skill.activeSkill = activatedSkill;
            _game.ActiveSkillComponent[playerId] = skill;
        }

        #endregion
    }
}
