using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonCrawler.SplashScreens
{
    public class Credits : SplashScreen
    {
        private const float DISPLAY_TIMER = .5f;
        private float _timer;
        private bool _displayText;
        private int _currentIndex;

        private SpriteFont _font;
        private Vector2 _position;

        private string[] _credits = new string[]
        {
        };

        public Credits()
        {
            //Song = 
            //_font =
            _position = new Vector2(50);
            //NextLevel = 
            Done = false;
            _currentIndex = 0;
            _displayText = true;
            _timer = 0;
        }

        public override void Update(float elapsedTime)
        {
            if (Done)
                return;

            _timer += elapsedTime;
            if (_timer >= DISPLAY_TIMER)
            {
                _displayText = !_displayText;
                if (_displayText)
                    _currentIndex++;
                if (_currentIndex + 1 == _credits.Length)
                    Done = true;
            }
        }

        public override void Draw(float elapsedTime)
        {
            //if(_displayText)
                //spriteBatch.Draw(_font, _credits[_currentIndex], _position, Color.White);
        }
    }
}
