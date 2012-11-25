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

        /// <summary>
        /// Contains the names of the quests, sorted by their quest id
        /// </summary>
        private string[] questNames = { "Dungeon Extraordinaire" };

        /// <summary>
        /// Contains all of the descriptions for the quests, sorted by their quest id
        /// </summary>
        private string[] questDescriptions = { "Reach the end of the dungeon." };

        /// <summary>
        /// Contains the objective goal for each quest, sorted by their quest id. For a kill quest,
        /// this would be the number of kills needed to complete the quest. For a non count quest, 
        /// such as a quest to reach a certain room, this will just be 1
        /// </summary>
        private int[] objectiveGoalCount = { 1 };

        /// <summary>
        /// The useable width, in pixels, of the quest log gui
        /// </summary>
        private int questLogWidth = 275;

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
            List<Quest> quests = new List<Quest>();
            // Sets the current quest to the first in progress quest
            foreach (Quest q in game.QuestComponent.All)
            {
                if (q.questStatus == QuestStatus.InProgress)
                {
                    currentQuest = q;
                    if (currentQuest.objectiveCount >= objectiveGoalCount[currentQuest.questID])
                    {
                        currentQuest.objectiveCount = objectiveGoalCount[currentQuest.questID];
                        currentQuest.questStatus = QuestStatus.Finished;
                        displayGoals = false;
                    }
                    quests.Add(currentQuest);
                }
            }
            foreach (Quest q in quests)
            {
                game.QuestComponent[q.EntityID] = q;
            }
        }

        /// <summary>
        /// Renders the quest log when appropriate
        /// </summary>
        public void Draw()
        {
            spriteBatch.Begin();

            if (displayLog) spriteBatch.Draw(questLog, questLogLocation, Color.White);

            if (displayGoals)
            {
                #region Draw Mini Quest Objectives

                float xCoord = game.GraphicsDevice.Viewport.Width - descriptionFont.MeasureString("* " + questDescriptions[currentQuest.questID]).X - 10;

                spriteBatch.DrawString(descriptionFont, questNames[currentQuest.questID], new Vector2(xCoord, 160), Color.Red);
                spriteBatch.DrawString(descriptionFont, "*" + questDescriptions[currentQuest.questID], new Vector2(xCoord, 180), Color.White);

                #endregion

                if (displayLog)
                {
                    #region Draw Quest Name

                    //Computes the center of the quest log
                    questLogTextLocation.X = (((questLogLocation.X * 2) + 40 + questLogWidth) / 2) - (nameFont.MeasureString(questNames[currentQuest.questID]).X / 2);
                    questLogTextLocation.Y = questLogLocation.Y + 75;
                    spriteBatch.DrawString(nameFont, questNames[currentQuest.questID], questLogTextLocation, Color.Black);

                    #endregion

                    #region Draw Quest Description

                    string[] strings = questDescriptions[currentQuest.questID].Split(' ');
                    questLogTextLocation.X = questLogLocation.X + 40;
                    string newstring = "";
                    int i = 0;
                    int j = 0;
                    int length = (int)Math.Ceiling(descriptionFont.MeasureString(questDescriptions[currentQuest.questID]).X / questLogWidth);
                    // Splits up the quest description into lines that fit into the quest log gui, then draws them
                    while (j <= length)
                    {
                        if (i < strings.Length)
                        {
                            newstring += strings[i] + " ";
                            i++;
                        }
                        if (i >= strings.Length || descriptionFont.MeasureString(newstring + strings[i]).X >= 275)
                        {
                            questLogTextLocation.Y = questLogLocation.Y + 75 + ((j + 1) * 25);
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

        /// <summary>
        /// Attempts to activate a quest. If successful, all players in the game will receive the quest.
        /// This method will fail if any current player has a quest marked as InProgress
        /// </summary>
        /// <param name="QuestID">Quest ID of the quest to activate</param>
        /// <returns>A boolean indicating whether the activation was a success or not. It will fail if a quest is already in progress</returns>
        public Boolean ActivateQuest(uint QuestID)
        {
            foreach (Quest quest in game.QuestComponent.All)
            {
                if (quest.questStatus == QuestStatus.InProgress) return false;
            }
            if (QuestID >= this.questNames.Length) return false;
            foreach (Player p in game.PlayerComponent.All)
            {
                Quest q = new Quest()
                {
                    EntityID = p.EntityID,
                    questID = QuestID,
                    questStatus = QuestStatus.InProgress,
                    countingObjective = this.objectiveGoalCount[QuestID] > 1,
                    objectiveCount = 0,
                };
                game.QuestComponent.Add(p.EntityID, q);
            }
            displayGoals = true;
            return true;
        }

        /// <summary>
        /// Increments the objective count for the specified quest
        /// </summary>
        /// <param name="QuestID">The quest to increment</param>
        public void IncremementObjective(uint QuestID)
        {
            List<Quest> quests = new List<Quest>();
            Quest quest = new Quest();
            foreach (Quest q in game.QuestComponent.All)
            {
                quest = q;
                if (quest.questID == QuestID && quest.questStatus == QuestStatus.InProgress)
                {
                    quest.objectiveCount++;
                    quests.Add(quest);
                }
            }
            foreach (Quest q in quests)
            {
                game.QuestComponent[quest.EntityID] = quest;
            }
        }

        #endregion
    }
}
