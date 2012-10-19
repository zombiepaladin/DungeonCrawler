using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonCrawler
{
    public class CharSelectPlayer
    {
        public ImageSprite cursor;

        public bool connected;

        public bool selected;

        public int xPos;

        public int yPos;

        public float timer;

        public PlayerIndex playerIndex;

        public CharSelectPlayer(ImageSprite image, bool connected, PlayerIndex playerIndex)
        {
            cursor = image;
            this.connected = connected;
            xPos = 0;
            yPos = 0;
            timer = 0;
            this.playerIndex = playerIndex;
        }

        public void MoveUpDown()
        {
            if (yPos == 0)
                yPos = 1;
            else
                yPos = 0;
        }

        public void MoveLeft()
        {
            xPos--;
            if (xPos < 0) xPos = 2;
        }

        public void MoveRight()
        {
            xPos++;
            if (xPos > 2) xPos = 0;
        }
    }
}
