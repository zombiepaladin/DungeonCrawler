#region File Description
//-----------------------------------------------------------------------------
// SkillSystem.cs 
//
// Author: Nicholas Boen
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
using Microsoft.Xna.Framework.Graphics;

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

            int x;
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
                            {

                                #region Skill Variables
                                Turret turret;
                                TimedEffect timedEffect;
                                float effectDuration;
                                Sprite sprite;
                                Position turretPosition;

                                #endregion

                                switch (rank)
                                {
                                    #region Checking Rank
                                    case 1:
                                        eid = Entity.NextEntity();
                                        effectDuration = 3;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        turretPosition = _game.PositionComponent[GetPlayerID()];
                                        turret = new Turret()
                                        {
                                            EntityID = eid,
                                            position = turretPosition, 
                                            range = 100,
                                        };
                                        _game.TurretComponent.Add(eid, turret);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Turret"),
                                            SpriteBounds = new Rectangle(0, 0, 38, 27),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        _game.PositionComponent.Add(eid, turret.position);

                                        break;

                                    case 2:
                                        eid = Entity.NextEntity();
                                        effectDuration = 4;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        turretPosition = _game.PositionComponent[GetPlayerID()];
                                        turret = new Turret()
                                        {
                                            EntityID = eid,
                                            position = turretPosition, 
                                            range = 100,
                                        };
                                        _game.TurretComponent.Add(eid, turret);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Turret"),
                                            SpriteBounds = new Rectangle(0, 0, 38, 27),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        _game.PositionComponent.Add(eid, turret.position);
                                        
                                        break;

                                    case 3:
                                        eid = Entity.NextEntity();
                                        effectDuration = 5;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        turretPosition = _game.PositionComponent[GetPlayerID()];
                                        turret = new Turret()
                                        {
                                            EntityID = eid,
                                            position = turretPosition, 
                                            range = 150,
                                        };
                                        _game.TurretComponent.Add(eid, turret);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Turret"),
                                            SpriteBounds = new Rectangle(0, 0, 38, 27),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        _game.PositionComponent.Add(eid, turret.position);
                                        break;

                                    case 4:
                                        eid = Entity.NextEntity();
                                        effectDuration = 6;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                       turretPosition = _game.PositionComponent[GetPlayerID()];
                                        turret = new Turret()
                                        {
                                            EntityID = eid,
                                            position = turretPosition, 
                                            range = 200,
                                        };
                                        _game.TurretComponent.Add(eid, turret);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Turret"),
                                            SpriteBounds = new Rectangle(0, 0, 38, 27),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        _game.PositionComponent.Add(eid, turret.position);
                                        break;

                                    case 5:
                                        eid = Entity.NextEntity();
                                        effectDuration = 7;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                       turretPosition = _game.PositionComponent[GetPlayerID()];
                                        turret = new Turret()
                                        {
                                            EntityID = eid,
                                            position = turretPosition, 
                                            range = 250,
                                        };
                                        _game.TurretComponent.Add(eid, turret);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Turret"),
                                            SpriteBounds = new Rectangle(0, 0, 38, 27),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        _game.PositionComponent.Add(eid, turret.position);
                                        break;

                                    case 6:
                                        eid = Entity.NextEntity();
                                        effectDuration = 8;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        turretPosition = _game.PositionComponent[GetPlayerID()];
                                        turret = new Turret()
                                        {
                                            EntityID = eid,
                                            position = turretPosition, 
                                            range = 300,
                                        };
                                        _game.TurretComponent.Add(eid, turret);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Turret"),
                                            SpriteBounds = new Rectangle(0, 0, 38, 27),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        _game.PositionComponent.Add(eid, turret.position);
                                        break;

                                    case 7:
                                        eid = Entity.NextEntity();
                                        effectDuration = 9;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        turretPosition = _game.PositionComponent[GetPlayerID()];
                                        turret = new Turret()
                                        {
                                            EntityID = eid,
                                            position = turretPosition, 
                                            range = 350,
                                        };
                                        _game.TurretComponent.Add(eid, turret);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Turret"),
                                            SpriteBounds = new Rectangle(0, 0, 38, 27),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        _game.PositionComponent.Add(eid, turret.position);
                                        break;

                                    case 8:
                                        eid = Entity.NextEntity();
                                        effectDuration = 10;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        turretPosition = _game.PositionComponent[GetPlayerID()];
                                        turret = new Turret()
                                        {
                                            EntityID = eid,
                                            position = turretPosition, 
                                            range = 400,
                                        };
                                        _game.TurretComponent.Add(eid, turret);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Turret"),
                                            SpriteBounds = new Rectangle(0, 0, 38, 27),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        _game.PositionComponent.Add(eid, turret.position);
                                        break;

                                    case 9:
                                        eid = Entity.NextEntity();
                                        effectDuration = 11;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        turretPosition = _game.PositionComponent[GetPlayerID()];
                                        turret = new Turret()
                                        {
                                            EntityID = eid,
                                            position = turretPosition, 
                                            range = 450,
                                        };
                                        _game.TurretComponent.Add(eid, turret);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Turret"),
                                            SpriteBounds = new Rectangle(0, 0, 38, 27),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        _game.PositionComponent.Add(eid, turret.position);
                                        break;

                                    case 10:
                                        eid = Entity.NextEntity();
                                        effectDuration = 12;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        turretPosition = _game.PositionComponent[GetPlayerID()];
                                        turret = new Turret()
                                        {
                                            EntityID = eid,
                                            position = turretPosition, 
                                            range = 500,
                                        };
                                        _game.TurretComponent.Add(eid, turret);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Turret"),
                                            SpriteBounds = new Rectangle(0, 0, 38, 27),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        _game.PositionComponent.Add(eid, turret.position);
                                        break;

                                    default:
                                        break;
                                    #endregion
                                }
                            }
                            break;

                        #endregion

                        #region Trap

                        case SkillType.Trap:
                            {

                                #region Skill Variables

                                TimedEffect timedEffect;
                                float effectDuration;
                                Trap trap;
                                Sprite sprite;
                                Position trapPosition;

                                #endregion

                                switch (rank)
                                {
                                    #region Checking Rank
                                    case 1:
                                        eid = Entity.NextEntity();
                                        effectDuration = 3;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        trapPosition = _game.PositionComponent[GetPlayerID()];
                                        trap = new Trap()
                                        {
                                            EntityID = eid,
                                            position = trapPosition,
                                            isSet = false,
                                            range = 20,
                                            duration = effectDuration,
                                        };
                                        _game.TrapComponent.Add(eid, trap);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Turret"),
                                            SpriteBounds = new Rectangle(0, 0, 38, 27),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        _game.PositionComponent.Add(eid, trap.position);
                                        break;

                                    case 2:
                                        eid = Entity.NextEntity();
                                        effectDuration = 4;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        trapPosition = _game.PositionComponent[GetPlayerID()];
                                        trap = new Trap()
                                        {
                                            EntityID = eid,
                                            position = trapPosition,
                                            isSet = false,
                                            range = 20,
                                            duration = effectDuration,
                                        };
                                        _game.TrapComponent.Add(eid, trap);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Turret"),
                                            SpriteBounds = new Rectangle(0, 0, 38, 27),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        _game.PositionComponent.Add(eid, trap.position);
                                        break;

                                    case 3:
                                        eid = Entity.NextEntity();
                                        effectDuration = 4;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        trapPosition = _game.PositionComponent[GetPlayerID()];
                                        trap = new Trap()
                                        {
                                            EntityID = eid,
                                            position = trapPosition,
                                            isSet = false,
                                            range = 30,
                                            duration = effectDuration,
                                        };
                                        _game.TrapComponent.Add(eid, trap);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Turret"),
                                            SpriteBounds = new Rectangle(0, 0, 38, 27),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        _game.PositionComponent.Add(eid, trap.position);
                                        break;

                                    case 4:
                                        eid = Entity.NextEntity();
                                        effectDuration = 4;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        trapPosition = _game.PositionComponent[GetPlayerID()];
                                        trap = new Trap()
                                        {
                                            EntityID = eid,
                                            position = trapPosition,
                                            isSet = false,
                                            range = 30,
                                            duration = effectDuration,
                                        };
                                        _game.TrapComponent.Add(eid, trap);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Turret"),
                                            SpriteBounds = new Rectangle(0, 0, 38, 27),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        _game.PositionComponent.Add(eid, trap.position);
                                        break;

                                    case 5:
                                        eid = Entity.NextEntity();
                                        effectDuration = 5;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        trapPosition = _game.PositionComponent[GetPlayerID()];
                                        trap = new Trap()
                                        {
                                            EntityID = eid,
                                            position = trapPosition,
                                            isSet = false,
                                            range = 40,
                                            duration = effectDuration,
                                        };
                                        _game.TrapComponent.Add(eid, trap);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Turret"),
                                            SpriteBounds = new Rectangle(0, 0, 38, 27),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        _game.PositionComponent.Add(eid, trap.position);
                                        break;

                                    case 6:
                                        eid = Entity.NextEntity();
                                        effectDuration = 5;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        trapPosition = _game.PositionComponent[GetPlayerID()];
                                        trap = new Trap()
                                        {
                                            EntityID = eid,
                                            position = trapPosition,
                                            isSet = false,
                                            range = 40,
                                            duration = effectDuration,
                                        };
                                        _game.TrapComponent.Add(eid, trap);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Turret"),
                                            SpriteBounds = new Rectangle(0, 0, 38, 27),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        _game.PositionComponent.Add(eid, trap.position);
                                        break;

                                    case 7:
                                        eid = Entity.NextEntity();
                                        effectDuration = 6;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        trapPosition = _game.PositionComponent[GetPlayerID()];
                                        trap = new Trap()
                                        {
                                            EntityID = eid,
                                            position = trapPosition,
                                            isSet = false,
                                            range = 50,
                                            duration = effectDuration,
                                        };
                                        _game.TrapComponent.Add(eid, trap);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Turret"),
                                            SpriteBounds = new Rectangle(0, 0, 38, 27),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        _game.PositionComponent.Add(eid, trap.position);
                                        break;

                                    case 8:
                                        eid = Entity.NextEntity();
                                        effectDuration = 6;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        trapPosition = _game.PositionComponent[GetPlayerID()];
                                        trap = new Trap()
                                        {
                                            EntityID = eid,
                                            position = trapPosition,
                                            isSet = false,
                                            range = 50,
                                            duration = effectDuration,
                                        };
                                        _game.TrapComponent.Add(eid, trap);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Turret"),
                                            SpriteBounds = new Rectangle(0, 0, 38, 27),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        _game.PositionComponent.Add(eid, trap.position);
                                        break;

                                    case 9:
                                        eid = Entity.NextEntity();
                                        effectDuration = 6;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        trapPosition = _game.PositionComponent[GetPlayerID()];
                                        trap = new Trap()
                                        {
                                            EntityID = eid,
                                            position = trapPosition,
                                            isSet = false,
                                            range = 60,
                                            duration = effectDuration,
                                        };
                                        _game.TrapComponent.Add(eid, trap);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Turret"),
                                            SpriteBounds = new Rectangle(0, 0, 38, 27),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        _game.PositionComponent.Add(eid, trap.position);
                                        break;

                                    case 10:
                                        eid = Entity.NextEntity();
                                        effectDuration = 7;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        trapPosition = _game.PositionComponent[GetPlayerID()];
                                        trap = new Trap()
                                        {
                                            EntityID = eid,
                                            position = trapPosition,
                                            isSet = false,
                                            range = 60,
                                            duration = effectDuration,
                                        };
                                        _game.TrapComponent.Add(eid, trap);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Turret"),
                                            SpriteBounds = new Rectangle(0, 0, 38, 27),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        _game.PositionComponent.Add(eid, trap.position);
                                        break;

                                    default:
                                        break;
                                    #endregion
                                }
                            }
                            break;

                        #endregion

                        #region Exploding Droids

                        case SkillType.ExplodingDroids:
                            {

                                #region Skill Variables

                                TimedEffect timedEffect;
                                float effectDuration;

                                Movement movement;
                                float droidSpeed;

                                ExplodingDroid explodingDroid;
                                Sprite sprite;

                                Position droidPosition;

                                Collideable collideable;

                                #endregion

                                switch (rank)
                                {
                                    #region Checking Rank
                                    case 1:
                                        eid = Entity.NextEntity();
                                        effectDuration = 15;
                                        droidSpeed = 110;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        movement = new Movement()
                                        {
                                            EntityID = eid,
                                            Speed = droidSpeed,
                                        };
                                        _game.MovementComponent.Add(eid, movement);

                                        droidPosition = _game.PositionComponent[GetPlayerID()];
                                        explodingDroid = new ExplodingDroid()
                                        {
                                            EntityID = eid,
                                            position = droidPosition,
                                            hasEnemy = false,
                                        };
                                        _game.ExplodingDroidComponent.Add(eid, explodingDroid);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Turret"),
                                            SpriteBounds = new Rectangle(0, 0, 80, 93),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        collideable = new Collideable()
                                        {
                                            EntityID = eid,
                                            RoomID = droidPosition.RoomID,
                                            Bounds = new CircleBounds(droidPosition.Center, droidPosition.Radius),
                                        };
                                        _game.CollisionComponent.Add(eid, collideable);

                                        _game.PositionComponent.Add(eid, explodingDroid.position);
                                        break;

                                    case 2:
                                        eid = Entity.NextEntity();
                                        effectDuration = 5;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        _game.SkillEntityFactory.CreateSkillDeployable(Skills.healingStation, _game.PositionComponent[GetPlayerID()]);
                                        break;

                                    case 3:
                                        eid = Entity.NextEntity();
                                        effectDuration = 5;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        _game.SkillEntityFactory.CreateSkillDeployable(Skills.healingStation, _game.PositionComponent[GetPlayerID()]);
                                        break;

                                    case 4:
                                        eid = Entity.NextEntity();
                                        effectDuration = 5;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        _game.SkillEntityFactory.CreateSkillDeployable(Skills.healingStation, _game.PositionComponent[GetPlayerID()]);
                                        break;

                                    case 5:
                                        eid = Entity.NextEntity();
                                        effectDuration = 5;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        _game.SkillEntityFactory.CreateSkillDeployable(Skills.healingStation, _game.PositionComponent[GetPlayerID()]);
                                        break;

                                    case 6:
                                        eid = Entity.NextEntity();
                                        effectDuration = 5;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        _game.SkillEntityFactory.CreateSkillDeployable(Skills.healingStation, _game.PositionComponent[GetPlayerID()]);
                                        break;

                                    case 7:
                                        eid = Entity.NextEntity();
                                        effectDuration = 5;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        _game.SkillEntityFactory.CreateSkillDeployable(Skills.healingStation, _game.PositionComponent[GetPlayerID()]);
                                        break;

                                    case 8:
                                        eid = Entity.NextEntity();
                                        effectDuration = 5;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        _game.SkillEntityFactory.CreateSkillDeployable(Skills.healingStation, _game.PositionComponent[GetPlayerID()]);
                                        break;

                                    case 9:
                                        eid = Entity.NextEntity();
                                        effectDuration = 5;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        _game.SkillEntityFactory.CreateSkillDeployable(Skills.healingStation, _game.PositionComponent[GetPlayerID()]);
                                        break;

                                    case 10:
                                        eid = Entity.NextEntity();
                                        effectDuration = 5;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        _game.SkillEntityFactory.CreateSkillDeployable(Skills.healingStation, _game.PositionComponent[GetPlayerID()]);
                                        break;

                                    default:
                                        break;
                                    #endregion
                                }
                            }
                            break;

                        #endregion

                        #region Healing Station

                        case SkillType.HealingStation:
                            {

                            #region Skill Variables

                            TimedEffect timedEffect;
                            float effectDuration;

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
                                    eid = Entity.NextEntity();
                                    effectDuration = 5;

                                    timedEffect = new TimedEffect()
                                    {
                                        EntityID = eid,
                                        TotalDuration = effectDuration,
                                        TimeLeft = effectDuration
                                    };
                                    _game.TimedEffectComponent.Add(eid, timedEffect);

                                    _game.SkillEntityFactory.CreateSkillDeployable(Skills.healingStation, _game.PositionComponent[GetPlayerID()]);
                                    break;

                                case 2:
                                    eid = Entity.NextEntity();
                                    effectDuration = 5;

                                    timedEffect = new TimedEffect()
                                    {
                                        EntityID = eid,
                                        TotalDuration = effectDuration,
                                        TimeLeft = effectDuration
                                    };
                                    _game.TimedEffectComponent.Add(eid, timedEffect);

                                    _game.SkillEntityFactory.CreateSkillDeployable(Skills.healingStation, _game.PositionComponent[GetPlayerID()]);
                                    break;

                                case 3:
                                    eid = Entity.NextEntity();
                                    effectDuration = 5;

                                    timedEffect = new TimedEffect()
                                    {
                                        EntityID = eid,
                                        TotalDuration = effectDuration,
                                        TimeLeft = effectDuration
                                    };
                                    _game.TimedEffectComponent.Add(eid, timedEffect);

                                    _game.SkillEntityFactory.CreateSkillDeployable(Skills.healingStation, _game.PositionComponent[GetPlayerID()]);
                                    break;

                                case 4:
                                    eid = Entity.NextEntity();
                                    effectDuration = 5;

                                    timedEffect = new TimedEffect()
                                    {
                                        EntityID = eid,
                                        TotalDuration = effectDuration,
                                        TimeLeft = effectDuration
                                    };
                                    _game.TimedEffectComponent.Add(eid, timedEffect);

                                    _game.SkillEntityFactory.CreateSkillDeployable(Skills.healingStation, _game.PositionComponent[GetPlayerID()]);
                                    break;

                                case 5:
                                    eid = Entity.NextEntity();
                                    effectDuration = 5;

                                    timedEffect = new TimedEffect()
                                    {
                                        EntityID = eid,
                                        TotalDuration = effectDuration,
                                        TimeLeft = effectDuration
                                    };
                                    _game.TimedEffectComponent.Add(eid, timedEffect);

                                    _game.SkillEntityFactory.CreateSkillDeployable(Skills.healingStation, _game.PositionComponent[GetPlayerID()]);
                                    break;

                                case 6:
                                    eid = Entity.NextEntity();
                                    effectDuration = 5;

                                    timedEffect = new TimedEffect()
                                    {
                                        EntityID = eid,
                                        TotalDuration = effectDuration,
                                        TimeLeft = effectDuration
                                    };
                                    _game.TimedEffectComponent.Add(eid, timedEffect);

                                    _game.SkillEntityFactory.CreateSkillDeployable(Skills.healingStation, _game.PositionComponent[GetPlayerID()]);
                                    break;

                                case 7:
                                    eid = Entity.NextEntity();
                                    effectDuration = 5;

                                    timedEffect = new TimedEffect()
                                    {
                                        EntityID = eid,
                                        TotalDuration = effectDuration,
                                        TimeLeft = effectDuration
                                    };
                                    _game.TimedEffectComponent.Add(eid, timedEffect);

                                    _game.SkillEntityFactory.CreateSkillDeployable(Skills.healingStation, _game.PositionComponent[GetPlayerID()]);
                                    break;

                                case 8:
                                    eid = Entity.NextEntity();
                                    effectDuration = 5;

                                    timedEffect = new TimedEffect()
                                    {
                                        EntityID = eid,
                                        TotalDuration = effectDuration,
                                        TimeLeft = effectDuration
                                    };
                                    _game.TimedEffectComponent.Add(eid, timedEffect);

                                    _game.SkillEntityFactory.CreateSkillDeployable(Skills.healingStation, _game.PositionComponent[GetPlayerID()]);
                                    break;

                                case 9:
                                    eid = Entity.NextEntity();
                                    effectDuration = 5;

                                    timedEffect = new TimedEffect()
                                    {
                                        EntityID = eid,
                                        TotalDuration = effectDuration,
                                        TimeLeft = effectDuration
                                    };
                                    _game.TimedEffectComponent.Add(eid, timedEffect);

                                    _game.SkillEntityFactory.CreateSkillDeployable(Skills.healingStation, _game.PositionComponent[GetPlayerID()]);
                                    break;

                                case 10:
                                    eid = Entity.NextEntity();
                                    effectDuration = 5;

                                    timedEffect = new TimedEffect()
                                    {
                                        EntityID = eid,
                                        TotalDuration = effectDuration,
                                        TimeLeft = effectDuration
                                    };
                                    _game.TimedEffectComponent.Add(eid, timedEffect);

                                    _game.SkillEntityFactory.CreateSkillDeployable(Skills.healingStation, _game.PositionComponent[GetPlayerID()]);
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            }
                            break;

                        #endregion

                        #region Portable Shop

                        case SkillType.PortableShop:
                            {

                                #region Skill Variables
                                TimedEffect timedEffect;
                                float effectDuration;
                                #endregion

                                switch (rank)
                                {
                                    #region Checking Rank
                                    case 1:
                                        eid = Entity.NextEntity();
                                        effectDuration = 5;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        _game.SkillEntityFactory.CreateSkillDeployable(Skills.healingStation, _game.PositionComponent[GetPlayerID()]);
                                        break;

                                    case 2:
                                        eid = Entity.NextEntity();
                                        effectDuration = 5;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        _game.SkillEntityFactory.CreateSkillDeployable(Skills.healingStation, _game.PositionComponent[GetPlayerID()]);
                                        break;

                                    case 3:
                                        eid = Entity.NextEntity();
                                        effectDuration = 7;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        _game.SkillEntityFactory.CreateSkillDeployable(Skills.healingStation, _game.PositionComponent[GetPlayerID()]);
                                        break;

                                    case 4:
                                        eid = Entity.NextEntity();
                                        effectDuration = 9;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        _game.SkillEntityFactory.CreateSkillDeployable(Skills.healingStation, _game.PositionComponent[GetPlayerID()]);
                                        break;

                                    case 5:
                                        eid = Entity.NextEntity();
                                        effectDuration = 9;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        _game.SkillEntityFactory.CreateSkillDeployable(Skills.healingStation, _game.PositionComponent[GetPlayerID()]);
                                        break;

                                    case 6:
                                        eid = Entity.NextEntity();
                                        effectDuration = 11;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        _game.SkillEntityFactory.CreateSkillDeployable(Skills.healingStation, _game.PositionComponent[GetPlayerID()]);
                                        break;

                                    case 7:
                                        eid = Entity.NextEntity();
                                        effectDuration = 13;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        _game.SkillEntityFactory.CreateSkillDeployable(Skills.healingStation, _game.PositionComponent[GetPlayerID()]);
                                        break;

                                    case 8:
                                        eid = Entity.NextEntity();
                                        effectDuration = 15;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        _game.SkillEntityFactory.CreateSkillDeployable(Skills.healingStation, _game.PositionComponent[GetPlayerID()]);
                                        break;

                                    case 9:
                                        eid = Entity.NextEntity();
                                        effectDuration = 17;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        _game.SkillEntityFactory.CreateSkillDeployable(Skills.healingStation, _game.PositionComponent[GetPlayerID()]);
                                        break;

                                    case 10:
                                        eid = Entity.NextEntity();
                                        effectDuration = 20;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        _game.SkillEntityFactory.CreateSkillDeployable(Skills.healingStation, _game.PositionComponent[GetPlayerID()]);
                                        break;

                                    default:
                                        break;
                                    #endregion
                                }
                            }
                            break;

                        #endregion

                        #region Portable Shield

                        case SkillType.PortableShield:
                            {

                                #region Skill Variables
                                TimedEffect timedEffect;
                                float effectDuration;
                                #endregion

                                switch (rank)
                                {
                                    #region Checking Rank
                                    case 1:
                                        eid = Entity.NextEntity();
                                        effectDuration = 5;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        _game.SkillEntityFactory.CreateSkillDeployable(Skills.healingStation, _game.PositionComponent[GetPlayerID()]);
                                        break;

                                    case 2:
                                        eid = Entity.NextEntity();
                                        effectDuration = 8;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        _game.SkillEntityFactory.CreateSkillDeployable(Skills.healingStation, _game.PositionComponent[GetPlayerID()]);
                                        break;

                                    case 3:
                                        eid = Entity.NextEntity();
                                        effectDuration = 10;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        _game.SkillEntityFactory.CreateSkillDeployable(Skills.healingStation, _game.PositionComponent[GetPlayerID()]);
                                        break;

                                    case 4:
                                        eid = Entity.NextEntity();
                                        effectDuration = 8;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        _game.SkillEntityFactory.CreateSkillDeployable(Skills.healingStation, _game.PositionComponent[GetPlayerID()]);
                                        break;

                                    case 5:
                                        eid = Entity.NextEntity();
                                        effectDuration = 10;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        _game.SkillEntityFactory.CreateSkillDeployable(Skills.healingStation, _game.PositionComponent[GetPlayerID()]);
                                        break;

                                    case 6:
                                        eid = Entity.NextEntity();
                                        effectDuration = 12;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        _game.SkillEntityFactory.CreateSkillDeployable(Skills.healingStation, _game.PositionComponent[GetPlayerID()]);
                                        break;

                                    case 7:
                                        eid = Entity.NextEntity();
                                        effectDuration = 10;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        _game.SkillEntityFactory.CreateSkillDeployable(Skills.healingStation, _game.PositionComponent[GetPlayerID()]);
                                        break;

                                    case 8:
                                        eid = Entity.NextEntity();
                                        effectDuration = 12;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        _game.SkillEntityFactory.CreateSkillDeployable(Skills.healingStation, _game.PositionComponent[GetPlayerID()]);
                                        break;

                                    case 9:
                                        eid = Entity.NextEntity();
                                        effectDuration = 15;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        _game.SkillEntityFactory.CreateSkillDeployable(Skills.healingStation, _game.PositionComponent[GetPlayerID()]);
                                        break;

                                    case 10:
                                        eid = Entity.NextEntity();
                                        effectDuration = 15;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        _game.SkillEntityFactory.CreateSkillDeployable(Skills.healingStation, _game.PositionComponent[GetPlayerID()]);
                                        break;

                                    default:
                                        break;
                                    #endregion
                                }
                            }
                            break;

                        #endregion

                        #region Motivate

                        case SkillType.Motivate:
                            {

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

                            List<Player> players;

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank

                                #region Rank 1

                                case 1:

                                    players = new List<Player>();
                                    foreach (Player player in _game.PlayerComponent.All)
                                    {
                                        Player p = _game.PlayerComponent[player.EntityID];
                                        players.Add(p);
                                    }

                                    for (int i = 0; i < players.Count; i++)
                                    {
                                        Player p = players[i];
                                        p.abilityModifiers.HealthBonus += 5;
                                        _game.PlayerComponent[p.EntityID] = p;
                                    }

                                    //eid = Entity.NextEntity();
                                    //effectDuration = 5;
                                    //targetID = GetPlayerID();
                                    //speedIncrease = 200;

                                    //eid_2 = Entity.NextEntity();
                                    //origin = _game.PositionComponent[targetID].Center;
                                    //distance = 100;

                                    //timedEffect = new TimedEffect()
                                    //{
                                    //    EntityID = eid,
                                    //    TotalDuration = effectDuration,
                                    //    TimeLeft = effectDuration
                                    //};
                                    //_game.TimedEffectComponent.Add(eid, timedEffect);

                                    //buffEffect = new Buff()
                                    //{
                                    //    EntityID = eid,
                                    //    TargetID = targetID,
                                    //    MovementSpeed = speedIncrease
                                    //};
                                    //_game.BuffComponent.Add(eid, buffEffect);


                                    //instantEffect = new InstantEffect()
                                    //{
                                    //    EntityID = eid_2,
                                    //};
                                    //_game.InstantEffectComponent.Add(eid_2, instantEffect);

                                    //knockBackEffect = new KnockBack()
                                    //{
                                    //    EntityID = eid_2,
                                    //    Origin = origin,
                                    //    Distance = distance,
                                    //};
                                    //_game.KnockBackComponent.Add(eid_2, knockBackEffect);

                                    break;

                                #endregion

                                #region Rank 2

                                case 2:
                                    players = new List<Player>();
                                    foreach (Player player in _game.PlayerComponent.All)
                                    {
                                        Player p = _game.PlayerComponent[player.EntityID];
                                        players.Add(p);
                                    }

                                    for (int i = 0; i < players.Count; i++)
                                    {
                                        Player p = players[i];
                                        p.abilityModifiers.HealthBonus += 10;
                                        _game.PlayerComponent[p.EntityID] = p;
                                    }
                                    break;

                                #endregion

                                #region Rank 3

                                case 3:
                                    players = new List<Player>();
                                    foreach (Player player in _game.PlayerComponent.All)
                                    {
                                        Player p = _game.PlayerComponent[player.EntityID];
                                        players.Add(p);
                                    }

                                    for (int i = 0; i < players.Count; i++)
                                    {
                                        Player p = players[i];
                                        p.abilityModifiers.HealthBonus += 15;
                                        _game.PlayerComponent[p.EntityID] = p;
                                    }
                                    eid = Entity.NextEntity();
                                    break;

                                #endregion

                                #region Rank 4

                                case 4:
                                    players = new List<Player>();
                                    foreach (Player player in _game.PlayerComponent.All)
                                    {
                                        Player p = _game.PlayerComponent[player.EntityID];
                                        players.Add(p);
                                    }

                                    for (int i = 0; i < players.Count; i++)
                                    {
                                        Player p = players[i];
                                        p.abilityModifiers.HealthBonus += 20;
                                        _game.PlayerComponent[p.EntityID] = p;
                                    }
                                    eid = Entity.NextEntity();
                                    break;

                                #endregion

                                #region Rank 5

                                case 5:
                                    players = new List<Player>();
                                    foreach (Player player in _game.PlayerComponent.All)
                                    {
                                        Player p = _game.PlayerComponent[player.EntityID];
                                        players.Add(p);
                                    }

                                    for (int i = 0; i < players.Count; i++)
                                    {
                                        Player p = players[i];
                                        p.abilityModifiers.HealthBonus += 25;
                                        _game.PlayerComponent[p.EntityID] = p;
                                    }
                                    eid = Entity.NextEntity();
                                    break;

                                #endregion

                                #region Rank 6

                                case 6:
                                    players = new List<Player>();
                                    foreach (Player player in _game.PlayerComponent.All)
                                    {
                                        Player p = _game.PlayerComponent[player.EntityID];
                                        players.Add(p);
                                    }

                                    for (int i = 0; i < players.Count; i++)
                                    {
                                        Player p = players[i];
                                        p.abilityModifiers.HealthBonus += 30;
                                        _game.PlayerComponent[p.EntityID] = p;
                                    }
                                    eid = Entity.NextEntity();
                                    break;

                                #endregion

                                #region Rank 7

                                case 7:
                                    players = new List<Player>();
                                    foreach (Player player in _game.PlayerComponent.All)
                                    {
                                        Player p = _game.PlayerComponent[player.EntityID];
                                        players.Add(p);
                                    }

                                    for (int i = 0; i < players.Count; i++)
                                    {
                                        Player p = players[i];
                                        p.abilityModifiers.HealthBonus += 30;
                                        _game.PlayerComponent[p.EntityID] = p;
                                    }
                                    eid = Entity.NextEntity();
                                    break;

                                #endregion

                                #region Rank 8

                                case 8:
                                    players = new List<Player>();
                                    foreach (Player player in _game.PlayerComponent.All)
                                    {
                                        Player p = _game.PlayerComponent[player.EntityID];
                                        players.Add(p);
                                    }

                                    for (int i = 0; i < players.Count; i++)
                                    {
                                        Player p = players[i];
                                        p.abilityModifiers.HealthBonus += 40;
                                        _game.PlayerComponent[p.EntityID] = p;
                                    }
                                    eid = Entity.NextEntity();
                                    break;

                                #endregion

                                #region Rank 9

                                case 9:
                                    players = new List<Player>();
                                    foreach (Player player in _game.PlayerComponent.All)
                                    {
                                        Player p = _game.PlayerComponent[player.EntityID];
                                        players.Add(p);
                                    }

                                    for (int i = 0; i < players.Count; i++)
                                    {
                                        Player p = players[i];
                                        p.abilityModifiers.HealthBonus += 45;
                                        _game.PlayerComponent[p.EntityID] = p;
                                    }
                                    eid = Entity.NextEntity();
                                    break;

                                #endregion

                                #region Rank 10

                                case 10:
                                    players = new List<Player>();
                                    foreach (Player player in _game.PlayerComponent.All)
                                    {
                                        Player p = _game.PlayerComponent[player.EntityID];
                                        players.Add(p);
                                    }

                                    for (int i = 0; i < players.Count; i++)
                                    {
                                        Player p = players[i];
                                        p.abilityModifiers.HealthBonus += 50;
                                        _game.PlayerComponent[p.EntityID] = p;
                                    }
                                    eid = Entity.NextEntity();
                                    break;

                                #endregion

                                default:
                                    break;
                                #endregion
                            }
                            }
                            break;

                        #endregion

                        #region Fall Back

                        case SkillType.FallBack:
                            {
                                #region Skill Variables

                                TimedEffect timedEffect;
                                float effectDuration;
                                List<Player> players;

                                #endregion

                                switch (rank)
                                {
                                    #region Checking Rank
                                    case 1:
                                        eid = Entity.NextEntity();
                                        effectDuration = 15;

                                        players = new List<Player>();
                                        //for (int i = 0; i < _game.PlayerComponent.All.Count(); i++)
                                        //{ }
                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            Player p = _game.PlayerComponent[player.EntityID];
                                            players.Add(p);
                                        }

                                        for (int i = 0; i < players.Count; i++)
                                        {
                                            Player p = players[i];
                                            p.abilityModifiers.MeleeDefenseBonus += 1;
                                            p.abilityModifiers.meleeAttackBonus -= 1;
                                            _game.PlayerComponent[p.EntityID] = p;
                                        }

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        break;

                                    case 2:
                                        eid = Entity.NextEntity();                                        
                                        effectDuration = 15;

                                        players = new List<Player>();

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            Player p = _game.PlayerComponent[player.EntityID];
                                            players.Add(p);
                                        }

                                        for (int i = 0; i < players.Count; i++)
                                        {
                                            Player p = players[i];
                                            p.abilityModifiers.MeleeDefenseBonus += 2;
                                            p.abilityModifiers.meleeAttackBonus -= 1;
                                            _game.PlayerComponent[p.EntityID] = p;
                                        }

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        break;

                                    case 3:
                                        eid = Entity.NextEntity();
                                        effectDuration = 15;

                                        players = new List<Player>();

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            Player p = _game.PlayerComponent[player.EntityID];
                                            players.Add(p);
                                        }

                                        for (int i = 0; i < players.Count; i++)
                                        {
                                            Player p = players[i];
                                            p.abilityModifiers.MeleeDefenseBonus += 3;
                                            p.abilityModifiers.meleeAttackBonus -= 2;
                                            _game.PlayerComponent[p.EntityID] = p;
                                        }

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);
                                        break;

                                    case 4:
                                        eid = Entity.NextEntity();
                                        effectDuration = 15;

                                        players = new List<Player>();

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            Player p = _game.PlayerComponent[player.EntityID];
                                            players.Add(p);
                                        }

                                        for (int i = 0; i < players.Count; i++)
                                        {
                                            Player p = players[i];
                                            p.abilityModifiers.MeleeDefenseBonus += 4;
                                            p.abilityModifiers.meleeAttackBonus -= 2;
                                            _game.PlayerComponent[p.EntityID] = p;
                                        }

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);
                                        break;

                                    case 5:
                                        eid = Entity.NextEntity();
                                        effectDuration = 15;

                                        players = new List<Player>();

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            Player p = _game.PlayerComponent[player.EntityID];
                                            players.Add(p);
                                        }

                                        for (int i = 0; i < players.Count; i++)
                                        {
                                            Player p = players[i];
                                            p.abilityModifiers.MeleeDefenseBonus += 5;
                                            p.abilityModifiers.meleeAttackBonus -= 3;
                                            _game.PlayerComponent[p.EntityID] = p;
                                        }

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);
                                        break;

                                    case 6:
                                        eid = Entity.NextEntity();
                                        effectDuration = 15;

                                        players = new List<Player>();

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            Player p = _game.PlayerComponent[player.EntityID];
                                            players.Add(p);
                                        }

                                        for (int i = 0; i < players.Count; i++)
                                        {
                                            Player p = players[i];
                                            p.abilityModifiers.MeleeDefenseBonus += 6;
                                            p.abilityModifiers.meleeAttackBonus -= 3;
                                            _game.PlayerComponent[p.EntityID] = p;
                                        }

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);
                                        break;

                                    case 7:
                                        eid = Entity.NextEntity();
                                        effectDuration = 15;

                                        players = new List<Player>();

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            Player p = _game.PlayerComponent[player.EntityID];
                                            players.Add(p);
                                        }

                                        for (int i = 0; i < players.Count; i++)
                                        {
                                            Player p = players[i];
                                            p.abilityModifiers.MeleeDefenseBonus += 7;
                                            p.abilityModifiers.meleeAttackBonus -= 4;
                                            _game.PlayerComponent[p.EntityID] = p;
                                        }

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);
                                        break;

                                    case 8:
                                        eid = Entity.NextEntity();
                                        effectDuration = 15;

                                        players = new List<Player>();

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            Player p = _game.PlayerComponent[player.EntityID];
                                            players.Add(p);
                                        }

                                        for (int i = 0; i < players.Count; i++)
                                        {
                                            Player p = players[i];
                                            p.abilityModifiers.MeleeDefenseBonus += 8;
                                            p.abilityModifiers.meleeAttackBonus -= 4;
                                            _game.PlayerComponent[p.EntityID] = p;
                                        }

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);
                                        break;

                                    case 9:
                                        eid = Entity.NextEntity();
                                        effectDuration = 15;

                                        players = new List<Player>();

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            Player p = _game.PlayerComponent[player.EntityID];
                                            players.Add(p);
                                        }

                                        for (int i = 0; i < players.Count; i++)
                                        {
                                            Player p = players[i];
                                            p.abilityModifiers.MeleeDefenseBonus += 9;
                                            p.abilityModifiers.meleeAttackBonus -= 5;
                                            _game.PlayerComponent[p.EntityID] = p;
                                        }

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);
                                        break;

                                    case 10:
                                        eid = Entity.NextEntity();
                                        effectDuration = 15;

                                        players = new List<Player>();

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            Player p = _game.PlayerComponent[player.EntityID];
                                            players.Add(p);
                                        }

                                        for (int i = 0; i < players.Count; i++)
                                        {
                                            Player p = players[i];
                                            p.abilityModifiers.MeleeDefenseBonus += 10;
                                            p.abilityModifiers.meleeAttackBonus -= 5;
                                            _game.PlayerComponent[p.EntityID] = p;
                                        }

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);
                                        break;

                                    default:
                                        break;
                                    #endregion
                                }
                            }
                            break;

                        #endregion

                        #region Charge

                        case SkillType.Charge:
                            {

                            #region Skill Variables
                                TimedEffect timedEffect;
                                float effectDuration;
                                List<Player> players;

                            #endregion

                                switch (rank)
                                {
                                    #region Checking Rank
                                    case 1:
                                        eid = Entity.NextEntity();
                                        effectDuration = 15;

                                        players = new List<Player>();
                                        //for (int i = 0; i < _game.PlayerComponent.All.Count(); i++)
                                        //{ }
                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            Player p = _game.PlayerComponent[player.EntityID];
                                            players.Add(p);
                                        }

                                        for (int i = 0; i < players.Count; i++)
                                        {
                                            Player p = players[i];
                                            p.abilityModifiers.meleeAttackBonus += 1;
                                            p.abilityModifiers.MeleeDefenseBonus -= 1;
                                            _game.PlayerComponent[p.EntityID] = p;
                                        }

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        break;

                                    case 2:
                                        eid = Entity.NextEntity();
                                        effectDuration = 15;

                                        players = new List<Player>();

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            Player p = _game.PlayerComponent[player.EntityID];
                                            players.Add(p);
                                        }

                                        for (int i = 0; i < players.Count; i++)
                                        {
                                            Player p = players[i];
                                            p.abilityModifiers.meleeAttackBonus += 2;
                                            p.abilityModifiers.MeleeDefenseBonus -= 1;
                                            _game.PlayerComponent[p.EntityID] = p;
                                        }

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        break;

                                    case 3:
                                        eid = Entity.NextEntity();
                                        effectDuration = 15;

                                        players = new List<Player>();

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            Player p = _game.PlayerComponent[player.EntityID];
                                            players.Add(p);
                                        }

                                        for (int i = 0; i < players.Count; i++)
                                        {
                                            Player p = players[i];
                                            p.abilityModifiers.meleeAttackBonus += 3;
                                            p.abilityModifiers.MeleeDefenseBonus -= 2;
                                            _game.PlayerComponent[p.EntityID] = p;
                                        }

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);
                                        break;

                                    case 4:
                                        eid = Entity.NextEntity();
                                        effectDuration = 15;

                                        players = new List<Player>();

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            Player p = _game.PlayerComponent[player.EntityID];
                                            players.Add(p);
                                        }

                                        for (int i = 0; i < players.Count; i++)
                                        {
                                            Player p = players[i];
                                            p.abilityModifiers.meleeAttackBonus += 4;
                                            p.abilityModifiers.MeleeDefenseBonus -= 2;
                                            _game.PlayerComponent[p.EntityID] = p;
                                        }

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);
                                        break;

                                    case 5:
                                        eid = Entity.NextEntity();
                                        effectDuration = 15;

                                        players = new List<Player>();

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            Player p = _game.PlayerComponent[player.EntityID];
                                            players.Add(p);
                                        }

                                        for (int i = 0; i < players.Count; i++)
                                        {
                                            Player p = players[i];
                                            p.abilityModifiers.meleeAttackBonus += 5;
                                            p.abilityModifiers.MeleeDefenseBonus -= 3;
                                            _game.PlayerComponent[p.EntityID] = p;
                                        }

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);
                                        break;

                                    case 6:
                                        eid = Entity.NextEntity();
                                        effectDuration = 15;

                                        players = new List<Player>();

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            Player p = _game.PlayerComponent[player.EntityID];
                                            players.Add(p);
                                        }

                                        for (int i = 0; i < players.Count; i++)
                                        {
                                            Player p = players[i];
                                            p.abilityModifiers.meleeAttackBonus += 6;
                                            p.abilityModifiers.MeleeDefenseBonus -= 3;
                                            _game.PlayerComponent[p.EntityID] = p;
                                        }

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);
                                        break;

                                    case 7:
                                        eid = Entity.NextEntity();
                                        effectDuration = 15;

                                        players = new List<Player>();

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            Player p = _game.PlayerComponent[player.EntityID];
                                            players.Add(p);
                                        }

                                        for (int i = 0; i < players.Count; i++)
                                        {
                                            Player p = players[i];
                                            p.abilityModifiers.meleeAttackBonus += 7;
                                            p.abilityModifiers.MeleeDefenseBonus -= 4;
                                            _game.PlayerComponent[p.EntityID] = p;
                                        }

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);
                                        break;

                                    case 8:
                                        eid = Entity.NextEntity();
                                        effectDuration = 15;

                                        players = new List<Player>();

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            Player p = _game.PlayerComponent[player.EntityID];
                                            players.Add(p);
                                        }

                                        for (int i = 0; i < players.Count; i++)
                                        {
                                            Player p = players[i];
                                            p.abilityModifiers.meleeAttackBonus += 8;
                                            p.abilityModifiers.MeleeDefenseBonus -= 4;
                                            _game.PlayerComponent[p.EntityID] = p;
                                        }

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);
                                        break;

                                    case 9:
                                        eid = Entity.NextEntity();
                                        effectDuration = 15;

                                        players = new List<Player>();

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            Player p = _game.PlayerComponent[player.EntityID];
                                            players.Add(p);
                                        }

                                        for (int i = 0; i < players.Count; i++)
                                        {
                                            Player p = players[i];
                                            p.abilityModifiers.meleeAttackBonus += 9;
                                            p.abilityModifiers.MeleeDefenseBonus -= 5;
                                            _game.PlayerComponent[p.EntityID] = p;
                                        }

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);
                                        break;

                                    case 10:
                                        eid = Entity.NextEntity();
                                        effectDuration = 15;

                                        players = new List<Player>();

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            Player p = _game.PlayerComponent[player.EntityID];
                                            players.Add(p);
                                        }

                                        for (int i = 0; i < players.Count; i++)
                                        {
                                            Player p = players[i];
                                            p.abilityModifiers.meleeAttackBonus += 10;
                                            p.abilityModifiers.MeleeDefenseBonus -= 5;
                                            _game.PlayerComponent[p.EntityID] = p;
                                        }

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);
                                        break;

                                    default:
                                        break;
                                    #endregion
                                }
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

                case Aggregate.SpacePiratePlayer:

                    #region Race Variables

                    #endregion

                    switch (skillType)
                    {
                        #region Checking Skill Type

                        case SkillType.AgilityBerserker:

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

                        case SkillType.HeavyDrinker:

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

                        case SkillType.PowerShot:

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

                        case SkillType.Theft:

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

                        case SkillType.LockPicking:

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

                default:
                    break;

                #endregion
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