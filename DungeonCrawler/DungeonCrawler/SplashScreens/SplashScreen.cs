using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;

namespace DungeonCrawler.SplashScreens
{
    public abstract class SplashScreen
    {
        public Song Music;
        public string NextLevel;
        public bool Done;

        public abstract void Update(float elapsedTime);
        public abstract void Draw(float elapsedTime);
    }
}
