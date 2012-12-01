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
        private DungeonCrawlerGame game;

        private int getRank(uint eid, SkillType skill)
        {
            PlayerSkillInfo sInfo = game.PlayerSkillInfoComponent[eid];
            PlayerInfo pInfo = game.PlayerInfoComponent[eid];

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
            this.game = game;
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
            foreach(Player player in game.PlayerComponent.All) { players.Add(player); }
            foreach (Player player in players)
            {
                // Grab input for the player
                InputHelper state = InputHelper.GetInput(player.PlayerIndex);

                // Update the player's movement component
                Movement movement = game.MovementComponent[player.EntityID];
                movement.Direction = state.GetLeftDirection();

                SpriteAnimation spriteAnimation = game.SpriteAnimationComponent[player.EntityID];

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

                game.SpriteAnimationComponent[spriteAnimation.EntityID] = spriteAnimation;
                    
                game.MovementComponent[player.EntityID] = movement;

                PlayerInfo info = game.PlayerInfoComponent[player.EntityID];
                info.State = PlayerState.Default;

                if(state.IsPressed(Keys.Enter, Buttons.LeftTrigger))
                {
                    info.State = PlayerState.Attacking;
                }
               
                if (state.IsPressed(Keys.L, Buttons.RightStick)) game.QuestLogSystem.displayLog = !game.QuestLogSystem.displayLog;

                if (state.IsPressed(Keys.Space, Buttons.RightTrigger))
                {
                    uint thisPlayerKey = 0;
                    foreach(Player p in game.PlayerComponent.All)
                    {
                        if(p.PlayerIndex == PlayerIndex.One)
                            thisPlayerKey = p.EntityID;
                    }

                    uint eid = player.EntityID;
  
                    SkillType activeSkill = game.ActiveSkillComponent[eid].activeSkill;
                    game.SkillSystem.UseSkill(player.PlayerRace,activeSkill,getRank(eid,activeSkill),eid);
                    
                    //game.SkillSystem.UseSkill(player.PlayerRace, SkillType.Invisibility, 1, thisPlayerKey);

                    //game.SkillSystem.UseSkill(player.PlayerRace, SkillType.ThrusterRush, 1, thisPlayerKey);
                }


                game.PlayerInfoComponent[player.EntityID] = info;
                
                
                #region HUD Displays
                // Show HUD (A,B,X,Y, or Dpad Item)
               // HUD hud = game.HUDComponent[player.EntityID];
              //  HUDSprite hs;
                Inventory inv = game.InventoryComponent[player.EntityID];
                InventorySprite isb;
                InventorySprite iss;
                #region key/Button DOWN
                //Fields for selecting skills
                SkillType currentActiveSkill = game.ActiveSkillComponent[player.EntityID].activeSkill;
                SkillType desiredSkill;
                if (state.IsPressed(Keys.D1, Buttons.A))
                {
                    //hs = game.HUDSpriteComponent[hud.AButtonSpriteID];
                    //hs.isSeen = true;
                    //game.HUDSpriteComponent[hud.AButtonSpriteID] = hs;
                    
                    //Set Skill1 *****MAKE SURE TO ASSIGN YOU SKILLS IN THE AGGREGATE FACTORY*****
                    desiredSkill = game.PlayerInfoComponent[player.EntityID].skill1;

                    if(desiredSkill!=currentActiveSkill)
                    {
                        game.ActiveSkillComponent[player.EntityID] = new ActiveSkill(){activeSkill=desiredSkill,};
                    }
                    
                    
                    //temp activate shot skill
                    //Test Skill buttons
                    //game.SkillEntityFactory.CreateSkillProjectile(SkillType.BenignParasite, (Facing)game.SpriteAnimationComponent[player.EntityID].CurrentAnimationRow, game.PositionComponent[player.EntityID],1,300);

                }
                if (state.IsPressed(Keys.D2, Buttons.B))
                {
                   // hs = game.HUDSpriteComponent[hud.BButtonSpriteID];
                    //hs.isSeen = true;
                    //game.HUDSpriteComponent[hud.BButtonSpriteID] = hs;

                    //Set skill2 *****MAKE SURE TO ASSIGN YOU SKILLS IN THE AGGREGATE FACTORY*****
                    desiredSkill = game.PlayerInfoComponent[player.EntityID].skill2;
                    if(desiredSkill!=currentActiveSkill)
                    {
                        game.ActiveSkillComponent[player.EntityID] = new ActiveSkill(){activeSkill=desiredSkill,};
                    }

                }
                if (state.IsPressed(Keys.D3, Buttons.X))
                {
                   // hs = game.HUDSpriteComponent[hud.XButtonSpriteID];
                    //hs.isSeen = true;
                   // game.HUDSpriteComponent[hud.XButtonSpriteID] = hs;

                    //Set skill3 *****MAKE SURE TO ASSIGN YOU SKILLS IN THE AGGREGATE FACTORY*****
                    desiredSkill = game.PlayerInfoComponent[player.EntityID].skill3;
                    if(desiredSkill!=currentActiveSkill)
                    {
                        game.ActiveSkillComponent[player.EntityID] = new ActiveSkill(){activeSkill=desiredSkill,};
                    }
                }
                if (state.IsPressed(Keys.D4, Buttons.Y))
                {
                    //hs = game.HUDSpriteComponent[hud.YButtonSpriteID];
                    //hs.isSeen = true;
                    //game.HUDSpriteComponent[hud.YButtonSpriteID] = hs;
                    
                    //Set skill4 *****MAKE SURE TO ASSIGN YOU SKILLS IN THE AGGREGATE FACTORY*****
                    desiredSkill = game.PlayerInfoComponent[player.EntityID].skill4;
                    if(desiredSkill!=currentActiveSkill)
                    {
                        game.ActiveSkillComponent[player.EntityID] = new ActiveSkill(){activeSkill=desiredSkill,};
                    }
                }
                if (state.IsPressed(Keys.D5, Buttons.DPadUp))
                {
                    //hs = game.HUDSpriteComponent[hud.DPadSpriteID];
                    //hs.isSeen = true;
                    //game.HUDSpriteComponent[hud.DPadSpriteID] = hs;
                    
                    //Set Skill5 *****MAKE SURE TO ASSIGN YOU SKILLS IN THE AGGREGATE FACTORY*****
                    desiredSkill = game.PlayerInfoComponent[player.EntityID].skill5;
                    if(desiredSkill!=currentActiveSkill)
                    {
                        game.ActiveSkillComponent[player.EntityID] = new ActiveSkill(){activeSkill=desiredSkill,};
                    }
                }
                if (state.IsPressed(Keys.D6, Buttons.DPadDown))
                {
                    //hs = game.HUDSpriteComponent[hud.DPadSpriteID];
                    //hs.isSeen = true;
                    //game.HUDSpriteComponent[hud.DPadSpriteID] = hs;
                    
                    //Set Skill6 *****MAKE SURE TO ASSIGN YOU SKILLS IN THE AGGREGATE FACTORY*****
                    desiredSkill = game.PlayerInfoComponent[player.EntityID].skill6;
                    if(desiredSkill!=currentActiveSkill)
                    {
                        game.ActiveSkillComponent[player.EntityID] = new ActiveSkill(){activeSkill=desiredSkill,};
                    }
                }
                if (state.IsPressed(Keys.D7, Buttons.DPadLeft))
                {
                    //hs = game.HUDSpriteComponent[hud.DPadSpriteID];
                    //hs.isSeen = true;
                    //game.HUDSpriteComponent[hud.DPadSpriteID] = hs;
                    
                    //Set SKill7 *****MAKE SURE TO ASSIGN YOU SKILLS IN THE AGGREGATE FACTORY*****
                    desiredSkill = game.PlayerInfoComponent[player.EntityID].skill7;
                    if(desiredSkill!=currentActiveSkill)
                    {
                        game.ActiveSkillComponent[player.EntityID] = new ActiveSkill(){activeSkill=desiredSkill,};
                    }
                }
                if (state.IsPressed(Keys.D8, Buttons.DPadRight))
                {
                    //hs = game.HUDSpriteComponent[hud.DPadSpriteID];
                    //hs.isSeen = true;
                    //game.HUDSpriteComponent[hud.DPadSpriteID] = hs;
                    
                    //Set Skill8 *****MAKE SURE TO ASSIGN YOU SKILLS IN THE AGGREGATE FACTORY*****
                    desiredSkill = game.PlayerInfoComponent[player.EntityID].skill8;
                    if(desiredSkill!=currentActiveSkill)
                    {
                        game.ActiveSkillComponent[player.EntityID] = new ActiveSkill(){activeSkill=desiredSkill,};
                    }
                }
                if (state.IsPressed(Keys.D9, Buttons.DPadDown))
                {
                    //Set Skill9 *****MAKE SURE TO ASSIGN YOU SKILLS IN THE AGGREGATE FACTORY*****
                    desiredSkill = game.PlayerInfoComponent[player.EntityID].skill9;
                    if(desiredSkill!=currentActiveSkill)
                    {
                        game.ActiveSkillComponent[player.EntityID] = new ActiveSkill(){activeSkill=desiredSkill,};
                    }
                }
                if (state.IsPressed(Keys.Tab, Buttons.LeftShoulder))
                {
                    isb = game.InventorySpriteComponent[inv.BackgroundSpriteID];
                    isb.isSeen = true;
                    game.InventorySpriteComponent[inv.BackgroundSpriteID] = isb;
                    iss = game.InventorySpriteComponent[inv.SelectorSpriteID];
                    iss.isSeen = true;
                    game.InventorySpriteComponent[inv.SelectorSpriteID] = iss;
                }
                #endregion // end key down
                #region key/Button UP
                /*if (!state.IsPressed(Keys.D1, Buttons.A))
                {
                    hs = game.HUDSpriteComponent[hud.AButtonSpriteID];
                    hs.isSeen = false;
                    game.HUDSpriteComponent[hud.AButtonSpriteID] = hs;
                }
                if (!state.IsPressed(Keys.D2, Buttons.B))
                {
                    hs = game.HUDSpriteComponent[hud.BButtonSpriteID];
                    hs.isSeen = false;
                    game.HUDSpriteComponent[hud.BButtonSpriteID] = hs;
                }
                if (!state.IsPressed(Keys.D3, Buttons.X))
                {
                    hs = game.HUDSpriteComponent[hud.XButtonSpriteID];
                    hs.isSeen = false;
                    game.HUDSpriteComponent[hud.XButtonSpriteID] = hs;
                }
                if (!state.IsPressed(Keys.D4, Buttons.Y))
                {
                    hs = game.HUDSpriteComponent[hud.YButtonSpriteID];
                    hs.isSeen = false;
                    game.HUDSpriteComponent[hud.YButtonSpriteID] = hs;
                }
                //A temporary solution...
                if (!state.IsPressed(Keys.Right, Buttons.DPadRight)
                    && !state.IsPressed(Keys.Left, Buttons.DPadLeft)
                    && !state.IsPressed(Keys.Up, Buttons.DPadUp)
                    && !state.IsPressed(Keys.Down, Buttons.DPadDown))
                {
                    hs = game.HUDSpriteComponent[hud.DPadSpriteID];
                    hs.isSeen = false;
                    game.HUDSpriteComponent[hud.DPadSpriteID] = hs;
                }
                if (!state.IsPressed(Keys.Tab, Buttons.LeftShoulder))
                {
                    isb = game.InventorySpriteComponent[inv.BackgroundSpriteID];
                    isb.isSeen = false;
                    game.InventorySpriteComponent[inv.BackgroundSpriteID] = isb;
                    iss = game.InventorySpriteComponent[inv.SelectorSpriteID];
                    iss.isSeen = false;
                    game.InventorySpriteComponent[inv.SelectorSpriteID] = iss;
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
                }
                 **/
                #endregion //end key up
                #endregion //end hud control
                
            }
        }

        #endregion
    }
}
