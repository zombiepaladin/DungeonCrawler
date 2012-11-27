#region File Description
//-----------------------------------------------------------------------------
// SkillSystem.cs 
//
// Author: Nicholas Boen 
// Contributers: Austin Murphy
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
using DungeonCrawler.Entities;
using DungeonCrawler.Components;

namespace DungeonCrawler.Systems
{
    //A list of all 52 skills that we currently have
    public enum SkillType
    {
        EnergyShield,
        Defibrillate,
        Nanobots,
        TargettingUpgrade,
        RepulsorArm,
        EnergyShot,
        AlloyBody,
        CyberneticSlam,
        ThrusterRush,
        MindLock,
        Invisibility,
        Possess,
        PsionicSpear,
        Push,
        Detnate,
        MentalBarrier,
        WormOfGargran,
        Soothe,
        Enslave,
        Fear,
        Sacrifice,
        Taint,
        Rot,
        Lightning,
        Malice,
        ThrownBlades,
        FrenziedAttack,
        CausticWeapons,
        MeatShield,
        HardenedBody,
        GraspingBlade,
        BenignParasite,
        MaliciousParasite,
        MindlessParasites,
        Turret,
        Trap,
        ExplodingDroids,
        HealingStation,
        PortableShop,
        PortableShield,
        Motivate,
        FallBack,
        Charge,
        AgilityBerserker,
        DualWielding,
        HeavyDrinker,
        TrickShot,
        PowerShot,
        EagleShot,
        Theft,
        Mug,
        LockPicking,
        DamagingPull,
    }

    public class SkillSystem
    {
        #region Private Variables
        
        private DungeonCrawlerGame _game;

        #endregion

        #region Constructor

        public SkillSystem(DungeonCrawlerGame game)
        {
            this._game = game;
        }

        #endregion

        #region Public Methods

        public void Update(float elapsedTime)
        {
            List<uint> deleteList = new List<uint>();

            //TODO: Might need some form of optimization
            List<uint> timedEffectKeyList = new List<uint>(_game.TimedEffectComponent.Keys);

            foreach (uint key in timedEffectKeyList)
            {
                TimedEffect effect = _game.TimedEffectComponent[key];

                effect.TimeLeft -= elapsedTime;

                _game.TimedEffectComponent[key] = effect;

                if (effect.TimeLeft <= 0)
                    deleteList.Add(key);
            }

            List<uint> cooldownKeyList = new List<uint>(_game.CoolDownComponent.Keys);

            foreach (uint key in cooldownKeyList)
            {
                CoolDown coolDown = _game.CoolDownComponent[key];

                coolDown.TimeLeft -= elapsedTime;

                _game.CoolDownComponent[key] = coolDown;

                if (coolDown.TimeLeft <= 0)
                    deleteList.Add(key);
            }

            List<uint> instantEffectKeyList = new List<uint>(_game.InstantEffectComponent.Keys);

            foreach (uint key in instantEffectKeyList)
            {
                InstantEffect effect = _game.InstantEffectComponent[key];

                if (!effect.isTriggered)
                {
                    HandleEffects(key, elapsedTime);
                    effect.isTriggered = true;
                }
                else
                {
                    deleteList.Add(key);
                }

                _game.InstantEffectComponent[key] = effect;
            }

            //Handle the Visual and non timed effect cases
            //delete the instant effects that don't need to linger

            foreach (uint key in deleteList)
            {
                _game.GarbagemanSystem.ScheduleVisit(key, GarbagemanSystem.ComponentType.Effect);
            }
        }

        public void EnemyUseSkill(SkillType skillType, uint callerID, uint targetID)
        {
            foreach (CoolDown cd in _game.CoolDownComponent.All)
            {
                if (cd.Type == skillType && cd.UserID == callerID)
                {
                    return;
                }
            }
           
            uint eid = Entity.NextEntity();
            uint eid_2 = Entity.NextEntity();
            uint eid_3 = Entity.NextEntity();

            switch (skillType) //add in any skills you need an enemy to use
            {
                case SkillType.DamagingPull:
                    
                    InstantEffect instantEffect = new InstantEffect()
                    {
                        EntityID = eid,
                    };
                    _game.InstantEffectComponent.Add(eid, instantEffect);

                    TargetedKnockBack targetedKnockBack = new TargetedKnockBack()
                    {
                        TargetID = targetID,
                        Origin = _game.PositionComponent[callerID].Center,
                        Distance = -10,
                    };
                    _game.TargetedKnockBackComponent.Add(eid, targetedKnockBack);

                    CoolDown coolDown = new CoolDown()
                    {
                        EntityID = eid_2,
                        MaxTime = 5f,
                        TimeLeft = 5f,
                        Type = SkillType.DamagingPull,
                        UserID = callerID,
                    };
                    _game.CoolDownComponent.Add(eid_2, coolDown);

                    TimedEffect timedEffect = new TimedEffect()
                    {
                        EntityID = eid_3,
                        TimeLeft = 2f,
                        TotalDuration = 2f,
                    };
                    _game.TimedEffectComponent.Add(eid_3, timedEffect);

                    Buff buffEffect = new Buff()
                    {
                        EntityID = eid_3,
                        TargetID = targetID,
                        MovementSpeed = -30,
                        isPercentMovementSpeed = true,
                    };
                    _game.BuffComponent.Add(eid_3, buffEffect);

                    break;
            }
        }

        public void UseSkill(Aggregate playerType, SkillType skillType, int rank, uint userID)
        {
            #region Global Variables

            uint eid;

            #endregion
            
            #region Check Cool Down

            //make sure the user isn't cooling down from a previous use
            foreach (CoolDown cd in _game.CoolDownComponent.All)
            {
                if (cd.Type == skillType && cd.UserID == userID)
                    return;
            }

            #endregion

            switch (playerType)
            {
                #region Checking Player Type

                #region Cyborg

                case Aggregate.CyborgPlayer:
                    
                    #region Race Variables

                    #endregion

                    switch (skillType)
                    {
                        #region Checking Skill Type

                        case SkillType.EnergyShield:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;
                                
                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;
                                        
                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        case SkillType.Defibrillate:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        case SkillType.Nanobots:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        case SkillType.TargettingUpgrade:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        case SkillType.RepulsorArm:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        case SkillType.EnergyShot:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        case SkillType.AlloyBody:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        case SkillType.CyberneticSlam:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        case SkillType.ThrusterRush:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        default:
                            break;

                        #endregion
                    }
                    break;

                #endregion

                #region Gargranian

                case Aggregate.GargranianPlayer:

                    #region Race Variables

                    #endregion

                    switch (skillType)
                    {
                        #region Checking Skill Type

                        case SkillType.MindLock:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        case SkillType.Invisibility:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        case SkillType.Possess:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        case SkillType.PsionicSpear:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        case SkillType.Push:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        case SkillType.Detnate:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        case SkillType.MentalBarrier:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        case SkillType.WormOfGargran:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        case SkillType.Soothe:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        default:
                            break;

                        #endregion
                    }
                    break;

                #endregion

                #region Cultist

                case Aggregate.CultistPlayer:

                    #region Race Variables

                    #endregion

                    switch (skillType)
                    {
                        #region Checking Skill Type

                        case SkillType.Enslave:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        case SkillType.Fear:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        case SkillType.Sacrifice:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        case SkillType.PsionicSpear:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        case SkillType.Taint:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        case SkillType.Rot:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        case SkillType.Push:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        case SkillType.Lightning:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        case SkillType.Malice:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        default:
                            break;

                        #endregion
                    }
                    break;

                #endregion

                #region Vermis

                case Aggregate.ZombiePlayer:

                    #region Race Variables

                    #endregion

                    switch (skillType)
                    {
                        #region Checking Skill Type

                        case SkillType.ThrownBlades:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        case SkillType.FrenziedAttack:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        case SkillType.CausticWeapons:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        case SkillType.MeatShield:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        case SkillType.HardenedBody:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        case SkillType.GraspingBlade:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        case SkillType.BenignParasite:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        case SkillType.MaliciousParasite:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        case SkillType.MindlessParasites:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        default:
                            break;

                        #endregion
                    }
                    break;

                #endregion

                #region Earthian

                case Aggregate.EarthianPlayer:

                    #region Race Variables

                    #endregion

                    switch (skillType)
                    {
                        #region Checking Skill Type

                        #region Turret

                        case SkillType.Turret:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        #endregion

                        #region Trap

                        case SkillType.Trap:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        #endregion

                        #region Exploding Droids

                        case SkillType.ExplodingDroids:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        #endregion

                        #region Healing Station

                        case SkillType.HealingStation:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        #endregion

                        #region Portable Shop

                        case SkillType.PortableShop:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        #endregion

                        #region Portable Shield

                        case SkillType.PortableShield:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        #endregion

                        #region Motivate

                        case SkillType.Motivate:
                            
                            #region Skill Variables

                            TimedEffect timedEffect;
                            float effectDuration;

                            Buff buffEffect;
                            uint targetID;
                            int speedIncrease;

                            InstantEffect instantEffect;
                            uint eid_2;

                            KnockBack knockBackEffect;
                            Vector2 origin;
                            float distance;

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank

                                #region Rank 1

                                case 1:
									eid = Entity.NextEntity();
                                    effectDuration = 5;
                                    targetID = GetPlayerID();
                                    speedIncrease = 200;

                                    eid_2 = Entity.NextEntity();
                                    origin = _game.PositionComponent[targetID].Center;
                                    distance = 100;

                                    timedEffect = new TimedEffect()
                                    {
                                        EntityID = eid,
                                        TotalDuration = effectDuration,
                                        TimeLeft = effectDuration,
                                    };
                                    _game.TimedEffectComponent.Add(eid, timedEffect);

                                    buffEffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        MovementSpeed = speedIncrease
                                    };
                                    _game.BuffComponent.Add(eid, buffEffect);


                                    instantEffect = new InstantEffect()
                                    {
                                        EntityID = eid_2,
                                    };
                                    _game.InstantEffectComponent.Add(eid_2, instantEffect);

                                    knockBackEffect = new KnockBack()
                                    {
                                        EntityID = eid_2,
                                        Origin = origin,
                                        Distance = distance,
                                    };
                                    _game.KnockBackComponent.Add(eid_2, knockBackEffect);

                                    break;

                                #endregion

                                #region Rank 2

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                #endregion

                                #region Rank 3

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                #endregion

                                #region Rank 4

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                #endregion

                                #region Rank 5

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                #endregion

                                #region Rank 6

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                #endregion

                                #region Rank 7

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                #endregion

                                #region Rank 8

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                #endregion

                                #region Rank 9

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                #endregion

                                #region Rank 10

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                #endregion

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        #endregion

                        #region Fall Back

                        case SkillType.FallBack:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        #endregion

                        #region Charge

                        case SkillType.Charge:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        #endregion

                        default:
                            break;

                        #endregion
                    }
                    break;

                #endregion

                #region Space Pirate
                //This was the part contributed by Austin Murphy
                case Aggregate.SpacePiratePlayer:

                    #region Race Variables

                    #endregion

                    switch (skillType)
                    {
                        #region Checking Skill Type

                        #region AgilityBerserker
                        case SkillType.AgilityBerserker:

                            #region Skill Variables
                            TimedEffect te1, te2;
                            int speedIncrease = 1000;
                            int attackDecrease = -500;
                            float duration, cd;
                            uint targetID;
                            Buff buffeffect;
                            int afterS = -500;
                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    duration = 10;
                                    cd = 55;
                                    targetID = GetPlayerID();
                                    te1 = new TimedEffect()
                                    {
                                        EntityID = eid,
                                        TotalDuration = duration,
                                        TimeLeft = duration
                                    };

                                    _game.TimedEffectComponent.Add(eid, te1);
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        MovementSpeed = speedIncrease
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    duration = 15;
                                    cd = 50;
                                    targetID = GetPlayerID();
                                    te1 = new TimedEffect()
                                    {
                                        EntityID = eid,
                                        TotalDuration = duration,
                                        TimeLeft = duration
                                    };

                                    _game.TimedEffectComponent.Add(eid, te1);
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        MovementSpeed = speedIncrease
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);
                                    break;

                                case 3:
									eid = Entity.NextEntity();                                    
                                    duration = 20;
                                    cd = 45;
                                    targetID = GetPlayerID();
                                    te1 = new TimedEffect()
                                    {
                                        EntityID = eid,
                                        TotalDuration = duration,
                                        TimeLeft = duration
                                    };

                                    _game.TimedEffectComponent.Add(eid, te1);
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        MovementSpeed = speedIncrease
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    duration = 25;
                                    cd = 40;
                                    targetID = GetPlayerID();
                                    te1 = new TimedEffect()
                                    {
                                        EntityID = eid,
                                        TotalDuration = duration,
                                        TimeLeft = duration
                                    };

                                    _game.TimedEffectComponent.Add(eid, te1);
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        MovementSpeed = speedIncrease
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    duration = 30;
                                    cd = 35;
                                    targetID = GetPlayerID();
                                    te1 = new TimedEffect()
                                    {
                                        EntityID = eid,
                                        TotalDuration = duration,
                                        TimeLeft = duration
                                    };

                                    _game.TimedEffectComponent.Add(eid, te1);
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        MovementSpeed = speedIncrease
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    duration = 35;
                                    cd = 30;
                                    targetID = GetPlayerID();
                                    te1 = new TimedEffect()
                                    {
                                        EntityID = eid,
                                        TotalDuration = duration,
                                        TimeLeft = duration
                                    };

                                    _game.TimedEffectComponent.Add(eid, te1);
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        MovementSpeed = speedIncrease
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    duration = 40;
                                    cd = 25;
                                    targetID = GetPlayerID();
                                    te1 = new TimedEffect()
                                    {
                                        EntityID = eid,
                                        TotalDuration = duration,
                                        TimeLeft = duration
                                    };

                                    _game.TimedEffectComponent.Add(eid, te1);
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        MovementSpeed = speedIncrease
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    duration = 45;
                                    cd = 20;
                                    targetID = GetPlayerID();
                                    te1 = new TimedEffect()
                                    {
                                        EntityID = eid,
                                        TotalDuration = duration,
                                        TimeLeft = duration
                                    };

                                    _game.TimedEffectComponent.Add(eid, te1);
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        MovementSpeed = speedIncrease
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    duration = 50;
                                    cd = 15;
                                    targetID = GetPlayerID();
                                    te1 = new TimedEffect()
                                    {
                                        EntityID = eid,
                                        TotalDuration = duration,
                                        TimeLeft = duration
                                    };

                                    _game.TimedEffectComponent.Add(eid, te1);
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        MovementSpeed = speedIncrease
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    duration = 55;
                                    cd = 5;
                                    targetID = GetPlayerID();
                                    te1 = new TimedEffect()
                                    {
                                        EntityID = eid,
                                        TotalDuration = duration,
                                        TimeLeft = duration
                                    };

                                    _game.TimedEffectComponent.Add(eid, te1);
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        MovementSpeed = speedIncrease
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;
                        #endregion

                        #region DualWielding
                        case SkillType.DualWielding:

                            #region Skill Variables
                            
                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;
                        #endregion

                        #region HeavyDrinker
                        case SkillType.HeavyDrinker:

                            #region Skill Variables
                            int resistance;
                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    resistance = 5;
                                    targetID = GetPlayerID();

                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        ResistPoison = resistance
                                    };
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    resistance = 10;
                                    targetID = GetPlayerID();

                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        ResistPoison = resistance
                                    };
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    resistance = 20;
                                    targetID = GetPlayerID();

                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        ResistPoison = resistance
                                    };
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    resistance = 30;
                                    targetID = GetPlayerID();

                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        ResistPoison = resistance
                                    };
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    resistance = 40;
                                    targetID = GetPlayerID();

                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        ResistPoison = resistance
                                    };
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    resistance = 50;
                                    targetID = GetPlayerID();

                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        ResistPoison = resistance
                                    };
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    resistance = 60;
                                    targetID = GetPlayerID();

                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        ResistPoison = resistance
                                    };
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    resistance = 70;
                                    targetID = GetPlayerID();

                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        ResistPoison = resistance
                                    };
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    resistance = 80;
                                    targetID = GetPlayerID();

                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        ResistPoison = resistance
                                    };
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    resistance = 90;
                                    targetID = GetPlayerID();

                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        ResistPoison = resistance
                                    };
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;
                        #endregion

                        #region TrickShot
                        case SkillType.TrickShot:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;
                        #endregion

                        #region PowerShot
                        case SkillType.PowerShot:

                            #region Skill Variables
                            int PSDamageIncrease;
                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;
                        #endregion

                        #region EagleShot
                        case SkillType.EagleShot:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;
                        #endregion

                        #region Theft
                        case SkillType.Theft:
                           
                            #region Skill Variables
                            ChanceToSucceed cts;
                            int prob;
                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    prob = 5;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    prob = 10;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    prob = 20;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    prob = 30;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    prob = 40;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    prob = 50;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    prob = 60;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    prob = 70;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    prob = 80;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    prob = 90;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;
                        #endregion

                        #region Mug
                        case SkillType.Mug:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
									eid = Entity.NextEntity();
                                    break;

                                case 2:
									eid = Entity.NextEntity();
                                    break;

                                case 3:
									eid = Entity.NextEntity();
                                    break;

                                case 4:
									eid = Entity.NextEntity();
                                    break;

                                case 5:
									eid = Entity.NextEntity();
                                    break;

                                case 6:
									eid = Entity.NextEntity();
                                    break;

                                case 7:
									eid = Entity.NextEntity();
                                    break;

                                case 8:
									eid = Entity.NextEntity();
                                    break;

                                case 9:
									eid = Entity.NextEntity();
                                    break;

                                case 10:
									eid = Entity.NextEntity();
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;
                        #endregion

                        #region LockPick
                        case SkillType.LockPicking:

                            #region Skill Variables

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
                                    eid = Entity.NextEntity();
                                    prob = 5;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    break;

                                case 2:
                                    eid = Entity.NextEntity();
                                    prob = 10;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    break;

                                case 3:
                                    eid = Entity.NextEntity();
                                    prob = 20;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    break;

                                case 4:
                                    eid = Entity.NextEntity();
                                    prob = 30;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    break;

                                case 5:
                                    eid = Entity.NextEntity();
                                    prob = 40;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    break;

                                case 6:
                                    eid = Entity.NextEntity();
                                    prob = 50;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    break;

                                case 7:
                                    eid = Entity.NextEntity();
                                    prob = 60;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    break;

                                case 8:
                                    eid = Entity.NextEntity();
                                    prob = 70;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    break;

                                case 9:
                                    eid = Entity.NextEntity();
                                    prob = 80;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    break;

                                case 10:
                                    eid = Entity.NextEntity();
                                    prob = 90;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;
                        #endregion

                        default:
                            break;

                        #endregion
                    }
                    break;

                #endregion


                default:
                    break;

                #endregion
            }
        }

        //public void TriggerSkill(Aggregate playerType, SkillType skillType, int rank, uint userID)
        public void TriggerEffect(SkillType type, int rank, bool friendly, uint target)    
        {
            uint eid;
            switch (type)
            {
                case SkillType.BenignParasite:
                    TimedEffect timedEffect;
                    float effectDuration;
                    eid = Entity.NextEntity();
                    HealOverTime HoT;
                    switch (rank)
                    {
                        case 1:
                            //apply hot to target
                            effectDuration = 1;
                            timedEffect = new TimedEffect()
                            {
                                EntityID = eid,
                                TotalDuration = effectDuration,
                                TimeLeft = effectDuration
                            };
                            _game.TimedEffectComponent.Add(eid, timedEffect);
                            HoT = new HealOverTime()
                            {
                                AmountPerTick = 1,
                                CurrentStack = 1,
                                CurrentTime = effectDuration,
                                EntityID = eid,
                                MaxStack = 1,
                                TargetID = target,
                                TickTime = 1,
                            };
                            _game.HealOverTimeComponent.Add(eid,HoT);
                            break;
                        case 2:
                            break;
                        case 3:
                            break;
                        case 4:
                            break;
                        case 5:
                            break;
                        case 6:
                            break;
                        case 7:
                            break;
                        case 8:
                            break;
                        case 9:
                            break;
                        case 10:
                            break;
                        default:
                            throw new Exception("Unimplemented Rank");
                    }
                    break;
                default:
                    throw new Exception("Unimplemented SKill");
            }
        }


        #endregion

        #region PrivateMethods

        private uint GetPlayerID()
        {
            foreach (Player player in _game.PlayerComponent.All)
            {
                if (player.PlayerIndex == PlayerIndex.One)
                    return player.EntityID;
            }

            return 0;
        }

        private void HandleEffects(uint key, float elapsedTime)
        {
            #region Chance To Succeed

            if (_game.ChanceToSucceedComponent.Contains(key))
            {
                ChanceToSucceed chanceInstance = _game.ChanceToSucceedComponent[key];
                Random rand = new Random();
                double calculatedAmount = rand.NextDouble() * 100;

                if (calculatedAmount > chanceInstance.SuccessRateAsPercentage)
                {
                    //Handle a failed skill use
                    _game.GarbagemanSystem.ScheduleVisit(chanceInstance.EntityID, GarbagemanSystem.ComponentType.Effect);
                    return;
                }
            }

            #endregion

            #region Damage Over Time

            if (_game.DamageOverTimeComponent.Contains(key))
            {
                DamageOverTime dOT = _game.DamageOverTimeComponent[key];

                dOT.CurrentTime -= elapsedTime;

                if (dOT.CurrentTime <= 0)
                {
                    if (_game.EnemyComponent.Contains(dOT.TargetID))
                    {
                        Enemy enemy = _game.EnemyComponent[dOT.TargetID];
                        enemy.Health -= (dOT.AmountPerTick * dOT.CurrentStack);
                        _game.EnemyComponent[dOT.TargetID] = enemy;
                    }
                    else if (_game.PlayerComponent.Contains(dOT.TargetID))
                    {
                        PlayerInfo player = _game.PlayerInfoComponent[dOT.TargetID];
                        player.Health -= (dOT.AmountPerTick * dOT.CurrentStack);
                        _game.PlayerInfoComponent[dOT.TargetID] = player;
                    }

                    dOT.CurrentTime = dOT.TickTime;
                }
            }

            #endregion

            #region Direct Damage

            if (_game.DirectDamageComponent.Contains(key))
            {
                DirectDamage dD = _game.DirectDamageComponent[key];

                if (_game.EnemyComponent.Contains(dD.TargetID))
                {
                    Enemy enemy = _game.EnemyComponent[dD.TargetID];
                    enemy.Health -= dD.Damage;
                    _game.EnemyComponent[dD.TargetID] = enemy;
                }
                else if (_game.PlayerComponent.Contains(dD.TargetID))
                {
                    PlayerInfo player = _game.PlayerInfoComponent[dD.TargetID];
                    player.Health -= dD.Damage;
                    _game.PlayerInfoComponent[dD.TargetID] = player;
                }
            }

            #endregion

            #region Direct Heal

            if (_game.DirectHealComponent.Contains(key))
            {
                DirectHeal dH = _game.DirectHealComponent[key];

                if (_game.EnemyComponent.Contains(dH.TargetID))
                {
                    Enemy enemy = _game.EnemyComponent[dH.TargetID];
                    enemy.Health += dH.Amount;
                    _game.EnemyComponent[dH.TargetID] = enemy;
                }
                else if (_game.PlayerComponent.Contains(dH.TargetID))
                {
                    PlayerInfo player = _game.PlayerInfoComponent[dH.TargetID];
                    player.Health += dH.Amount;
                    _game.PlayerInfoComponent[dH.TargetID] = player;
                }
            }

            #endregion

            #region Heal Over Time

            if (_game.HealOverTimeComponent.Contains(key))
            {
                HealOverTime hOT = _game.HealOverTimeComponent[key];

                hOT.CurrentTime -= elapsedTime;

                if (hOT.CurrentTime <= 0)
                {
                    if (_game.EnemyComponent.Contains(hOT.TargetID))
                    {
                        Enemy enemy = _game.EnemyComponent[hOT.TargetID];
                        enemy.Health += (hOT.AmountPerTick * hOT.CurrentStack);
                        _game.EnemyComponent[hOT.TargetID] = enemy;
                    }
                    else if (_game.PlayerComponent.Contains(hOT.TargetID))
                    {
                        PlayerInfo player = _game.PlayerInfoComponent[hOT.TargetID];
                        player.Health += (hOT.AmountPerTick * hOT.CurrentStack);
                        _game.PlayerInfoComponent[hOT.TargetID] = player;
                    }

                    hOT.CurrentTime = hOT.TickTime;
                }
            }

            #endregion

            #region KnockBack

            if (_game.KnockBackComponent.Contains(key))
            {
                KnockBack knockbackInstance = _game.KnockBackComponent[key];
                Position enemyPosition;
                Vector2 originToEnemy;

                foreach (Enemy en in _game.EnemyComponent.All)
                {
                    enemyPosition = _game.PositionComponent[en.EntityID];

                    //Check to see if the attack is within range
                    if (Vector2.DistanceSquared(enemyPosition.Center, knockbackInstance.Origin) <= 10000)
                    {
                        //Get a vector from the origin to the enemy
                        originToEnemy = enemyPosition.Center - knockbackInstance.Origin;
                        //Then convert it to a unit vector
                        originToEnemy.Normalize();

                        //Finally, apply the knockback to the enemy
                        enemyPosition.Center += (originToEnemy * knockbackInstance.Distance);
                        //And apply the change to the component list
                        _game.PositionComponent[en.EntityID] = enemyPosition;
                    }
                }
            }

            #endregion

            #region TargetedKnockBack

            if (_game.TargetedKnockBackComponent.Contains(key))
            {
                TargetedKnockBack targetedKnockBackInstance = _game.TargetedKnockBackComponent[key];
                Position targetPosition;
                Vector2 originToEnemy;
                targetPosition = _game.PositionComponent[targetedKnockBackInstance.TargetID];

                //Get a vector from the origin to the enemy
                originToEnemy = targetPosition.Center - targetedKnockBackInstance.Origin;
                //Then convert it to a unit vector
                originToEnemy.Normalize();

                //Finally, apply the knockback to the enemy
                targetPosition.Center += (originToEnemy * targetedKnockBackInstance.Distance);
                //And apply the change to the component list
                _game.PositionComponent[targetPosition.EntityID] = targetPosition;
            }

            #endregion

            #region Resurrect

            if (_game.ResurrectComponent.Contains(key))
            {
                //Don't know how to handle this just yet
            }

            #endregion

        }

        #endregion
    }
}