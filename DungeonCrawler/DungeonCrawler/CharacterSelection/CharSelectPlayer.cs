#region File Description
//-----------------------------------------------------------------------------
// CharSelectPlayer.cs 
//
// This is an object that keeps track of the player's cursor, index, 
// connection/selection state, timer delay, and selected game save.
// This is used to simplify things in the ContinueNewGameScreen and CharacterSelectionScreen.
//
// Author: Joseph Shaw
//
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence 
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace DungeonCrawler
{
    public class CharSelectPlayer
    {
        #region Fields
        public ImageSprite cursor;

        public bool connected;

        public bool selected;

        public int xPos;

        public int yPos;

        public float timer;

        public PlayerIndex playerIndex;

        public CharSelectPreview gameSave;
        #endregion

        #region Properties
        /// <summary>
        /// An ImageSprite representing the player's cursor, so they can see what they are selecting
        /// </summary>
        public ImageSprite Cursor
        {
            get { return cursor; }
            set { cursor = value; }
        }

        /// <summary>
        /// Whether or not the player is connected to the system
        /// </summary>
        public bool Connected
        {
            get { return connected; }
            set { connected = value; }
        }

        /// <summary>
        /// Whether or not the player has selected a file or character
        /// </summary>
        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        /// <summary>
        /// Keeps track of the player's x position in the CharacterSelectionScreen grid
        /// </summary>
        public int XPos
        {
            get { return xPos; }
            set { xPos = value; }
        }

        /// <summary>
        /// Keeps track of the player's y position in the CharacterSelectionScreen grid
        /// </summary>
        public int YPos
        {
            get { return yPos; }
            set { yPos = value; }
        }

        /// <summary>
        /// Controller delay for this player so that the buttons don't rapid fire
        /// </summary>
        public float Timer
        {
            get { return timer; }
            set { timer = value; }
        }

        /// <summary>
        /// Player index of this player
        /// </summary>
        public PlayerIndex PlayerIndex
        {
            get { return playerIndex; }
            set { playerIndex = value; }
        }

        /// <summary>
        /// Selected gamesave for this player, used when loading the full save file 
        /// in the GoToNetwork method in the ContinueNewGameScreen
        /// </summary>
        public CharSelectPreview GameSave
        {
            get { return gameSave; }
            set { gameSave = value; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Constructor for this class
        /// </summary>
        /// <param name="image">The ImageSprite for the cursor</param>
        /// <param name="connected">Whether or not this player is initially connected</param>
        /// <param name="playerIndex">This player's player index</param>
        public CharSelectPlayer(ImageSprite image, bool connected, PlayerIndex playerIndex)
        {
            cursor = image;
            this.connected = connected;
            xPos = 0;
            yPos = 0;
            timer = 0;
            this.playerIndex = playerIndex;
        }

        /// <summary>
        /// Moves the cursor up and down on the CharacterSelectionScreen grid
        /// </summary>
        public void MoveUpDown()
        {
            if (yPos == 0)
                yPos = 1;
            else
                yPos = 0;
        }

        /// <summary>
        /// Moves the cursor left on the CharacterSelectionScreen grid
        /// </summary>
        public void MoveLeft()
        {
            xPos--;
            if (xPos < 0) xPos = 2;
        }

        /// <summary>
        /// Moves the cursor right on the CharacterSelectionScreen grid
        /// </summary>
        public void MoveRight()
        {
            xPos++;
            if (xPos > 2) xPos = 0;
        }
        #endregion
    }
}
