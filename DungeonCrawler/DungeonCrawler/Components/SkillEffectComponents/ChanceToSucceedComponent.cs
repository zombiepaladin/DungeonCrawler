#region File Description
//-----------------------------------------------------------------------------
// ChanceToSucceedComponent.cs 
//
// Author: Nicholas Boen
// 
// Modified: Nick Boen 11/30/2012 - Added a 
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
    public struct ChanceToSucceed
    {
        /// <summary>
        /// The entity ID of this component.
        /// WARNING: Do not use this as the id
        /// of other effect components, they should
        /// have their own. This ID will be sent to
        /// the GarbageMan system regardless of whether
        /// the chance was a success or not
        /// </summary>
        public uint EntityID;

        /// <summary>
        /// The success rate or 'chance' that
        /// all of the effects in the AffectedIDList
        /// will succeed
        /// </summary>
        public int SuccessRateAsPercentage;

        /// <summary>
        /// The list of effect ID's that this
        /// ChanceToSucceed component will affect,
        /// if the chance fails, these ID's will be
        /// removed from all effect component lists
        /// The id for this component will not need to
        /// be added, it will be cleared as well.
        /// WARNING: Do not use the entityID of this
        /// component to indicate a chance to succeed for
        /// other effect components, they should have their
        /// own ID's. This EntityID will be deleted regardless
        /// of whether the check for success was a failure or not
        /// </summary>
        public List<uint> AffectedIDList;
    }

    public class ChanceToSucceedComponent : GameComponent<ChanceToSucceed>
    {
    }
}
