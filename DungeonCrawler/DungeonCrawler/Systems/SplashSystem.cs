using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DungeonCrawler.SplashScreens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonCrawler.Systems
{
    public class SplashSystem
    {
        public enum SplashType
        {
            GameOver,
            GameStart,
            Credits,
        }

        private DungeonCrawlerGame _game;
        private SplashScreen _screen;
        private SpriteBatch _spriteBatch;

        public SplashSystem(DungeonCrawlerGame game)
        {
            _game = game;
            _spriteBatch = new SpriteBatch(game.GraphicsDevice);
        }

        public void Load(SplashType splash)
        {
            switch (splash)
            {
                case SplashType.GameStart:
                    _screen = new GameStart(_game);
                    break;
                case SplashType.GameOver:
                    _screen = new GameOver(_game);
                    break;
                case SplashType.Credits:
                    _screen = new Credits(_game);
                    break;
            }
        }

        public void Update(float elapsedTime)
        {
            _screen.Update(elapsedTime);
            if (_screen.Done && InputHelper.GetInput(PlayerIndex.One).IsPressed(Keys.Enter, Buttons.Start))
            {
                if (_screen is GameStart)
                {
                    DungeonCrawlerGame.game.GameState = GameState.CharacterSelection;
                }
                else
                {
                    Load(SplashType.GameStart);
                }
            }
        }

        public void Draw(float elapsedTime)
        {
            _spriteBatch.Begin(SpriteSortMode.BackToFront, null, SamplerState.PointClamp, null, null, null);
            _screen.Draw(_spriteBatch);
            _spriteBatch.End();
        }
    }
}
