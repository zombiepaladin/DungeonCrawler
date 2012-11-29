#region File Description
//-----------------------------------------------------------------------------
// AggregateFactory.cs 
//
// Author: 
//
// Modified: Devin Kelly-Collins, Factory methods return eid, 10/24/2012
// Modified: Adam Clark- added cyborg class and stats
// Modified by Samuel Fike and Jiri Malina: Removed use of MovementSprite and added code for SpriteAnimationComponent
// Modified by:Nick Boen
//      Added Stat values to each of the players as well as a stat component
// Modified: Devin Kelly-Collins - Added roomID to collisions (11/15/12)
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
        /// <param name="playerIndex">The player index for this player</param>
        /// <param name="aggregate">The game save that we are creating</param>
        public uint CreateFromAggregate(Aggregate aggregate, PlayerIndex playerIndex, string fileName, out DungeonCrawlerGame.CharacterSaveFile gameSave)
        {
            gameSave = new DungeonCrawlerGame.CharacterSaveFile();

            uint entityID = 0xFFFFFF;
            Texture2D spriteSheet;
            Position position;
            Movement movement;

            Sprite sprite;
            SpriteAnimation spriteAnimation;


            //MovementSprite movementSprite;
            Collideable collideable;
            Local local;
            Player player;
            PlayerInfo info;
            Stats stats = new Stats();

            HUDAggregateFactory hudagg = new HUDAggregateFactory(game);
            InvAggregateFactory invagg = new InvAggregateFactory(game);

            //Miscelaneous modifyers for the potential ability modifiers
            //Placeholders for racial/class bonuses and item bonuses.
            int miscMeleeAttack = 0;
            int miscRangedAttack = 0;
            int miscMeleeSpeed = 0;
            int miscAccuracy = 0;
            int miscMeleeDef = 0;
            int miscRangedDef = 0;
            int miscSpell = 0;
            int miscHealth = 0;

            switch (aggregate)
            {
                /****************************************
                 * Fairy
                 * *************************************/
                case Aggregate.FairyPlayer:
                    entityID = Entity.NextEntity();
                    spriteSheet = game.Content.Load<Texture2D>("Spritesheets/wind_fae");
                    spriteSheet.Name = "Spritesheets/wind_fae";

                    position = new Position()
                    {
                        EntityID = entityID,
                        Center = new Vector2(400, 150),
                        Radius = 32f,
                    };
                    game.PositionComponent[entityID] = position;

                    collideable = new Collideable()
                    {
                        EntityID = entityID,
                        RoomID = position.RoomID,
                        Bounds = new CircleBounds(position.Center, position.Radius)
                    };
                    game.CollisionComponent[entityID] = collideable;

                    movement = new Movement()
                    {
                        EntityID = entityID,
                        Direction = new Vector2(0, 1),
                        Speed = 200f,
                    };
                    game.MovementComponent[entityID] = movement;


                    /*
                    movementSprite = new MovementSprite()
                    {
                        EntityID = entityID,
                        Facing = Facing.South,
                        SpriteSheet = spriteSheet,
                        SpriteBounds = new Rectangle(0, 0, 64, 64),
                        Timer = 0f,
                    };
                    game.MovementSpriteComponent[entityID] = movementSprite;
                    */

                    spriteAnimation = new SpriteAnimation()
                    {
                        EntityID = entityID,
                        FramesPerSecond = 10,
                        IsLooping = true,
                        IsPlaying = true,
                        TimePassed = 0f,
                        CurrentFrame = 0,
                        CurrentAnimationRow = 0

                    };


                    game.SpriteAnimationComponent[entityID] = spriteAnimation;

                    sprite = new Sprite()
                    {
                        EntityID = entityID,
                        SpriteBounds = new Rectangle(0, 0, 64, 64),
                        SpriteSheet = spriteSheet
                    };
                    game.SpriteComponent[entityID] = sprite;

                    local = new Local()
                    {
                        EntityID = entityID,
                    };
                    game.LocalComponent[entityID] = local;

                    //This will add a stats section for the player in the stats component
                    stats = new Stats()
                    {
                        EntityID = entityID,

                        //So here we just define our base values. Total sum is 50
                        //The base stats are 10 across the board
                        Strength = 4,
                        Stamina = 10,
                        Agility = 10,
                        Intelligence = 16,
                        Defense = 10
                    };
                    game.StatsComponent[entityID] = stats;

                    player = new Player()
                    {
                        EntityID = entityID,
                        PlayerIndex = playerIndex,
                        PlayerRace = aggregate,
                        abilityModifiers = new AbilityModifiers()
                        {
                            meleeDamageReduction = miscMeleeDef + (int)((stats.Defense - 10) / 2),
                            rangedDamageReduction = miscRangedDef + (int)((stats.Defense - 10) / 2),
                            meleeAttackBonus = miscMeleeAttack + (int)((stats.Strength - 10) / 2),
                            RangedAttackBonus = miscRangedAttack + (int)((stats.Agility - 10) / 2),
                            MeleeAttackSpeed = miscMeleeSpeed + (int)((stats.Strength - 10) / 2),
                            Accuracy = miscAccuracy + (int)((stats.Agility - 10) / 2),
                            SpellBonus = miscSpell + (int)((stats.Intelligence - 10) / 2),
                            HealthBonus = miscHealth + (int)((stats.Stamina - 10) / 2),
                        }
                    };
                    game.PlayerComponent[entityID] = player;

                    info = new PlayerInfo()
                    {
                        Health = 100,
                        Psi = 100,
                        State = PlayerState.Default,
                    };
                    game.PlayerInfoComponent[entityID] = info;

                    break;

                /****************************************
                * Cultist
                * *************************************/
                case Aggregate.CultistPlayer:
                    entityID = Entity.NextEntity();
                    spriteSheet = game.Content.Load<Texture2D>("Spritesheets/Cultist");
                    spriteSheet.Name = "Spritesheets/Cultist";

                    position = new Position()
                    {
                        EntityID = entityID,
                        Center = new Vector2(400, 150),
                        Radius = 32f,
                    };
                    game.PositionComponent[entityID] = position;

                    collideable = new Collideable()
                    {
                        EntityID = entityID,
                        RoomID = position.RoomID,
                        Bounds = new CircleBounds(position.Center, position.Radius)
                    };
                    game.CollisionComponent[entityID] = collideable;

                    movement = new Movement()
                    {
                        EntityID = entityID,
                        Direction = new Vector2(0, 1),
                        Speed = 200f,
                    };
                    game.MovementComponent[entityID] = movement;

                    //movementSprite = new MovementSprite() {
                    //    EntityID = entityID,
                    //    Facing = Facing.South,
                    //    SpriteSheet = spriteSheet,
                    //    SpriteBounds = new Rectangle(0, 0, 64, 64),
                    //    Timer = 0f,
                    //};
                    //game.MovementSpriteComponent[entityID] = movementSprite;

                    spriteAnimation = new SpriteAnimation()
                    {
                        EntityID = entityID,
                        FramesPerSecond = 10,
                        IsLooping = true,
                        IsPlaying = true,
                        TimePassed = 0f,
                        CurrentFrame = 0,
                        CurrentAnimationRow = 0

                    };


                    game.SpriteAnimationComponent[entityID] = spriteAnimation;

                    sprite = new Sprite()
                    {
                        EntityID = entityID,
                        SpriteBounds = new Rectangle(0, 0, 64, 64),
                        SpriteSheet = spriteSheet
                    };
                    game.SpriteComponent[entityID] = sprite;




                    local = new Local()
                    {
                        EntityID = entityID,
                    };
                    game.LocalComponent[entityID] = local;

                    //This will add a stats section for the player in the stats component
                    stats = new Stats()
                    {
                        EntityID = entityID,

                        //So here we just define our base values. Total sum is 50
                        //The base stats are 10 across the board
                        Strength = 4,
                        Stamina = 10,
                        Agility = 10,
                        Intelligence = 16,
                        Defense = 10
                    };

                    player = new Player()
                    {
                        EntityID = entityID,
                        PlayerIndex = playerIndex,
                        PlayerRace = aggregate,
                        abilityModifiers = new AbilityModifiers()
                        {
                            meleeDamageReduction = miscMeleeDef + (int)((stats.Defense - 10) / 2),
                            rangedDamageReduction = miscRangedDef + (int)((stats.Defense - 10) / 2),
                            meleeAttackBonus = miscMeleeAttack + (int)((stats.Strength - 10) / 2),
                            RangedAttackBonus = miscRangedAttack + (int)((stats.Agility - 10) / 2),
                            MeleeAttackSpeed = miscMeleeSpeed + (int)((stats.Strength - 10) / 2),
                            Accuracy = miscAccuracy + (int)((stats.Agility - 10) / 2),
                            SpellBonus = miscSpell + (int)((stats.Intelligence - 10) / 2),
                            HealthBonus = miscHealth + (int)((stats.Stamina - 10) / 2),
                        }
                    };

                    info = new PlayerInfo()
                    {
                        Health = 100,
                        Psi = 100,
                        State = PlayerState.Default,
                    };
                    game.PlayerInfoComponent[entityID] = info;

                    game.PlayerComponent[entityID] = player;
                    //Create HUD
                    hudagg.CreateHUD(player);
                    //create Inv
                    invagg.CreateInv(player);

                    break;

                /****************************************
                * Cyborg - Added by adam Clark
                * *************************************/
                case Aggregate.CyborgPlayer:
                    entityID = Entity.NextEntity();
                    spriteSheet = game.Content.Load<Texture2D>("Spritesheets/cyborg");
                    spriteSheet.Name = "Spritesheets/cyborg";

                    position = new Position()
                    {
                        EntityID = entityID,
                        Center = new Vector2(400, 150),
                        Radius = 32f,
                    };
                    game.PositionComponent[entityID] = position;

                    collideable = new Collideable()
                    {
                        EntityID = entityID,
                        RoomID = position.RoomID,
                        Bounds = new CircleBounds(position.Center, position.Radius)
                    };
                    game.CollisionComponent[entityID] = collideable;

                    movement = new Movement()
                    {
                        EntityID = entityID,
                        Direction = new Vector2(0, 1),
                        Speed = 200f,
                    };
                    game.MovementComponent[entityID] = movement;

                    /*movementSprite = new MovementSprite()
                    {
                        EntityID = entityID,
                        Facing = Facing.South,
                        SpriteSheet = spriteSheet,
                        SpriteBounds = new Rectangle(0, 0, 64, 64),
                        Timer = 0f,
                    };
                    game.MovementSpriteComponent[entityID] = movementSprite;*/

                    spriteAnimation = new SpriteAnimation()
                    {
                        EntityID = entityID,
                        FramesPerSecond = 10,
                        IsLooping = true,
                        IsPlaying = true,
                        TimePassed = 0f,
                        CurrentFrame = 0,
                        CurrentAnimationRow = 0

                    };


                    game.SpriteAnimationComponent[entityID] = spriteAnimation;

                    sprite = new Sprite()
                    {
                        EntityID = entityID,
                        SpriteBounds = new Rectangle(0, 0, 64, 64),
                        SpriteSheet = spriteSheet
                    };
                    game.SpriteComponent[entityID] = sprite;

                    local = new Local()
                    {
                        EntityID = entityID,
                    };
                    game.LocalComponent[entityID] = local;

                    //This will add a stats section for the player in the stats component
                    stats = new Stats()
                    {
                        EntityID = entityID,

                        //So here we just define our base values. Total sum is 50
                        //The base stats are 10 across the board
                        Strength = 13,
                        Stamina = 12,
                        Agility = 13,
                        Intelligence = 0,
                        Defense = 12
                    };
                    game.StatsComponent[entityID] = stats;

                    player = new Player()
                    {
                        EntityID = entityID,
                        PlayerIndex = playerIndex,
                        PlayerRace = aggregate,

                    };

                    info = new PlayerInfo()
                    {
                        Health = 100,
                        Psi = 100,
                        State = PlayerState.Default,
                    };
                    game.PlayerInfoComponent[entityID] = info;

                    game.PlayerComponent[entityID] = player;
                    //create HUD
                    hudagg.CreateHUD(player);
                    //create Inv
                    invagg.CreateInv(player);
                    break;

                /*******************************************************************************
                * Earthian
                * Done by Andrew Bellinder. I added the character's sprite and his skill sprites
                * ******************************************************************************/
                case Aggregate.EarthianPlayer:
                    entityID = Entity.NextEntity();
                    spriteSheet = game.Content.Load<Texture2D>("Spritesheets/Earthian2x");
                    spriteSheet.Name = "Spritesheets/Earthian2x";

                    position = new Position()
                    {
                        EntityID = entityID,
                        Center = new Vector2(400, 150),
                        Radius = 32f,
                    };
                    game.PositionComponent[entityID] = position;

                    collideable = new Collideable()
                    {
                        EntityID = entityID,
                        RoomID = position.RoomID,
                        Bounds = new CircleBounds(position.Center, position.Radius)
                    };
                    game.CollisionComponent[entityID] = collideable;

                    movement = new Movement()
                    {
                        EntityID = entityID,
                        Direction = new Vector2(0, 1),
                        Speed = 200f,
                    };
                    game.MovementComponent[entityID] = movement;
                    /*
                    movementSprite = new MovementSprite() {
                        EntityID = entityID,
                        Facing = Facing.South,
                        SpriteSheet = spriteSheet,
                        SpriteBounds = new Rectangle(0, 0, 64, 64),
                        Timer = 0f,
                    };
                    game.MovementSpriteComponent[entityID] = movementSprite;
                    */

                    spriteAnimation = new SpriteAnimation()
                    {
                        EntityID = entityID,
                        FramesPerSecond = 10,
                        IsLooping = true,
                        IsPlaying = true,
                        TimePassed = 0f,
                        CurrentFrame = 0,
                        CurrentAnimationRow = 0

                    };


                    game.SpriteAnimationComponent[entityID] = spriteAnimation;

                    sprite = new Sprite()
                    {
                        EntityID = entityID,
                        SpriteBounds = new Rectangle(0, 0, 64, 64),
                        SpriteSheet = spriteSheet
                    };
                    game.SpriteComponent[entityID] = sprite;
                    local = new Local()
                    {
                        EntityID = entityID,
                    };
                    game.LocalComponent[entityID] = local;

                    //This will add a stats section for the player in the stats component
                    stats = new Stats()
                    {
                        EntityID = entityID,

                        //So here we just define our base values. Total sum is 50
                        //The base stats are 10 across the board
                        Strength = 10,
                        Stamina = 10,
                        Agility = 10,
                        Intelligence = 10,
                        Defense = 10
                    };
                    game.StatsComponent[entityID] = stats;

                    player = new Player()
                    {
                        EntityID = entityID,
                        PlayerIndex = playerIndex,
                        PlayerRace = aggregate,
                        abilityModifiers = new AbilityModifiers()
                        {
                            meleeDamageReduction = miscMeleeDef + (int)((stats.Defense - 10) / 2),
                            rangedDamageReduction = miscRangedDef + (int)((stats.Defense - 10) / 2),
                            meleeAttackBonus = miscMeleeAttack + (int)((stats.Strength - 10) / 2),
                            RangedAttackBonus = miscRangedAttack + (int)((stats.Agility - 10) / 2),
                            MeleeAttackSpeed = miscMeleeSpeed + (int)((stats.Strength - 10) / 2),
                            Accuracy = miscAccuracy + (int)((stats.Agility - 10) / 2),
                            SpellBonus = miscSpell + (int)((stats.Intelligence - 10) / 2),
                            HealthBonus = miscHealth + (int)((stats.Stamina - 10) / 2),
                        }

                    };
                    game.PlayerComponent[entityID] = player;

                    info = new PlayerInfo()
                    {
                        Health = 100,
                        Psi = 100,
                        State = PlayerState.Default,
                    };
                    game.PlayerInfoComponent[entityID] = info;

                    //Create HUD
                    hudagg.CreateHUD(player);
                    //create Inv
                    invagg.CreateInv(player);
                    break;

                /****************************************
                * Gargranian by Michael Fountain
                * *************************************/
                case Aggregate.GargranianPlayer:
                    entityID = Entity.NextEntity();
                    spriteSheet = game.Content.Load<Texture2D>("Spritesheets/gargranian");
                    spriteSheet.Name = "Spritesheets/gargranian";

                    position = new Position()
                    {
                        EntityID = entityID,
                        Center = new Vector2(400, 150),
                        Radius = 32f,
                    };
                    game.PositionComponent[entityID] = position;

                    collideable = new Collideable()
                    {
                        EntityID = entityID,
                        RoomID = position.RoomID,
                        Bounds = new CircleBounds(position.Center, position.Radius)
                    };
                    game.CollisionComponent[entityID] = collideable;

                    movement = new Movement()
                    {
                        EntityID = entityID,
                        Direction = new Vector2(0, 1),
                        Speed = 200f,
                    };
                    game.MovementComponent[entityID] = movement;
                    /*
                    movementSprite = new MovementSprite()
                    {
                        EntityID = entityID,
                        Facing = Facing.South,
                        SpriteSheet = spriteSheet,
                        SpriteBounds = new Rectangle(0, 0, 64, 64),
                        Timer = 0f,
                    };
                    game.MovementSpriteComponent[entityID] = movementSprite;
                    */

                    spriteAnimation = new SpriteAnimation()
                    {
                        EntityID = entityID,
                        FramesPerSecond = 10,
                        IsLooping = true,
                        IsPlaying = true,
                        TimePassed = 0f,
                        CurrentFrame = 0,
                        CurrentAnimationRow = 0

                    };


                    game.SpriteAnimationComponent[entityID] = spriteAnimation;

                    sprite = new Sprite()
                    {
                        EntityID = entityID,
                        SpriteBounds = new Rectangle(0, 0, 64, 64),
                        SpriteSheet = spriteSheet
                    };
                    game.SpriteComponent[entityID] = sprite;
                    local = new Local()
                    {
                        EntityID = entityID,
                    };
                    game.LocalComponent[entityID] = local;

                    //This will add a stats section for the player in the stats component
                    stats = new Stats()
                    {
                        EntityID = entityID,

                        //So here we just define our base values. Total sum is 50
                        //The base stats are 10 across the board
                        Strength = 4,
                        Stamina = 10,
                        Agility = 10,
                        Intelligence = 14,
                        Defense = 12
                    };
                    game.StatsComponent[entityID] = stats;

                    player = new Player()
                    {
                        EntityID = entityID,
                        PlayerIndex = playerIndex,
                        PlayerRace = aggregate,
                    };
                    
                    game.PlayerComponent[entityID] = player;

                    info = new PlayerInfo()
                    {
                        Health = 100,
                        Psi = 100,
                        State = PlayerState.Default,
                    };
                    game.PlayerInfoComponent[entityID] = info;

                    //Create HUD
                    hudagg.CreateHUD(player);
                    //create Inv
                    invagg.CreateInv(player);
                    break;

                /****************************************
                * Space Pirate
                * Done by Austin Murphy and I also have posted the 9 sprites for my skills that are listed in the design document.
                * *************************************/
                case Aggregate.SpacePiratePlayer:
                    entityID = Entity.NextEntity();
                    spriteSheet = game.Content.Load<Texture2D>("Spritesheets/SpacePBig");
                    spriteSheet.Name = "Spritesheets/SpacePBig";

                    position = new Position()
                    {
                        EntityID = entityID,
                        Center = new Vector2(400, 150),
                        Radius = 32f,
                    };
                    game.PositionComponent[entityID] = position;

                    collideable = new Collideable()
                    {
                        EntityID = entityID,
                        RoomID = position.RoomID,
                        Bounds = new CircleBounds(position.Center, position.Radius)
                    };
                    game.CollisionComponent[entityID] = collideable;

                    movement = new Movement()
                    {
                        EntityID = entityID,
                        Direction = new Vector2(0, 1),
                        Speed = 200f,
                    };
                    game.MovementComponent[entityID] = movement;
                    /*
                    movementSprite = new MovementSprite()
                    {
                        EntityID = entityID,
                        Facing = Facing.South,
                        SpriteSheet = spriteSheet,
                        SpriteBounds = new Rectangle(0, 0, 64, 64),
                        Timer = 0f,
                    };
                    game.MovementSpriteComponent[entityID] = movementSprite;
                    */

                    spriteAnimation = new SpriteAnimation()
                    {
                        EntityID = entityID,
                        FramesPerSecond = 10,
                        IsLooping = true,
                        IsPlaying = true,
                        TimePassed = 0f,
                        CurrentFrame = 0,
                        CurrentAnimationRow = 0

                    };


                    game.SpriteAnimationComponent[entityID] = spriteAnimation;

                    sprite = new Sprite()
                    {
                        EntityID = entityID,
                        SpriteBounds = new Rectangle(0, 0, 64, 64),
                        SpriteSheet = spriteSheet
                    };
                    game.SpriteComponent[entityID] = sprite;
                    local = new Local()
                    {
                        EntityID = entityID,
                    };
                    game.LocalComponent[entityID] = local;

                    //This will add a stats section for the player in the stats component
                    stats = new Stats()
                    {
                        EntityID = entityID,

                        //So here we just define our base values. Total sum is 50
                        //The base stats are 10 across the board
                        Strength = 5,
                        Stamina = 5,
                        Agility = 25,
                        Intelligence = 5,
                        Defense = 5
                    };
                    game.StatsComponent[entityID] = stats;

                    player = new Player()
                    {
                        EntityID = entityID,
                        PlayerIndex = playerIndex,
                        PlayerRace = aggregate,
                    };
                    game.PlayerComponent[entityID] = player;

                    info = new PlayerInfo()
                    {
                        Health = 100,
                        Psi = 100,
                        State = PlayerState.Default,
                    };
                    game.PlayerInfoComponent[entityID] = info;

                    //Create HUD
                    hudagg.CreateHUD(player);
                    //create Inv
                    invagg.CreateInv(player);
                    break;

                /****************************************
                * Zombie
                 * written by Matthew Hart
                * *************************************/
                case Aggregate.ZombiePlayer:
                    entityID = Entity.NextEntity();
                    spriteSheet = game.Content.Load<Texture2D>("Spritesheets/MzombieBx2");
                    spriteSheet.Name = "Spritesheets/MzombieBx2";

                    //Placeholder values
                    miscMeleeAttack = 5;
                    miscMeleeDef = 5;
                    miscRangedDef = -5;

                    position = new Position()
                    {
                        EntityID = entityID,
                        Center = new Vector2(400, 150),
                        Radius = 32f,
                    };

                    game.PositionComponent[entityID] = position;

                    collideable = new Collideable()
                    {
                        EntityID = entityID,
                        RoomID = position.RoomID,
                        Bounds = new CircleBounds(position.Center, position.Radius)
                    };
                    game.CollisionComponent[entityID] = collideable;

                    movement = new Movement()
                    {
                        EntityID = entityID,
                        Direction = new Vector2(0, 1),
                        Speed = 200f,
                    };
                    game.MovementComponent[entityID] = movement;
                    /*
                    movementSprite = new MovementSprite()
                    {
                        EntityID = entityID,
                        Facing = Facing.South,
                        SpriteSheet = spriteSheet,
                        SpriteBounds = new Rectangle(0, 0, 64, 64),
                        Timer = 0f,
                    };
                    game.MovementSpriteComponent[entityID] = movementSprite;
                    */

                    spriteAnimation = new SpriteAnimation()
                    {
                        EntityID = entityID,
                        FramesPerSecond = 10,
                        IsLooping = true,
                        IsPlaying = true,
                        TimePassed = 0f,
                        CurrentFrame = 0,
                        CurrentAnimationRow = 0

                    };


                    game.SpriteAnimationComponent[entityID] = spriteAnimation;

                    sprite = new Sprite()
                    {
                        EntityID = entityID,
                        SpriteBounds = new Rectangle(0, 0, 64, 64),
                        SpriteSheet = spriteSheet
                    };
                    game.SpriteComponent[entityID] = sprite;
                    local = new Local()
                    {
                        EntityID = entityID,
                    };
                    game.LocalComponent[entityID] = local;

                    //This will add a stats section for the player in the stats component
                    stats = new Stats()
                    {
                        EntityID = entityID,

                        //So here we just define our base values. Total sum is 50
                        //The base stats are 10 across the board
                        Strength = 16,
                        Stamina = 5,
                        Agility = 5,
                        Intelligence = 10,
                        Defense = 14
                    };
                    game.StatsComponent[entityID] = stats;

                    player = new Player()
                    {
                        EntityID = entityID,
                        PlayerIndex = playerIndex,
                        PlayerRace = aggregate,
                        abilityModifiers = new AbilityModifiers()
                        {
                            meleeDamageReduction = miscMeleeDef + (int)((stats.Defense - 10) / 2),
                            rangedDamageReduction = miscRangedDef + (int)((stats.Defense - 10) / 2),
                            meleeAttackBonus = miscMeleeAttack + (int)((stats.Strength - 10) / 2),
                            RangedAttackBonus = miscRangedAttack + (int)((stats.Agility - 10) / 2),
                            MeleeAttackSpeed = miscMeleeSpeed + (int)((stats.Strength - 10) / 2),
                            Accuracy = miscAccuracy + (int)((stats.Agility - 10) / 2),
                            SpellBonus = miscSpell + (int)((stats.Intelligence - 10) / 2),
                            HealthBonus = miscHealth + (int)((stats.Stamina - 10) / 2),
                        }
                    };

                    game.PlayerComponent[entityID] = player;

                    info = new PlayerInfo()
                    {
                        Health = 100,
                        Psi = 100,
                        State = PlayerState.Default,
                    };
                    game.PlayerInfoComponent[entityID] = info;

                    //Create HUD
                    hudagg.CreateHUD(player);
                    //create Inv
                    invagg.CreateInv(player);
                    break;

                default:
                    throw new Exception("Unknown type.");
            }

            // Store all of the data into the game save
            gameSave.aggregate = (int)aggregate;
            gameSave.health = 100;
            gameSave.psi = 100;
            gameSave.stats = stats;
            gameSave.Level = 1;
            gameSave.charAnimation = spriteSheet.Name;
            gameSave.fileName = fileName;
            info.FileName = fileName;

            game.QuestLogSystem.ActivateQuest(entityID, 0);

            return entityID;
        }

        /// <summary>
        /// Creates Entities from aggregates (collections of components)
        /// </summary>
        /// <param name="aggregate">The specific aggreage to create</param>
        public uint CreateFromGameSave(DungeonCrawlerGame.CharacterSaveFile gameSave, PlayerIndex playerIndex)
        {
            uint entityID = 0xFFFFFF;
            Texture2D spriteSheet;
            Position position;
            Movement movement;

            Sprite sprite;
            SpriteAnimation spriteAnimation;


            //MovementSprite movementSprite;
            Collideable collideable;
            Local local;
            Player player;
            PlayerInfo info;
            Stats stats = new Stats();

            HUDAggregateFactory hudagg = new HUDAggregateFactory(game);
            InvAggregateFactory invagg = new InvAggregateFactory(game);

            //Miscelaneous modifyers for the potential ability modifiers
            //Placeholders for racial/class bonuses and item bonuses.
            int miscMeleeAttack = 0;
            int miscRangedAttack = 0;
            int miscMeleeSpeed = 0;
            int miscAccuracy = 0;
            int miscMeleeDef = 0;
            int miscRangedDef = 0;
            int miscSpell = 0;
            int miscHealth = 0;

            // Create the player
            {
                entityID = Entity.NextEntity();
                spriteSheet = game.Content.Load<Texture2D>(gameSave.charAnimation);
                spriteSheet.Name = gameSave.charAnimation;

                position = new Position()
                {
                    EntityID = entityID,
                    Center = new Vector2(400, 150),
                    Radius = 32f,
                };
                game.PositionComponent[entityID] = position;

                collideable = new Collideable()
                {
                    EntityID = entityID,
                    RoomID = position.RoomID,
                    Bounds = new CircleBounds(position.Center, position.Radius)
                };
                game.CollisionComponent[entityID] = collideable;

                movement = new Movement()
                {
                    EntityID = entityID,
                    Direction = new Vector2(0, 1),
                    Speed = 200f,
                };
                game.MovementComponent[entityID] = movement;

                spriteAnimation = new SpriteAnimation()
                {
                    EntityID = entityID,
                    FramesPerSecond = 10,
                    IsLooping = true,
                    IsPlaying = true,
                    TimePassed = 0f,
                    CurrentFrame = 0,
                    CurrentAnimationRow = 0

                };

                game.SpriteAnimationComponent[entityID] = spriteAnimation;

                sprite = new Sprite()
                {
                    EntityID = entityID,
                    SpriteBounds = new Rectangle(0, 0, 64, 64),
                    SpriteSheet = spriteSheet
                };
                game.SpriteComponent[entityID] = sprite;

                local = new Local()
                {
                    EntityID = entityID,
                };
                game.LocalComponent[entityID] = local;

                //This will add a stats section for the player in the stats component
                stats = new Stats()
                {
                    EntityID = entityID,

                    // Load from game save
                    Strength = gameSave.stats.Strength,
                    Stamina = gameSave.stats.Stamina,
                    Agility = gameSave.stats.Agility,
                    Intelligence = gameSave.stats.Intelligence,
                    Defense = gameSave.stats.Defense
                };
                game.StatsComponent[entityID] = stats;

                player = new Player()
                {
                    EntityID = entityID,
                    PlayerIndex = playerIndex,
                    PlayerRace = (Aggregate)gameSave.aggregate,
                    abilityModifiers = new AbilityModifiers()
                    {
                        meleeDamageReduction = miscMeleeDef + (int)((stats.Defense - 10) / 2),
                        rangedDamageReduction = miscRangedDef + (int)((stats.Defense - 10) / 2),
                        meleeAttackBonus = miscMeleeAttack + (int)((stats.Strength - 10) / 2),
                        RangedAttackBonus = miscRangedAttack + (int)((stats.Agility - 10) / 2),
                        MeleeAttackSpeed = miscMeleeSpeed + (int)((stats.Strength - 10) / 2),
                        Accuracy = miscAccuracy + (int)((stats.Agility - 10) / 2),
                        SpellBonus = miscSpell + (int)((stats.Intelligence - 10) / 2),
                        HealthBonus = miscHealth + (int)((stats.Stamina - 10) / 2),
                    }
                };
                game.PlayerComponent[entityID] = player;

                info = new PlayerInfo()
                {
                    Health = 100,
                    Psi = 100,
                    State = PlayerState.Default,
                };
                game.PlayerInfoComponent[entityID] = info;

                game.PlayerComponent[entityID] = player;
                //Create HUD
                hudagg.CreateHUD(player);
                //create Inv
                invagg.CreateInv(player);
            }

            game.QuestLogSystem.ActivateQuest(entityID, 0);

            return entityID;
        }
    }
}
