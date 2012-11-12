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
        /// A sprite font for text
        /// </summary>
        private SpriteFont spriteFont;

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
            spriteFont = game.Content.Load<SpriteFont>("Spritefonts/Pescadero");
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
                    questLogTextLocation.Y = questLogLocation.Y + 90;
                    for (int i = 0; i < currentQuest.questGoals.Length; i++)
                    {
                        questLogTextLocation.Y += 25;
                        spriteBatch.DrawString(spriteFont, currentQuest.questGoals[i], questLogTextLocation, Color.Black);
                    }
                }
            }

            spriteBatch.End();
        }

        #endregion
    }
}
