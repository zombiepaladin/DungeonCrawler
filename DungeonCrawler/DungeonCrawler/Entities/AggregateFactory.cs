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
        public void CreateFromAggregate(Aggregate aggregate, PlayerIndex playerIndex)
        {
            uint entityID;
            Texture2D spriteSheet;
            Position position;
            Movement movement;
            MovementSprite movementSprite;
            Local local;
            Player player;
            HUDAggregateFactory hudagg = new HUDAggregateFactory(game);
            InvAggregateFactory invagg = new InvAggregateFactory(game);

            //I've defined these stats here more for convention really, this way
            //all changeable code is at the top of each aggregate and the 
            //implementation is afterwards. The wind fae is an example of how to 
            //implement them, just define and use them to construct the player var.
            int strength;
            int stamina;
            int agility;
            int intelligence;
            int defense;

            switch (aggregate)
            {
                case Aggregate.FairyPlayer:
                    entityID = Entity.NextEntity();
                    spriteSheet = game.Content.Load<Texture2D>("Spritesheets/wind_fae");
                    spriteSheet.Name = "Spritesheets/wind_fae";

                    //So here we just define our base values. Total sum is 50
                    //The Earthian shoudl probably have base stats of 10 across the board
                    strength = 4;
                    stamina = 13;
                    agility = 15;
                    intelligence = 10;
                    defense = 8;

                    position = new Position()
                    {
                        EntityID = entityID,
                        Center = new Vector2(400, 50),
                        Radius = 32f,
                    };
                    game.PositionComponent[entityID] = position;

                    movement = new Movement()
                    {
                        EntityID = entityID,
                        Direction = new Vector2(0, 1),
                        Speed = 200f,
                    };
                    game.MovementComponent[entityID] = movement;

                    movementSprite = new MovementSprite()
                    {
                        EntityID = entityID,
                        Facing = Facing.South,
                        SpriteSheet = spriteSheet,
                        SpriteBounds = new Rectangle(0, 0, 64, 64),
                        Timer = 0f,
                    };
                    game.MovementSpriteComponent[entityID] = movementSprite;

                    local = new Local()
                    {
                        EntityID = entityID,
                    };
                    game.LocalComponent[entityID] = local;

                    player = new Player()
                    {
                        EntityID = entityID,
                        PlayerIndex = playerIndex,
                        Strength = strength,
                        Stamina = stamina,
                        Agility = agility,
                        Intelligence = intelligence,
                        Defense = defense
                    };
                    game.PlayerComponent[entityID] = player;

                    break;

                case Aggregate.CultistPlayer:
                    entityID = Entity.NextEntity();
                    spriteSheet = game.Content.Load<Texture2D>("Spritesheets/Cultist");
                    spriteSheet.Name = "Spritesheets/Cultist";
                    
                    strength = 4;
                    stamina = 10;
                    agility = 10;
                    intelligence = 16;
                    defense = 10;

                    position = new Position()
                    {
                        EntityID = entityID,
                        Center = new Vector2(400, 50),
                        Radius = 32f,
                    };
                    game.PositionComponent[entityID] = position;
                    
                    movement = new Movement() {
                        EntityID = entityID,
                        Direction = new Vector2(0, 1),
                        Speed = 200f,
                    };
                    game.MovementComponent[entityID] = movement;
                    
                    movementSprite = new MovementSprite() {
                        EntityID = entityID,
                        Facing = Facing.South,
                        SpriteSheet = spriteSheet,
                        SpriteBounds = new Rectangle(0, 0, 64, 64),
                        Timer = 0f,
                    };
                    game.MovementSpriteComponent[entityID] = movementSprite;
                    
                    local = new Local(){
                        EntityID = entityID,
                    };
                    game.LocalComponent[entityID] = local;

                    player = new Player()
                    {
                        EntityID = entityID,
                        PlayerIndex = PlayerIndex.One,
                        Strength = strength,
                        Stamina = stamina,
                        Agility = agility,
                        Intelligence = intelligence,
                        Defense = defense
                    };
                    game.PlayerComponent[entityID] = player;
                    //Create HUD
                    hudagg.CreateHUD(player);
                    //create Inv
                    invagg.CreateInv(player);

                    break;

                case Aggregate.CyborgPlayer:
                    entityID = Entity.NextEntity();
                    spriteSheet = game.Content.Load<Texture2D>("Spritesheets/cyborg");
                    spriteSheet.Name = "Spritesheets/cyborg";

                    position = new Position()
                    {
                        EntityID = entityID,
                        Center = new Vector2(400, 50),
                        Radius = 32f,
                    };
                    game.PositionComponent[entityID] = position;

                    movement = new Movement()
                    {
                        EntityID = entityID,
                        Direction = new Vector2(0, 1),
                        Speed = 200f,
                    };
                    game.MovementComponent[entityID] = movement;

                    movementSprite = new MovementSprite()
                    {
                        EntityID = entityID,
                        Facing = Facing.South,
                        SpriteSheet = spriteSheet,
                        SpriteBounds = new Rectangle(0, 0, 64, 64),
                        Timer = 0f,
                    };
                    game.MovementSpriteComponent[entityID] = movementSprite;

                    local = new Local()
                    {
                        EntityID = entityID,
                    };
                    game.LocalComponent[entityID] = local;

                    player = new Player()
                    {
                        EntityID = entityID,
                        PlayerIndex = PlayerIndex.One,
                    };
                    game.PlayerComponent[entityID] = player;
                    //create HUD
                    hudagg.CreateHUD(player);
                    //create Inv
                    invagg.CreateInv(player);
                    break;

                case Aggregate.EarthianPlayer:
                    entityID = Entity.NextEntity();
                    spriteSheet = game.Content.Load<Texture2D>("Spritesheets/Earthian2x");
                    spriteSheet.Name = "Spritesheets/Earthian2x";
                    
                    position = new Position()
                    {
                        EntityID = entityID,
                        Center = new Vector2(400, 50),
                        Radius = 32f,
                    };
                    game.PositionComponent[entityID] = position;
                    
                    movement = new Movement() {
                        EntityID = entityID,
                        Direction = new Vector2(0, 1),
                        Speed = 200f,
                    };
                    game.MovementComponent[entityID] = movement;
                    
                    movementSprite = new MovementSprite() {
                        EntityID = entityID,
                        Facing = Facing.South,
                        SpriteSheet = spriteSheet,
                        SpriteBounds = new Rectangle(0, 0, 64, 64),
                        Timer = 0f,
                    };
                    game.MovementSpriteComponent[entityID] = movementSprite;
                    
                    local = new Local(){
                        EntityID = entityID,
                    };
                    game.LocalComponent[entityID] = local;

                    player = new Player()
                    {
                        EntityID = entityID,
                        PlayerIndex = playerIndex,
                    };
                    game.PlayerComponent[entityID] = player;
                    //Create HUD
                    hudagg.CreateHUD(player);
                    //create Inv
                    invagg.CreateInv(player);
                    break;

                case Aggregate.GargranianPlayer:
                    entityID = Entity.NextEntity();
                    spriteSheet = game.Content.Load<Texture2D>("Spritesheets/gargranian");
                    spriteSheet.Name = "Spritesheets/gargranian";

                    position = new Position()
                    {
                        EntityID = entityID,
                        Center = new Vector2(400, 50),
                        Radius = 32f,
                    };
                    game.PositionComponent[entityID] = position;

                    movement = new Movement()
                    {
                        EntityID = entityID,
                        Direction = new Vector2(0, 1),
                        Speed = 200f,
                    };
                    game.MovementComponent[entityID] = movement;

                    movementSprite = new MovementSprite()
                    {
                        EntityID = entityID,
                        Facing = Facing.South,
                        SpriteSheet = spriteSheet,
                        SpriteBounds = new Rectangle(0, 0, 64, 64),
                        Timer = 0f,
                    };
                    game.MovementSpriteComponent[entityID] = movementSprite;

                    local = new Local()
                    {
                        EntityID = entityID,
                    };
                    game.LocalComponent[entityID] = local;

                    player = new Player()
                    {
                        EntityID = entityID,
                        PlayerIndex = playerIndex,
                    };
                    game.PlayerComponent[entityID] = player;
                    //Create HUD
                    hudagg.CreateHUD(player);
                    //create Inv
                    invagg.CreateInv(player);
                    break;

                case Aggregate.SpacePiratePlayer:
                    entityID = Entity.NextEntity();
                    spriteSheet = game.Content.Load<Texture2D>("Spritesheets/SpacePBig");
                    spriteSheet.Name = "Spritesheets/SpacePBig";

                    position = new Position()
                    {
                        EntityID = entityID,
                        Center = new Vector2(400, 50),
                        Radius = 32f,
                    };
                    game.PositionComponent[entityID] = position;

                    movement = new Movement()
                    {
                        EntityID = entityID,
                        Direction = new Vector2(0, 1),
                        Speed = 200f,
                    };
                    game.MovementComponent[entityID] = movement;

                    movementSprite = new MovementSprite()
                    {
                        EntityID = entityID,
                        Facing = Facing.South,
                        SpriteSheet = spriteSheet,
                        SpriteBounds = new Rectangle(0, 0, 64, 64),
                        Timer = 0f,
                    };
                    game.MovementSpriteComponent[entityID] = movementSprite;

                    local = new Local()
                    {
                        EntityID = entityID,
                    };
                    game.LocalComponent[entityID] = local;

                    player = new Player()
                    {
                        EntityID = entityID,
                        PlayerIndex = PlayerIndex.One,
                    };
                    game.PlayerComponent[entityID] = player;
                    //Create HUD
                    hudagg.CreateHUD(player);
                    //create Inv
                    invagg.CreateInv(player);
                    break;

                case Aggregate.ZombiePlayer:
                    entityID = Entity.NextEntity();
                    spriteSheet = game.Content.Load<Texture2D>("Spritesheets/MzombieBx2");
                    spriteSheet.Name = "Spritesheets/MzombieBx2";

                    position = new Position()
                    {
                        EntityID = entityID,
                        Center = new Vector2(400, 50),
                        Radius = 32f,
                    };
                    game.PositionComponent[entityID] = position;

                    movement = new Movement()
                    {
                        EntityID = entityID,
                        Direction = new Vector2(0, 1),
                        Speed = 200f,
                    };
                    game.MovementComponent[entityID] = movement;

                    movementSprite = new MovementSprite()
                    {
                        EntityID = entityID,
                        Facing = Facing.South,
                        SpriteSheet = spriteSheet,
                        SpriteBounds = new Rectangle(0, 0, 64, 64),
                        Timer = 0f,
                    };
                    game.MovementSpriteComponent[entityID] = movementSprite;

                    local = new Local()
                    {
                        EntityID = entityID,
                    };
                    game.LocalComponent[entityID] = local;

                    player = new Player()
                    {
                        EntityID = entityID,
                        PlayerIndex = PlayerIndex.One,
                    };
                    game.PlayerComponent[entityID] = player;
                    //Create HUD
                    hudagg.CreateHUD(player);
                    //create Inv
                    invagg.CreateInv(player);
                    break;
            }
        }
    }
}
