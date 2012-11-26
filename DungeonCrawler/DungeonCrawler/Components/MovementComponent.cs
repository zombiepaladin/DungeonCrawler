#region File Description
//-----------------------------------------------------------------------------
// MovementComponent.cs 
//
// Author: Nathan Bean
//
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence 
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using System;
#endregion

namespace DungeonCrawler.Components
{
    public enum FacingDirection
    {
        South = 0,
        West = 1,
        East = 2,
        North = 3,
    }

    /// <summary>
    /// A struture representing the motion of a single entity
    /// in the game world
    /// </summary>
    public struct Movement
    {
        /// <summary>
        /// The ID of the Entity this position belongs to
        /// </summary>
        public uint EntityID;

        /// <summary>
        /// The direction of the entity's movement in the game world
        /// (Should be normalized)
        /// </summary>
        public Vector2 Direction;

        /// <summary>
        /// The speed of the entity's movement in the game world
        /// </summary>
        public float Speed;
    }

    /// <summary>
    /// The movement components for all entities in a game world
    /// </summary>
    public class MovementComponent : GameComponent<Movement>
    {
        public FacingDirection GetFacingFromDirection(uint id)
        {
            Vector2 direction = elements[id].Direction;

            if (direction.X > 0 && direction.X > Math.Abs(direction.Y))
                return FacingDirection.East;
            else if (direction.X < 0 && Math.Abs(direction.X) > Math.Abs(direction.Y))
                return FacingDirection.West;
            else if (direction.Y < 0)
                return FacingDirection.North;

            return FacingDirection.South;
        }
    }
}
