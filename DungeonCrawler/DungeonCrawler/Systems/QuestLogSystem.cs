#region File Description
//-----------------------------------------------------------------------------
// QuestLogSystem.cs
//
// Author: Nicholas Strub
//
// Modified By: Nicholas Strub (Assignment 9)
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
        private string[] questNames = { "Dungeon Extraordinaire", "Door of Death" };

        /// <summary>
        /// Contains all of the descriptions for the quests, sorted by their quest id
        /// </summary>
        private string[] questDescriptions = { "Reach the end of the dungeon", "Fight the door of death" };

        /// <summary>
        /// Contains the objective goal for each quest, sorted by their quest id. For a kill quest,
        /// this would be the number of kills needed to complete the quest. For a non count quest, 
        /// such as a quest to reach a certain room, this will just be 1
        /// </summary>
        private int[] objectiveGoalCount = { 1, 1 };

        /// <summary>
        /// The useable width, in pixels, of the quest log gui
        /// </summary>
        private int questLogWidth = 275;

        /// <summary>
        /// The starting coordinate, within the quest log, of the usable gui
        /// </summary>
        private int questLogLeftBound = 40;

        #endregion

        #region Public Members

        /// <summary>
        /// Indicates whether the quest log gui should be displayed
        /// </summary>
        public bool displayLog;

        /// <summary>
        /// A list of the questIDs of the currently active quests
        /// </summary>
        public List<uint> ActiveQuests;

        /// <summary>
        /// A list of the questIDs of the completed quests that should show as being completed
        /// </summary>
        public List<uint> CompletedQuests;

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
            ActiveQuests = new List<uint>();
            CompletedQuests = new List<uint>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the quest log system
        /// </summary>
        /// <param name="elapsedTime">The elapsed time in seconds</param>
        public void Update(float elapsedTime)
        {
            List<int> questIDs = new List<int>();

            // Sets the current quest to the first in progress quest
            foreach (Quest q in game.Quests)
            {
                if (q.questStatus == QuestStatus.InProgress && q.objectiveCount >= objectiveGoalCount[q.questID])
                {
                    questIDs.Add(game.Quests.IndexOf(q));
                }
            }

            foreach (int ID in questIDs)
            {
                Quest quest = game.Quests[ID];
                quest.questStatus = QuestStatus.Completed;
                game.Quests[ID] = quest;

                if (ActiveQuests.Contains(quest.questID))
                    ActiveQuests.Remove(quest.questID);

                if (!CompletedQuests.Contains(quest.questID))
                    CompletedQuests.Add(quest.questID);
            }

            if (ActiveQuests.Count <= 0 && CompletedQuests.Count <= 0) displayGoals = false;
            else displayGoals = true;
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
                int k = 0;
                float xCoordMini = 0;
                float yCoordMini = 0;
                float xCoordGUI = 0;
                float yCoordGUI = questLogLocation.Y + 75;

                foreach (uint questID in ActiveQuests)
                {
                    // Compute the x coordinate for the quests to be left aligned at
                    float temp = game.GraphicsDevice.Viewport.Width - descriptionFont.MeasureString("*" + questDescriptions[questID]).X - 10;
                    if (xCoordMini == 0 || temp < xCoordMini) xCoordMini = temp;
                }

                foreach (uint questID in CompletedQuests)
                {
                    // Compute the x coordinate for the quests to be left aligned at
                    float temp = game.GraphicsDevice.Viewport.Width - descriptionFont.MeasureString("*" + questDescriptions[questID]).X - 10;
                    if (xCoordMini == 0 || temp < xCoordMini) xCoordMini = temp;
                }

                #region Draw Active Quests
                foreach (uint questID in ActiveQuests)
                {
                    #region Draw Mini Quest Objectives

                    yCoordMini = 160 + (k * 60);

                    spriteBatch.DrawString(descriptionFont, questNames[questID], new Vector2(xCoordMini, yCoordMini), Color.Red);
                    spriteBatch.DrawString(descriptionFont, "*" + questDescriptions[questID], new Vector2(xCoordMini, yCoordMini + 20), Color.White);

                    #endregion

                    if (displayLog)
                    {
                        #region Draw Quest Name

                        //Computes the center of the quest log
                        //                             left coordinate of usable GUI                    right coordinate of usable GUI
                        xCoordGUI = (((questLogLocation.X + questLogLeftBound) + (questLogLocation.X + questLogLeftBound + questLogWidth)) / 2) - (nameFont.MeasureString(questNames[questID]).X / 2);
                        spriteBatch.DrawString(nameFont, questNames[questID], new Vector2(xCoordGUI, yCoordGUI), Color.Black);

                        #endregion

                        #region Draw Quest Description

                        string[] strings = questDescriptions[questID].Split(' ');
                        xCoordGUI = questLogLocation.X + questLogLeftBound;
                        string newstring = "";
                        int i = 0;
                        int j = 0;
                        int length = (int)Math.Ceiling(descriptionFont.MeasureString(questDescriptions[questID]).X / questLogWidth);
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
                                yCoordGUI += 25;
                                spriteBatch.DrawString(descriptionFont, newstring, new Vector2(xCoordGUI, yCoordGUI), Color.Black);
                                newstring = "";
                                j++;
                            }
                        }

                        #endregion
                    }
                    k++;
                }
                #endregion

                #region Draw Completed Quests
                //yCoordGUI += 25;
                foreach (uint questID in CompletedQuests)
                {
                    #region Draw Mini Quest Objectives

                    yCoordMini = 160 + (k * 60);

                    spriteBatch.DrawString(descriptionFont, questNames[questID], new Vector2(xCoordMini, yCoordMini), Color.Red);
                    spriteBatch.DrawString(descriptionFont, "COMPLETED", new Vector2(xCoordMini, yCoordMini + 20), Color.Green);

                    #endregion

                    if (displayLog)
                    {
                        #region Draw Quest Name

                        //Computes the center of the quest log
                        //                             left coordinate of usable GUI                    right coordinate of usable GUI
                        xCoordGUI = (((questLogLocation.X + questLogLeftBound) + (questLogLocation.X + questLogLeftBound + questLogWidth)) / 2) - (nameFont.MeasureString(questNames[questID]).X / 2);
                        spriteBatch.DrawString(nameFont, questNames[questID], new Vector2(xCoordGUI, yCoordGUI), Color.Black);

                        #endregion

                        #region Draw Quest Description

                        string[] strings = questDescriptions[questID].Split(' ');
                        xCoordGUI = questLogLocation.X + questLogLeftBound;
                        string newstring = "";
                        int i = 0;
                        int j = 0;
                        int length = (int)Math.Ceiling(descriptionFont.MeasureString(questDescriptions[questID]).X / questLogWidth);
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
                                yCoordGUI += 25;
                                spriteBatch.DrawString(descriptionFont, newstring, new Vector2(xCoordGUI, yCoordGUI), Color.Black);
                                newstring = "";
                                j++;
                            }
                        }

                        #endregion
                    }
                    k++;
                }

                #endregion
            }

            spriteBatch.End();
        }

        /// <summary>
        /// Attempts to activate a quest. If successful, all players in the game will receive the quest.
        /// This method will fail if any current player has a quest marked as InProgress
        /// </summary>
        /// /// <param name="entityID">Entity ID of the player to activate the quest on</param>
        /// <param name="QuestID">Quest ID of the quest to activate</param>
        /// <returns>A boolean indicating whether the activation was a success or not. It will fail if a quest is already in progress</returns>
        public Boolean ActivateQuest(uint entityID, uint QuestID)
        {
            // If the quest doesn't exist in the defined lists of quests, the quest activation will fail
            if (QuestID >= this.questNames.Length) return false;
            
            Quest q = new Quest()
            {
                EntityID = entityID,
                questID = QuestID,
                questStatus = QuestStatus.InProgress,
                countingObjective = this.objectiveGoalCount[QuestID] > 1,
                objectiveCount = 0,
            };

            game.Quests.Add(q);

            if (!ActiveQuests.Contains(q.questID))
                ActiveQuests.Add(q.questID);
            
            return true;
        }

        /// <summary>
        /// Removes the specified quest for the specified player. Should only be done upon turning in the quest to the npc
        /// </summary>
        /// <param name="entityID">Entity ID of the player turning in the quest</param>
        /// <param name="QuestID">Quest ID of the quest being turned in</param>
        public void TurnInQuest(uint entityID, uint QuestID)
        {
            int index = -1;
            foreach (Quest q in game.Quests)
            {
                if (q.EntityID == entityID && q.questID == QuestID)
                    index = game.Quests.IndexOf(q);
            }

            if (index != -1)
            {
                Quest newQuest = game.Quests[index];
                newQuest.questStatus = QuestStatus.TurnedIn;
                game.Quests[index] = newQuest;

                if (CompletedQuests.Contains(newQuest.questID))
                    CompletedQuests.Remove(newQuest.questID);
            }
        }

        /// <summary>
        /// Removes the specified quest for all players. Should only be done upon turning in the quest to the npc
        /// </summary>
        /// <param name="QuestID">Quest ID of the quest to remove</param>
        public void RemoveQuest(uint QuestID)
        {
            List<int> questsToRemove = new List<int>();
            foreach (Quest q in game.Quests)
            {
                if (q.questID == QuestID) questsToRemove.Add(game.Quests.IndexOf(q));
            }

            foreach (int questIndex in questsToRemove)
            {
                Quest newQuest = game.Quests[questIndex];
                newQuest.questStatus = QuestStatus.TurnedIn;
                game.Quests[questIndex] = newQuest;

                if (CompletedQuests.Contains(newQuest.questID))
                    CompletedQuests.Remove(newQuest.questID);
            }
        }

        /// <summary>
        /// Increments the objective count for the specified quest for all the players
        /// </summary>
        /// <param name="QuestID">The quest to increment</param>
        public void IncremementObjective(uint QuestID)
        {
            List<int> quests = new List<int>();
            foreach (Quest q in game.Quests)
            {
                if (q.questID == QuestID && q.questStatus == QuestStatus.InProgress)
                {
                    quests.Add(game.Quests.IndexOf(q));
                }
            }
            foreach (int questIndex in quests)
            {
                Quest newQuest = game.Quests[questIndex];
                newQuest.objectiveCount++;
                game.Quests[questIndex] = newQuest;
            }
        }

        /// <summary>
        /// Increments the objective count for the active quest of the specified player
        /// </summary>
        /// <param name="EntityID">The entity ID of the player with the quest to increment</param>
        public void IncrementObjective(uint entityID, uint questID)
        {
            int index = -1;
            foreach (Quest q in game.Quests)
            {
                if (q.questID == questID && q.EntityID == entityID && q.questStatus == QuestStatus.InProgress)
                    index = game.Quests.IndexOf(q);
            }
            if (index != -1)
            {
                Quest newQuest = game.Quests[index];
                newQuest.objectiveCount++;
                game.Quests[index] = newQuest;
            }
        }

        /// <summary>
        /// Restores the states of the quests after loading from a save
        /// </summary>
        public void RestoreFromSave()
        {
            foreach (Quest q in game.Quests)
            {
                if (q.questStatus == QuestStatus.InProgress)
                {
                    if (!ActiveQuests.Contains(q.questID))
                        ActiveQuests.Add(q.questID);
                }
                else if (q.questStatus == QuestStatus.Completed)
                {
                    if (!CompletedQuests.Contains(q.questID))
                        CompletedQuests.Add(q.questID);
                }
            }
        }

        #endregion
    }
}
