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
// Modified: Devin Kelly-Collins - Replaced HUDComponent with HUDSystem. (11/29/12)
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
            Equipment equipment;
            WeaponType weaponType;

            PlayerSkillInfo skillInfo;
            ActiveSkill active_Skill;
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

            int defaultHealthPotQty = 1;
            int defaultPsiPotQty = 1;

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
                #region Fairy
                /****************************************
                 * Fairy
                 * *************************************/
                case Aggregate.FairyPlayer:
                    entityID = Entity.NextEntity();
                    spriteSheet = game.Content.Load<Texture2D>("Spritesheets/Aggregate/wind_fae");
                    spriteSheet.Name = "Spritesheets/Aggregate/wind_fae";

                    /*Author: Josh Zavala, Assignment 9
                     *This has been transferred from ContinueNewGameScreen.goToNetworking
                     *Allows default weapons assignment per class
                     */
                    /*Update:Joseph Shaw, Assignment 9
                     *Set the weapon type here and use it in the create weapon so that it 
                     *can be saved into the gameSave after the switch statement.
                     *We could abstract this method entirely but leaving it here gives more flexibility.
                     */
                    weaponType = WeaponType.StandardSword;
                    equipment = new Equipment()
                    {
                        EntityID = entityID,
                        WeaponID = game.WeaponFactory.CreateWeapon(weaponType),
                        HealthPotsQty = defaultHealthPotQty,
                        PsiPotsQty = defaultPsiPotQty,
                        PogsQty = 0,
                    };
                    game.EquipmentComponent.Add(equipment.EntityID, equipment);

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

                        //So here we just define our base values. Total sum is 50
                        //The base stats are 10 across the board
                        Strength = 4,
                        Stamina = 10,
                        Agility = 10,
                        Intelligence = 16,
                        Defense = 10,
                        HealthBase = 100,
                        PsiBase = 100,
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
                        PsiOrFatige = 100,
                        Level = 1,
                        Experience = 0,
                        State = PlayerState.Default,
                    };
                    game.PlayerInfoComponent[entityID] = info;

                    break;
                #endregion

                #region Cultist
                /****************************************
                * Cultist
                * *************************************/
                case Aggregate.CultistPlayer:
                    entityID = Entity.NextEntity();
                    spriteSheet = game.Content.Load<Texture2D>("Spritesheets/Aggregate/Cultist");
                    spriteSheet.Name = "Spritesheets/Aggregate/Cultist";

                    /*Author: Josh Zavala, Assignment 9
                     *This has been transferred from ContinueNewGameScreen.goToNetworking
                     *Allows default weapons assignment per class
                     */
                    /*Update:Joseph Shaw, Assignment 9
                     *Set the weapon type here and use it in the create weapon so that it 
                     *can be saved into the gameSave after the switch statement.
                     *We could abstract this method entirely but leaving it here gives more flexibility.
                     */
                    weaponType = WeaponType.StandardSword;
                    equipment = new Equipment()
                    {
                        EntityID = entityID,
                        WeaponID = game.WeaponFactory.CreateWeapon(weaponType),
                        HealthPotsQty = defaultHealthPotQty,
                        PsiPotsQty = defaultPsiPotQty,
                        PogsQty = 0,
                    };
                    game.EquipmentComponent.Add(equipment.EntityID, equipment);

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

                        //So here we just define our base values. Total sum is 50
                        //The base stats are 10 across the board
                        Strength = 4,
                        Stamina = 10,
                        Agility = 10,
                        Intelligence = 16,
                        Defense = 10,
                        HealthBase = 100,
                        PsiBase = 100,
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

                    info = new PlayerInfo()
                    {
                        Health = 100,
                        PsiOrFatige = 100,
                        Level = 1,
                        Experience = 0,
                        State = PlayerState.Default,
                        skill1 = Systems.SkillType.Enslave,
                        skill2 = Systems.SkillType.Fear,
                        skill3 = Systems.SkillType.Sacrifice,
                        skill4 = Systems.SkillType.PsionicSpear,
                        skill5 = Systems.SkillType.Taint,
                        skill6 = Systems.SkillType.Rot,
                        skill7 = Systems.SkillType.Push,
                        skill8 = Systems.SkillType.Lightning,
                        skill9 = Systems.SkillType.Malice,
                    };
                    game.PlayerInfoComponent[entityID] = info;
                    
                    skillInfo = new PlayerSkillInfo()
                    {
                        Skill1Rank = 1,
                        Skill2Rank = 1,
                        Skill3Rank = 1,
                        Skill4Rank = 1,
                        Skill5Rank = 1,
                        Skill6Rank = 1,
                        Skill7Rank = 1,
                        Skill8Rank = 1,
                        Skill9Rank = 1,
                    };
                    game.PlayerSkillInfoComponent[entityID] = skillInfo;

                    active_Skill = new ActiveSkill()
                    {
                        activeSkill = info.skill8,
                    };
                    game.ActiveSkillComponent[entityID] = active_Skill;
                    game.PlayerComponent[entityID] = player;
                    //Create HUD
                    game.HUDSystem.LoadPlayerHUD(player);
                    //create Inv
                    invagg.CreateInv(player);

                    break;
                #endregion

                #region Cyborg
                /****************************************
                * Cyborg - Added by adam Clark
                * *************************************/
                case Aggregate.CyborgPlayer:
                    entityID = Entity.NextEntity();
                    spriteSheet = game.Content.Load<Texture2D>("Spritesheets/Aggregate/cyborg");
                    spriteSheet.Name = "Spritesheets/Aggregate/cyborg";

                    /*Author: Josh Zavala, Assignment 9
                     *This has been transferred from ContinueNewGameScreen.goToNetworking
                     *Allows default weapons assignment per class
                     */
                    /*Update:Joseph Shaw, Assignment 9
                     *Set the weapon type here and use it in the create weapon so that it 
                     *can be saved into the gameSave after the switch statement.
                     *We could abstract this method entirely but leaving it here gives more flexibility.
                     */
                    weaponType = WeaponType.ShockRod;
                    equipment = new Equipment()
                    {
                        EntityID = entityID,
                        WeaponID = game.WeaponFactory.CreateWeapon(weaponType),
                        HealthPotsQty = defaultHealthPotQty,
                        PsiPotsQty = defaultPsiPotQty,
                        PogsQty = 0,
                    };
                    game.EquipmentComponent.Add(equipment.EntityID, equipment);

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

                        //So here we just define our base values. Total sum is 50
                        //The base stats are 10 across the board
                        Strength = 13,
                        Stamina = 12,
                        Agility = 13,
                        Intelligence = 0,
                        Defense = 12,
                        HealthBase = 100,
                        PsiBase = 100,
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
                        PsiOrFatige = 100,
                        Level = 1,
                        Experience = 0,
                        State = PlayerState.Default,
                        skill1= Systems.SkillType.EnergyShield,
                        skill2 = Systems.SkillType.Defibrillate,
                        skill3 = Systems.SkillType.Nanobots,
                        skill4 = Systems.SkillType.TargettingUpgrade,
                        skill5 = Systems.SkillType.RepulsorArm,
                        skill6 = Systems.SkillType.EnergyShield,
                        skill7= Systems.SkillType.AlloyBody,
                        skill8 = Systems.SkillType.CyberneticSlam,
                        skill9= Systems.SkillType.ThrusterRush,
                    };
                    game.PlayerInfoComponent[entityID] = info;

                    skillInfo = new PlayerSkillInfo()
                    {
                        Skill1Rank = 1,
                        Skill2Rank = 1,
                        Skill3Rank = 1,
                        Skill4Rank = 1,
                        Skill5Rank = 1,
                        Skill6Rank = 1,
                        Skill7Rank = 1,
                        Skill8Rank = 1,
                        Skill9Rank = 1,
                    };
                    game.PlayerSkillInfoComponent[entityID] = skillInfo;

                    active_Skill = new ActiveSkill()
                    {
                        activeSkill = info.skill1,
                    };
                    game.ActiveSkillComponent[entityID] = active_Skill;

                    game.PlayerComponent[entityID] = player;
                    //create HUD
                    game.HUDSystem.LoadPlayerHUD(player);
                    //create Inv
                    invagg.CreateInv(player);
                    break;
                #endregion

                #region Earthian
                /*******************************************************************************
                * Earthian
                * Done by Andrew Bellinder. I added the character's sprite and his skill sprites
                * ******************************************************************************/
                case Aggregate.EarthianPlayer:
                    entityID = Entity.NextEntity();
                    spriteSheet = game.Content.Load<Texture2D>("Spritesheets/Aggregate/Earthian2x");
                    spriteSheet.Name = "Spritesheets/Aggregate/Earthian2x";

                    /*Author: Josh Zavala, Assignment 9
                     *This has been transferred from ContinueNewGameScreen.goToNetworking
                     *Allows default weapons assignment per class
                     */
                    /*Update:Joseph Shaw, Assignment 9
                     *Set the weapon type here and use it in the create weapon so that it 
                     *can be saved into the gameSave after the switch statement.
                     *We could abstract this method entirely but leaving it here gives more flexibility.
                     */
                    weaponType = WeaponType.TreeBranch;
                    equipment = new Equipment()
                    {
                        EntityID = entityID,
                        WeaponID = game.WeaponFactory.CreateWeapon(weaponType),
                        HealthPotsQty = defaultHealthPotQty,
                        PsiPotsQty = defaultPsiPotQty,
                        PogsQty = 0,
                    };
                    game.EquipmentComponent.Add(equipment.EntityID, equipment);

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

                        //So here we just define our base values. Total sum is 50
                        //The base stats are 10 across the board
                        Strength = 10,
                        Stamina = 10,
                        Agility = 10,
                        Intelligence = 10,
                        Defense = 10,
                        HealthBase = 100,
                        PsiBase = 100,
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
                        PsiOrFatige = 100,
                        Level = 1,
                        Experience = 0,
                        State = PlayerState.Default,
                        skill1 = Systems.SkillType.Trap,
                        skill2 = Systems.SkillType.ExplodingDroids,
                        skill3 = Systems.SkillType.Turret,
                        skill4 = Systems.SkillType.HealingStation,
                        skill5 = Systems.SkillType.PortableShop,
                        skill6 = Systems.SkillType.PortableShield,
                        skill7 = Systems.SkillType.Charge,
                        skill8 = Systems.SkillType.FallBack,
                        skill9 = Systems.SkillType.Motivate,

                    };
                    game.PlayerInfoComponent[entityID] = info;

                    skillInfo = new PlayerSkillInfo()
                    {
                        Skill1Rank = 1,
                        Skill2Rank = 1,
                        Skill3Rank = 1,
                        Skill4Rank = 1,
                        Skill5Rank = 1,
                        Skill6Rank = 1,
                        Skill7Rank = 1,
                        Skill8Rank = 1,
                        Skill9Rank = 1,
                    };
                    game.PlayerSkillInfoComponent[entityID] = skillInfo;

                    active_Skill = new ActiveSkill()
                    {
                        activeSkill = info.skill1,
                    };
                    game.ActiveSkillComponent[entityID] = active_Skill;

                    //Create HUD
                    game.HUDSystem.LoadPlayerHUD(player);
                    //create Inv
                    invagg.CreateInv(player);
                    break;
                #endregion

                #region Gargranian
                /****************************************
                * Gargranian by Michael Fountain
                * *************************************/
                case Aggregate.GargranianPlayer:
                    entityID = Entity.NextEntity();
                    spriteSheet = game.Content.Load<Texture2D>("Spritesheets/Aggregate/gargranian");
                    spriteSheet.Name = "Spritesheets/Aggregate/gargranian";

                    /*Author: Josh Zavala, Assignment 9
                     *This has been transferred from ContinueNewGameScreen.goToNetworking
                     *Allows default weapons assignment per class
                     */
                    /*Update:Joseph Shaw, Assignment 9
                     *Set the weapon type here and use it in the create weapon so that it 
                     *can be saved into the gameSave after the switch statement.
                     *We could abstract this method entirely but leaving it here gives more flexibility.
                     */
                    weaponType = WeaponType.PsychicStun;
                    equipment = new Equipment()
                    {
                        EntityID = entityID,
                        WeaponID = game.WeaponFactory.CreateWeapon(weaponType),
                        HealthPotsQty = defaultHealthPotQty,
                        PsiPotsQty = defaultPsiPotQty,
                        PogsQty = 0,
                    };
                    game.EquipmentComponent.Add(equipment.EntityID, equipment);

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
                        SpriteSheet = spriteSheet,
                        //SpriteColor = new Color(45, 45, 45, 0),
                        //UseDifferentColor = true
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
                        Defense = 12,
                        HealthBase = 100,
                        PsiBase = 100,
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
                        PsiOrFatige = 100,
                        Level = 1,
                        Experience = 0,
                        State = PlayerState.Default,
                        skill1 = Systems.SkillType.Teleport, 
                        skill2 = Systems.SkillType.Invisibility,
                        skill3 = Systems.SkillType.Meditate, 
                        skill4 = Systems.SkillType.PsionicSpear,
                        skill5 = Systems.SkillType.Push,
                        skill6 = Systems.SkillType.ImprovedPsionicSpear,
                        skill7 = Systems.SkillType.MentalBarrier,
                        skill8 = Systems.SkillType.WormOfGargran,
                        skill9 = Systems.SkillType.Soothe,
                    };
                    game.PlayerInfoComponent[entityID] = info;
                    skillInfo = new PlayerSkillInfo()
                    {
                        Skill1Rank = 1,
                        Skill2Rank = 1,
                        Skill3Rank = 1,
                        Skill4Rank = 1,
                        Skill5Rank = 1,
                        Skill6Rank = 1,
                        Skill7Rank = 1,
                        Skill8Rank = 1,
                        Skill9Rank = 1,
                    };
                    game.PlayerSkillInfoComponent[entityID] = skillInfo;

                    active_Skill = new ActiveSkill()
                    {
                        activeSkill = info.skill1,
                    };
                    game.ActiveSkillComponent[entityID] = active_Skill;

                    //Create HUD
                    game.HUDSystem.LoadPlayerHUD(player);
                    //create Inv
                    invagg.CreateInv(player);
                    break;
                #endregion

                #region Space Pirate
                /****************************************
                * Space Pirate
                * Done by Austin Murphy and I also have posted the 9 sprites for my skills that are listed in the design document.
                * *************************************/
                case Aggregate.SpacePiratePlayer:
                    entityID = Entity.NextEntity();
                    spriteSheet = game.Content.Load<Texture2D>("Spritesheets/Aggregate/SpacePBig");
                    spriteSheet.Name = "Spritesheets/Aggregate/SpacePBig";

                    /*Author: Josh Zavala, Assignment 9
                     *This has been transferred from ContinueNewGameScreen.goToNetworking
                     *Allows default weapons assignment per class
                     */
                    /*Update:Joseph Shaw, Assignment 9
                     *Set the weapon type here and use it in the create weapon so that it 
                     *can be saved into the gameSave after the switch statement.
                     *We could abstract this method entirely but leaving it here gives more flexibility.
                     */
                    weaponType = WeaponType.StolenCutlass;
                    equipment = new Equipment()
                    {
                        EntityID = entityID,
                        WeaponID = game.WeaponFactory.CreateWeapon(weaponType),
                        HealthPotsQty = defaultHealthPotQty,
                        PsiPotsQty = defaultPsiPotQty,
                        PogsQty = 0,
                    };
                    game.EquipmentComponent.Add(equipment.EntityID, equipment);

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

                        //So here we just define our base values. Total sum is 50
                        //The base stats are 10 across the board
                        Strength = 5,
                        Stamina = 5,
                        Agility = 25,
                        Intelligence = 5,
                        Defense = 5,
                        HealthBase = 100,
                        PsiBase = 100,
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
                        PsiOrFatige = 100,
                        Level = 1,
                        Experience = 0,
                        State = PlayerState.Default,
                        skill1 = Systems.SkillType.AgilityBerserker,
                        skill2 = Systems.SkillType.DualWielding,
                        skill3 = Systems.SkillType.HeavyDrinker,
                        skill4 = Systems.SkillType.PowerShot,
                        skill5 = Systems.SkillType.EagleShot,
                        skill6 = Systems.SkillType.TrickShot,
                        skill7 = Systems.SkillType.Mug,
                        skill8 = Systems.SkillType.LockPicking,
                        skill9 = Systems.SkillType.Theft,
                    };
                    game.PlayerInfoComponent[entityID] = info;

                    skillInfo = new PlayerSkillInfo()
                    {
                        Skill1Rank = 1,
                        Skill2Rank = 1,
                        Skill3Rank = 1,
                        Skill4Rank = 1,
                        Skill5Rank = 1,
                        Skill6Rank = 1,
                        Skill7Rank = 1,
                        Skill8Rank = 1,
                        Skill9Rank = 1,
                    };
                    game.PlayerSkillInfoComponent[entityID] = skillInfo;

                    active_Skill = new ActiveSkill()
                    {
                        activeSkill = info.skill1,
                    };
                    game.ActiveSkillComponent[entityID] = active_Skill;

                    //Create HUD
                    game.HUDSystem.LoadPlayerHUD(player);
                    //create Inv
                    invagg.CreateInv(player);
                    break;
                #endregion

                #region Zombie
                /****************************************
                * Zombie
                 * written by Matthew Hart
                * *************************************/
                case Aggregate.ZombiePlayer:
                    entityID = Entity.NextEntity();
                    spriteSheet = game.Content.Load<Texture2D>("Spritesheets/Aggregate/MzombieBx2");
                    spriteSheet.Name = "Spritesheets/Aggregate/MzombieBx2";

                    /*Author: Josh Zavala, Assignment 9
                     *This has been transferred from ContinueNewGameScreen.goToNetworking
                     *Allows default weapons assignment per class
                     */
                    /*Update:Joseph Shaw, Assignment 9
                     *Set the weapon type here and use it in the create weapon so that it 
                     *can be saved into the gameSave after the switch statement.
                     *We could abstract this method entirely but leaving it here gives more flexibility.
                     */
                    weaponType = WeaponType.DeadHand;
                    equipment = new Equipment()
                    {
                        EntityID = entityID,
                        WeaponID = game.WeaponFactory.CreateWeapon(weaponType),
                        HealthPotsQty = defaultHealthPotQty,
                        PsiPotsQty = defaultPsiPotQty,
                        PogsQty = 0,
                    };
                    game.EquipmentComponent.Add(equipment.EntityID, equipment);

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
                        Defense = 14,
                        HealthBase = 100,
                        PsiBase = 100,
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
                        PsiOrFatige = 100,
                        State = PlayerState.Default,
                        skill1 = Systems.SkillType.ThrownBlades,
                        skill2 = Systems.SkillType.FrenziedAttack,
                        skill3 = Systems.SkillType.CausticWeapons,
                        skill4 = Systems.SkillType.MeatShield,
                        skill5 = Systems.SkillType.HardenedBody,
                        skill6 = Systems.SkillType.Regeneration,
                        skill7 = Systems.SkillType.BenignParasite,
                        skill8 = Systems.SkillType.MaliciousParasite,
                        skill9 = Systems.SkillType.MindlessParasites,
                    };
                    game.PlayerInfoComponent[entityID] = info;

                    skillInfo = new PlayerSkillInfo()
                    {
                        Skill1Rank = 1,
                        Skill2Rank = 1,
                        Skill3Rank = 1,
                        Skill4Rank = 1,
                        Skill5Rank = 1,
                        Skill6Rank = 1,
                        Skill7Rank = 1,
                        Skill8Rank = 1,
                        Skill9Rank = 1,
                    };
                    game.PlayerSkillInfoComponent[entityID] = skillInfo;

                    active_Skill = new ActiveSkill()
                    {
                        activeSkill = info.skill1,
                    };
                    game.ActiveSkillComponent[entityID] = active_Skill;

                    //Create HUD
                    game.HUDSystem.LoadPlayerHUD(player);
                    //create Inv
                    invagg.CreateInv(player);
                    break;
                #endregion

                default:
                    throw new Exception("Unknown type.");
            }

            // Store all of the data into the game save
            gameSave.aggregate = (int)aggregate;
            gameSave.health = 100;
            gameSave.psi = 100;
            gameSave.stats = stats;
            gameSave.level = 1;
            gameSave.experience = 0;
            gameSave.charAnimation = spriteSheet.Name;
            gameSave.fileName = fileName;
            info.FileName = fileName;
            gameSave.weaponType = (int)weaponType;
            gameSave.healthPotions = defaultHealthPotQty;
            gameSave.manaPotions = defaultPsiPotQty;
            gameSave.pogs = 0;

            game.QuestLogSystem.ActivateQuest(entityID, 0);
            game.QuestLogSystem.ActivateQuest(entityID, 1);

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
            Equipment equipment;

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

                /*Author: Josh Zavala, Assignment 9
                 *This has been transferred from ContinueNewGameScreen.goToNetworking
                 *Allows default weapons assignment per class
                 */
                equipment = new Equipment()
                {
                    EntityID = entityID,
                    WeaponID = game.WeaponFactory.CreateWeapon((WeaponType)gameSave.weaponType),
                    HealthPotsQty = gameSave.healthPotions,
                    PsiPotsQty = gameSave.manaPotions,
                    PogsQty = gameSave.pogs,
                };
                game.EquipmentComponent.Add(equipment.EntityID, equipment);

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
                    Defense = gameSave.stats.Defense,
                    HealthBase = gameSave.stats.HealthBase,
                    PsiBase = gameSave.stats.PsiBase,
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
                    Health = gameSave.health,
                    PsiOrFatige = gameSave.psi,
                    Level = gameSave.level,
                    Experience = gameSave.experience,
                    State = PlayerState.Default,
                };
                game.PlayerInfoComponent[entityID] = info;

                game.PlayerComponent[entityID] = player;
                //Create HUD
                game.HUDSystem.LoadPlayerHUD(player);
                //create Inv
                invagg.CreateInv(player);
            }

            return entityID;
        }
    }
}
