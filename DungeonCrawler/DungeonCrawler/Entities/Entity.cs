#region File Description
//-----------------------------------------------------------------------------
// Entity.cs
//
// Author: Nathan Bean
//
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence 
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
#endregion

namespace DungeonCrawler.Entities
{
    /// <summary>
    /// A class for generating entity IDs
    /// </summary>
    public class Entity
    {
        /// <summary>
        /// The count of unique entities in the game
        /// </summary>
        private static uint entityCounter = 0;

        /// <summary>
        /// The current number of entities in the game
        /// </summary>
        public int Count
        {
            get { return (int)entityCounter; }
        }

        /// <summary>
        /// Generate a new, unique Entity ID
        /// </summary>
        /// <returns>The Entity ID</returns>
        public static uint NextEntity()
        {
            return entityCounter++;
        }
    }
}
