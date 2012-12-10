using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DungeonCrawler.SplashScreens
{
    public class GameOver : SplashScreen
    {
        SpriteFont _font;
        Vector2 _position;

        public GameOver(DungeonCrawlerGame game)
        {
            _font = game.Content.Load<SpriteFont>("SpriteFonts/BoldPescadero");
            _position = new Vector2(50);
        }

        public override void Update(float elapsedTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, "GAME OVER", _position, Color.White);
        }
    }
}
