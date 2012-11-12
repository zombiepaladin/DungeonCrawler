#region File Description
//-----------------------------------------------------------------------------
// QuestComponent.cs 
//
// Author: Nicholas Strub
// Note: The system for actually creating the quests will have to be implemented
//       later when there are story and gameplay elements to trigger quests.
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
#endregion

namespace DungeonCrawler.Components
{
    /// <summary>
    /// A list of all the quest names
    /// </summary>
    public enum QuestName
    {
        // Main/Story Quests
        ReachNextRoom,
        SecondQuest,
        // Side Quests (if they exist)
    };

    /// <summary>
    /// A list of all statuses for the quests
    /// </summary>
    public enum QuestStatus
    {
       InProgress,
       NextGoal,
       Finished,
    };

    /// <summary>
    /// A struct representing a quest
    /// </summary>
    public struct Quest
    {
        /// <summary>
        /// The ID of the entity this sprite belongs to
        /// </summary>
        public uint EntityID;

        /// <summary>
        /// The name of the quest
        /// </summary>
        public QuestName questName;

        /// <summary>
        /// The status of the quest. This may not be needed in the long run.
        /// </summary>
        public QuestStatus questStatus;

        /// <summary>
        /// Contains strings that describe the goals of the quest
        /// </summary>
        public string[] questGoals;
    };

    public class QuestComponent : GameComponent<Quest>
    {

    }
}
