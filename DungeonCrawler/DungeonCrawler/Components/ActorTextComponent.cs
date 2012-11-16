#region File Description
//-----------------------------------------------------------------------------
// ActorTextComponent.cs 
//
// Author: Devin Kelly-Collins
//
// Modified: Nick Stanley added HUDSpriteComponent, 10/15/2012
// Modified: Devin Kelly-Collins added WeaponSprite rendering, 10/24/2012
// Modified: Samuel Fike and Jiri Malina: Fixed errors due to removal of movementSprite for players
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
    public struct ActorText
    {
        public uint EntityID;

        public string Text;

        public int Offset;
    }

    public class ActorTextComponent : GameComponent<ActorText>
    {
        /// <summary>
        /// Handles added ActorText to the component list.
        /// If an ActorText with the entityID is already in the list, that value will be overwritten.
        /// </summary>
        /// <param name="entityID"></param>
        /// <param name="actorText"></param>
        new public void Add(uint entityID, ActorText actorText)
        {
            if(this.Contains(entityID))
                this[entityID] = actorText;
            else
                base.Add(entityID, actorText);
        }

        public void Add(uint entityID, string text)
        {
            ActorText actorText = new ActorText()
            {
                EntityID = entityID,
                Text = text,
                Offset = 0,
            };
            this.Add(entityID, actorText);
        }
    }
}
