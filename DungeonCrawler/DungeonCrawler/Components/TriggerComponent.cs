//-----------------------------------------------------------------------------
// TriggerComponent.cs
//
// Author: Samuel Fike and Jiri Malina
//
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence 
//-----------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonCrawler.Components
{
    public struct Trigger
    {
        /// <summary>
        /// The ID of the entity this sprite belongs to
        /// </summary>
        public uint EntityID;

        /// <summary>
        /// The room-based id of the target.
        /// Look up the target entityID using the idMap in the current RoomComponent.
        /// </summary>
        public string TargetID;

        /// <summary>
        /// The name of the trigger. Defined in the map editor and in the Component's HandleTrigger method.
        /// An example is DoorComponent, which has "Lock", "Unlock", etc.
        /// </summary>
        public string TriggerType;
    }

    public class TriggerComponent : GameComponent<Trigger>
    {
        public void Trigger(uint entityID)
        {
            Trigger trigger = elements[entityID];

            Room room = DungeonCrawlerGame.LevelManager.getCurrentRoom();

            uint targetEntityID = room.idMap[trigger.TargetID];

            switch (room.targetTypeMap[trigger.TargetID])
            {
                case "Door":
                    DungeonCrawlerGame.game.DoorComponent.HandleTrigger(targetEntityID, trigger.TriggerType);
                    break;
            }
        }

    }
}
