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
        private const float DISPLAY_TIMER = 3f;
        private float _timer;
        private bool _displayText;
        private int _currentIndex;

        private SpriteFont _font;
        private Vector2 _position;

        private string[] _credits = new string[]
        {
            "Dungeon Crawler\n\nCIS 580 - Intro to Game Design\n\nKansas State University",
            "Studio Staff-Producers\n\nNathen Bean \nMicheal Marlen",
            "Studio Staff-Programmers/Designers\n\nDevin Kelly-Collins\nJoseph Shaw\nAustin Murphy\nNick Boen\nMatthew McHaney",
            "Studio Staff-Programmers/Designers\n\nAdam Clark\nNick Stanley\nJosh Zavala\nMicheal Fountain\nSam Fike",
            "Studio Staff-Programmers/Designers\n\nMatthew Hart\nAndrew Bellinder\nNicholes Strub\nBrett Barger\nJiri Malina",
            "Studio Staff-Programmers/Designers\n\nAdam Steen\nBen Jazen\nDaniel Rymph",
            "Artwork\n\n Napoleon - Globe_0.png \n Tiled Map Editor \nDaniel Cook \n ",
            "Artwork\n\nAkaiSeigi - Andriod\n Rajawali - Robo Tank, Robo Butler, Avadan Sidewiew Mecha",
            "Arkwork\n\nRetired-Pine - Slime\n",
            "Developed with Microsoft's \nXNA framwork",
            "Thank you for Playing\n\nPress enter to play again",
        };

        public Credits(DungeonCrawlerGame game)
        {
            _font = game.Content.Load<SpriteFont>("SpriteFonts/Pescadero");;
            _position = new Vector2(50);
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
                _timer = 0;
                _displayText = !_displayText;
                if (_displayText)
                    _currentIndex++;
                if (_currentIndex + 1 == _credits.Length)
                    Done = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(_displayText)
                spriteBatch.DrawString(_font, _credits[_currentIndex], _position, Color.White);
        }
    }
}
