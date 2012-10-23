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
            #region buttons
            //Make A button
            entityID = Entity.NextEntity();
            spriteSheet = game.Content.Load<Texture2D>("ControllerTGAs/xboxControllerButtonA");
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
            };
            game.HUDSpriteComponent[entityID] = AButtonSprite;

            //Make B button
            entityID = Entity.NextEntity();
            spriteSheet = game.Content.Load<Texture2D>("ControllerTGAs/xboxControllerButtonB");
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
            spriteSheet = game.Content.Load<Texture2D>("ControllerTGAs/xboxControllerButtonX");

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
            spriteSheet = game.Content.Load<Texture2D>("ControllerTGAs/xboxControllerButtonY");

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
            //Health/Psi Status Bar
            entityID = Entity.NextEntity();
            spriteSheet = game.Content.Load<Texture2D>("Spritesheets/HealthPsi");
            //Choose player corner
            switch (player.PlayerIndex)
            {
                case PlayerIndex.One:
                    corner = new Vector2(0, 0);
                    break;
                case PlayerIndex.Two:
                    corner = new Vector2(900, 0);
                    break;
                case PlayerIndex.Three:
                    corner = new Vector2(0, 600);
                    break;
                case PlayerIndex.Four:
                    corner = new Vector2(900, 600);
                    break;
                default:
                    corner = new Vector2(0,0);
                    break;
            }
            position = new Position()
            {
                EntityID = entityID,
                Center = corner,
                Radius = 40f,
            };
            game.PositionComponent[entityID] = position;
            HeatlhPsiStatusSprite = new HUDSprite()
            {
                EntityID = entityID,
                isSeen = true,
                SpriteSheet = spriteSheet,
                SpriteBounds = new Rectangle(0,0,256,64),
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
