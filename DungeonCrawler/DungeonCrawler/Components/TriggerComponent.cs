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
        /// The type of action to perform
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
