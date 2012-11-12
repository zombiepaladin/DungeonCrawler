#region Using Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;
using DungeonCrawler.Components;
using DungeonCrawler.Systems;
using DungeonCrawler.Entities;
#endregion

namespace DungeonCrawler
{
    public class VideoPlayerSystem
    {
        private DungeonCrawlerGame game;
        private SpriteBatch spriteBatch;
        private Rectangle drawSpace = new Rectangle(0,150,1280,420);
        private Video cutscene;
        private VideoPlayer videoPlayer;

        public VideoPlayerSystem(DungeonCrawlerGame game)
        {
            this.game = game;
            this.spriteBatch = new SpriteBatch(game.GraphicsDevice);
            this.videoPlayer = new VideoPlayer();
        }

        public void Update(float elapsedTime, Video v, VideoPlayer vp)
        {
            if (cutscene == null)
                cutscene = v;
            if (videoPlayer.State == MediaState.Stopped)
            {
                videoPlayer.Play(cutscene);
                //game.GameState = GameState.Gameplay;
            }
            
        }

        public void Draw(float elapsedTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, null, SamplerState.PointClamp, null, null, null);
            if (videoPlayer.State != MediaState.Stopped)
            {
                spriteBatch.Draw(videoPlayer.GetTexture(), drawSpace, Color.White);
            }
            spriteBatch.End();
        }
    }
}
