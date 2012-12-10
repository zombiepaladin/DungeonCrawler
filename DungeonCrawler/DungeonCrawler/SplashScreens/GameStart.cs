using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DungeonCrawler.SplashScreens
{
    public class GameStart : SplashScreen
    {
        private const float BLINK_TIME = 1f;

        private Texture2D _mainSpriteSheet;
        private Vector2 _mainPosition;

        private SpriteFont _font;
        private Vector2 _fontPosition;

        private bool _drawFont;
        private float _timer;

        public GameStart(DungeonCrawlerGame game)
        {
            _mainSpriteSheet = game.Content.Load<Texture2D>("Spritesheets/TitleScreen");
            _font = game.Content.Load<SpriteFont>("SpriteFonts/BoldPescadero");

            _mainPosition = new Vector2(0);
            _fontPosition = new Vector2(1000, 100);

            _drawFont = true;
            _timer = 0;
        }

        public override void Update(float elapsedTime)
        {
            _timer += elapsedTime;
            if (_timer >= BLINK_TIME)
            {
                Done = true;
                _drawFont = !_drawFont;
                _timer = 0;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_mainSpriteSheet, _mainPosition, Color.White);
            if (_drawFont)
            {
                spriteBatch.DrawString(_font, "Press enter to start", _fontPosition, Color.White);
            }
        }
    }
}
