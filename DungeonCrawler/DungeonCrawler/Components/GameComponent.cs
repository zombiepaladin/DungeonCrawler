#region File Description
//-----------------------------------------------------------------------------
// GameComponent.cs
//
// Author: Nathan Bean
//
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence 
//-----------------------------------------------------------------------------

//Samuel Fike and Jiri Malina: Created a virtual HandleTrigger method

#endregion

#region Using Statements
using System.Collections.Generic;
using System;
#endregion

namespace DungeonCrawler.Components
{
    /// <summary>
    /// A generic data structure for holding Structs 
    /// representing a single aspect of an Entity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GameComponent<T>
    {
        #region Private and Protected Variables

        /// <summary>
        /// A Dictionary of T, keyed by EntityIDs
        /// Using a Dictionary gives us O(1) random
        /// access, and we can supply an interator
        /// from the value array
        /// </summary>
        protected Dictionary<uint, T> elements;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new generic GameComponent
        /// </summary>
        public GameComponent()
        {
            elements = new Dictionary<uint, T>();
        }

        /// <summary>
        /// Creates a new generic GameComponent with initial capacity n
        /// </summary>
        /// <param name="initialCapacity">The initial capacity</param>
        public GameComponent(int initialCapacity)
        {
            elements = new Dictionary<uint, T>(initialCapacity);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the component for the specified entity
        /// </summary>
        /// <param name="entityID">The entity ID</param>
        /// <returns>The component</returns>
        public T this[uint entityID]
        {
            set { elements[entityID] = value; }
            get{ return elements[entityID]; }
        }


        /// <summary>
        /// Gets all components of this type in the game
        /// </summary>
        public IEnumerable<T> All
        {
            get { return elements.Values; }
        }

        /// <summary>
        /// Adds the supplied component to the specified entity
        /// </summary>
        /// <param name="entityID">The entity ID</param>
        /// <param name="component">The component</param>
        public void Add(uint entityID, T component)
        {
            elements.Add(entityID, component);
        }


        /// <summary>
        /// Removes the component from the specified entity
        /// </summary>
        /// <param name="entityID">The entity ID</param>
        public void Remove(uint entityID)
        {
            elements.Remove(entityID);
        }


        /// <summary>
        /// Returns if the specified entity has the component
        /// </summary>
        /// <param name="entityID">The entity ID</param>
        /// <returns>
        /// true if the entity possesses the component,
        /// false otherwise
        /// </returns>
        public bool Contains(uint entityID)
        {
            return elements.ContainsKey(entityID);
        }

        public virtual void HandleTrigger(uint entityID, string type)
        {
            throw new Exception("Handle Trigger is not implemented");
        }
        #endregion
    }
}
