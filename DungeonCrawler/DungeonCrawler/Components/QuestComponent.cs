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
    /// A list of all statuses for the quests
    /// </summary>
    public enum QuestStatus
    {
       InProgress,
       Completed,
       TurnedIn,
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
        /// The ID of the quest. It is used to retrieve the quest name and description from the Quest Log System
        /// </summary>
        public uint questID;

        /// <summary>
        /// The status of the quest. This may not be needed in the long run.
        /// </summary>
        public QuestStatus questStatus;

        /// <summary>
        /// Indicates whether the quest is a quest with a counting object (kill X monsters, collect X things)
        /// </summary>
        public Boolean countingObjective;

        /// <summary>
        /// The count towards the current objective. If countingObjective is False, this will be treated as a
        /// Boolean value, indicating whether the quest is finished or not
        /// </summary>
        public int objectiveCount;
    };

    public class QuestComponent : GameComponent<Quest>
    {

    }
}
