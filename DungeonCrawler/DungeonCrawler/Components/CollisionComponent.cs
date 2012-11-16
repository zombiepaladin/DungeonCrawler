#region File Description
//-----------------------------------------------------------------------------
// CollisionComponent.cs 
//
// Author: Devin Kelly-Collins & Matthew McHaney
//
// Modified: Devin Kelly-Collins - Added RoomID to Collideable and InRoom method to CollisionComponent (11/15/12)
//
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence
//-----------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonCrawler.Components
{
    /// <summary>
    /// Represents an object that can be collided with in game.
    /// </summary>
    public struct Collideable
    {
        /// <summary>
        /// The ID of the entity.
        /// </summary>
        public uint EntityID;

        /// <summary>
        /// The id of the rom this collision is in.
        /// </summary>
        public uint RoomID;

        /// <summary>
        /// The bounds of this collision.
        /// </summary>
        public Bounds Bounds;
    }

    /// <summary>
    /// Manages the Collideable values in game.
    /// </summary>
    public class CollisionComponent : GameComponent<Collideable>
    {
        /// <summary>
        /// Returns all the collision in the given room.
        /// </summary>
        /// <param name="roomID">Id of the room.</param>
        /// <returns>A list of Collideable</returns>
        public List<Collideable> InRoom(uint roomID)
        {
            List<Collideable> collisions = new List<Collideable>();

            foreach (Collideable collision in this.All)
            {
                if (collision.RoomID == roomID)
                    collisions.Add(collision);
            }

            return collisions;
        }
    }
}
