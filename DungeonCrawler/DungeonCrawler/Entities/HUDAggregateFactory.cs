#region File Description
//-----------------------------------------------------------------------------
// HUDAggregateFactory.cs 
//
// Author: Nick Stanley
//
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence 
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DungeonCrawler.Components;
using System;
#endregion


namespace DungeonCrawler.Entities
{
    //A,B,X,Y = 80x80 pixels
    //Dpad = 186x186 pixels

    public class HUDAggregateFactory
    {
        DungeonCrawlerGame game;
        public HUDAggregateFactory(DungeonCrawlerGame game)
        {
            this.game = game;
        }

        public void CreateHUD(Player player)
        {
            uint entityID;
            Texture2D spriteSheet;
            Position position;
            Vector2 corner; //Which corner of the screen the Health/Psi status goes
            HUDSprite AButtonSprite,
                      BButtonSprite,
                      XButtonSprite,
                      YButtonSprite,
                      DPadSprite,
                      HeatlhPsiStatusSprite;
            HUD hud;
            Sprite avatar = new Sprite { EntityID = Entity.NextEntity(), SpriteBounds = new Rectangle(0, 0, 64, 64) };
            switch(player.PlayerRace)
            {
                case Aggregate.CultistPlayer:
                    avatar.SpriteSheet = game.Content.Load<Texture2D>("Spritesheets/Cultist");
                    break;
                case Aggregate.CyborgPlayer:
                    avatar.SpriteSheet = game.Content.Load<Texture2D>("Spritesheets/cyborg");
                    break;
                case Aggregate.EarthianPlayer:
                    avatar.SpriteSheet = game.Content.Load<Texture2D>("Spritesheets/Earthian2x");
                    break;
                case Aggregate.FairyPlayer:
                    avatar.SpriteSheet = game.Content.Load<Texture2D>("Spritesheets/wind_fae");
                    break;
                case Aggregate.GargranianPlayer:
                    avatar.SpriteSheet = game.Content.Load<Texture2D>("Spritesheets/gargranian");
                    break;
                case Aggregate.SpacePiratePlayer:
                    avatar.SpriteSheet = game.Content.Load<Texture2D>("Spritesheets/SpacePBig");
                    break;
                case Aggregate.ZombiePlayer:
                    avatar.SpriteSheet = game.Content.Load<Texture2D>("Spritesheets/MzombieBx2");
                    break;
                default:
                    throw new Exception("Unknown player race");
            }
            game.SpriteComponent.Add(avatar.EntityID, avatar);
            hud.AvatarSpriteID = avatar.EntityID;
            #region buttons
            //Make A button
            entityID = Entity.NextEntity();
            spriteSheet = game.Content.Load<Texture2D>("ControllerTGAs/xboxControllerButtonA-BW");
            position = new Position()
            {
                EntityID = entityID,
                Center = new Vector2(960,440),
                Radius = 40f,
            };
            game.PositionComponent[entityID] = position;
            AButtonSprite = new HUDSprite()
            {
                EntityID = entityID,
                isSeen = false,
                SpriteSheet = spriteSheet,
                SpriteBounds = new Rectangle(0, 0, 80, 80),
                PlayerIndex = player.PlayerIndex,
                isStatus = false,
            };
            game.HUDSpriteComponent[entityID] = AButtonSprite;

            //Make B button
            entityID = Entity.NextEntity();
            spriteSheet = game.Content.Load<Texture2D>("ControllerTGAs/xboxControllerButtonB-BW");
            position = new Position()
            {
                EntityID = entityID,
                Center = new Vector2(1040,360),
                Radius = 40f,
            };
            game.PositionComponent[entityID] = position;

            BButtonSprite = new HUDSprite()
            {
                EntityID = entityID,
                isSeen = false,
                SpriteSheet = spriteSheet,
                SpriteBounds = new Rectangle(0, 0, 80, 80),
                PlayerIndex = player.PlayerIndex,
                isStatus = false,
            };
            game.HUDSpriteComponent[entityID] = BButtonSprite;

            //Make X Button
            entityID = Entity.NextEntity();
            spriteSheet = game.Content.Load<Texture2D>("ControllerTGAs/xboxControllerButtonX-BW");

            position = new Position()
            {
                EntityID = entityID,
                Center = new Vector2(880,360),
                Radius = 40f,
            };
            game.PositionComponent[entityID] = position;

            XButtonSprite= new HUDSprite()
            {
                EntityID = entityID,
                isSeen = false,
                SpriteSheet = spriteSheet,
                SpriteBounds = new Rectangle(0, 0, 80, 80),
                PlayerIndex = player.PlayerIndex,
                isStatus = false,
            };
            game.HUDSpriteComponent[entityID] = XButtonSprite;

            //Make Y Button
            entityID = Entity.NextEntity();
            spriteSheet = game.Content.Load<Texture2D>("ControllerTGAs/xboxControllerButtonY-BW");

            position = new Position()
            {
                EntityID = entityID,
                Center = new Vector2(960,280),
                Radius = 40f,
            };
            game.PositionComponent[entityID] = position;

            YButtonSprite = new HUDSprite()
            {
                EntityID = entityID,
                isSeen = false,
                SpriteSheet = spriteSheet,
                SpriteBounds = new Rectangle(0, 0, 80, 80),
                PlayerIndex = player.PlayerIndex,
                isStatus = false,
            };
            game.HUDSpriteComponent[entityID] = YButtonSprite;

            //Make Dpad
            entityID = Entity.NextEntity();
            spriteSheet = game.Content.Load<Texture2D>("ControllerTGAs/xboxControllerDPad");

            position = new Position()
            {
                EntityID = entityID,
                Center = new Vector2(320,360),
                Radius = 93f,
            };
            game.PositionComponent[entityID] = position;

            DPadSprite = new HUDSprite()
            {
                EntityID = entityID,
                isSeen = false,
                SpriteSheet = spriteSheet,
                SpriteBounds = new Rectangle(0, 0, 186, 186),
                PlayerIndex = player.PlayerIndex,
                isStatus = false,
            };
            game.HUDSpriteComponent[entityID] = DPadSprite;
            #endregion
            //Use a Health/Psi or Health/Fatigue Status Bar depending on the player's race
            entityID = Entity.NextEntity();
            
            //Choose player corner
            switch (player.PlayerIndex)
            {
                case PlayerIndex.One:
                    corner = new Vector2(0, 0);
                    spriteSheet = game.Content.Load<Texture2D>("Spritesheets/hud_one");
                    break;
                case PlayerIndex.Two:
                    corner = new Vector2(1024, 0);
                    spriteSheet = game.Content.Load<Texture2D>("Spritesheets/hud_two");
                    break;
                case PlayerIndex.Three:
                    corner = new Vector2(0, 656);
                    spriteSheet = game.Content.Load<Texture2D>("Spritesheets/hud_three");
                    break;
                case PlayerIndex.Four:
                    corner = new Vector2(1024, 656);
                    spriteSheet = game.Content.Load<Texture2D>("Spritesheets/hud_four");
                    break;
                default:
                    throw new Exception("Too many players");
            }
            position = new Position()
            {
                EntityID = entityID,
                Center = corner,
                Radius = 0f,//its a rectangle, so no radius for proper drawing
            };
            game.PositionComponent[entityID] = position;
            HeatlhPsiStatusSprite = new HUDSprite()
            {
                EntityID = entityID,
                isSeen = true,
                SpriteSheet = spriteSheet,
                SpriteBounds = new Rectangle(0,0,270,143),
                PlayerIndex = player.PlayerIndex,
                isStatus = true,
            };
            game.HUDSpriteComponent[entityID] = HeatlhPsiStatusSprite;
            //Set hud
            hud = new HUD()
            {
                EntityID = player.EntityID,
                AButtonSpriteID = AButtonSprite.EntityID,
                BButtonSpriteID = BButtonSprite.EntityID,
                XButtonSpriteID = XButtonSprite.EntityID,
                YButtonSpriteID = YButtonSprite.EntityID,
                DPadSpriteID = DPadSprite.EntityID,
                HealthPsiStatusSpriteID = HeatlhPsiStatusSprite.EntityID,
            };
            game.HUDComponent[player.EntityID] = hud;
        }
    }
}
