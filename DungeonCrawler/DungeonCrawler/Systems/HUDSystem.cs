#region File Description
//-----------------------------------------------------------------------------
// HUDSystem.cs 
//
// Author: Devin Kelly-Collins
//
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence 
//-----------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using DungeonCrawler.Components;

namespace DungeonCrawler.Systems
{
    /// <summary>
    /// Handles drawing the hud.
    /// </summary>
    public class HUDSystem
    {
        //Contains all the information needed to draw the main HUD
        private struct HUDSprite
        {
            public bool Show;

            public Texture2D HudBgSpriteSheet;
            public Rectangle HudBgSpriteBounds;
            public Vector2 HudBgPosition;

            public Texture2D AvatarSpriteSheet;
            public Rectangle AvatarSpriteBounds;
            public Vector2 AvatarPosition;

            public Texture2D SkillSpriteSheet;
            public Rectangle SkillSpriteBounds;
            public Vector2 SkillPosition;

            public Texture2D CollectSpriteSheet;
            public Rectangle CollectSpriteBounds;
            public string CollectAmount;
            public Vector2 CollectPosition;

            public Texture2D ItemSpriteSheet;
            public Rectangle ItemSpriteBounds;
            public string ItemAmount;
            public Vector2 ItemPosition;

            public Texture2D HealthSpriteSheet;
            public Rectangle HealthBounds;
            public Vector2 HealthPosition;

            public Texture2D PsiSpriteSheet;
            public Rectangle PsiBounds;
            public Vector2 PsiPosition;

            public Texture2D ExpSpriteSheet;
            public Rectangle ExpBounds;
            public Vector2 ExpPosition;

            public string Name;
            public Vector2 NamePosition;
            public string Level;
            public Vector2 LevelPosition;
        }

        //Constants for the status bars.
        private const int HEALTH_BAR_MAX_BOUNDS = 157;
        private const int PSI_BAR_MAX_BOUNDS = 157;
        private const int EXP_BAR_MAX_BOUNDS = 157;
        private const string LEVEL_PREFIX = "Level: ";

        //Refernces to the game.
        private DungeonCrawlerGame _game;
        private ContentManager _content;
        private PlayerComponent _playerComponent;

        private HUDSprite _p1;
        private HUDSprite _p2;
        private HUDSprite _p3;
        private HUDSprite _p4;

        private SpriteFont _font;

        /// <summary>
        /// Creates a new HUDSystem.
        /// </summary>
        /// <param name="game">Parent game.</param>
        public HUDSystem(DungeonCrawlerGame game)
        {
            _game = game;
            _content = game.Content;
            _playerComponent = game.PlayerComponent;

            _p1 = new HUDSprite { Show = false };
            _p2 = new HUDSprite { Show = false };
            _p3 = new HUDSprite { Show = false };
            _p4 = new HUDSprite { Show = false };
        }

        /// <summary>
        /// Loads content needed for the HUD and creates a HUD for any players in the game.
        /// </summary>
        public void LoadContent()
        {
            _font = _content.Load<SpriteFont>("SpriteFonts/Pescadero");

            //Now load for each player. Yay
            foreach (Player player in _playerComponent.All)
            {
                LoadPlayerHUD(player);
            }
        }

        /// <summary>
        /// Creates a HUD for the specific player.
        /// </summary>
        /// <param name="player">Player to create hud for.</param>
        public void LoadPlayerHUD(Player player)
        {
            HUDSprite hud = new HUDSprite { Show = true };

            //Load avatar
            switch (player.PlayerRace)
            {
                case Entities.Aggregate.CultistPlayer:
                    hud.AvatarSpriteSheet = _content.Load<Texture2D>("Spritesheets/Aggregate/Cultist");
                    break;
                case Entities.Aggregate.CyborgPlayer:
                    hud.AvatarSpriteSheet = _content.Load<Texture2D>("Spritesheets/Aggregate/cyborg");
                    break;
                case Entities.Aggregate.EarthianPlayer:
                    hud.AvatarSpriteSheet = _content.Load<Texture2D>("Spritesheets/Aggregate/Earthian2x");
                    break;
                case Entities.Aggregate.FairyPlayer:
                    hud.AvatarSpriteSheet = _content.Load<Texture2D>("Spritesheets/Aggregate/wind_fae");
                    break;
                case Entities.Aggregate.GargranianPlayer:
                    hud.AvatarSpriteSheet = _content.Load<Texture2D>("Spritesheets/Aggregate/gargranian");
                    break;
                case Entities.Aggregate.SpacePiratePlayer:
                    hud.AvatarSpriteSheet = _content.Load<Texture2D>("Spritesheets/Aggregate/SpacePBig");
                    break;
                case Entities.Aggregate.ZombiePlayer:
                    hud.AvatarSpriteSheet = _content.Load<Texture2D>("Spritesheets/Aggregate/MzombieBx2");
                    break;
                default:
                    throw new Exception("Unknown race");
            }
            hud.AvatarSpriteBounds = new Rectangle(0, 0, 64, 64);

            //Load skill icon

            //Load the rest of the spritesheets. Dont worry about position yet.
            hud.HudBgSpriteBounds = new Rectangle(0, 0, 270, 143);
            hud.HealthSpriteSheet = _content.Load<Texture2D>("Spritesheets/HUD/HealthBar");
            hud.HealthBounds = new Rectangle(0, 0, HEALTH_BAR_MAX_BOUNDS, 17);
            hud.PsiSpriteSheet = _content.Load<Texture2D>("Spritesheets/HUD/PsiBar");
            hud.PsiBounds = new Rectangle(0, 0, PSI_BAR_MAX_BOUNDS, 17);
            hud.ExpSpriteSheet = _content.Load<Texture2D>("Spritesheets/HUD/ExpBar");
            hud.ExpBounds = new Rectangle(0, 0, EXP_BAR_MAX_BOUNDS, 7);

            PlayerInfo info = _game.PlayerInfoComponent[player.EntityID];
            hud.Level = LEVEL_PREFIX + info.Level;

            //Load the right background and set all the positions based on the playerindex.
            switch (player.PlayerIndex)
            {
                case PlayerIndex.One:
                    hud.HudBgSpriteSheet = _content.Load<Texture2D>("Spritesheets/HUD/hud_one");
                    hud.HudBgPosition = new Vector2(0, 0);
                    hud.AvatarPosition = new Vector2(20, 20);
                    hud.HealthPosition = new Vector2(109, 4);
                    hud.PsiPosition = new Vector2(109, 26);
                    hud.ExpPosition = new Vector2(109, 48);
                    hud.LevelPosition = new Vector2(20, 84);

                    _p1 = hud;
                    break;
                case PlayerIndex.Two:
                    hud.HudBgSpriteSheet = _content.Load<Texture2D>("Spritesheets/HUD/hud_two");
                    hud.HudBgPosition = new Vector2(1024, 0);
                    hud.AvatarPosition = new Vector2(940, 20);
                    hud.HealthPosition = new Vector2(925 - HEALTH_BAR_MAX_BOUNDS, 4);
                    hud.PsiPosition = new Vector2(925 - PSI_BAR_MAX_BOUNDS, 26);
                    hud.ExpPosition = new Vector2(925 - EXP_BAR_MAX_BOUNDS, 48);
                    hud.LevelPosition = new Vector2(940, 84);

                    _p2 = hud;
                    break;
                case PlayerIndex.Three:
                    hud.HudBgSpriteSheet = _content.Load<Texture2D>("Spritesheets/HUD/hud_three");
                    hud.HudBgPosition = new Vector2(0, 656);
                    hud.AvatarPosition = new Vector2(20, 572);
                    hud.HealthPosition = new Vector2(109, 635);
                    hud.PsiPosition = new Vector2(109, 613);
                    hud.ExpPosition = new Vector2(109, 591);
                    hud.LevelPosition = new Vector2(20, 508);

                    _p3 = hud;
                    break;
                case PlayerIndex.Four:
                    hud.HudBgSpriteSheet = _content.Load<Texture2D>("Spritesheets/HUD/hud_four");
                    hud.HudBgPosition = new Vector2(1024, 656);
                    hud.AvatarPosition = new Vector2(940, 572);
                    hud.HealthPosition = new Vector2(925 - HEALTH_BAR_MAX_BOUNDS, 635);
                    hud.PsiPosition = new Vector2(925 - PSI_BAR_MAX_BOUNDS, 613);
                    hud.ExpPosition = new Vector2(109, 591);
                    hud.LevelPosition = new Vector2(940, 508);

                    _p4 = hud;
                    break;
                default:
                    throw new Exception("Invalid player index");
            }
        }

        /// <summary>
        /// Updates the HUD.
        /// </summary>
        /// <param name="elapsedTime">Time since the last call.</param>
        public void Update(float elapsedTime)
        {
            foreach (Player player in _playerComponent.All)
            {
                HUDSprite hud = getHUDSprite(player.PlayerIndex);
                if (hud.Show == false)
                    continue;

                //Update status bars.
                PlayerInfo info = _game.PlayerInfoComponent[player.EntityID];
                Stats stats = _game.StatsComponent[player.EntityID];

                float healthPercent = info.Health / stats.HealthBase;
                float psiPercent = info.Psi / stats.PsiBase;
                float expPercent = info.Experience / 100; //Need to find expbase

                hud.HealthBounds.Width = (int)(HEALTH_BAR_MAX_BOUNDS * healthPercent);
                hud.PsiBounds.Width = (int)(PSI_BAR_MAX_BOUNDS * psiPercent);
                hud.ExpBounds.Width = (int)(EXP_BAR_MAX_BOUNDS * expPercent);

                //Update icons

                //Update level
                hud.Level = LEVEL_PREFIX + info.Level;

                setHUDSprite(player.PlayerIndex, hud);
            }
        }

        private HUDSprite getHUDSprite(PlayerIndex index)
        {
            switch(index)
            {
                case PlayerIndex.One:
                    return _p1;
                case PlayerIndex.Two:
                    return _p2;
                case PlayerIndex.Three:
                    return _p3;
                case PlayerIndex.Four:
                    return _p4;
                default:
                    return new HUDSprite() { Show = false }; 
            }
        }

        private void setHUDSprite(PlayerIndex index, HUDSprite sprite)
        {
            switch(index)
            {
                case PlayerIndex.One:
                    _p1 = sprite;
                    break;
                case PlayerIndex.Two:
                    _p2 = sprite;
                    break;
                case PlayerIndex.Three:
                    _p3 = sprite;
                    break;
                case PlayerIndex.Four:
                    _p4 = sprite;
                    break; 
            }
        }

        /// <summary>
        /// Draws the HUD to the given spritebatch.
        /// </summary>
        /// <param name="elapsedTime">Time since last call.</param>
        /// <param name="spriteBatch">SpriteBatch to draw to.</param>
        public void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            if (_p1.Show)
                drawHud(_p1, elapsedTime, spriteBatch);
            if (_p2.Show)
                drawHud(_p2, elapsedTime, spriteBatch);
            if (_p3.Show)
                drawHud(_p3, elapsedTime, spriteBatch);
            if (_p4.Show)
                drawHud(_p4, elapsedTime, spriteBatch);
        }

        //Handles drawing the individual HUDs.
        private void drawHud(HUDSprite hud, float elapsedTime, SpriteBatch spriteBatch)
        {
            //Background
            spriteBatch.Draw(hud.HudBgSpriteSheet,
                                    hud.HudBgPosition,
                                    hud.HudBgSpriteBounds,
                                    Color.White,
                                    0f,                                             // rotation
                                    new Vector2(0),  // origin
                                    1f,                                             // scale
                                    SpriteEffects.None,
                                    0.1f);

            //Avatar
            spriteBatch.Draw(hud.AvatarSpriteSheet,
                                    hud.AvatarPosition,
                                    hud.AvatarSpriteBounds,
                                    Color.White,
                                    0f,                                             // rotation
                                    new Vector2(0),  // origin
                                    1f,                                             // scale
                                    SpriteEffects.None,
                                    0);

            //Health bar
            spriteBatch.Draw(hud.HealthSpriteSheet,
                                    hud.HealthPosition,
                                    hud.HealthBounds,
                                    Color.White,
                                    0,                                             // rotation
                                    new Vector2(0),  // origin
                                    1f,                                             // scale
                                    SpriteEffects.None,
                                    0);

            //Psi bar
            spriteBatch.Draw(hud.PsiSpriteSheet,
                                    hud.PsiPosition,
                                    hud.PsiBounds,
                                    Color.White,
                                    0,                                             // rotation
                                    new Vector2(0),  // origin
                                    1f,                                             // scale
                                    SpriteEffects.None,
                                    0);

            //Exp bar
            spriteBatch.Draw(hud.ExpSpriteSheet,
                                    hud.ExpPosition,
                                    hud.ExpBounds,
                                    Color.White,
                                    0,                                             // rotation
                                    new Vector2(0),  // origin
                                    1f,                                             // scale
                                    SpriteEffects.None,
                                    0);

            //Skill icon

            //Item icon

            //Collection icon

            //Level
            spriteBatch.DrawString(_font, hud.Level, hud.LevelPosition, Color.Black);
        }
    }
}
