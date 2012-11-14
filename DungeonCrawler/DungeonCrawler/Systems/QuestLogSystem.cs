#region File Description
//-----------------------------------------------------------------------------
// QuestLogSystem.cs
//
// Author: Nicholas Strub
//
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence bla bla bla
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using DungeonCrawler.Components;
#endregion


namespace DungeonCrawler.Systems
{
    public class QuestLogSystem
    {
        #region Private Members

        /// <summary>
        /// The game this system belongs to
        /// </summary>
        private DungeonCrawlerGame game;

        /// <summary>
        /// A SpriteBatch for drawing with
        /// </summary>
        private SpriteBatch spriteBatch;

        /// <summary>
        /// A sprite font for the quest anme
        /// </summary>
        private SpriteFont nameFont;

        /// <summary>
        /// A sprite font for the quest description
        /// </summary>
        private SpriteFont descriptionFont;

        /// <summary>
        /// The texture for the quest log
        /// </summary>
        private Texture2D questLog;

        /// <summary>
        /// The screen coordinates for the quest log
        /// </summary>
        private Rectangle questLogLocation;

        /// <summary>
        /// The starting location for the text in the quest log
        /// </summary>
        private Vector2 questLogTextLocation;

        /// <summary>
        /// A boolean indicating whether to display the current quest goals
        /// </summary>
        private bool displayGoals;

        private string[] questNames = { "Reach the next room." };

        /// <summary>
        /// Contains all of the descriptions for the quests, sorted by their quest id
        /// </summary>
        private string[] questDescriptions = { "Proceed to the next room. This can be accomplished by walking through the doorway on the left side of the room." };

        #endregion

        #region Public Members

        /// <summary>
        /// Indicates whether the quest log gui should be displayed
        /// </summary>
        public bool displayLog;

        /// <summary>
        /// The current quest
        /// </summary>
        public Quest currentQuest;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new quest log system
        /// </summary>
        /// <param name="Game">A reference to the dungeon crawler game</param>
        public QuestLogSystem(DungeonCrawlerGame Game)
        {
            game = Game;
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            nameFont = game.Content.Load<SpriteFont>("Spritefonts/BoldPescadero");
            descriptionFont = game.Content.Load<SpriteFont>("Spritefonts/Pescadero");
            questLog = game.Content.Load<Texture2D>("Spritesheets/QuestLog");
            questLogLocation = new Rectangle(600, 65, 376, 593);
            questLogTextLocation = new Vector2(questLogLocation.X + 45, questLogLocation.Y + 90);
            displayLog = false;
            displayGoals = false;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the quest log system
        /// </summary>
        /// <param name="elapsedTime">The elapsed time in seconds</param>
        public void Update(float elapsedTime)
        {
            // Sets the current quest to the first in progress quest
            foreach (Quest q in game.QuestComponent.All)
            {
                if (q.questStatus == QuestStatus.InProgress)
                {
                    currentQuest = q;
                    displayGoals = true;
                }
            }
            // If the quest is finished, do not display the goals for it
            if (currentQuest.questStatus == QuestStatus.Finished) displayGoals = false;
        }

        /// <summary>
        /// Renders the quest log when appropriate
        /// </summary>
        public void Draw()
        {
            spriteBatch.Begin();

            if (displayLog)
            {
                spriteBatch.Draw(questLog, questLogLocation, Color.White);
                if (displayGoals)
                {
                    #region Draw Quest Name

                    //Computes the center of the line
                    questLogTextLocation.X = ((((questLogLocation.X + 45) * 2) + 66) / 2) - (questNames[currentQuest.questID].Length / 2);
                    questLogTextLocation.Y = questLogLocation.Y + 90;
                    spriteBatch.DrawString(nameFont, questNames[currentQuest.questID], questLogTextLocation, Color.Black);

                    #endregion

                    #region Draw Quest Description

                    string[] strings = questDescriptions[currentQuest.questID].Split(' ');
                    questLogTextLocation.X = questLogLocation.X + 45;
                    string newstring = "";
                    int i = 0;
                    int j = 0;
                    int length = (int)Math.Ceiling(questDescriptions[currentQuest.questID].Length / 33.0);
                    // Splits up the quest description into lines that fit into the quest log gui, then draws them
                    while (j <= length)
                    {
                        if (i < strings.Length)
                        {
                            newstring += strings[i] + " ";
                            i++;
                        }
                        if (i >= strings.Length || newstring.Length + strings[i].Length >= 33)
                        {
                            questLogTextLocation.Y = questLogLocation.Y + 90 + ((j + 1) * 25);
                            spriteBatch.DrawString(descriptionFont, newstring, questLogTextLocation, Color.Black);
                            newstring = "";
                            j++;
                        }
                    }

                    #endregion
                }
            }

            spriteBatch.End();
        }

        #endregion
    }
}
