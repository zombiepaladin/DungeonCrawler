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
