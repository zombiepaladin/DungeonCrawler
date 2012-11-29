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
            public string Level;
        }

        private const int HEALTH_BAR_MAX_BOUNDS = 115;
        private const int PSI_BAR_MAX_BOUNDS = 115;
        private const int EXP_BAR_MAX_BOUNDS = 115;

        private DungeonCrawlerGame _game;
        private ContentManager _content;
        private PlayerComponent _playerComponent;

        private HUDSprite _p1;
        private HUDSprite _p2;
        private HUDSprite _p3;
        private HUDSprite _p4;

        private SpriteFont _font;

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

        public void LoadContent()
        {
            _font = _content.Load<SpriteFont>("SpriteFonts/Pescadero");

            //Now load for each player. Yay
            foreach (Player player in _playerComponent.All)
            {
                LoadPlayerHUD(player);
            }
        }

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
            hud.HealthBounds = new Rectangle(0, 0, 15, HEALTH_BAR_MAX_BOUNDS);
            hud.PsiSpriteSheet = _content.Load<Texture2D>("Spritesheets/HUD/PsiBar");
            hud.PsiBounds = new Rectangle(0, 0, 15, PSI_BAR_MAX_BOUNDS);
            hud.ExpSpriteSheet = _content.Load<Texture2D>("Spritesheets/HUD/ExpBar");
            hud.ExpBounds = new Rectangle(0, 0, 15, EXP_BAR_MAX_BOUNDS);

            //Load the right background and set all the positions base on the playerindex.
            switch (player.PlayerIndex)
            {
                case PlayerIndex.One:
                    hud.HudBgSpriteSheet = _content.Load<Texture2D>("Spritesheets/HUD/hud_one");
                    hud.HudBgPosition = new Vector2(0, 0);
                    hud.AvatarPosition = new Vector2(20, 20);

                    _p1 = hud;
                    break;
                case PlayerIndex.Two:
                    hud.HudBgSpriteSheet = _content.Load<Texture2D>("Spritesheets/HUD/hud_two");
                    hud.HudBgPosition = new Vector2(1024, 0);
                    hud.AvatarPosition = new Vector2(-20, 20);

                    _p2 = hud;
                    break;
                case PlayerIndex.Three:
                    hud.HudBgSpriteSheet = _content.Load<Texture2D>("Spritesheets/HUD/hud_three");
                    hud.HudBgPosition = new Vector2(0, 656);
                    hud.AvatarPosition = new Vector2(20, -84);

                    _p3 = hud;
                    break;
                case PlayerIndex.Four:
                    hud.HudBgSpriteSheet = _content.Load<Texture2D>("Spritesheets/HUD/hud_four");
                    hud.HudBgPosition = new Vector2(1024, 656);
                    hud.AvatarPosition = new Vector2(-20, -84);

                    _p4 = hud;
                    break;
                default:
                    throw new Exception("Invalid player index");
            }
        }

        public void Update(float elapsedTime)
        {

        }

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

        private void drawHud(HUDSprite hud, float elapsedTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(hud.HudBgSpriteSheet,
                                    hud.HudBgPosition,
                                    hud.HudBgSpriteBounds,
                                    Color.White,
                                    0f,                                             // rotation
                                    new Vector2(0),  // origin
                                    1f,                                             // scale
                                    SpriteEffects.None,
                                    0.1f);

            spriteBatch.Draw(hud.AvatarSpriteSheet,
                                    hud.AvatarPosition,
                                    hud.AvatarSpriteBounds,
                                    Color.White,
                                    0f,                                             // rotation
                                    new Vector2(0),  // origin
                                    1f,                                             // scale
                                    SpriteEffects.None,
                                    0);
        }
    }
}
