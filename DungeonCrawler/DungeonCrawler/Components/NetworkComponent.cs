#region File Description
//-----------------------------------------------------------------------------
// NetworkComponent.cs 
//
// Author: Nathan Bean
//
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence 
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System.Collections.Generic;
using Microsoft.Xna.Framework;
#endregion

namespace DungeonCrawler.Components
{
    /// <summary>
    /// A structure indicating the network user owning this variable
    /// </summary>
    public struct Network
    {
        /// <summary>
        /// The entity this Network component belongs to
        /// </summary>
        public uint EntityID;

        /// <summary>
        /// The remote entity this Network component belongs to
        /// </summary>
        public uint RemoteEntityID;

        /// <summary>
        /// The unique identifier of the remote system this Network 
        /// component belongs to - with this and the RemoteEntityID,
        /// we can uniquely identify all remote network elements
        /// </summary>
        public byte OwnerID;

        /// <summary>
        /// A flag indicating if this is a local or remote entity
        /// </summary>
        public bool IsLocal;
    }

    /// <summary>
    /// The network components for all entities in a game world
    /// </summary>
    public class NetworkComponent : GameComponent<Network>
    {
        #region Private and Protected Members
        
        /// <summary>
        /// A secondary data structure to hold local network objects
        /// </summary>
        private List<Network> locals;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new network component
        /// </summary>
        public NetworkComponent()
            : base()
        {
            locals = new List<Network>();
        }

        /// <summary>
        /// Constructs a new network component
        /// </summary>
        /// <param name="capacity">The expected number of network entities</param>
        public NetworkComponent(int capacity) : base(capacity)
        {
            locals = new List<Network>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// All local objects
        /// </summary>
        public IEnumerable<Network> Local
        {
            get { return locals; }
        }

        /// <summary>
        /// Adds a network component to an entity
        /// </summary>
        /// <param name="entityID">The entity to add this component to</param>
        /// <param name="component">The component to add</param>
        public void Add(uint entityID, Network component)
        {
            if(component.IsLocal) locals.Add(component);
            base.Add(entityID, component);
        }

        /// <summary>
        /// Removes a network component from an entity
        /// </summary>
        /// <param name="entityID">The entity to remove the component from</param>
        public void Remove(uint entityID)
        {
            if(elements[entityID].IsLocal) locals.Remove(elements[entityID]);
            base.Remove(entityID);
        }

        #endregion
    }
}
