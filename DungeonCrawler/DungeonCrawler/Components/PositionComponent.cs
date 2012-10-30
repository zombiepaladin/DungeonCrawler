#region File Description
//-----------------------------------------------------------------------------
// PositionComponent.cs 
//
// Author: Nathan Bean
//
// Modified: Matthew McHaney added Colliedable attribute.
//
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence 
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
    /// A struture representing the position of a single entity
    /// in the game world
    /// </summary>
    public struct Position
    {
        /// <summary>
        /// The ID of the Entity this position belongs to
        /// </summary>
        public uint EntityID;

        /// <summary>
        /// The center of the entity in the game world
        /// </summary>
        public Vector2 Center;

        /// <summary>
        /// The radius of the entity's position in the game world
        /// </summary>
        public float Radius;

        /// <summary>
        /// If the entity can collide with other entities
        /// </summary>
        public bool Collideable;

        /// <summary>
        /// Identifies what room the object is in.
        /// </summary>
        public uint RoomID;
    }

    /// <summary>
    /// The PositionComponents for all entities in a game world
    /// TODO: Provide an optimized spatial representation
    /// </summary>
    public class PositionComponent : GameComponent<Position>
    {
        #region Public Methods

        /// <summary>
        /// Returns all Position components within the specfied circular region
        /// </summary>
        /// <param name="center">The center of the region</param>
        /// <param name="radius">The radius of the region</param>
        /// <returns>The Position components found in the region</returns>
        public IEnumerable<Position> InRegion(Vector2 center, float radius)
        {
            List<Position> results = new List<Position>();

            foreach (Position position in arrayList)
            {
                if (Vector2.DistanceSquared(center, position.Center) < Math.Pow(radius + position.Radius, 2))
                {
                    results.Add(position);
                }
            }

            return results;
        }

        /// <summary>
        /// Returns Position components within a certian room.
        /// </summary>
        /// <param name="roomID"></param>
        /// <returns></returns>
        public IEnumerable<Position> InRoom(uint roomID)
        {
            List<Position> returnList = new List<Position>();
            foreach (Position position in arrayList)
            {
                if (position.RoomID == roomID)
                {
                    returnList.Add(position);
                }
            }
            return returnList;
        }

        #endregion
    }

    public static class PositionExtensions
    {
        /// <summary>
        /// Returns all Positon components withing the specfied circular region. This method will allow us ot further sort data we already have.
        /// </summary>
        /// <param name="elements">The components to query</param>
        /// <param name="center">The center of the region</param>
        /// <param name="radius">The redius of the region</param>
        /// <returns>The position within the region.</returns>
        public static IEnumerable<Position> InRegion(this IEnumerable<Position> elements, Vector2 center, float radius)
        {
            List<Position> results = new List<Position>();

            foreach (Position position in elements)
            {
                if (Vector2.DistanceSquared(center, position.Center) < Math.Pow(radius + position.Radius, 2))
                {
                    results.Add(position);
                }
            }

            return results;
        }
    }
}
