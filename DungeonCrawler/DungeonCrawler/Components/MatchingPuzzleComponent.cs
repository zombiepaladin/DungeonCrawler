#region File Description
//-----------------------------------------------------------------------------
// DoorComponent.cs 
//
// Author: Adam Steen
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
    /// A struct representing a Matching Puzzle Component
    /// </summary>
    public struct MatchingPuzzlePiece
    {
        /// <summary>
        /// The ID of the entity this sprite belongs to
        /// </summary>
        public uint EntityID;

        /// <summary>
        /// The color assigned to the piece.
        /// </summary>
        public int Color;

        /// <summary>
        /// Boolean descibing if the puzzles have been matched or not.
        /// </summary>
        public bool Matched;


    }

    public class MatchingPuzzleComponent : GameComponent<MatchingPuzzlePiece>
    {
        public override void HandleTrigger(uint entityID, string type)
        {
            #region BinaryTreeArrayImplementation
            //MatchingPuzzlePiece MPP = this[entityID];
            #endregion

            #region DictionaryImplementation
            MatchingPuzzlePiece MPP = elements[entityID];
            #endregion


            switch (type)
            {
                case "Solved":
                    MPP.Matched = true;
                    break;
                case "Unsolved":
                    MPP.Matched = false;
                    break;
            }
        }

    }

}
