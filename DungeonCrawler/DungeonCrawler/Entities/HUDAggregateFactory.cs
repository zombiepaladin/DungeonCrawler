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
            HUDSprite AButtonSprite,
                      BButtonSprite,
                      XButtonSprite,
                      YButtonSprite,
                      DPadSprite,
                      HeatlhStatusSprite,
                      ItemStatusSprite,
                      SkillStatusSprite;
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
            };
            game.HUDSpriteComponent[entityID] = DPadSprite;
            #endregion

            #region status
            //Health
            entityID = Entity.NextEntity();
            spriteSheet = game.Content.Load<Texture2D>("Spritesheets/StatusBar");
            position = new Position()
            {
                EntityID = entityID,
                Center = new Vector2(500, 695),
                Radius = 40f, //dont care really...
            };
            game.PositionComponent[entityID] = position;
            HeatlhStatusSprite = new HUDSprite()
            {
                EntityID = entityID,
                isSeen = true,
                SpriteSheet = spriteSheet,
                SpriteBounds = new Rectangle(5, 95, 346, 66),
            };
            game.HUDSpriteComponent[entityID] = HeatlhStatusSprite;
            //Item
            entityID = Entity.NextEntity();
            spriteSheet = game.Content.Load<Texture2D>("Spritesheets/StatusBar");
            position = new Position()
            {
                EntityID = entityID,
                Center = new Vector2(45, 695),
                Radius = 40f, //dont care really...
            };
            game.PositionComponent[entityID] = position;
            ItemStatusSprite = new HUDSprite()
            {
                EntityID = entityID,
                isSeen = true,
                SpriteSheet = spriteSheet,
                SpriteBounds = new Rectangle(3, 10, 346, 66),
            };
            game.HUDSpriteComponent[entityID] = ItemStatusSprite;
            //Skill
            entityID = Entity.NextEntity();
            spriteSheet = game.Content.Load<Texture2D>("Spritesheets/StatusBar");
            position = new Position()
            {
                EntityID = entityID,
                Center = new Vector2(960, 695),
                Radius = 40f, //dont care really...
            };
            game.PositionComponent[entityID] = position;
            SkillStatusSprite = new HUDSprite()
            {
                EntityID = entityID,
                isSeen = true,
                SpriteSheet = spriteSheet,
                SpriteBounds = new Rectangle(3, 180, 346, 66),
            };
            game.HUDSpriteComponent[entityID] = SkillStatusSprite;
            #endregion
            hud = new HUD()
            {
                EntityID = player.EntityID,
                AButtonSpriteID = AButtonSprite.EntityID,
                BButtonSpriteID = BButtonSprite.EntityID,
                XButtonSpriteID = XButtonSprite.EntityID,
                YButtonSpriteID = YButtonSprite.EntityID,
                DPadSpriteID = DPadSprite.EntityID,
                HealthStatusSpriteID = HeatlhStatusSprite.EntityID,
                ItemStatusSpriteID = ItemStatusSprite.EntityID,
                SkillStatusSpriteID = SkillStatusSprite.EntityID,
            };
            game.HUDComponent[player.EntityID] = hud;
        }
    }
}
