using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DungeonCrawler.Components;

namespace DungeonCrawler.Entities
{
    /// <summary>
    /// A class for quickly creating entities from pre-defined collections
    /// of components
    /// </summary>
    public class AggregateFactory
    {
        /// <summary>
        /// The game this AggregateFactory belongs to
        /// </summary>
        DungeonCrawlerGame game;

        /// <summary>
        /// Creates a new AggregateFactory instance
        /// </summary>
        /// <param name="game"></param>
        public AggregateFactory(DungeonCrawlerGame game)
        {
            this.game = game;
        }

        /// <summary>
        /// Creates Entities from aggregates (collections of components)
        /// </summary>
        /// <param name="aggregate">The specific aggreage to create</param>
        public void CreateFromAggregate(Aggregate aggregate)
        {
            switch (aggregate)
            {
                case Aggregate.FairyPlayer:
                    uint entityID = Entity.NextEntity();
                    Texture2D spriteSheet = game.Content.Load<Texture2D>("Spritesheets/wind_fae");
                    spriteSheet.Name = "Spritesheets/wind_fae";
                    
                    Position position = new Position()
                    {
                        EntityID = entityID,
                        Center = new Vector2(400, 50),
                        Radius = 32f,
                    };
                    game.PositionComponent[entityID] = position;
                    
                    Movement movement = new Movement() {
                        EntityID = entityID,
                        Direction = new Vector2(0, 1),
                        Speed = 200f,
                    };
                    game.MovementComponent[entityID] = movement;
                    
                    MovementSprite movementSprite = new MovementSprite() {
                        EntityID = entityID,
                        Facing = Facing.South,
                        SpriteSheet = spriteSheet,
                        SpriteBounds = new Rectangle(0, 0, 64, 64),
                        Timer = 0f,
                    };
                    game.MovementSpriteComponent[entityID] = movementSprite;
                    
                    Local local = new Local(){
                        EntityID = entityID,
                    };
                    game.LocalComponent[entityID] = local;

                    Player player = new Player()
                    {
                        EntityID = entityID,
                        PlayerIndex = PlayerIndex.One,
                    };
                    game.PlayerComponent[entityID] = player;
                    
                    break;

                case Aggregate.CultistPlayer:
                    break;

                case Aggregate.CyborgPlayer:
                    break;

                case Aggregate.EarthianPlayer:
                    break;

                case Aggregate.GargranianPlayer:
                    break;

                case Aggregate.SpacePiratePlayer:
                    break;

                case Aggregate.ZombiePlayer:
                    break;
            }
        }
    }
}
