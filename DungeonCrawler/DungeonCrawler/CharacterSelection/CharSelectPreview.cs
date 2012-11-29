#region File Description
//-----------------------------------------------------------------------------
// CharSelectPreview.cs 
//
// This in an object that represents the preview of the user's save file.
// Used to show the preview and store information from the DungeonCrawlerGame.CharacterSaveFilePreview
// stored in the MasterGameSave.charFiles list.
// Used on the ContinueNewGameScreen
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
    public class CharSelectPreview
    {
        #region Fields
        private ImageSprite image;

        private TextSprite charType;

        private TextSprite level;

        private Vector2 position;

        private Color color;

        private String fileName;

        private int fileNumber;

        private int aggregate;

        private bool newGame;
        #endregion

        #region Properties
        /// <summary>
        /// An ImageSprite representing the preview's character.
        /// Used to show which character the game save uses.
        /// </summary>
        public ImageSprite Image
        {
            get { return image; }
            set { image = value; }
        }

        /// <summary>
        /// A TextSprite representing the preview's character type.
        /// Used to show which character the game save uses.
        /// </summary>
        public TextSprite CharType
        {
            get { return charType; }
            set { charType = value; }
        }

        /// <summary>
        /// A TextSprite representing the preview's character's level.
        /// Used to show which character the game save uses.
        /// </summary>
        public TextSprite Level
        {
            get { return level; }
            set { level = value; }
        }

        /// <summary>
        /// The base position for this preview
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// The color of the sprite, used to show
        /// if it is highlighted/selected or not
        /// using the alpha value.
        /// </summary>
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        /// <summary>
        /// The filename for the game save this preview represents
        /// </summary>
        public String FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        /// <summary>
        /// The file number this preview represents.
        /// Used for counting files if this is a new game file
        /// </summary>
        public int FileNumber
        {
            get { return fileNumber; }
            set { fileNumber = value; }
        }

        /// <summary>
        /// The aggregate of the character this preview represents
        /// </summary>
        public int Aggregate
        {
            get { return aggregate; }
            set { aggregate = value; }
        }

        /// <summary>
        /// Whether or not this represents a new game or an existing game save
        /// </summary>
        public bool NewGame
        {
            get { return newGame; }
            set { newGame = value; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Constructor for this class
        /// </summary>
        /// <param name="image">The image of this preview's character</param>
        /// <param name="pos">The position of this preview</param>
        /// <param name="font">The font for the text sprites</param>
        /// <param name="charType">The character type of this preview</param>
        /// <param name="level">The level of this preview's character</param>
        /// <param name="color">The color of this game save (namely bright or dimmed using the alpha value)</param>
        /// <param name="fileName">The filename for the game save this preview represents</param>
        public CharSelectPreview(Texture2D image, Vector2 pos, SpriteFont font, String charType, String level, Color color, String fileName)
        {
            this.fileName = fileName;
            this.image = new ImageSprite(image, (int)(pos.X - image.Width / 2), (int)pos.Y, color);
            this.charType = new TextSprite(font, charType, new Vector2(pos.X + 25, pos.Y - 8), color);
            this.level = new TextSprite(font, level, new Vector2(pos.X + 25, pos.Y + 12), color);
            this.position = pos;
            this.color = color;
            this.newGame = false;
        }

        /// <summary>
        /// Draws the image and text sprites of this preview
        /// </summary>
        /// <param name="spriteBatch">The spritebatch from the ContinueNewGameScreen draw method</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image.Image, image.Position, new Rectangle?(), Color, 0, image.Origin, 1, SpriteEffects.None, 1);
            spriteBatch.DrawString(charType.Font, charType.Text, charType.Position, Color, 0, new Vector2(0, charType.Origin.Y), 1, SpriteEffects.None, 1);
            spriteBatch.DrawString(level.Font, level.Text, level.Position, Color, 0, new Vector2(0, level.Origin.Y), 1, SpriteEffects.None, 1);
        }

        /// <summary>
        /// Moves the sprites by a given offest.  Used in the
        /// ContinueNewGameScreen update method when the user moves
        /// up or down in the list.
        /// </summary>
        /// <param name="offset"></param>
        public void MoveByOffset(Vector2 offset)
        {
            Image.Position += offset;
            CharType.Position += offset;
            Level.Position += offset;
        }

        /// <summary>
        /// Sets the sprites to a given position.  Used in the
        /// ContinueNewGameScreen ReloadGameSaves method.
        /// </summary>
        /// <param name="position">The position to move to</param>
        public void SetPosition(Vector2 position)
        {
            Image.X = (int)(position.X - Image.Image.Width / 2);
            Image.Y = (int)position.Y;
            CharType.Position = new Vector2(position.X + 25, position.Y - 8);
            Level.Position = new Vector2(position.X + 25, position.Y + 12);
        }
        #endregion
    }
}
