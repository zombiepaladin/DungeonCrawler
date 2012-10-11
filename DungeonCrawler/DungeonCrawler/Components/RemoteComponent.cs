#region File Description
//-----------------------------------------------------------------------------
// LocalComponent.cs 
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
    /// A structure indicating the remote nature of an entity
    /// </summary>
    public struct Remote
    {
        /// <summary>
        /// The entity this Local component belongs to
        /// </summary>
        public uint EntityID;

        public byte RemoteGamerID;

        public uint RemoteEntityID;
    }

    /// <summary>
    /// The remote components for all entities in a game world
    /// TODO: Provide an optimized way for finding remote entities
    /// by thier remote gamer ID & remote entity ID
    /// </summary>
    public class RemoteComponent : GameComponent<Remote>
    {
        /// <summary>
        /// Finds the local entity ID for the remote entity, identified by
        /// the remote gamer id and remote entity id
        /// </summary>
        /// <param name="remoteGamerID">The ID of the remote gamer with authority over the entity</param>
        /// <param name="remoteEntityID">The ID of the remote entity</param>
        /// <returns>The local entity ID</returns>
        public uint FindRemoteEntity(byte remoteGamerID, uint remoteEntityID)
        {
            foreach (Remote remote in elements.Values)
            {
                if (remote.RemoteGamerID == remoteGamerID && remote.RemoteEntityID == remoteEntityID)
                    return remote.EntityID;
            }

            // If the remote entity was not found, create one
            uint entityID = Entity.NextEntity();
            Remote newRemote = new Remote()
            {
                EntityID = entityID,
                RemoteGamerID = remoteGamerID,
                RemoteEntityID = remoteEntityID,
            };
            Add(entityID, newRemote);

            return entityID;
        }
    }
}
