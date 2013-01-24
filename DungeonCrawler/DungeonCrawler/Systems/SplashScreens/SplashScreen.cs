using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonCrawler.SplashScreens
{
    public abstract class SplashScreen
    {
        public Song Music;
        public bool Done;

        public abstract void Update(float elapsedTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
