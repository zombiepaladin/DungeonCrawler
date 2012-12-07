#region File Description
//-----------------------------------------------------------------------------
// SkillSystem.cs 
//
// Author: Nicholas Boen 
// Contributers: Austin Murphy
// Modfied by:  Adam Clark: cyborg skill added
//              Michael Fountain: Gargranian skills have been updated
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
        Teleport,
        Invisibility,
        Meditate,
        PsionicSpear,
        Push,
        ImprovedPsionicSpear,
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
        Regeneration,
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
        SniperShot,
        Cloak,
        BasicMeleeAttack,
        BasicRangedAttack,
    }

    public class SkillSystem
    {
        #region Private Variables

        private DungeonCrawlerGame _game;
        private Random rand;

        #endregion

        #region Constructor

        public SkillSystem(DungeonCrawlerGame game)
        {
            this._game = game;
            rand = new Random();
        }

        #endregion

        #region Public Methods

        public void Update(float elapsedTime)
        {
            //For the chance to succeed components
            Random chanceFinder = new Random();

            List<uint> deleteList = new List<uint>();

            List<uint> chanceToSucceedEffectKeyList = new List<uint>(_game.ChanceToSucceedComponent.Keys);

            foreach (uint key in chanceToSucceedEffectKeyList)
            {
                //No need to process the effects of a skill that needs to be deleted
                //Probably a useless check since deleteList was just created,
                //but its quick and harmless and will be necessary if anything else needs
                //to be added
                // ~Nick B.
                if (deleteList.Contains(key)) continue;

                //Gets a random number between 1 and 100 inclusive
                int randomChance = chanceFinder.Next(100) + 1;

                ChanceToSucceed effect = _game.ChanceToSucceedComponent[key];

                //Checks to see if the skill was a success.
                //Essentially, if we have a success rate of 65 (or 65%)
                //then we find a number between 1 and 100
                //with a 35% chance of getting a number above 65
                //indicating a failure, otherwise it is a success
                // ~Nick B.
                if (randomChance > effect.SuccessRateAsPercentage)
                {
                    //Adding the list of ids in the effect to
                    //the deleted list since the skill was a dud
                    // ~Nick B.
                    deleteList.AddRange(effect.AffectedIDList);
                }

                //Either way we'll need to delete this component
                //we wouldn't want the chance to persist every frame
                //that an effect needs to be applied. If that's necessary
                //then we can make a special component for it
                // ~Nick B.
                deleteList.Add(key);

            }

            //TODO: Might need some form of optimization
            List<uint> timedEffectKeyList = new List<uint>(_game.TimedEffectComponent.Keys);

            foreach (uint key in timedEffectKeyList)
            {
                //No need to process the effects of a skill that needs to be deleted
                if (deleteList.Contains(key)) continue;

                TimedEffect effect = _game.TimedEffectComponent[key];

                effect.TimeLeft -= elapsedTime;

                HandleEffects(key, elapsedTime);

                _game.TimedEffectComponent[key] = effect;

                if (effect.TimeLeft <= 0)
                    deleteList.Add(key);
            }

            List<uint> cooldownKeyList = new List<uint>(_game.CoolDownComponent.Keys);

            foreach (uint key in cooldownKeyList)
            {
                //No need to process the effects of a skill that needs to be deleted
                if (deleteList.Contains(key)) continue;

                CoolDown coolDown = _game.CoolDownComponent[key];

                coolDown.TimeLeft -= elapsedTime;

                _game.CoolDownComponent[key] = coolDown;

                if (coolDown.TimeLeft <= 0)
                    deleteList.Add(key);
            }

            List<uint> psiOrFatigueRegenList = new List<uint>(_game.PsiOrFatigueRegenComponent.Keys);

            foreach (uint key in psiOrFatigueRegenList)
            {
                //No need to process the effects of a skill that needs to be deleted
                if (deleteList.Contains(key)) continue;
                if (_game.TimedEffectComponent.Contains(key)) continue;

                HandleEffects(key, elapsedTime);
            }

            List<uint> instantEffectKeyList = new List<uint>(_game.InstantEffectComponent.Keys);

            foreach (uint key in instantEffectKeyList)
            {
                //No need to process the effects of a skill that needs to be deleted
                if (deleteList.Contains(key)) continue;

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

        public void EnemyUseBasicMelee(uint callerID, uint targetID, float damage, float cooldown)
        {
            foreach (CoolDown cd in _game.CoolDownComponent.All)
            {
                if (cd.Type == SkillType.BasicMeleeAttack && cd.UserID == callerID)
                {
                    return;
                }
            }

            uint eid = Entity.NextEntity();
            uint eid_2 = Entity.NextEntity();

            InstantEffect instantEffect = new InstantEffect()
            {
                EntityID = eid,
            };
            _game.InstantEffectComponent.Add(eid, instantEffect);

            DirectDamage directDamage = new DirectDamage()
            {
                TargetID = targetID,
                Damage = damage,
                EntityID = eid,
            };
            _game.DirectDamageComponent.Add(eid, directDamage);

            CoolDown coolDown = new CoolDown()
            {
                EntityID = eid_2,
                MaxTime = cooldown,
                TimeLeft = cooldown,
                Type = SkillType.BasicMeleeAttack,
                UserID = callerID,
            };
            _game.CoolDownComponent.Add(eid_2, coolDown);
        }

        public void EnemyUseBasicRanged(uint callerID, uint targetID, float damage, float cooldown, string spriteSheet, Rectangle spriteBounds)
        {
            foreach (CoolDown cd in _game.CoolDownComponent.All)
            {
                if (cd.Type == SkillType.BasicRangedAttack && cd.UserID == callerID)
                {
                    return;
                }
            }

            Position callerPos = _game.PositionComponent[callerID];
            Position targetPos = _game.PositionComponent[targetID];

            Vector2 direction = targetPos.Center - callerPos.Center;

            uint eid = _game.SkillEntityFactory.CreateSkillProjectile(SkillType.BasicRangedAttack, direction, callerPos, 1, 300, callerID, true, false, spriteSheet, spriteBounds);

            CoolDown coolDown = new CoolDown()
            {
                EntityID = eid,
                MaxTime = cooldown,
                TimeLeft = cooldown,
                Type = SkillType.BasicRangedAttack,
                UserID = callerID,
            };
            _game.CoolDownComponent.Add(eid, coolDown);
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
            InstantEffect instantEffect;
            DirectDamage directDamage;
            CoolDown coolDown;
            Sprite sprite;
            TimedEffect timedEffect;

            switch (skillType) //add in any skills you need an enemy to use
            {
                case SkillType.DamagingPull:
                    
                    instantEffect = new InstantEffect()
                    {
                        EntityID = eid,
                    };
                    _game.InstantEffectComponent.Add(eid, instantEffect);

                    TargetedKnockBack targetedKnockBack = new TargetedKnockBack()
                    {
                        TargetID = targetID,
                        Origin = _game.PositionComponent[callerID].Center,
                        Distance = -15,
                    };
                    _game.TargetedKnockBackComponent.Add(eid, targetedKnockBack);

                    directDamage = new DirectDamage()
                    {
                        TargetID = targetID,
                        Damage = 1,
                        EntityID = eid,
                    };
                    _game.DirectDamageComponent.Add(eid, directDamage);

                    coolDown = new CoolDown()
                    {
                        EntityID = eid_2,
                        MaxTime = 8f,
                        TimeLeft = 8f,
                        Type = SkillType.DamagingPull,
                        UserID = callerID,
                    };
                    _game.CoolDownComponent.Add(eid_2, coolDown);

                    timedEffect = new TimedEffect()
                    {
                        EntityID = eid_3,
                        TimeLeft = 2f,
                        TotalDuration = 2f,
                    };
                    _game.TimedEffectComponent.Add(eid_3, timedEffect);

                    sprite = new Sprite()
                    {
                        EntityID = eid_3,
                        SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/SpiderWeb"),
                        SpriteBounds = new Rectangle(0, 0, 50, 50),
                    };
                    _game.SpriteComponent.Add(eid_3, sprite);

                    Position webPosition = _game.PositionComponent[targetID];
                    webPosition.EntityID = eid_3;
                    _game.PositionComponent.Add(eid_3, webPosition);

                    Buff buffEffect = new Buff()
                    {
                        EntityID = eid_3,
                        TargetID = targetID,
                        MovementSpeed = -50,
                    };
                    _game.BuffComponent.Add(eid_3, buffEffect);

                    break;

                case SkillType.SniperShot:
                    Position callerPos = _game.PositionComponent[callerID];
                    Position targetPos = _game.PositionComponent[targetID];

                    Vector2 direction = targetPos.Center - callerPos.Center;

                    //if (_game.SpriteAnimationComponent.Contains(callerID))
                    //{
                    //    SpriteAnimation spriteAnimation = _game.SpriteAnimationComponent[callerID];
                    //    spriteAnimation.CurrentAnimationRow = (int)_game.MovementComponent.GetFacingFromDirection(direction);
                    //    _game.SpriteAnimationComponent[callerID] = spriteAnimation;
                    //}
                        
                    eid = _game.SkillEntityFactory.CreateSkillProjectile(SkillType.SniperShot, direction, callerPos, 1, 300,callerID, true, false);

                    coolDown = new CoolDown()
                    {
                        EntityID = eid,
                        MaxTime = 1f,
                        TimeLeft = 1f,
                        Type = SkillType.SniperShot,
                        UserID = callerID,
                    };
                    _game.CoolDownComponent.Add(eid, coolDown);

                    break;
                
                case SkillType.Cloak:
                    timedEffect = new TimedEffect()
                    {
                        EntityID = eid,
                        TimeLeft = 12f,
                        TotalDuration = 12f,
                    };
                    _game.TimedEffectComponent.Add(eid, timedEffect);

                    Cloak cloak = new Cloak(eid, targetID, 10);
                    _game.CloakComponent.Add(eid, cloak);

                    coolDown = new CoolDown()
                    {
                        EntityID = eid,
                        MaxTime = 12f,
                        TimeLeft = 12f,
                        Type = SkillType.Cloak,
                        UserID = callerID,
                    };
                    _game.CoolDownComponent.Add(eid, coolDown);

                    

                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        public void UseSkill(Aggregate playerType, SkillType skillType, int rank, uint userID)
        {
            #region Global Variables

            uint eid;
            Random random = new Random();

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
                            TimedEffect timedEffectShield;
                            Buff buffEffectShield;
                            float effectDurationShield;
                            uint targetIDShield;
                            int damageDecreaseShield;
                            int healShield;
                            HealOverTime hotShield;
                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
                                    eid = Entity.NextEntity();
                                    targetIDShield = GetPlayerID();
                                    healShield = 1;
                                    effectDurationShield = 5;
                                    damageDecreaseShield = 10;

                                    foreach (Player player in _game.PlayerComponent.All)
                                    {
                                        targetIDShield = player.EntityID;
                                        timedEffectShield = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDurationShield,
                                            TimeLeft = effectDurationShield,
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffectShield);

                                        hotShield = new HealOverTime()
                                        {
                                            EntityID = eid,
                                            AmountPerTick = healShield,
                                            TickTime = 1
                                        };
                                        _game.HealOverTimeComponent.Add(eid, hotShield);

                                        buffEffectShield = new Buff()
                                        {
                                            EntityID = eid,
                                            TargetID = targetIDShield,
                                            DefenseMelee = damageDecreaseShield,
                                            DefenseRanged = damageDecreaseShield
                                        };
                                        _game.BuffComponent.Add(eid, buffEffectShield);
                                    }


                                    break;

                                case 2:
                                    eid = Entity.NextEntity();
                                    targetIDShield = GetPlayerID();
                                    healShield = 1;
                                    effectDurationShield = 5;
                                    damageDecreaseShield = 12;

                                    foreach (Player player in _game.PlayerComponent.All)
                                    {
                                        targetIDShield = player.EntityID;
                                        timedEffectShield = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDurationShield,
                                            TimeLeft = effectDurationShield,
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffectShield);

                                        hotShield = new HealOverTime()
                                        {
                                            EntityID = eid,
                                            AmountPerTick = healShield,
                                            TickTime = 1
                                        };
                                        _game.HealOverTimeComponent.Add(eid, hotShield);

                                        buffEffectShield = new Buff()
                                        {
                                            EntityID = eid,
                                            TargetID = targetIDShield,
                                            DefenseMelee = damageDecreaseShield,
                                            DefenseRanged = damageDecreaseShield
                                        };
                                        _game.BuffComponent.Add(eid, buffEffectShield);
                                    }
                                    break;

                                case 3:
                                    eid = Entity.NextEntity();
                                    targetIDShield = GetPlayerID();
                                    healShield = 2;
                                    effectDurationShield = 6;
                                    damageDecreaseShield = 12;

                                    foreach (Player player in _game.PlayerComponent.All)
                                    {
                                        targetIDShield = player.EntityID;
                                        timedEffectShield = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDurationShield,
                                            TimeLeft = effectDurationShield,
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffectShield);

                                        hotShield = new HealOverTime()
                                        {
                                            EntityID = eid,
                                            AmountPerTick = healShield,
                                            TickTime = 1
                                        };
                                        _game.HealOverTimeComponent.Add(eid, hotShield);

                                        buffEffectShield = new Buff()
                                        {
                                            EntityID = eid,
                                            TargetID = targetIDShield,
                                            DefenseMelee = damageDecreaseShield,
                                            DefenseRanged = damageDecreaseShield
                                        };
                                        _game.BuffComponent.Add(eid, buffEffectShield);
                                    }
                                    break;

                                case 4:
                                    eid = Entity.NextEntity();
                                    targetIDShield = GetPlayerID();
                                    healShield = 2;
                                    effectDurationShield = 6;
                                    damageDecreaseShield = 14;

                                    foreach (Player player in _game.PlayerComponent.All)
                                    {
                                        targetIDShield = player.EntityID;
                                        timedEffectShield = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDurationShield,
                                            TimeLeft = effectDurationShield,
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffectShield);

                                        hotShield = new HealOverTime()
                                        {
                                            EntityID = eid,
                                            AmountPerTick = healShield,
                                            TickTime = 1
                                        };
                                        _game.HealOverTimeComponent.Add(eid, hotShield);

                                        buffEffectShield = new Buff()
                                        {
                                            EntityID = eid,
                                            TargetID = targetIDShield,
                                            DefenseMelee = damageDecreaseShield,
                                            DefenseRanged = damageDecreaseShield
                                        };
                                        _game.BuffComponent.Add(eid, buffEffectShield);
                                    }
                                    break;

                                case 5:
                                    eid = Entity.NextEntity();
                                    targetIDShield = GetPlayerID();
                                    healShield = 3;
                                    effectDurationShield = 7;
                                    damageDecreaseShield = 15;

                                    foreach (Player player in _game.PlayerComponent.All)
                                    {
                                        targetIDShield = player.EntityID;
                                        timedEffectShield = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDurationShield,
                                            TimeLeft = effectDurationShield,
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffectShield);

                                        hotShield = new HealOverTime()
                                        {
                                            EntityID = eid,
                                            AmountPerTick = healShield,
                                            TickTime = 1
                                        };
                                        _game.HealOverTimeComponent.Add(eid, hotShield);

                                        buffEffectShield = new Buff()
                                        {
                                            EntityID = eid,
                                            TargetID = targetIDShield,
                                            DefenseMelee = damageDecreaseShield,
                                            DefenseRanged = damageDecreaseShield
                                        };
                                        _game.BuffComponent.Add(eid, buffEffectShield);
                                    }
                                    break;

                                case 6:
                                    eid = Entity.NextEntity();
                                    targetIDShield = GetPlayerID();
                                    healShield = 3;
                                    effectDurationShield = 8;
                                    damageDecreaseShield = 16;

                                    foreach (Player player in _game.PlayerComponent.All)
                                    {
                                        targetIDShield = player.EntityID;
                                        timedEffectShield = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDurationShield,
                                            TimeLeft = effectDurationShield,
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffectShield);

                                        hotShield = new HealOverTime()
                                        {
                                            EntityID = eid,
                                            AmountPerTick = healShield,
                                            TickTime = 1
                                        };
                                        _game.HealOverTimeComponent.Add(eid, hotShield);

                                        buffEffectShield = new Buff()
                                        {
                                            EntityID = eid,
                                            TargetID = targetIDShield,
                                            DefenseMelee = damageDecreaseShield,
                                            DefenseRanged = damageDecreaseShield
                                        };
                                        _game.BuffComponent.Add(eid, buffEffectShield);
                                    }
                                    break;

                                case 7:
                                    eid = Entity.NextEntity();
                                    targetIDShield = GetPlayerID();
                                    healShield = 4;
                                    effectDurationShield = 17;
                                    damageDecreaseShield = 9;

                                    foreach (Player player in _game.PlayerComponent.All)
                                    {
                                        targetIDShield = player.EntityID;
                                        timedEffectShield = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDurationShield,
                                            TimeLeft = effectDurationShield,
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffectShield);

                                        hotShield = new HealOverTime()
                                        {
                                            EntityID = eid,
                                            AmountPerTick = healShield,
                                            TickTime = 1
                                        };
                                        _game.HealOverTimeComponent.Add(eid, hotShield);

                                        buffEffectShield = new Buff()
                                        {
                                            EntityID = eid,
                                            TargetID = targetIDShield,
                                            DefenseMelee = damageDecreaseShield,
                                            DefenseRanged = damageDecreaseShield
                                        };
                                        _game.BuffComponent.Add(eid, buffEffectShield);
                                    }
                                    break;

                                case 8:
                                    eid = Entity.NextEntity();
                                    targetIDShield = GetPlayerID();
                                    healShield = 4;
                                    effectDurationShield = 10;
                                    damageDecreaseShield = 18;

                                    foreach (Player player in _game.PlayerComponent.All)
                                    {
                                        targetIDShield = player.EntityID;
                                        timedEffectShield = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDurationShield,
                                            TimeLeft = effectDurationShield,
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffectShield);

                                        hotShield = new HealOverTime()
                                        {
                                            EntityID = eid,
                                            AmountPerTick = healShield,
                                            TickTime = 1
                                        };
                                        _game.HealOverTimeComponent.Add(eid, hotShield);

                                        buffEffectShield = new Buff()
                                        {
                                            EntityID = eid,
                                            TargetID = targetIDShield,
                                            DefenseMelee = damageDecreaseShield,
                                            DefenseRanged = damageDecreaseShield
                                        };
                                        _game.BuffComponent.Add(eid, buffEffectShield);
                                    }
                                    break;

                                case 9:
                                    eid = Entity.NextEntity();
                                    targetIDShield = GetPlayerID();
                                    healShield = 5;
                                    effectDurationShield = 10;
                                    damageDecreaseShield = 20;

                                    foreach (Player player in _game.PlayerComponent.All)
                                    {
                                        targetIDShield = player.EntityID;
                                        timedEffectShield = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDurationShield,
                                            TimeLeft = effectDurationShield,
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffectShield);

                                        hotShield = new HealOverTime()
                                        {
                                            EntityID = eid,
                                            AmountPerTick = healShield,
                                            TickTime = 1
                                        };
                                        _game.HealOverTimeComponent.Add(eid, hotShield);

                                        buffEffectShield = new Buff()
                                        {
                                            EntityID = eid,
                                            TargetID = targetIDShield,
                                            DefenseMelee = damageDecreaseShield,
                                            DefenseRanged = damageDecreaseShield
                                        };
                                        _game.BuffComponent.Add(eid, buffEffectShield);
                                    }
                                    break;

                                case 10:
                                    eid = Entity.NextEntity();
                                    targetIDShield = GetPlayerID();
                                    healShield = 5;
                                    effectDurationShield = 10;
                                    damageDecreaseShield = 25;

                                    foreach (Player player in _game.PlayerComponent.All)
                                    {
                                        targetIDShield = player.EntityID;
                                        timedEffectShield = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDurationShield,
                                            TimeLeft = effectDurationShield,
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffectShield);

                                        hotShield = new HealOverTime()
                                        {
                                            EntityID = eid,
                                            AmountPerTick = healShield,
                                            TickTime = 1
                                        };
                                        _game.HealOverTimeComponent.Add(eid, hotShield);

                                        buffEffectShield = new Buff()
                                        {
                                            EntityID = eid,
                                            TargetID = targetIDShield,
                                            DefenseMelee = damageDecreaseShield,
                                            DefenseRanged = damageDecreaseShield
                                        };
                                        _game.BuffComponent.Add(eid, buffEffectShield);
                                    }
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        case SkillType.Defibrillate:

                            #region Skill Variables

                            TimedEffect timedEffect;
                            float effectDuration;

                            Buff buffEffect;
                            uint targetID;
                            int speedIncrease;
                            int AttackSpeedIncrease;



                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
                                    eid = Entity.NextEntity();
                                    effectDuration = 3;
                                    targetID = GetPlayerID();
                                    speedIncrease = 200;
                                    AttackSpeedIncrease = 150;

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
                                        MovementSpeed = speedIncrease,
                                        AttackSpeed = AttackSpeedIncrease
                                    };
                                    _game.BuffComponent.Add(eid, buffEffect);
                                    break;

                                case 2:
                                    eid = Entity.NextEntity();
                                    effectDuration = 3;
                                    targetID = GetPlayerID();
                                    speedIncrease = 200;
                                    AttackSpeedIncrease = 200;

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
                                        MovementSpeed = speedIncrease,
                                        AttackSpeed = AttackSpeedIncrease
                                    };
                                    _game.BuffComponent.Add(eid, buffEffect);
                                    break;

                                case 3:
                                    eid = Entity.NextEntity();
                                    effectDuration = 4;
                                    targetID = GetPlayerID();
                                    speedIncrease = 200;
                                    AttackSpeedIncrease = 200;

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
                                        MovementSpeed = speedIncrease,
                                        AttackSpeed = AttackSpeedIncrease
                                    };
                                    _game.BuffComponent.Add(eid, buffEffect);
                                    break;

                                case 4:
                                    eid = Entity.NextEntity();
                                    effectDuration = 4;
                                    targetID = GetPlayerID();
                                    speedIncrease = 200;
                                    AttackSpeedIncrease = 250;

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
                                        MovementSpeed = speedIncrease,
                                        AttackSpeed = AttackSpeedIncrease
                                    };
                                    _game.BuffComponent.Add(eid, buffEffect);
                                    break;

                                case 5:
                                    eid = Entity.NextEntity();
                                    effectDuration = 5;
                                    targetID = GetPlayerID();
                                    speedIncrease = 200;
                                    AttackSpeedIncrease = 250;

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
                                        MovementSpeed = speedIncrease,
                                        AttackSpeed = AttackSpeedIncrease
                                    };
                                    _game.BuffComponent.Add(eid, buffEffect);
                                    break;

                                case 6:
                                    eid = Entity.NextEntity();
                                    effectDuration = 5;
                                    targetID = GetPlayerID();
                                    speedIncrease = 200;
                                    AttackSpeedIncrease = 300;

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
                                        MovementSpeed = speedIncrease,
                                        AttackSpeed = AttackSpeedIncrease
                                    };
                                    _game.BuffComponent.Add(eid, buffEffect);
                                    break;

                                case 7:
                                    eid = Entity.NextEntity();
                                    effectDuration = 5;
                                    targetID = GetPlayerID();
                                    speedIncrease = 200;
                                    AttackSpeedIncrease = 300;

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
                                        MovementSpeed = speedIncrease,
                                        AttackSpeed = AttackSpeedIncrease
                                    };
                                    _game.BuffComponent.Add(eid, buffEffect);
                                    break;

                                case 8:
                                    eid = Entity.NextEntity();
                                    effectDuration = 6;
                                    targetID = GetPlayerID();
                                    speedIncrease = 200;
                                    AttackSpeedIncrease = 300;

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
                                        MovementSpeed = speedIncrease,
                                        AttackSpeed = AttackSpeedIncrease
                                    };
                                    _game.BuffComponent.Add(eid, buffEffect);
                                    break;

                                case 9:
                                    eid = Entity.NextEntity();
                                    effectDuration = 6;
                                    targetID = GetPlayerID();
                                    speedIncrease = 200;
                                    AttackSpeedIncrease = 350;

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
                                        MovementSpeed = speedIncrease,
                                        AttackSpeed = AttackSpeedIncrease
                                    };
                                    _game.BuffComponent.Add(eid, buffEffect);
                                    break;

                                case 10:
                                    eid = Entity.NextEntity();
                                    effectDuration = 8;
                                    targetID = GetPlayerID();
                                    speedIncrease = 200;
                                    AttackSpeedIncrease = 400;

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
                                        MovementSpeed = speedIncrease,
                                        AttackSpeed = AttackSpeedIncrease
                                    };
                                    _game.BuffComponent.Add(eid, buffEffect);
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        case SkillType.Nanobots:

                            #region Skill Variables
                            TimedEffect timedEffectNano;
                            float effectDurationNano;

                            DirectHeal directheal;
                            uint targetIDNano;
                            int heal;

                            HealOverTime hot;
                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
                                    eid = Entity.NextEntity();
                                    targetIDNano = GetPlayerID();
                                    heal = 5;

                                    directheal = new DirectHeal()
                                    {
                                        EntityID = eid,
                                        Amount = heal
                                    };
                                    _game.DirectHealComponent.Add(eid, directheal);
                                    break;

                                case 2:
                                    eid = Entity.NextEntity();
                                    targetIDNano = GetPlayerID();
                                    heal = 8;

                                    directheal = new DirectHeal()
                                    {
                                        EntityID = eid,
                                        Amount = heal
                                    };
                                    _game.DirectHealComponent.Add(eid, directheal);
                                    break;

                                case 3:
                                    eid = Entity.NextEntity();
                                    targetIDNano = GetPlayerID();
                                    heal = 10;

                                    directheal = new DirectHeal()
                                    {
                                        EntityID = eid,
                                        Amount = heal
                                    };
                                    _game.DirectHealComponent.Add(eid, directheal);
                                    break;

                                case 4:
                                    eid = Entity.NextEntity();
                                    targetIDNano = GetPlayerID();
                                    heal = 12;

                                    directheal = new DirectHeal()
                                    {
                                        EntityID = eid,
                                        Amount = heal
                                    };
                                    _game.DirectHealComponent.Add(eid, directheal);
                                    break;

                                case 5:
                                    eid = Entity.NextEntity();
                                    targetIDNano = GetPlayerID();
                                    heal = 14;

                                    directheal = new DirectHeal()
                                    {
                                        EntityID = eid,
                                        Amount = heal
                                    };
                                    _game.DirectHealComponent.Add(eid, directheal); eid = Entity.NextEntity();
                                    break;

                                case 6:
                                    eid = Entity.NextEntity();
                                    targetIDNano = GetPlayerID();
                                    heal = 16;

                                    directheal = new DirectHeal()
                                    {
                                        EntityID = eid,
                                        Amount = heal
                                    };
                                    _game.DirectHealComponent.Add(eid, directheal);
                                    break;

                                case 7:
                                    eid = Entity.NextEntity();
                                    targetIDNano = GetPlayerID();
                                    heal = 18;

                                    directheal = new DirectHeal()
                                    {
                                        EntityID = eid,
                                        Amount = heal
                                    };
                                    _game.DirectHealComponent.Add(eid, directheal);
                                    break;

                                case 8:
                                    eid = Entity.NextEntity();
                                    targetIDNano = GetPlayerID();
                                    heal = 20;

                                    directheal = new DirectHeal()
                                    {
                                        EntityID = eid,
                                        Amount = heal
                                    };
                                    _game.DirectHealComponent.Add(eid, directheal);
                                    break;

                                case 9:
                                    eid = Entity.NextEntity();
                                    targetIDNano = GetPlayerID();
                                    heal = 25;

                                    directheal = new DirectHeal()
                                    {
                                        EntityID = eid,
                                        Amount = heal
                                    };
                                    _game.DirectHealComponent.Add(eid, directheal);
                                    break;

                                case 10:
                                    eid = Entity.NextEntity();
                                    targetIDNano = GetPlayerID();
                                    heal = 25;
                                    effectDurationNano = 10;

                                    directheal = new DirectHeal()
                                    {
                                        EntityID = eid,
                                        Amount = heal
                                    };
                                    _game.DirectHealComponent.Add(eid, directheal);

                                    timedEffectNano = new TimedEffect()
                                    {
                                        EntityID = eid,
                                        TotalDuration = effectDurationNano,
                                        TimeLeft = effectDurationNano,
                                    };
                                    _game.TimedEffectComponent.Add(eid, timedEffectNano);

                                    hot = new HealOverTime()
                                    {
                                        EntityID = eid,
                                        AmountPerTick = 1,
                                        TickTime = 2
                                    };
                                    _game.HealOverTimeComponent.Add(eid, hot);
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        case SkillType.TargettingUpgrade:

                            #region Skill Variables

                            Buff buffEffectTarget;
                            int WeaponIncrease;

                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
                                    eid = Entity.NextEntity();
                                    targetID = GetPlayerID();
                                    WeaponIncrease = 120;
                                    buffEffectTarget = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        WeaponStrength = WeaponIncrease
                                    };
                                    _game.BuffComponent.Add(eid, buffEffectTarget);
                                    break;

                                case 2:
                                    eid = Entity.NextEntity();
                                    targetID = GetPlayerID();
                                    WeaponIncrease = 145;
                                    buffEffectTarget = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        WeaponStrength = WeaponIncrease
                                    };
                                    _game.BuffComponent.Add(eid, buffEffectTarget);
                                    break;

                                case 3:
                                    eid = Entity.NextEntity();
                                    targetID = GetPlayerID();
                                    WeaponIncrease = 130;
                                    buffEffectTarget = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        WeaponStrength = WeaponIncrease
                                    };
                                    _game.BuffComponent.Add(eid, buffEffectTarget);
                                    break;

                                case 4:
                                    eid = Entity.NextEntity();
                                    targetID = GetPlayerID();
                                    WeaponIncrease = 135;
                                    buffEffectTarget = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        WeaponStrength = WeaponIncrease
                                    };
                                    _game.BuffComponent.Add(eid, buffEffectTarget);
                                    break;

                                case 5:
                                    eid = Entity.NextEntity();
                                    targetID = GetPlayerID();
                                    WeaponIncrease = 145;
                                    buffEffectTarget = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        WeaponStrength = WeaponIncrease
                                    };
                                    _game.BuffComponent.Add(eid, buffEffectTarget);
                                    break;

                                case 6:
                                    eid = Entity.NextEntity();
                                    targetID = GetPlayerID();
                                    WeaponIncrease = 160;
                                    buffEffectTarget = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        WeaponStrength = WeaponIncrease
                                    };
                                    _game.BuffComponent.Add(eid, buffEffectTarget);
                                    break;

                                case 7:
                                    eid = Entity.NextEntity();
                                    targetID = GetPlayerID();
                                    WeaponIncrease = 175;
                                    buffEffectTarget = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        WeaponStrength = WeaponIncrease
                                    };
                                    _game.BuffComponent.Add(eid, buffEffectTarget);
                                    break;

                                case 8:
                                    eid = Entity.NextEntity();
                                    targetID = GetPlayerID();
                                    WeaponIncrease = 200;
                                    buffEffectTarget = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        WeaponStrength = WeaponIncrease
                                    };
                                    _game.BuffComponent.Add(eid, buffEffectTarget);
                                    break;

                                case 9:
                                    eid = Entity.NextEntity();
                                    targetID = GetPlayerID();
                                    WeaponIncrease = 225;
                                    buffEffectTarget = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        WeaponStrength = WeaponIncrease
                                    };
                                    _game.BuffComponent.Add(eid, buffEffectTarget);
                                    break;

                                case 10:
                                    eid = Entity.NextEntity();
                                    targetID = GetPlayerID();
                                    WeaponIncrease = 250;
                                    buffEffectTarget = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        WeaponStrength = WeaponIncrease
                                    };
                                    _game.BuffComponent.Add(eid, buffEffectTarget);
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        case SkillType.RepulsorArm:

                            #region Skill Variables
                            InstantEffect instantEffectRepulse;
                            uint eid_2Repulse;
                            uint targetIDRepulse;

                            KnockBack knockBackEffectRepulse;
                            Vector2 originRepulse;
                            float distanceRepulse;
                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
                                    eid = Entity.NextEntity();
                                    targetIDRepulse = GetPlayerID();
                                    eid_2Repulse = Entity.NextEntity();
                                    originRepulse = _game.PositionComponent[targetIDRepulse].Center;
                                    distanceRepulse = 100;


                                    instantEffectRepulse = new InstantEffect()
                                    {
                                        EntityID = eid_2Repulse,
                                    };
                                    _game.InstantEffectComponent.Add(eid_2Repulse, instantEffectRepulse);

                                    knockBackEffectRepulse = new KnockBack()
                                    {
                                        EntityID = eid_2Repulse,
                                        Origin = originRepulse,
                                        Distance = distanceRepulse,
                                    };
                                    _game.KnockBackComponent.Add(eid_2Repulse, knockBackEffectRepulse);
                                    break;

                                case 2:
                                    eid = Entity.NextEntity();
                                    targetIDRepulse = GetPlayerID();
                                    eid_2Repulse = Entity.NextEntity();
                                    originRepulse = _game.PositionComponent[targetIDRepulse].Center;
                                    distanceRepulse = 100;


                                    instantEffectRepulse = new InstantEffect()
                                    {
                                        EntityID = eid_2Repulse,
                                    };
                                    _game.InstantEffectComponent.Add(eid_2Repulse, instantEffectRepulse);

                                    knockBackEffectRepulse = new KnockBack()
                                    {
                                        EntityID = eid_2Repulse,
                                        Origin = originRepulse,
                                        Distance = distanceRepulse,
                                    };
                                    _game.KnockBackComponent.Add(eid_2Repulse, knockBackEffectRepulse);
                                    break;

                                case 3:
                                    eid = Entity.NextEntity();
                                    targetIDRepulse = GetPlayerID();
                                    eid_2Repulse = Entity.NextEntity();
                                    originRepulse = _game.PositionComponent[targetIDRepulse].Center;
                                    distanceRepulse = 100;


                                    instantEffectRepulse = new InstantEffect()
                                    {
                                        EntityID = eid_2Repulse,
                                    };
                                    _game.InstantEffectComponent.Add(eid_2Repulse, instantEffectRepulse);

                                    knockBackEffectRepulse = new KnockBack()
                                    {
                                        EntityID = eid_2Repulse,
                                        Origin = originRepulse,
                                        Distance = distanceRepulse,
                                    };
                                    _game.KnockBackComponent.Add(eid_2Repulse, knockBackEffectRepulse);
                                    break;

                                case 4:
                                    eid = Entity.NextEntity();
                                    targetIDRepulse = GetPlayerID();
                                    eid_2Repulse = Entity.NextEntity();
                                    originRepulse = _game.PositionComponent[targetIDRepulse].Center;
                                    distanceRepulse = 100;


                                    instantEffectRepulse = new InstantEffect()
                                    {
                                        EntityID = eid_2Repulse,
                                    };
                                    _game.InstantEffectComponent.Add(eid_2Repulse, instantEffectRepulse);

                                    knockBackEffectRepulse = new KnockBack()
                                    {
                                        EntityID = eid_2Repulse,
                                        Origin = originRepulse,
                                        Distance = distanceRepulse,
                                    };
                                    _game.KnockBackComponent.Add(eid_2Repulse, knockBackEffectRepulse);
                                    break;

                                case 5:
                                    eid = Entity.NextEntity();
                                    targetIDRepulse = GetPlayerID();
                                    eid_2Repulse = Entity.NextEntity();
                                    originRepulse = _game.PositionComponent[targetIDRepulse].Center;
                                    distanceRepulse = 100;


                                    instantEffectRepulse = new InstantEffect()
                                    {
                                        EntityID = eid_2Repulse,
                                    };
                                    _game.InstantEffectComponent.Add(eid_2Repulse, instantEffectRepulse);

                                    knockBackEffectRepulse = new KnockBack()
                                    {
                                        EntityID = eid_2Repulse,
                                        Origin = originRepulse,
                                        Distance = distanceRepulse,
                                    };
                                    _game.KnockBackComponent.Add(eid_2Repulse, knockBackEffectRepulse);
                                    break;

                                case 6:
                                    eid = Entity.NextEntity();
                                    targetIDRepulse = GetPlayerID();
                                    eid_2Repulse = Entity.NextEntity();
                                    originRepulse = _game.PositionComponent[targetIDRepulse].Center;
                                    distanceRepulse = 100;


                                    instantEffectRepulse = new InstantEffect()
                                    {
                                        EntityID = eid_2Repulse,
                                    };
                                    _game.InstantEffectComponent.Add(eid_2Repulse, instantEffectRepulse);

                                    knockBackEffectRepulse = new KnockBack()
                                    {
                                        EntityID = eid_2Repulse,
                                        Origin = originRepulse,
                                        Distance = distanceRepulse,
                                    };
                                    _game.KnockBackComponent.Add(eid_2Repulse, knockBackEffectRepulse);
                                    break;

                                case 7:
                                    eid = Entity.NextEntity();
                                    targetIDRepulse = GetPlayerID();
                                    eid_2Repulse = Entity.NextEntity();
                                    originRepulse = _game.PositionComponent[targetIDRepulse].Center;
                                    distanceRepulse = 100;


                                    instantEffectRepulse = new InstantEffect()
                                    {
                                        EntityID = eid_2Repulse,
                                    };
                                    _game.InstantEffectComponent.Add(eid_2Repulse, instantEffectRepulse);

                                    knockBackEffectRepulse = new KnockBack()
                                    {
                                        EntityID = eid_2Repulse,
                                        Origin = originRepulse,
                                        Distance = distanceRepulse,
                                    };
                                    _game.KnockBackComponent.Add(eid_2Repulse, knockBackEffectRepulse);
                                    break;

                                case 8:
                                    eid = Entity.NextEntity();
                                    targetIDRepulse = GetPlayerID();
                                    eid_2Repulse = Entity.NextEntity();
                                    originRepulse = _game.PositionComponent[targetIDRepulse].Center;
                                    distanceRepulse = 100;


                                    instantEffectRepulse = new InstantEffect()
                                    {
                                        EntityID = eid_2Repulse,
                                    };
                                    _game.InstantEffectComponent.Add(eid_2Repulse, instantEffectRepulse);

                                    knockBackEffectRepulse = new KnockBack()
                                    {
                                        EntityID = eid_2Repulse,
                                        Origin = originRepulse,
                                        Distance = distanceRepulse,
                                    };
                                    _game.KnockBackComponent.Add(eid_2Repulse, knockBackEffectRepulse);
                                    break;

                                case 9:
                                    eid = Entity.NextEntity();
                                    targetIDRepulse = GetPlayerID();
                                    eid_2Repulse = Entity.NextEntity();
                                    originRepulse = _game.PositionComponent[targetIDRepulse].Center;
                                    distanceRepulse = 100;


                                    instantEffectRepulse = new InstantEffect()
                                    {
                                        EntityID = eid_2Repulse,
                                    };
                                    _game.InstantEffectComponent.Add(eid_2Repulse, instantEffectRepulse);

                                    knockBackEffectRepulse = new KnockBack()
                                    {
                                        EntityID = eid_2Repulse,
                                        Origin = originRepulse,
                                        Distance = distanceRepulse,
                                    };
                                    _game.KnockBackComponent.Add(eid_2Repulse, knockBackEffectRepulse);
                                    break;

                                case 10:
                                    eid = Entity.NextEntity();
                                    targetIDRepulse = GetPlayerID();
                                    eid_2Repulse = Entity.NextEntity();
                                    originRepulse = _game.PositionComponent[targetIDRepulse].Center;
                                    distanceRepulse = 100;


                                    instantEffectRepulse = new InstantEffect()
                                    {
                                        EntityID = eid_2Repulse,
                                    };
                                    _game.InstantEffectComponent.Add(eid_2Repulse, instantEffectRepulse);

                                    knockBackEffectRepulse = new KnockBack()
                                    {
                                        EntityID = eid_2Repulse,
                                        Origin = originRepulse,
                                        Distance = distanceRepulse,
                                    };
                                    _game.KnockBackComponent.Add(eid_2Repulse, knockBackEffectRepulse);
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        case SkillType.EnergyShot:

                            #region Skill Variables
                            DirectDamage DirectDamageShot;
                            InstantEffect instantEffectShot;
                            int shotDamage;
                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
                                    eid = Entity.NextEntity();
                                    shotDamage = 5;

                                    instantEffectShot = new InstantEffect()
                                    {
                                        EntityID = eid,
                                    };
                                    _game.InstantEffectComponent.Add(eid, instantEffectShot);

                                    DirectDamageShot = new DirectDamage()
                                    {
                                        EntityID = eid,
                                        Damage = shotDamage
                                    };
                                    _game.DirectDamageComponent.Add(eid, DirectDamageShot);

                                    break;

                                case 2:
                                    eid = Entity.NextEntity();
                                    shotDamage = 10;

                                    instantEffectShot = new InstantEffect()
                                    {
                                        EntityID = eid,
                                    };
                                    _game.InstantEffectComponent.Add(eid, instantEffectShot);

                                    DirectDamageShot = new DirectDamage()
                                    {
                                        EntityID = eid,
                                        Damage = shotDamage
                                    };
                                    _game.DirectDamageComponent.Add(eid, DirectDamageShot);
                                    break;

                                case 3:
                                    eid = Entity.NextEntity();
                                    shotDamage = 15;

                                    instantEffectShot = new InstantEffect()
                                    {
                                        EntityID = eid,
                                    };
                                    _game.InstantEffectComponent.Add(eid, instantEffectShot);

                                    DirectDamageShot = new DirectDamage()
                                    {
                                        EntityID = eid,
                                        Damage = shotDamage
                                    };
                                    _game.DirectDamageComponent.Add(eid, DirectDamageShot);
                                    break;

                                case 4:
                                    eid = Entity.NextEntity();
                                    shotDamage = 20;

                                    instantEffectShot = new InstantEffect()
                                    {
                                        EntityID = eid,
                                    };
                                    _game.InstantEffectComponent.Add(eid, instantEffectShot);

                                    DirectDamageShot = new DirectDamage()
                                    {
                                        EntityID = eid,
                                        Damage = shotDamage
                                    };
                                    _game.DirectDamageComponent.Add(eid, DirectDamageShot);
                                    break;

                                case 5:
                                    eid = Entity.NextEntity();
                                    shotDamage = 25;

                                    instantEffectShot = new InstantEffect()
                                    {
                                        EntityID = eid,
                                    };
                                    _game.InstantEffectComponent.Add(eid, instantEffectShot);

                                    DirectDamageShot = new DirectDamage()
                                    {
                                        EntityID = eid,
                                        Damage = shotDamage
                                    };
                                    _game.DirectDamageComponent.Add(eid, DirectDamageShot);
                                    break;

                                case 6:
                                    eid = Entity.NextEntity();
                                    shotDamage = 30;

                                    instantEffectShot = new InstantEffect()
                                    {
                                        EntityID = eid,
                                    };
                                    _game.InstantEffectComponent.Add(eid, instantEffectShot);

                                    DirectDamageShot = new DirectDamage()
                                    {
                                        EntityID = eid,
                                        Damage = shotDamage
                                    };
                                    _game.DirectDamageComponent.Add(eid, DirectDamageShot);
                                    break;

                                case 7:
                                    eid = Entity.NextEntity();
                                    shotDamage = 35;

                                    instantEffectShot = new InstantEffect()
                                    {
                                        EntityID = eid,
                                    };
                                    _game.InstantEffectComponent.Add(eid, instantEffectShot);

                                    DirectDamageShot = new DirectDamage()
                                    {
                                        EntityID = eid,
                                        Damage = shotDamage
                                    };
                                    _game.DirectDamageComponent.Add(eid, DirectDamageShot);
                                    break;

                                case 8:
                                    eid = Entity.NextEntity();
                                    shotDamage = 38;

                                    instantEffectShot = new InstantEffect()
                                    {
                                        EntityID = eid,
                                    };
                                    _game.InstantEffectComponent.Add(eid, instantEffectShot);

                                    DirectDamageShot = new DirectDamage()
                                    {
                                        EntityID = eid,
                                        Damage = shotDamage
                                    };
                                    _game.DirectDamageComponent.Add(eid, DirectDamageShot);
                                    break;

                                case 9:
                                    eid = Entity.NextEntity();
                                    shotDamage = 40;

                                    instantEffectShot = new InstantEffect()
                                    {
                                        EntityID = eid,
                                    };
                                    _game.InstantEffectComponent.Add(eid, instantEffectShot);

                                    DirectDamageShot = new DirectDamage()
                                    {
                                        EntityID = eid,
                                        Damage = shotDamage
                                    };
                                    _game.DirectDamageComponent.Add(eid, DirectDamageShot);
                                    break;

                                case 10:
                                    eid = Entity.NextEntity();
                                    shotDamage = 45;

                                    instantEffectShot = new InstantEffect()
                                    {
                                        EntityID = eid,
                                    };
                                    _game.InstantEffectComponent.Add(eid, instantEffectShot);

                                    DirectDamageShot = new DirectDamage()
                                    {
                                        EntityID = eid,
                                        Damage = shotDamage
                                    };
                                    _game.DirectDamageComponent.Add(eid, DirectDamageShot);
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        case SkillType.AlloyBody:

                            #region Skill Variables

                            Buff buffEffectAlloy;
                            int damageDecrease;
                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
                                    eid = Entity.NextEntity();
                                    targetID = GetPlayerID();
                                    damageDecrease = 5;
                                    buffEffectAlloy = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        DefenseMelee = damageDecrease,
                                        DefenseRanged = damageDecrease
                                    };
                                    _game.BuffComponent.Add(eid, buffEffectAlloy);
                                    break;

                                case 2:
                                    eid = Entity.NextEntity();
                                    targetID = GetPlayerID();
                                    damageDecrease = 10;
                                    buffEffectAlloy = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        DefenseMelee = damageDecrease,
                                        DefenseRanged = damageDecrease
                                    };
                                    _game.BuffComponent.Add(eid, buffEffectAlloy);
                                    break;

                                case 3:
                                    eid = Entity.NextEntity();
                                    targetID = GetPlayerID();
                                    damageDecrease = 12;
                                    buffEffectAlloy = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        DefenseMelee = damageDecrease,
                                        DefenseRanged = damageDecrease
                                    };
                                    _game.BuffComponent.Add(eid, buffEffectAlloy);
                                    break;

                                case 4:
                                    eid = Entity.NextEntity();
                                    targetID = GetPlayerID();
                                    damageDecrease = 14;
                                    buffEffectAlloy = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        DefenseMelee = damageDecrease,
                                        DefenseRanged = damageDecrease
                                    };
                                    _game.BuffComponent.Add(eid, buffEffectAlloy);
                                    break;

                                case 5:
                                    eid = Entity.NextEntity();
                                    targetID = GetPlayerID();
                                    damageDecrease = 16;
                                    buffEffectAlloy = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        DefenseMelee = damageDecrease,
                                        DefenseRanged = damageDecrease
                                    };
                                    _game.BuffComponent.Add(eid, buffEffectAlloy);
                                    break;

                                case 6:
                                    eid = Entity.NextEntity();
                                    targetID = GetPlayerID();
                                    damageDecrease = 18;
                                    buffEffectAlloy = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        DefenseMelee = damageDecrease,
                                        DefenseRanged = damageDecrease
                                    };
                                    _game.BuffComponent.Add(eid, buffEffectAlloy);
                                    break;

                                case 7:
                                    eid = Entity.NextEntity();
                                    targetID = GetPlayerID();
                                    damageDecrease = 20;
                                    buffEffectAlloy = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        DefenseMelee = damageDecrease,
                                        DefenseRanged = damageDecrease
                                    };
                                    _game.BuffComponent.Add(eid, buffEffectAlloy);
                                    break;

                                case 8:
                                    eid = Entity.NextEntity();
                                    targetID = GetPlayerID();
                                    damageDecrease = 26;
                                    buffEffectAlloy = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        DefenseMelee = damageDecrease,
                                        DefenseRanged = damageDecrease
                                    };
                                    _game.BuffComponent.Add(eid, buffEffectAlloy);
                                    break;

                                case 9:
                                    eid = Entity.NextEntity();
                                    targetID = GetPlayerID();
                                    damageDecrease = 32;
                                    buffEffectAlloy = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        DefenseMelee = damageDecrease,
                                        DefenseRanged = damageDecrease
                                    };
                                    _game.BuffComponent.Add(eid, buffEffectAlloy);
                                    break;

                                case 10:
                                    eid = Entity.NextEntity();
                                    targetID = GetPlayerID();
                                    damageDecrease = 40;
                                    buffEffectAlloy = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        DefenseMelee = damageDecrease,
                                        DefenseRanged = damageDecrease
                                    };
                                    _game.BuffComponent.Add(eid, buffEffectAlloy);
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        case SkillType.CyberneticSlam:

                            #region Skill Variables
                            InstantEffect instantEffectSlam;
                            uint eid_2Slam;
                            uint targetIDSlam;

                            DirectDamage DirectDamageSlam;
                            int slamDamage;

                            KnockBack knockBackEffectSlam;
                            Vector2 originSlam;
                            float distanceSlam;
                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
                                    eid = Entity.NextEntity();
                                    targetIDSlam = GetPlayerID();
                                    eid_2Slam = Entity.NextEntity();
                                    originSlam = _game.PositionComponent[targetIDSlam].Center;
                                    distanceSlam = 40;
                                    slamDamage = 5;

                                    instantEffectSlam = new InstantEffect()
                                    {
                                        EntityID = eid_2Slam,
                                    };
                                    _game.InstantEffectComponent.Add(eid_2Slam, instantEffectSlam);

                                    DirectDamageSlam = new DirectDamage()
                                    {
                                        EntityID = eid_2Slam,
                                        Damage = slamDamage
                                    };
                                    _game.DirectDamageComponent.Add(eid_2Slam, DirectDamageSlam);

                                    knockBackEffectSlam = new KnockBack()
                                    {
                                        EntityID = eid_2Slam,
                                        Origin = originSlam,
                                        Distance = distanceSlam,
                                    };
                                    _game.KnockBackComponent.Add(eid_2Slam, knockBackEffectSlam);
                                    break;

                                case 2:
                                    eid = Entity.NextEntity();
                                    targetIDSlam = GetPlayerID();
                                    eid_2Slam = Entity.NextEntity();
                                    originSlam = _game.PositionComponent[targetIDSlam].Center;
                                    distanceSlam = 60;
                                    slamDamage = 10;


                                    instantEffectSlam = new InstantEffect()
                                    {
                                        EntityID = eid_2Slam,
                                    };
                                    _game.InstantEffectComponent.Add(eid_2Slam, instantEffectSlam);

                                    DirectDamageSlam = new DirectDamage()
                                    {
                                        EntityID = eid_2Slam,
                                        Damage = slamDamage
                                    };
                                    _game.DirectDamageComponent.Add(eid_2Slam, DirectDamageSlam);

                                    knockBackEffectSlam = new KnockBack()
                                    {
                                        EntityID = eid_2Slam,
                                        Origin = originSlam,
                                        Distance = distanceSlam,
                                    };
                                    _game.KnockBackComponent.Add(eid_2Slam, knockBackEffectSlam);
                                    break;

                                case 3:
                                    eid = Entity.NextEntity();
                                    targetIDSlam = GetPlayerID();
                                    eid_2Slam = Entity.NextEntity();
                                    originSlam = _game.PositionComponent[targetIDSlam].Center;
                                    distanceSlam = 80;
                                    slamDamage = 15;


                                    instantEffectSlam = new InstantEffect()
                                    {
                                        EntityID = eid_2Slam,
                                    };
                                    _game.InstantEffectComponent.Add(eid_2Slam, instantEffectSlam);

                                    DirectDamageSlam = new DirectDamage()
                                    {
                                        EntityID = eid_2Slam,
                                        Damage = slamDamage
                                    };
                                    _game.DirectDamageComponent.Add(eid_2Slam, DirectDamageSlam);

                                    knockBackEffectSlam = new KnockBack()
                                    {
                                        EntityID = eid_2Slam,
                                        Origin = originSlam,
                                        Distance = distanceSlam,
                                    };
                                    _game.KnockBackComponent.Add(eid_2Slam, knockBackEffectSlam);
                                    break;

                                case 4:
                                    eid = Entity.NextEntity();
                                    targetIDSlam = GetPlayerID();
                                    eid_2Slam = Entity.NextEntity();
                                    originSlam = _game.PositionComponent[targetIDSlam].Center;
                                    distanceSlam = 100;
                                    slamDamage = 20;

                                    instantEffectSlam = new InstantEffect()
                                    {
                                        EntityID = eid_2Slam,
                                    };
                                    _game.InstantEffectComponent.Add(eid_2Slam, instantEffectSlam);

                                    DirectDamageSlam = new DirectDamage()
                                    {
                                        EntityID = eid_2Slam,
                                        Damage = slamDamage
                                    };
                                    _game.DirectDamageComponent.Add(eid_2Slam, DirectDamageSlam);

                                    knockBackEffectSlam = new KnockBack()
                                    {
                                        EntityID = eid_2Slam,
                                        Origin = originSlam,
                                        Distance = distanceSlam,
                                    };
                                    _game.KnockBackComponent.Add(eid_2Slam, knockBackEffectSlam);
                                    break;

                                case 5:
                                    eid = Entity.NextEntity();
                                    targetIDSlam = GetPlayerID();
                                    eid_2Slam = Entity.NextEntity();
                                    originSlam = _game.PositionComponent[targetIDSlam].Center;
                                    distanceSlam = 120;
                                    slamDamage = 25;

                                    instantEffectSlam = new InstantEffect()
                                    {
                                        EntityID = eid_2Slam,
                                    };
                                    _game.InstantEffectComponent.Add(eid_2Slam, instantEffectSlam);

                                    DirectDamageSlam = new DirectDamage()
                                    {
                                        EntityID = eid_2Slam,
                                        Damage = slamDamage
                                    };
                                    _game.DirectDamageComponent.Add(eid_2Slam, DirectDamageSlam);

                                    knockBackEffectSlam = new KnockBack()
                                    {
                                        EntityID = eid_2Slam,
                                        Origin = originSlam,
                                        Distance = distanceSlam,
                                    };
                                    _game.KnockBackComponent.Add(eid_2Slam, knockBackEffectSlam);
                                    break;

                                case 6:
                                    eid = Entity.NextEntity();
                                    targetIDSlam = GetPlayerID();
                                    eid_2Slam = Entity.NextEntity();
                                    originSlam = _game.PositionComponent[targetIDSlam].Center;
                                    distanceSlam = 140;
                                    slamDamage = 30;

                                    instantEffectSlam = new InstantEffect()
                                    {
                                        EntityID = eid_2Slam,
                                    };
                                    _game.InstantEffectComponent.Add(eid_2Slam, instantEffectSlam);

                                    DirectDamageSlam = new DirectDamage()
                                    {
                                        EntityID = eid_2Slam,
                                        Damage = slamDamage
                                    };
                                    _game.DirectDamageComponent.Add(eid_2Slam, DirectDamageSlam);

                                    knockBackEffectSlam = new KnockBack()
                                    {
                                        EntityID = eid_2Slam,
                                        Origin = originSlam,
                                        Distance = distanceSlam,
                                    };
                                    _game.KnockBackComponent.Add(eid_2Slam, knockBackEffectSlam);
                                    break;

                                case 7:
                                    eid = Entity.NextEntity();
                                    targetIDSlam = GetPlayerID();
                                    eid_2Slam = Entity.NextEntity();
                                    originSlam = _game.PositionComponent[targetIDSlam].Center;
                                    distanceSlam = 160;
                                    slamDamage = 35;

                                    instantEffectSlam = new InstantEffect()
                                    {
                                        EntityID = eid_2Slam,
                                    };
                                    _game.InstantEffectComponent.Add(eid_2Slam, instantEffectSlam);

                                    DirectDamageSlam = new DirectDamage()
                                    {
                                        EntityID = eid_2Slam,
                                        Damage = slamDamage
                                    };
                                    _game.DirectDamageComponent.Add(eid_2Slam, DirectDamageSlam);

                                    knockBackEffectSlam = new KnockBack()
                                    {
                                        EntityID = eid_2Slam,
                                        Origin = originSlam,
                                        Distance = distanceSlam,
                                    };
                                    _game.KnockBackComponent.Add(eid_2Slam, knockBackEffectSlam);
                                    break;

                                case 8:
                                    eid = Entity.NextEntity();
                                    targetIDSlam = GetPlayerID();
                                    eid_2Slam = Entity.NextEntity();
                                    originSlam = _game.PositionComponent[targetIDSlam].Center;
                                    distanceSlam = 180;
                                    slamDamage = 40;

                                    instantEffectSlam = new InstantEffect()
                                    {
                                        EntityID = eid_2Slam,
                                    };
                                    _game.InstantEffectComponent.Add(eid_2Slam, instantEffectSlam);

                                    DirectDamageSlam = new DirectDamage()
                                    {
                                        EntityID = eid_2Slam,
                                        Damage = slamDamage
                                    };
                                    _game.DirectDamageComponent.Add(eid_2Slam, DirectDamageSlam);

                                    knockBackEffectSlam = new KnockBack()
                                    {
                                        EntityID = eid_2Slam,
                                        Origin = originSlam,
                                        Distance = distanceSlam,
                                    };
                                    _game.KnockBackComponent.Add(eid_2Slam, knockBackEffectSlam);
                                    break;

                                case 9:
                                    eid = Entity.NextEntity();
                                    targetIDSlam = GetPlayerID();
                                    eid_2Slam = Entity.NextEntity();
                                    originSlam = _game.PositionComponent[targetIDSlam].Center;
                                    distanceSlam = 200;
                                    slamDamage = 45;

                                    instantEffectSlam = new InstantEffect()
                                    {
                                        EntityID = eid_2Slam,
                                    };
                                    _game.InstantEffectComponent.Add(eid_2Slam, instantEffectSlam);

                                    DirectDamageSlam = new DirectDamage()
                                    {
                                        EntityID = eid_2Slam,
                                        Damage = slamDamage
                                    };
                                    _game.DirectDamageComponent.Add(eid_2Slam, DirectDamageSlam);

                                    knockBackEffectSlam = new KnockBack()
                                    {
                                        EntityID = eid_2Slam,
                                        Origin = originSlam,
                                        Distance = distanceSlam,
                                    };
                                    _game.KnockBackComponent.Add(eid_2Slam, knockBackEffectSlam);
                                    break;

                                case 10:
                                    eid = Entity.NextEntity();
                                    targetIDSlam = GetPlayerID();
                                    eid_2Slam = Entity.NextEntity();
                                    originSlam = _game.PositionComponent[targetIDSlam].Center;
                                    distanceSlam = 220;
                                    slamDamage = 50;

                                    instantEffectSlam = new InstantEffect()
                                    {
                                        EntityID = eid_2Slam,
                                    };
                                    _game.InstantEffectComponent.Add(eid_2Slam, instantEffectSlam);

                                    DirectDamageSlam = new DirectDamage()
                                    {
                                        EntityID = eid_2Slam,
                                        Damage = slamDamage
                                    };
                                    _game.DirectDamageComponent.Add(eid_2Slam, DirectDamageSlam);

                                    knockBackEffectSlam = new KnockBack()
                                    {
                                        EntityID = eid_2Slam,
                                        Origin = originSlam,
                                        Distance = distanceSlam,
                                    };
                                    _game.KnockBackComponent.Add(eid_2Slam, knockBackEffectSlam);
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;

                        case SkillType.ThrusterRush:

                            #region Skill Variables
                            TimedEffect timedEffectRush;
                            float effectDurationRush;

                            Buff buffEffectRush;
                            uint targetIDRush;
                            int speedIncreaseRush;
                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
                                    eid = Entity.NextEntity();
                                    effectDurationRush = 1;
                                    targetIDRush = GetPlayerID();
                                    speedIncreaseRush = 600;

                                    timedEffectRush = new TimedEffect()
                                    {
                                        EntityID = eid,
                                        TotalDuration = effectDurationRush,
                                        TimeLeft = effectDurationRush,
                                    };
                                    _game.TimedEffectComponent.Add(eid, timedEffectRush);

                                    buffEffectRush = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetIDRush,
                                        MovementSpeed = speedIncreaseRush,
                                    };
                                    _game.BuffComponent.Add(eid, buffEffectRush);
                                    break;

                                case 2:
                                    eid = Entity.NextEntity();
                                    effectDurationRush = 1;
                                    targetIDRush = GetPlayerID();
                                    speedIncreaseRush = 600;

                                    timedEffectRush = new TimedEffect()
                                    {
                                        EntityID = eid,
                                        TotalDuration = effectDurationRush,
                                        TimeLeft = effectDurationRush,
                                    };
                                    _game.TimedEffectComponent.Add(eid, timedEffectRush);

                                    buffEffectRush = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetIDRush,
                                        MovementSpeed = speedIncreaseRush,
                                    };
                                    _game.BuffComponent.Add(eid, buffEffectRush);
                                    break;

                                case 3:
                                    eid = Entity.NextEntity();
                                    effectDurationRush = 1;
                                    targetIDRush = GetPlayerID();
                                    speedIncreaseRush = 600;

                                    timedEffectRush = new TimedEffect()
                                    {
                                        EntityID = eid,
                                        TotalDuration = effectDurationRush,
                                        TimeLeft = effectDurationRush,
                                    };
                                    _game.TimedEffectComponent.Add(eid, timedEffectRush);

                                    buffEffectRush = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetIDRush,
                                        MovementSpeed = speedIncreaseRush,
                                    };
                                    _game.BuffComponent.Add(eid, buffEffectRush);
                                    break;

                                case 4:
                                    eid = Entity.NextEntity();
                                    effectDurationRush = 1;
                                    targetIDRush = GetPlayerID();
                                    speedIncreaseRush = 600;

                                    timedEffectRush = new TimedEffect()
                                    {
                                        EntityID = eid,
                                        TotalDuration = effectDurationRush,
                                        TimeLeft = effectDurationRush,
                                    };
                                    _game.TimedEffectComponent.Add(eid, timedEffectRush);

                                    buffEffectRush = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetIDRush,
                                        MovementSpeed = speedIncreaseRush,
                                    };
                                    _game.BuffComponent.Add(eid, buffEffectRush);
                                    break;

                                case 5:
                                    eid = Entity.NextEntity();
                                    effectDurationRush = 1;
                                    targetIDRush = GetPlayerID();
                                    speedIncreaseRush = 600;

                                    timedEffectRush = new TimedEffect()
                                    {
                                        EntityID = eid,
                                        TotalDuration = effectDurationRush,
                                        TimeLeft = effectDurationRush,
                                    };
                                    _game.TimedEffectComponent.Add(eid, timedEffectRush);

                                    buffEffectRush = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetIDRush,
                                        MovementSpeed = speedIncreaseRush,
                                    };
                                    _game.BuffComponent.Add(eid, buffEffectRush);
                                    break;

                                case 6:
                                    eid = Entity.NextEntity();
                                    effectDurationRush = 1;
                                    targetIDRush = GetPlayerID();
                                    speedIncreaseRush = 600;

                                    timedEffectRush = new TimedEffect()
                                    {
                                        EntityID = eid,
                                        TotalDuration = effectDurationRush,
                                        TimeLeft = effectDurationRush,
                                    };
                                    _game.TimedEffectComponent.Add(eid, timedEffectRush);

                                    buffEffectRush = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetIDRush,
                                        MovementSpeed = speedIncreaseRush,
                                    };
                                    _game.BuffComponent.Add(eid, buffEffectRush);
                                    break;

                                case 7:
                                    eid = Entity.NextEntity();
                                    effectDurationRush = 1;
                                    targetIDRush = GetPlayerID();
                                    speedIncreaseRush = 600;

                                    timedEffectRush = new TimedEffect()
                                    {
                                        EntityID = eid,
                                        TotalDuration = effectDurationRush,
                                        TimeLeft = effectDurationRush,
                                    };
                                    _game.TimedEffectComponent.Add(eid, timedEffectRush);

                                    buffEffectRush = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetIDRush,
                                        MovementSpeed = speedIncreaseRush,
                                    };
                                    _game.BuffComponent.Add(eid, buffEffectRush);
                                    break;

                                case 8:
                                    eid = Entity.NextEntity();
                                    effectDurationRush = 1;
                                    targetIDRush = GetPlayerID();
                                    speedIncreaseRush = 600;

                                    timedEffectRush = new TimedEffect()
                                    {
                                        EntityID = eid,
                                        TotalDuration = effectDurationRush,
                                        TimeLeft = effectDurationRush,
                                    };
                                    _game.TimedEffectComponent.Add(eid, timedEffectRush);

                                    buffEffectRush = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetIDRush,
                                        MovementSpeed = speedIncreaseRush,
                                    };
                                    _game.BuffComponent.Add(eid, buffEffectRush);
                                    break;

                                case 9:
                                    eid = Entity.NextEntity();
                                    effectDurationRush = 1;
                                    targetIDRush = GetPlayerID();
                                    speedIncreaseRush = 600;

                                    timedEffectRush = new TimedEffect()
                                    {
                                        EntityID = eid,
                                        TotalDuration = effectDurationRush,
                                        TimeLeft = effectDurationRush,
                                    };
                                    _game.TimedEffectComponent.Add(eid, timedEffectRush);

                                    buffEffectRush = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetIDRush,
                                        MovementSpeed = speedIncreaseRush,
                                    };
                                    _game.BuffComponent.Add(eid, buffEffectRush);
                                    break;

                                case 10:
                                    eid = Entity.NextEntity();
                                    effectDurationRush = 1;
                                    targetIDRush = GetPlayerID();
                                    speedIncreaseRush = 600;

                                    timedEffectRush = new TimedEffect()
                                    {
                                        EntityID = eid,
                                        TotalDuration = effectDurationRush,
                                        TimeLeft = effectDurationRush,
                                    };
                                    _game.TimedEffectComponent.Add(eid, timedEffectRush);

                                    buffEffectRush = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetIDRush,
                                        MovementSpeed = speedIncreaseRush,
                                    };
                                    _game.BuffComponent.Add(eid, buffEffectRush);
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

                        case SkillType.Teleport:
                            {
                                #region Skill Variables
                                int psiCost = (int)(_game.StatsComponent[userID].PsiBase * .05);
                                int distance = 300;
                                #endregion

                                switch (rank)
                                {
                                    #region Checking Rank
                                    case 1:
                                        psiCost += (int)(_game.StatsComponent[userID].PsiBase * .5);
                                        break;

                                    case 2:
                                        psiCost += (int)(_game.StatsComponent[userID].PsiBase * .45);
                                        break;

                                    case 3:
                                        psiCost += (int)(_game.StatsComponent[userID].PsiBase * .40);
                                        break;

                                    case 4:
                                        psiCost += (int)(_game.StatsComponent[userID].PsiBase * .35);
                                        break;

                                    case 5:
                                        psiCost += (int)(_game.StatsComponent[userID].PsiBase * .30);
                                        break;

                                    case 6:
                                        psiCost += (int)(_game.StatsComponent[userID].PsiBase * .25);
                                        break;

                                    case 7:
                                        psiCost += (int)(_game.StatsComponent[userID].PsiBase * .20);
                                        break;

                                    case 8:
                                        psiCost += (int)(_game.StatsComponent[userID].PsiBase * .15);
                                        break;

                                    case 9:
                                        psiCost += (int)(_game.StatsComponent[userID].PsiBase * .10);
                                        break;

                                    case 10:
                                        psiCost += (int)(_game.StatsComponent[userID].PsiBase * .05);
                                        break;

                                    default:
                                        break;
                                    #endregion
                                }
                                
                                #region Logic
                                if(DrainPsiOrFatigue(userID, psiCost))
                                {
                                    //a new eid for the animation
                                    uint entityId = Entity.NextEntity();

                                    //need to get your old position and which direction you were facing
                                    Position pos = _game.PositionComponent[userID];
                                    Facing facing = (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow;
                                    
                                    //create the animation for the after effect
                                    SpriteAnimation animation = new SpriteAnimation()
                                    {
                                        EntityID = entityId,
                                        IsLooping = false,
                                        CurrentFrame = 0,
                                        CurrentAnimationRow = (int)facing,
                                        FramesPerSecond = 15,
                                        IsPlaying = true,
                                        TimePassed = 0
                                    };
                                    _game.SpriteAnimationComponent[entityId] = animation;

                                    //give the after effect a position
                                    Position animationPos = new Position()
                                    {
                                        EntityID = entityId,
                                        Center = new Vector2(pos.Center.X - 32, pos.Center.Y - 32),
                                        Radius = 0,
                                        RoomID = _game.PositionComponent[userID].RoomID
                                    };
                                    _game.PositionComponent[entityId] = animationPos;

                                    //set the spritesheet for the after effect
                                    Texture2D spriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/Invis");
                                    spriteSheet.Name = "Spritesheets/Skills/Effects/Invis";

                                    //set up the sprite for the after effect
                                    Sprite sprite = new Sprite()
                                    {
                                        EntityID = entityId,
                                        SpriteBounds = new Rectangle(0, 0, 64, 64),
                                        SpriteColor = new Color(255, 255, 255, 255),
                                        SpriteSheet = spriteSheet,
                                        UseDifferentColor = false,
                                    };
                                    _game.SpriteComponent[entityId] = sprite;


                                    //allow the after effect to expire
                                    TimedEffect timedEffect = new TimedEffect()
                                    {
                                        EntityID = entityId,
                                        TotalDuration = 1,
                                        TimeLeft = 1
                                    };
                                    _game.TimedEffectComponent.Add(entityId, timedEffect);

                                    //depending at which direction the character is facing, move them in that direction
                                    switch (facing)
                                    {
                                        case Facing.North:
                                            pos.Center.Y -= distance;
                                            if (pos.Center.Y <= 0)
                                                pos.Center.Y = 5;
                                            break;
                                        case Facing.East:
                                            pos.Center.X += distance;
                                            if (pos.Center.X >= _game.GraphicsDevice.Viewport.Width)
                                                pos.Center.X = _game.GraphicsDevice.Viewport.Width - 5;
                                            break;
                                        case Facing.South:
                                            pos.Center.Y += distance;
                                            if (pos.Center.Y >= _game.GraphicsDevice.Viewport.Height)
                                                pos.Center.Y = _game.GraphicsDevice.Viewport.Height - 5;
                                            break;
                                        case Facing.West:
                                            pos.Center.X -= distance;
                                            if (pos.Center.X <= 0)
                                                pos.Center.X = 5;
                                            break;
                                    }

                                    //update their position
                                    _game.PositionComponent[userID] = pos;

                                    //check for collision with static objects
                                    _game.CollisionSystem.CheckTeleportCollision(userID, facing);
                                    
                                }
                                #endregion
                                break;
                            }

                        case SkillType.Invisibility:
                            {
                                #region Skill Variables
                                int duration = 0;
                                int psiCost = (int)(_game.StatsComponent[userID].PsiBase * .05);
                                #endregion

                                switch (rank)
                                {
                                    #region Checking Rank
                                    case 1:
                                        duration = 2;
                                        break;

                                    case 2:
                                        duration = 4;                                       
                                        break;

                                    case 3:
                                        duration = 6;
                                        break;

                                    case 4:
                                        duration = 8;
                                        break;

                                    case 5:
                                        duration = 10;
                                        break;

                                    case 6:
                                        duration = 12;
                                        break;

                                    case 7:
                                        duration = 14;
                                        break;

                                    case 8:
                                        duration = 16;
                                        break;

                                    case 9:
                                        duration = 18;
                                        break;

                                    case 10:
                                        duration = 20;
                                        break;

                                    default:
                                        break;
                                    #endregion
                                }
                                #region Logic
                                if (DrainPsiOrFatigue(userID, psiCost))
                                {
                                    eid = Entity.NextEntity();

                                    TimedEffect timedEffect;
                                    timedEffect = new TimedEffect()
                                    {
                                        EntityID = eid,
                                        TotalDuration = duration,
                                        TimeLeft = duration
                                    };
                                    _game.TimedEffectComponent.Add(eid, timedEffect);

                                    AgroDrop agroDrop = new AgroDrop()
                                    {
                                        EntityID = eid,
                                        PlayerID = userID
                                    };
                                    _game.AgroDropComponent.Add(eid, agroDrop);
                                    

                                    ChangeVisibility changeVisibility;
                                    changeVisibility = new ChangeVisibility()
                                    {
                                        EntityID = eid,
                                        TargetID = userID,
                                        newColor = new Color(45, 45, 45, 0)
                                    };
                                    _game.ChangeVisibilityComponent.Add(eid, changeVisibility);
                                }
                                #endregion
                                break;
                            }

                        case SkillType.Meditate:
                            {
                                #region Skill Variables
                                float psiAmount = (float)(_game.StatsComponent[userID].PsiBase * .01);
                                int duration = 5;
                                #endregion

                                switch (rank)
                                {
                                    #region Checking Rank
                                    case 1:
                                        psiAmount += (float)(_game.StatsComponent[userID].PsiBase * .02);
                                        break;

                                    case 2:
                                        psiAmount += (float)(_game.StatsComponent[userID].PsiBase * .04);
                                        break;

                                    case 3:
                                        psiAmount += (float)(_game.StatsComponent[userID].PsiBase * .06);
                                        break;

                                    case 4:
                                        psiAmount += (float)(_game.StatsComponent[userID].PsiBase * .08);
                                        break;

                                    case 5:
                                        psiAmount += (float)(_game.StatsComponent[userID].PsiBase * .10);
                                        break;

                                    case 6:
                                        psiAmount += (float)(_game.StatsComponent[userID].PsiBase * .12);
                                        break;

                                    case 7:
                                        psiAmount += (float)(_game.StatsComponent[userID].PsiBase * .14);
                                        break;

                                    case 8:
                                        psiAmount += (float)(_game.StatsComponent[userID].PsiBase * .16);
                                        break;

                                    case 9:
                                        psiAmount += (float)(_game.StatsComponent[userID].PsiBase * .18);
                                        break;

                                    case 10:
                                        psiAmount += (float)(_game.StatsComponent[userID].PsiBase * .2);
                                        break;

                                    default:
                                        break;
                                }
                                    #endregion

                                    #region logic
                                        
                                         uint entityId = Entity.NextEntity();

                                         TimedEffect timed = new TimedEffect()
                                         {
                                             EntityID = entityId,
                                             TimeLeft = duration,
                                             TotalDuration = duration
                                         };
                                         _game.TimedEffectComponent[entityId] = timed;

                                         PsiOrFatigueRegen regen = new PsiOrFatigueRegen()
                                         {
                                             EntityID = entityId,
                                             TargetID = userID,
                                             AmountPerTick = psiAmount,
                                             CurrentTime = 1,
                                             TickTime = 1
                                         };
                                         _game.PsiOrFatigueRegenComponent[entityId] = regen;

                                    #endregion
                                break;
                            }
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

                        case SkillType.ImprovedPsionicSpear:

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
                //Implementation for the Cultist Player ~Nick B.
                //Useful - Regex for separating rank cases into regions
                //Replace: break;.*:Cc:Cc
                //With: break;\n#endregion\n\n#region Rank \n
                //Turns
                //      break;
                //
                //      case 2:
                //Into
                //      break;
                //      #endregion
                //      
                //      #region Rank 
                //      case 2:
                //
                // All thats needed is to add #region Rank 1
                // and an #endregion after case 10
                // and put in the rank numbers
                // ~Nick B.
                case Aggregate.CultistPlayer:
                    {
                        #region Race Variables
                        int test;
                        #endregion

                        switch (skillType)
                        {
                            #region Checking Skill Type

                            case SkillType.Enslave: //Projectile
                                {
                                    #region Skill Logic

                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.Enslave,
                                        (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                        _game.PositionComponent[userID], rank, 300, userID);

                                    #endregion

                                    break;
                                }

                            case SkillType.Fear: //AOE
                                {
                                    #region Skill Variables
                                    int fearRange = 0;
                                    #endregion

                                    #region Skill Logic

                                    switch (rank)
                                    {
                                        #region Checking Rank

                                        #region Rank 1
                                        case 1:
                                            fearRange = 10;
                                            break;
                                        #endregion

                                        #region Rank 2
                                        case 2:
                                            fearRange = 13;
                                            break;
                                        #endregion

                                        #region Rank 3
                                        case 3:
                                            fearRange = 17;
                                            break;
                                        #endregion

                                        #region Rank 4
                                        case 4:
                                            fearRange = 25;
                                            break;
                                        #endregion

                                        #region Rank 5
                                        case 5:
                                            fearRange = 30;
                                            break;
                                        #endregion

                                        #region Rank 6
                                        case 6:
                                            fearRange = 37;
                                            break;
                                        #endregion

                                        #region Rank 7
                                        case 7:
                                            fearRange = 48;
                                            break;
                                        #endregion

                                        #region Rank 8
                                        case 8:
                                            fearRange = 55;
                                            break;
                                        #endregion

                                        #region Rank 9
                                        case 9:
                                            fearRange = 67;
                                            break;
                                        #endregion

                                        #region Rank 10
                                        case 10:
                                            fearRange = 90;
                                            break;
                                        #endregion

                                        default:
                                            break;
                                        #endregion
                                    }

                                    _game.SkillEntityFactory.CreateSkillAoE(SkillType.Fear,
                                        _game.PositionComponent[userID], rank, fearRange);

                                    #endregion

                                    break;
                                }

                            case SkillType.Sacrifice: //Instant
                                {
                                    #region Skill Variables

                                    InstantEffect instantEffect;
                                    DirectHeal directHeal;
                                    DirectDamage directDamage;

                                    uint enslavedEnemyID = int.MaxValue;
                                    float damageValue = 0, healValue = 0;

                                    #endregion

                                    #region Skill Logic

                                    //Getting whatever is the first monster enslaved by this player
                                    //Alternatively, could sacrifice all enslaved monsters
                                    foreach (Enslave effect in _game.EnslaveComponent.All)
                                    {
                                        if (effect.OwnerID == userID && _game.EnemyComponent.Contains(effect.TargetID))
                                        {
                                            enslavedEnemyID = effect.TargetID;
                                            break;
                                        }
                                    }

                                    if (!_game.EnemyComponent.Contains(enslavedEnemyID)) return;

                                    damageValue = _game.EnemyComponent[enslavedEnemyID].Health;

                                    switch (rank)
                                    {
                                        #region Checking Rank

                                        #region Rank 1
                                        case 1:
                                            healValue = damageValue * 0.10f; //10% of remaining life
                                            break;
                                        #endregion

                                        #region Rank 2
                                        case 2:
                                            healValue = damageValue * 0.15f; //15% of remaining life
                                            break;
                                        #endregion

                                        #region Rank 3
                                        case 3:
                                            healValue = damageValue * 0.20f; //20% of remaining life
                                            break;
                                        #endregion

                                        #region Rank 4
                                        case 4:
                                            healValue = damageValue * 0.25f; //25% of remaining life
                                            break;
                                        #endregion

                                        #region Rank 5
                                        case 5:
                                            healValue = damageValue * 0.35f; //35% of remaining life
                                            break;
                                        #endregion

                                        #region Rank 6
                                        case 6:
                                            healValue = damageValue * 0.50f; //50% of remaining life
                                            break;
                                        #endregion

                                        #region Rank 7
                                        case 7:
                                            healValue = damageValue * 0.80f; //80% of remaining life
                                            break;
                                        #endregion

                                        #region Rank 8
                                        case 8:
                                            healValue = damageValue * 1.00f; //100% of remaining life
                                            break;
                                        #endregion

                                        #region Rank 9
                                        case 9:
                                            healValue = damageValue * 1.10f; //110% of remaining life
                                            break;
                                        #endregion

                                        #region Rank 10
                                        case 10:
                                            healValue = damageValue * 0.1f; //10% of remaining life
                                            break;
                                        #endregion

                                        default:
                                            return;
                                        #endregion
                                    }

                                    //Since the only variance between ranks is the healValue variable, we can
                                    //keep it simple by just adding all the effects down here, after we've
                                    //got the variables initialized
                                    // ~Nick B.

                                    eid = Entity.NextEntity();

                                    instantEffect = new InstantEffect()
                                    {
                                        EntityID = eid,
                                        isTriggered = false,
                                    };
                                    _game.InstantEffectComponent.Add(eid, instantEffect);

                                    directDamage = new DirectDamage()
                                    {
                                        EntityID = eid,
                                        TargetID = enslavedEnemyID,
                                        Damage = damageValue,
                                    };
                                    _game.DirectDamageComponent.Add(eid, directDamage);

                                    directHeal = new DirectHeal()
                                    {
                                        EntityID = eid,
                                        TargetID = userID,
                                        Amount = healValue,
                                    };
                                    _game.DirectHealComponent.Add(eid, directHeal);

                                    #endregion

                                    break;
                                }

                            case SkillType.PsionicSpear: //Projectile
                                {
                                    #region Skill Logic

                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.PsionicSpear,
                                        (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                        _game.PositionComponent[userID], rank, 300, userID);

                                    #endregion

                                    break;
                                }

                            case SkillType.Taint: //Projectile
                                {
                                    #region Skill Logic

                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.Taint,
                                        (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                        _game.PositionComponent[userID], rank, 300, userID);

                                    #endregion

                                    break;
                                }

                            case SkillType.Rot: //Projectile
                                {
                                    #region Skill Logic

                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.Rot,
                                        (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                        _game.PositionComponent[userID], rank, 300, userID);

                                    #endregion

                                    break;
                                }

                            case SkillType.Push: //AOE
                                {
                                    #region Skill Variables
                                    int pushRange = 0;
                                    #endregion

                                    #region Skill Logic

                                    switch (rank)
                                    {
                                        #region Checking Rank

                                        #region Rank 1
                                        case 1:
                                            pushRange = 15;
                                            break;
                                        #endregion

                                        #region Rank 2
                                        case 2:
                                            pushRange = 20;
                                            break;
                                        #endregion

                                        #region Rank 3
                                        case 3:
                                            pushRange = 24;
                                            break;
                                        #endregion

                                        #region Rank 4
                                        case 4:
                                            pushRange = 30;
                                            break;
                                        #endregion

                                        #region Rank 5
                                        case 5:
                                            pushRange = 45;
                                            break;
                                        #endregion

                                        #region Rank 6
                                        case 6:
                                            pushRange = 60;
                                            break;
                                        #endregion

                                        #region Rank 7
                                        case 7:
                                            pushRange = 85;
                                            break;
                                        #endregion

                                        #region Rank 8
                                        case 8:
                                            pushRange = 100;
                                            break;
                                        #endregion

                                        #region Rank 9
                                        case 9:
                                            pushRange = 125;
                                            break;
                                        #endregion

                                        #region Rank 10
                                        case 10:
                                            pushRange = 150;
                                            break;
                                        #endregion

                                        default:
                                            break;
                                        #endregion
                                    }

                                    _game.SkillEntityFactory.CreateSkillAoE(SkillType.Push,
                                        _game.PositionComponent[userID], rank, pushRange);

                                    #endregion


                                    break;
                                }

                            case SkillType.Lightning: //Instant
                                {
                                    #region Skill Variables

                                    InstantEffect instantEffect;
                                    DirectDamage directDamage;

                                    uint eid_2;
                                    TimedEffect timedEffect;
                                    Stun stun;


                                    Random damageMod = new Random();

                                    Position evalPosition;
                                    uint tempEnemyID;
                                    List<uint> ignoreList = new List<uint>();
                                    int chainNumber = 0;
                                    float duration = 0;
                                    int damageValue = 0;
                                    float maxChainDistance = 0;

                                    #endregion

                                    #region Skill Logic
                                    //No need to continue if we can't even get the position of the player
                                    //which means there will need to be some bugs to iron out
                                    if (!_game.PositionComponent.Contains(userID)) return;

                                    evalPosition = _game.PositionComponent[userID];

                                    switch (rank)
                                    {
                                        #region Checking Rank

                                        #region Rank 1
                                        case 1:
                                            chainNumber = 1; //Chains through 1 enemy
                                            duration = 2; //Duration of 2 seconds
                                            damageValue = 2 + damageMod.Next(4); //Damage between 2 and 5
                                            maxChainDistance = 10; //Biggest gap an arc will span
                                            break;
                                        #endregion

                                        #region Rank 2
                                        case 2:
                                            chainNumber = 1; //Chains through 1 enemy
                                            duration = 3; //Duration of 3 seconds
                                            damageValue = 4 + damageMod.Next(5); //Damage between 4 and 8
                                            maxChainDistance = 15; //Biggest gap an arc will span
                                            break;
                                        #endregion

                                        #region Rank 3
                                        case 3:
                                            chainNumber = 2; //Chains through 2 enemies
                                            duration = 3; //Duration of 3 seconds
                                            damageValue = 6 + damageMod.Next(5); //Damage between 6 and 10
                                            maxChainDistance = 15; //Biggest gap an arc will span
                                            break;
                                        #endregion

                                        #region Rank 4
                                        case 4:
                                            chainNumber = 2; //Chains through 2 enemies
                                            duration = 4; //Duration of 4 seconds
                                            damageValue = 10 + damageMod.Next(3); //Damage between 10 and 12
                                            maxChainDistance = 18; //Biggest gap an arc with span
                                            break;
                                        #endregion

                                        #region Rank 5
                                        case 5:
                                            chainNumber = 3; //Chains through 3 enemies
                                            duration = 5; //Duration of 5 seconds
                                            damageValue = 13 + damageMod.Next(3); //Damage between 13 and 15
                                            maxChainDistance = 21; //Biggest gap an arc will span
                                            break;
                                        #endregion

                                        #region Rank 6
                                        case 6:
                                            chainNumber = 3; //Chains through 3 enemies
                                            duration = 5; //Duration of 5 seconds
                                            damageValue = 17 + damageMod.Next(4); //Damage between 17 and 20
                                            maxChainDistance = 26; //Biggest gap an arc will span
                                            break;
                                        #endregion

                                        #region Rank 7
                                        case 7:
                                            chainNumber = 4; //Chains through 4 enemies
                                            duration = 7; //Duration of 7 seconds
                                            damageValue = 22 + damageMod.Next(6); //Damage between 22 and 27
                                            maxChainDistance = 30; //Biggest gap an arc will span
                                            break;
                                        #endregion

                                        #region Rank 8
                                        case 8:
                                            chainNumber = 4; //Chains through 4 enemies
                                            duration = 7; //Duration of 7 seconds
                                            damageValue = 30 + damageMod.Next(11); //Damage between 30 and 40
                                            maxChainDistance = 38; //Biggest gap an arc will span
                                            break;
                                        #endregion

                                        #region Rank 9
                                        case 9:
                                            chainNumber = 5; //Chains through 5 enemies
                                            duration = 7; //Duration of 7 seconds
                                            damageValue = 35 + damageMod.Next(13); //Damage between 35 and 47
                                            maxChainDistance = 45; //Biggest gap an arc will span
                                            break;
                                        #endregion

                                        #region Rank 10
                                        case 10:
                                            chainNumber = 6; //Chains through 6 enemies
                                            duration = 10; //Duration of 10 seconds
                                            damageValue = 50 + damageMod.Next(16); //Damage between 50 and 65
                                            maxChainDistance = 50; //Biggest gap an arc will span
                                            break;
                                        #endregion

                                        default:
                                            return;
                                        #endregion
                                    }

                                    eid = Entity.NextEntity();
                                    instantEffect = new InstantEffect()
                                    {
                                        EntityID = eid,
                                        isTriggered = false,
                                    };
                                    _game.InstantEffectComponent.Add(eid, instantEffect);

                                    eid_2 = Entity.NextEntity();
                                    timedEffect = new TimedEffect()
                                    {
                                        EntityID = eid_2,
                                        TotalDuration = duration,
                                        TimeLeft = duration,
                                    };

                                    for (int x = 0; x < chainNumber; x++)
                                    {
                                        tempEnemyID = _game.CollisionSystem.GetClosestEnemy(evalPosition, ignoreList, maxChainDistance);

                                        //No need to continue, no enemy is close enough or no other enemies exist
                                        if (!_game.EnemyComponent.Contains(tempEnemyID)) break;

                                        //Add the enemy we just chained to to the ignore list, we don't want
                                        //to chain to them again
                                        ignoreList.Add(tempEnemyID);

                                        //This will be an instant effect, so we'll use eid here
                                        directDamage = new DirectDamage()
                                        {
                                            EntityID = eid,
                                            TargetID = tempEnemyID,
                                            Damage = damageValue,
                                        };
                                        _game.DirectDamageComponent.Add(eid, directDamage);

                                        //This will be a timed effect, so we'll need eid_2 here
                                        stun = new Stun()
                                        {
                                            EntityID = eid_2,
                                            TargetID = tempEnemyID,
                                            Type = StunType.CantBreak,
                                        };
                                        _game.StunComponent.Add(eid_2, stun);
                                    }

                                    #endregion

                                    break;
                                }

                            case SkillType.Malice: //AOE
                                {
                                    #region Skill Variables
                                    int meterConversion = 75;
                                    int maliceRange = 0;
                                    #endregion

                                    #region Skill Logic

                                    switch (rank)
                                    {
                                        #region Checking Rank

                                        #region Rank 1
                                        case 1:
                                            maliceRange = 2;
                                            break;
                                        #endregion

                                        #region Rank 2
                                        case 2:
                                            maliceRange = 2;
                                            break;
                                        #endregion

                                        #region Rank 3
                                        case 3:
                                            maliceRange = 3;
                                            break;
                                        #endregion

                                        #region Rank 4
                                        case 4:
                                            maliceRange = 3;
                                            break;
                                        #endregion

                                        #region Rank 5
                                        case 5:
                                            maliceRange = 4;
                                            break;
                                        #endregion

                                        #region Rank 6
                                        case 6:
                                            maliceRange = 5;
                                            break;
                                        #endregion

                                        #region Rank 7
                                        case 7:
                                            maliceRange = 5;
                                            break;
                                        #endregion

                                        #region Rank 8
                                        case 8:
                                            maliceRange = 6;
                                            break;
                                        #endregion

                                        #region Rank 9
                                        case 9:
                                            maliceRange = 6;
                                            break;
                                        #endregion

                                        #region Rank 10
                                        case 10:
                                            maliceRange = 6;
                                            break;
                                        #endregion

                                        default:
                                            break;
                                        #endregion
                                    }

                                    _game.SkillEntityFactory.CreateSkillAoE(SkillType.Malice,
                                        _game.PositionComponent[userID], rank, maliceRange * meterConversion);

                                    #endregion

                                    break;
                                }

                            default:
                                {
                                    break;
                                }

                            #endregion
                        }
                        break;
                    }

                #endregion

                #region Vermis

                case Aggregate.ZombiePlayer:
                    CoolDown coolDown;
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
                                    coolDown = new CoolDown()
                                    {
                                        EntityID = eid,
                                        MaxTime = 1,
                                        TimeLeft = 1,
                                        Type = SkillType.ThrownBlades,
                                        UserID = userID,
                                    };
                                    _game.CoolDownComponent.Add(eid, coolDown);
                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.ThrownBlades, (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                        _game.PositionComponent[userID], 1, 300, userID);
                                    break;

                                case 2:
                                    eid = Entity.NextEntity();
                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.ThrownBlades, (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                       _game.PositionComponent[userID], 2, 300, userID);
                                    break;

                                case 3:
                                    eid = Entity.NextEntity();
                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.ThrownBlades, (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                       _game.PositionComponent[userID], 3, 300, userID);
                                    break;

                                case 4:
                                    eid = Entity.NextEntity();
                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.ThrownBlades, (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                       _game.PositionComponent[userID], 4, 300, userID);
                                    break;

                                case 5:
                                    eid = Entity.NextEntity();
                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.ThrownBlades, (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                       _game.PositionComponent[userID], 5, 300, userID);
                                    break;

                                case 6:
                                    eid = Entity.NextEntity();
                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.ThrownBlades, (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                       _game.PositionComponent[userID], 6, 300, userID);
                                    break;

                                case 7:
                                    eid = Entity.NextEntity();
                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.ThrownBlades, (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                       _game.PositionComponent[userID], 7, 300, userID);
                                    break;

                                case 8:
                                    eid = Entity.NextEntity();
                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.ThrownBlades, (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                       _game.PositionComponent[userID], 8, 300, userID);
                                    break;

                                case 9:
                                    eid = Entity.NextEntity();
                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.ThrownBlades, (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                       _game.PositionComponent[userID], 9, 300, userID);
                                    break;

                                case 10:
                                    eid = Entity.NextEntity();
                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.ThrownBlades, (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                       _game.PositionComponent[userID], 10, 300, userID);
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

                        case SkillType.Regeneration:

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
                                    coolDown = new CoolDown()
                                    {
                                        EntityID = eid,
                                        MaxTime = 1,
                                        TimeLeft = 1,
                                        Type = SkillType.BenignParasite,
                                        UserID = userID,
                                    };
                                    _game.CoolDownComponent.Add(eid, coolDown);
                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.BenignParasite, (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                       _game.PositionComponent[userID], 1, 300, userID);
                                    break;

                                case 2:
                                    eid = Entity.NextEntity();
                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.BenignParasite, (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                       _game.PositionComponent[userID], 2, 300, userID);
                                    break;

                                case 3:
                                    eid = Entity.NextEntity();
                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.BenignParasite, (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                       _game.PositionComponent[userID], 3, 300, userID);
                                    break;

                                case 4:
                                    eid = Entity.NextEntity();
                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.BenignParasite, (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                       _game.PositionComponent[userID], 4, 300, userID);
                                    break;

                                case 5:
                                    eid = Entity.NextEntity();
                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.BenignParasite, (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                       _game.PositionComponent[userID], 5, 300, userID);
                                    break;

                                case 6:
                                    eid = Entity.NextEntity();
                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.BenignParasite, (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                       _game.PositionComponent[userID], 6, 300, userID);
                                    break;

                                case 7:
                                    eid = Entity.NextEntity();
                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.BenignParasite, (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                       _game.PositionComponent[userID], 7, 300, userID);
                                    break;

                                case 8:
                                    eid = Entity.NextEntity();
                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.BenignParasite, (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                       _game.PositionComponent[userID], 8, 300, userID);
                                    break;

                                case 9:
                                    eid = Entity.NextEntity(); _game.SkillEntityFactory.CreateSkillProjectile(SkillType.BenignParasite, (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                        _game.PositionComponent[userID], 9, 300, userID);
                                    break;

                                case 10:
                                    eid = Entity.NextEntity();
                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.BenignParasite, (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                       _game.PositionComponent[userID], 10, 300, userID);
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
                                    coolDown = new CoolDown()
                                    {
                                        EntityID = eid,
                                        MaxTime = 1,
                                        TimeLeft = 1,
                                        Type = SkillType.ThrownBlades,
                                        UserID = userID,
                                    };
                                    _game.CoolDownComponent.Add(eid, coolDown);
                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.MaliciousParasite, (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                       _game.PositionComponent[userID], 1, 300, userID);
                                    break;

                                case 2:
                                    eid = Entity.NextEntity();
                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.MaliciousParasite, (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                       _game.PositionComponent[userID], 2, 300, userID);
                                    break;

                                case 3:
                                    eid = Entity.NextEntity();
                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.MaliciousParasite, (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                       _game.PositionComponent[userID], 3, 300, userID);
                                    break;

                                case 4:
                                    eid = Entity.NextEntity();
                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.MaliciousParasite, (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                       _game.PositionComponent[userID], 4, 300, userID);
                                    break;

                                case 5:
                                    eid = Entity.NextEntity();
                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.MaliciousParasite, (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                       _game.PositionComponent[userID], 5, 300, userID);
                                    break;

                                case 6:
                                    eid = Entity.NextEntity();
                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.MaliciousParasite, (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                       _game.PositionComponent[userID], 6, 300, userID);
                                    break;

                                case 7:
                                    eid = Entity.NextEntity();
                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.MaliciousParasite, (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                       _game.PositionComponent[userID], 7, 300, userID);
                                    break;

                                case 8:
                                    eid = Entity.NextEntity();
                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.MaliciousParasite, (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                       _game.PositionComponent[userID], 8, 300, userID);
                                    break;

                                case 9:
                                    eid = Entity.NextEntity();
                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.MaliciousParasite, (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                       _game.PositionComponent[userID], 9, 300, userID);
                                    break;

                                case 10:
                                    eid = Entity.NextEntity();
                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.MaliciousParasite, (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                       _game.PositionComponent[userID], 10, 300, userID);
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
                                    coolDown = new CoolDown()
                                    {
                                        EntityID = eid,
                                        MaxTime = 1,
                                        TimeLeft = 1,
                                        Type = SkillType.ThrownBlades,
                                        UserID = userID,
                                    };
                                    _game.CoolDownComponent.Add(eid, coolDown);
                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.MindlessParasites, (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                       _game.PositionComponent[userID], 1, 300, userID);
                                    break;

                                case 2:
                                    eid = Entity.NextEntity();
                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.MindlessParasites, (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                       _game.PositionComponent[userID], 2, 300, userID);
                                    break;

                                case 3:
                                    eid = Entity.NextEntity();
                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.MindlessParasites, (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                       _game.PositionComponent[userID], 3, 300, userID);
                                    break;

                                case 4:
                                    eid = Entity.NextEntity();
                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.MindlessParasites, (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                       _game.PositionComponent[userID], 4, 300, userID);
                                    break;

                                case 5:
                                    eid = Entity.NextEntity();
                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.MindlessParasites, (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                       _game.PositionComponent[userID], 5, 300, userID);
                                    break;

                                case 6:
                                    eid = Entity.NextEntity();
                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.MindlessParasites, (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                       _game.PositionComponent[userID], 6, 300, userID);
                                    break;

                                case 7:
                                    eid = Entity.NextEntity();
                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.MindlessParasites, (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                       _game.PositionComponent[userID], 7, 300, userID);
                                    break;

                                case 8:
                                    eid = Entity.NextEntity();
                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.MindlessParasites, (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                       _game.PositionComponent[userID], 8, 300, userID);
                                    break;

                                case 9:
                                    eid = Entity.NextEntity();
                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.MindlessParasites, (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                       _game.PositionComponent[userID], 9, 300, userID);
                                    break;

                                case 10:
                                    eid = Entity.NextEntity();
                                    _game.SkillEntityFactory.CreateSkillProjectile(SkillType.MindlessParasites, (Facing)_game.SpriteAnimationComponent[userID].CurrentAnimationRow,
                                       _game.PositionComponent[userID], 10, 300, userID);
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

                //Earthian Skills done by Andrew Bellinder
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
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/EngineeringOffense"),
                                            SpriteBounds = new Rectangle(11, 49, 37, 63),
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
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/EngineeringOffense"),
                                            SpriteBounds = new Rectangle(11, 49, 37, 63),
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
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/EngineeringOffense"),
                                            SpriteBounds = new Rectangle(11, 49, 37, 63),
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
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/EngineeringOffense"),
                                            SpriteBounds = new Rectangle(11, 49, 37, 63),
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
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/EngineeringOffense"),
                                            SpriteBounds = new Rectangle(11, 49, 37, 63),
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
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/EngineeringOffense"),
                                            SpriteBounds = new Rectangle(11, 49, 37, 63),
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
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/EngineeringOffense"),
                                            SpriteBounds = new Rectangle(11, 49, 37, 63),
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
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/EngineeringOffense"),
                                            SpriteBounds = new Rectangle(11, 49, 37, 63),
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
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/EngineeringOffense"),
                                            SpriteBounds = new Rectangle(11, 49, 37, 63),
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
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/EngineeringOffense"),
                                            SpriteBounds = new Rectangle(11, 49, 37, 63),
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
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/EngineeringOffense"),
                                            SpriteBounds = new Rectangle(0, 10, 69, 42),
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
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/EngineeringOffense"),
                                            SpriteBounds = new Rectangle(0, 10, 69, 42),
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
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/EngineeringOffense"),
                                            SpriteBounds = new Rectangle(0, 10, 69, 42),
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
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/EngineeringOffense"),
                                            SpriteBounds = new Rectangle(0, 10, 69, 42),
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
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/EngineeringOffense"),
                                            SpriteBounds = new Rectangle(0, 10, 69, 42),
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
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/EngineeringOffense"),
                                            SpriteBounds = new Rectangle(0, 10, 69, 42),
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
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/EngineeringOffense"),
                                            SpriteBounds = new Rectangle(0, 10, 69, 42),
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
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/EngineeringOffense"),
                                            SpriteBounds = new Rectangle(0, 10, 69, 42),
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
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/EngineeringOffense"),
                                            SpriteBounds = new Rectangle(0, 10, 69, 42),
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
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/EngineeringOffense"),
                                            SpriteBounds = new Rectangle(0, 10, 69, 42),
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
                                if (_game.EnemyComponent.All.Count() > 0)
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
                                            effectDuration = 6;
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
                                            droidPosition.Radius = 32;
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
                                                SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/EngineeringOffense"),
                                                SpriteBounds = new Rectangle(51, 45, 71, 82),
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
                                            effectDuration = 6;
                                            droidSpeed = 115;

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
                                            droidPosition.Radius = 35;
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
                                                SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/EngineeringOffense"),
                                                SpriteBounds = new Rectangle(51, 45, 71, 82),
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

                                        case 3:
                                            eid = Entity.NextEntity();
                                            effectDuration = 6;
                                            droidSpeed = 120;

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
                                            droidPosition.Radius = 40;
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
                                                SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/EngineeringOffense"),
                                                SpriteBounds = new Rectangle(51, 45, 71, 82),
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

                                        case 4:
                                            eid = Entity.NextEntity();
                                            effectDuration = 6;
                                            droidSpeed = 125;

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
                                            droidPosition.Radius = 45;
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
                                                SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/EngineeringOffense"),
                                                SpriteBounds = new Rectangle(51, 45, 71, 82),
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

                                        case 5:
                                            eid = Entity.NextEntity();
                                            effectDuration = 6;
                                            droidSpeed = 130;

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
                                            droidPosition.Radius = 50;
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
                                                SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/EngineeringOffense"),
                                                SpriteBounds = new Rectangle(51, 45, 71, 82),
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

                                        case 6:
                                            eid = Entity.NextEntity();
                                            effectDuration = 6;
                                            droidSpeed = 135;

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
                                            droidPosition.Radius = 55;
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
                                                SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/EngineeringOffense"),
                                                SpriteBounds = new Rectangle(51, 45, 71, 82),
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

                                        case 7:
                                            eid = Entity.NextEntity();
                                            effectDuration = 6;
                                            droidSpeed = 140;

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
                                            droidPosition.Radius = 60;
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
                                                SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/EngineeringOffense"),
                                                SpriteBounds = new Rectangle(51, 45, 71, 82),
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

                                        case 8:
                                            eid = Entity.NextEntity();
                                            effectDuration = 6;
                                            droidSpeed = 145;

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
                                            droidPosition.Radius = 65;
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
                                                SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/EngineeringOffense"),
                                                SpriteBounds = new Rectangle(51, 45, 71, 82),
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

                                        case 9:
                                            eid = Entity.NextEntity();
                                            effectDuration = 6;
                                            droidSpeed = 150;

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
                                            droidPosition.Radius = 70;
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
                                                SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/EngineeringOffense"),
                                                SpriteBounds = new Rectangle(51, 45, 71, 82),
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

                                        case 10:
                                            eid = Entity.NextEntity();
                                            effectDuration = 6;
                                            droidSpeed = 155;

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
                                            droidPosition.Radius = 75;
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
                                                SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/EngineeringOffense"),
                                                SpriteBounds = new Rectangle(51, 45, 71, 82),
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

                                        default:
                                            break;
                                        #endregion
                                    }
                                }
                            }
                            break;

                        #endregion

                        #region Healing Station

                        case SkillType.HealingStation:
                            {

                                #region Skill Variables

                                HealingStation healingStation;
                                Sprite sprite;
                                Position stationPosition;

                                Collideable collideable;


                                #endregion

                                switch (rank)
                                {
                                    #region Checking Rank
                                    case 1:
                                        eid = Entity.NextEntity();

                                        stationPosition = _game.PositionComponent[GetPlayerID()];

                                        healingStation = new HealingStation()
                                        {
                                            EntityID = eid,
                                            position = stationPosition,
                                            healthAvailable = 10,

                                        };
                                        _game.HealingStationComponent.Add(eid, healingStation);

                                        collideable = new Collideable()
                                        {
                                            EntityID = eid,
                                            RoomID = stationPosition.RoomID,
                                            Bounds = new CircleBounds(stationPosition.Center, stationPosition.Radius),
                                        };
                                        _game.CollisionComponent.Add(eid, collideable);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Turret"),
                                            SpriteBounds = new Rectangle(0, 0, 37, 28),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        _game.PositionComponent.Add(eid, stationPosition);

                                        break;

                                    case 2:
                                        eid = Entity.NextEntity();

                                        stationPosition = _game.PositionComponent[GetPlayerID()];

                                        healingStation = new HealingStation()
                                        {
                                            EntityID = eid,
                                            position = stationPosition,
                                            healthAvailable = 15,

                                        };
                                        _game.HealingStationComponent.Add(eid, healingStation);

                                        collideable = new Collideable()
                                        {
                                            EntityID = eid,
                                            RoomID = stationPosition.RoomID,
                                            Bounds = new CircleBounds(stationPosition.Center, stationPosition.Radius),
                                        };
                                        _game.CollisionComponent.Add(eid, collideable);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Turret"),
                                            SpriteBounds = new Rectangle(0, 0, 37, 28),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        _game.PositionComponent.Add(eid, stationPosition);

                                        break;

                                    case 3:
                                        eid = Entity.NextEntity();

                                        stationPosition = _game.PositionComponent[GetPlayerID()];

                                        healingStation = new HealingStation()
                                        {
                                            EntityID = eid,
                                            position = stationPosition,
                                            healthAvailable = 20,

                                        };
                                        _game.HealingStationComponent.Add(eid, healingStation);

                                        collideable = new Collideable()
                                        {
                                            EntityID = eid,
                                            RoomID = stationPosition.RoomID,
                                            Bounds = new CircleBounds(stationPosition.Center, stationPosition.Radius),
                                        };
                                        _game.CollisionComponent.Add(eid, collideable);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Turret"),
                                            SpriteBounds = new Rectangle(0, 0, 37, 28),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        _game.PositionComponent.Add(eid, stationPosition);

                                        break;

                                    case 4:
                                        eid = Entity.NextEntity();

                                        stationPosition = _game.PositionComponent[GetPlayerID()];

                                        healingStation = new HealingStation()
                                        {
                                            EntityID = eid,
                                            position = stationPosition,
                                            healthAvailable = 25,

                                        };
                                        _game.HealingStationComponent.Add(eid, healingStation);

                                        collideable = new Collideable()
                                        {
                                            EntityID = eid,
                                            RoomID = stationPosition.RoomID,
                                            Bounds = new CircleBounds(stationPosition.Center, stationPosition.Radius),
                                        };
                                        _game.CollisionComponent.Add(eid, collideable);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Turret"),
                                            SpriteBounds = new Rectangle(0, 0, 37, 28),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        _game.PositionComponent.Add(eid, stationPosition);

                                        break;

                                    case 5:
                                        eid = Entity.NextEntity();

                                        stationPosition = _game.PositionComponent[GetPlayerID()];

                                        healingStation = new HealingStation()
                                        {
                                            EntityID = eid,
                                            position = stationPosition,
                                            healthAvailable = 30,

                                        };
                                        _game.HealingStationComponent.Add(eid, healingStation);

                                        collideable = new Collideable()
                                        {
                                            EntityID = eid,
                                            RoomID = stationPosition.RoomID,
                                            Bounds = new CircleBounds(stationPosition.Center, stationPosition.Radius),
                                        };
                                        _game.CollisionComponent.Add(eid, collideable);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Turret"),
                                            SpriteBounds = new Rectangle(0, 0, 37, 28),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        _game.PositionComponent.Add(eid, stationPosition);

                                        break;

                                    case 6:
                                        eid = Entity.NextEntity();

                                        stationPosition = _game.PositionComponent[GetPlayerID()];

                                        healingStation = new HealingStation()
                                        {
                                            EntityID = eid,
                                            position = stationPosition,
                                            healthAvailable = 35,

                                        };
                                        _game.HealingStationComponent.Add(eid, healingStation);

                                        collideable = new Collideable()
                                        {
                                            EntityID = eid,
                                            RoomID = stationPosition.RoomID,
                                            Bounds = new CircleBounds(stationPosition.Center, stationPosition.Radius),
                                        };
                                        _game.CollisionComponent.Add(eid, collideable);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Turret"),
                                            SpriteBounds = new Rectangle(0, 0, 37, 28),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        _game.PositionComponent.Add(eid, stationPosition);

                                        break;

                                    case 7:
                                        eid = Entity.NextEntity();

                                        stationPosition = _game.PositionComponent[GetPlayerID()];

                                        healingStation = new HealingStation()
                                        {
                                            EntityID = eid,
                                            position = stationPosition,
                                            healthAvailable = 40,

                                        };
                                        _game.HealingStationComponent.Add(eid, healingStation);

                                        collideable = new Collideable()
                                        {
                                            EntityID = eid,
                                            RoomID = stationPosition.RoomID,
                                            Bounds = new CircleBounds(stationPosition.Center, stationPosition.Radius),
                                        };
                                        _game.CollisionComponent.Add(eid, collideable);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Turret"),
                                            SpriteBounds = new Rectangle(0, 0, 37, 28),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        _game.PositionComponent.Add(eid, stationPosition);

                                        break;

                                    case 8:
                                        eid = Entity.NextEntity();

                                        stationPosition = _game.PositionComponent[GetPlayerID()];

                                        healingStation = new HealingStation()
                                        {
                                            EntityID = eid,
                                            position = stationPosition,
                                            healthAvailable = 45,

                                        };
                                        _game.HealingStationComponent.Add(eid, healingStation);

                                        collideable = new Collideable()
                                        {
                                            EntityID = eid,
                                            RoomID = stationPosition.RoomID,
                                            Bounds = new CircleBounds(stationPosition.Center, stationPosition.Radius),
                                        };
                                        _game.CollisionComponent.Add(eid, collideable);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Turret"),
                                            SpriteBounds = new Rectangle(0, 0, 37, 28),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        _game.PositionComponent.Add(eid, stationPosition);

                                        break;

                                    case 9:
                                        eid = Entity.NextEntity();

                                        stationPosition = _game.PositionComponent[GetPlayerID()];

                                        healingStation = new HealingStation()
                                        {
                                            EntityID = eid,
                                            position = stationPosition,
                                            healthAvailable = 50,

                                        };
                                        _game.HealingStationComponent.Add(eid, healingStation);

                                        collideable = new Collideable()
                                        {
                                            EntityID = eid,
                                            RoomID = stationPosition.RoomID,
                                            Bounds = new CircleBounds(stationPosition.Center, stationPosition.Radius),
                                        };
                                        _game.CollisionComponent.Add(eid, collideable);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Turret"),
                                            SpriteBounds = new Rectangle(0, 0, 37, 28),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        _game.PositionComponent.Add(eid, stationPosition);

                                        break;

                                    case 10:
                                        eid = Entity.NextEntity();

                                        stationPosition = _game.PositionComponent[GetPlayerID()];

                                        healingStation = new HealingStation()
                                        {
                                            EntityID = eid,
                                            position = stationPosition,
                                            healthAvailable = 60,

                                        };
                                        _game.HealingStationComponent.Add(eid, healingStation);

                                        collideable = new Collideable()
                                        {
                                            EntityID = eid,
                                            RoomID = stationPosition.RoomID,
                                            Bounds = new CircleBounds(stationPosition.Center, stationPosition.Radius),
                                        };
                                        _game.CollisionComponent.Add(eid, collideable);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Turret"),
                                            SpriteBounds = new Rectangle(0, 0, 37, 28),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        _game.PositionComponent.Add(eid, stationPosition);

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

                                        _game.SkillEntityFactory.CreateSkillDeployable(SkillType.PortableShop, _game.PositionComponent[GetPlayerID()], 1);
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

                                        _game.SkillEntityFactory.CreateSkillDeployable(SkillType.PortableShop, _game.PositionComponent[GetPlayerID()], 2);
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

                                        _game.SkillEntityFactory.CreateSkillDeployable(SkillType.PortableShop, _game.PositionComponent[GetPlayerID()], 2);
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

                                        _game.SkillEntityFactory.CreateSkillDeployable(SkillType.PortableShop, _game.PositionComponent[GetPlayerID()], 2);
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

                                        _game.SkillEntityFactory.CreateSkillDeployable(SkillType.PortableShop, _game.PositionComponent[GetPlayerID()], 2);
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

                                        _game.SkillEntityFactory.CreateSkillDeployable(SkillType.PortableShop, _game.PositionComponent[GetPlayerID()], 2);
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

                                        _game.SkillEntityFactory.CreateSkillDeployable(SkillType.PortableShop, _game.PositionComponent[GetPlayerID()], 2);
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

                                        _game.SkillEntityFactory.CreateSkillDeployable(SkillType.PortableShop, _game.PositionComponent[GetPlayerID()], 2);
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

                                        _game.SkillEntityFactory.CreateSkillDeployable(SkillType.PortableShop, _game.PositionComponent[GetPlayerID()], 2);
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

                                        _game.SkillEntityFactory.CreateSkillDeployable(SkillType.PortableShop, _game.PositionComponent[GetPlayerID()], 2);
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

                                PortableShield portableShield;
                                Position shieldPosition;

                                Sprite sprite;
                                Collideable collideable;

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

                                        shieldPosition = _game.PositionComponent[GetPlayerID()];
                                        shieldPosition.Radius = 100;

                                        portableShield = new PortableShield()
                                        {
                                            EntityID = eid,
                                            position = shieldPosition,
                                        };
                                        _game.PortableShieldComponent.Add(eid, portableShield);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/BubbleShield"),
                                            SpriteBounds = new Rectangle(18, 34, 229, 136),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        collideable = new Collideable()
                                        {
                                            EntityID = eid,
                                            RoomID = shieldPosition.RoomID,
                                            Bounds = new CircleBounds(shieldPosition.Center, shieldPosition.Radius),
                                        };
                                        _game.CollisionComponent.Add(eid, collideable);

                                        _game.PositionComponent.Add(eid, shieldPosition); break;

                                    case 2:
                                        eid = Entity.NextEntity();
                                        effectDuration = 6;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        shieldPosition = _game.PositionComponent[GetPlayerID()];
                                        shieldPosition.Radius = 100;

                                        portableShield = new PortableShield()
                                        {
                                            EntityID = eid,
                                            position = shieldPosition,
                                        };
                                        _game.PortableShieldComponent.Add(eid, portableShield);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/BubbleShield"),
                                            SpriteBounds = new Rectangle(18, 34, 229, 136),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        collideable = new Collideable()
                                        {
                                            EntityID = eid,
                                            RoomID = shieldPosition.RoomID,
                                            Bounds = new CircleBounds(shieldPosition.Center, shieldPosition.Radius),
                                        };
                                        _game.CollisionComponent.Add(eid, collideable);

                                        _game.PositionComponent.Add(eid, shieldPosition);
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

                                        shieldPosition = _game.PositionComponent[GetPlayerID()];
                                        shieldPosition.Radius = 100;

                                        portableShield = new PortableShield()
                                        {
                                            EntityID = eid,
                                            position = shieldPosition,
                                        };
                                        _game.PortableShieldComponent.Add(eid, portableShield);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/BubbleShield"),
                                            SpriteBounds = new Rectangle(18, 34, 229, 136),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        collideable = new Collideable()
                                        {
                                            EntityID = eid,
                                            RoomID = shieldPosition.RoomID,
                                            Bounds = new CircleBounds(shieldPosition.Center, shieldPosition.Radius),
                                        };
                                        _game.CollisionComponent.Add(eid, collideable);

                                        _game.PositionComponent.Add(eid, shieldPosition);
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

                                        shieldPosition = _game.PositionComponent[GetPlayerID()];
                                        shieldPosition.Radius = 100;

                                        portableShield = new PortableShield()
                                        {
                                            EntityID = eid,
                                            position = shieldPosition,
                                        };
                                        _game.PortableShieldComponent.Add(eid, portableShield);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/BubbleShield"),
                                            SpriteBounds = new Rectangle(18, 34, 229, 136),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        collideable = new Collideable()
                                        {
                                            EntityID = eid,
                                            RoomID = shieldPosition.RoomID,
                                            Bounds = new CircleBounds(shieldPosition.Center, shieldPosition.Radius),
                                        };
                                        _game.CollisionComponent.Add(eid, collideable);

                                        _game.PositionComponent.Add(eid, shieldPosition);
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

                                        shieldPosition = _game.PositionComponent[GetPlayerID()];
                                        shieldPosition.Radius = 100;

                                        portableShield = new PortableShield()
                                        {
                                            EntityID = eid,
                                            position = shieldPosition,
                                        };
                                        _game.PortableShieldComponent.Add(eid, portableShield);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/BubbleShield"),
                                            SpriteBounds = new Rectangle(18, 34, 229, 136),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        collideable = new Collideable()
                                        {
                                            EntityID = eid,
                                            RoomID = shieldPosition.RoomID,
                                            Bounds = new CircleBounds(shieldPosition.Center, shieldPosition.Radius),
                                        };
                                        _game.CollisionComponent.Add(eid, collideable);

                                        _game.PositionComponent.Add(eid, shieldPosition);
                                        break;

                                    case 6:
                                        eid = Entity.NextEntity();
                                        effectDuration = 10;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        shieldPosition = _game.PositionComponent[GetPlayerID()];
                                        shieldPosition.Radius = 100;

                                        portableShield = new PortableShield()
                                        {
                                            EntityID = eid,
                                            position = shieldPosition,
                                        };
                                        _game.PortableShieldComponent.Add(eid, portableShield);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/BubbleShield"),
                                            SpriteBounds = new Rectangle(18, 34, 229, 136),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        collideable = new Collideable()
                                        {
                                            EntityID = eid,
                                            RoomID = shieldPosition.RoomID,
                                            Bounds = new CircleBounds(shieldPosition.Center, shieldPosition.Radius),
                                        };
                                        _game.CollisionComponent.Add(eid, collideable);

                                        _game.PositionComponent.Add(eid, shieldPosition);
                                        break;

                                    case 7:
                                        eid = Entity.NextEntity();
                                        effectDuration = 11;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        shieldPosition = _game.PositionComponent[GetPlayerID()];
                                        shieldPosition.Radius = 100;

                                        portableShield = new PortableShield()
                                        {
                                            EntityID = eid,
                                            position = shieldPosition,
                                        };
                                        _game.PortableShieldComponent.Add(eid, portableShield);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/BubbleShield"),
                                            SpriteBounds = new Rectangle(18, 34, 229, 136),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        collideable = new Collideable()
                                        {
                                            EntityID = eid,
                                            RoomID = shieldPosition.RoomID,
                                            Bounds = new CircleBounds(shieldPosition.Center, shieldPosition.Radius),
                                        };
                                        _game.CollisionComponent.Add(eid, collideable);

                                        _game.PositionComponent.Add(eid, shieldPosition);
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

                                        shieldPosition = _game.PositionComponent[GetPlayerID()];
                                        shieldPosition.Radius = 100;

                                        portableShield = new PortableShield()
                                        {
                                            EntityID = eid,
                                            position = shieldPosition,
                                        };
                                        _game.PortableShieldComponent.Add(eid, portableShield);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/BubbleShield"),
                                            SpriteBounds = new Rectangle(18, 34, 229, 136),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        collideable = new Collideable()
                                        {
                                            EntityID = eid,
                                            RoomID = shieldPosition.RoomID,
                                            Bounds = new CircleBounds(shieldPosition.Center, shieldPosition.Radius),
                                        };
                                        _game.CollisionComponent.Add(eid, collideable);

                                        _game.PositionComponent.Add(eid, shieldPosition);
                                        break;

                                    case 9:
                                        eid = Entity.NextEntity();
                                        effectDuration = 13;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        shieldPosition = _game.PositionComponent[GetPlayerID()];
                                        shieldPosition.Radius = 100;

                                        portableShield = new PortableShield()
                                        {
                                            EntityID = eid,
                                            position = shieldPosition,
                                        };
                                        _game.PortableShieldComponent.Add(eid, portableShield);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/BubbleShield"),
                                            SpriteBounds = new Rectangle(18, 34, 229, 136),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        collideable = new Collideable()
                                        {
                                            EntityID = eid,
                                            RoomID = shieldPosition.RoomID,
                                            Bounds = new CircleBounds(shieldPosition.Center, shieldPosition.Radius),
                                        };
                                        _game.CollisionComponent.Add(eid, collideable);

                                        _game.PositionComponent.Add(eid, shieldPosition);
                                        break;

                                    case 10:
                                        eid = Entity.NextEntity();
                                        effectDuration = 14;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        shieldPosition = _game.PositionComponent[GetPlayerID()];
                                        shieldPosition.Radius = 100;

                                        portableShield = new PortableShield()
                                        {
                                            EntityID = eid,
                                            position = shieldPosition,
                                        };
                                        _game.PortableShieldComponent.Add(eid, portableShield);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/BubbleShield"),
                                            SpriteBounds = new Rectangle(18, 34, 229, 136),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        collideable = new Collideable()
                                        {
                                            EntityID = eid,
                                            RoomID = shieldPosition.RoomID,
                                            Bounds = new CircleBounds(shieldPosition.Center, shieldPosition.Radius),
                                        };
                                        _game.CollisionComponent.Add(eid, collideable);

                                        _game.PositionComponent.Add(eid, shieldPosition);
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

                                Sprite sprite;
                                Position motivatePosition;


                                #endregion

                                switch (rank)
                                {
                                    #region Checking Rank

                                    #region Rank 1

                                    case 1:
                                        eid = Entity.NextEntity();
                                        effectDuration = 0.5f;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/bubble"),
                                            SpriteBounds = new Rectangle(0, 0, 76, 48),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        motivatePosition = _game.PositionComponent[GetPlayerID()];
                                        motivatePosition.Center.X += 40;
                                        motivatePosition.Center.Y -= 30;

                                        _game.PositionComponent.Add(eid, motivatePosition);

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            eid = Entity.NextEntity();

                                            buffEffect = new Buff()
                                            {
                                                EntityID = eid,
                                                TargetID = player.EntityID,
                                                Health = 5,
                                            };
                                            _game.BuffComponent.Add(eid, buffEffect);
                                        }

                                        break;

                                    #endregion

                                    #region Rank 2

                                    case 2:
                                        eid = Entity.NextEntity();
                                        effectDuration = 0.5f;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/bubble"),
                                            SpriteBounds = new Rectangle(0, 0, 76, 48),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        motivatePosition = _game.PositionComponent[GetPlayerID()];
                                        motivatePosition.Center.X += 40;
                                        motivatePosition.Center.Y -= 30;

                                        _game.PositionComponent.Add(eid, motivatePosition);

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            eid = Entity.NextEntity();

                                            buffEffect = new Buff()
                                            {
                                                EntityID = eid,
                                                TargetID = player.EntityID,
                                                Health = 10,
                                            };
                                            _game.BuffComponent.Add(eid, buffEffect);
                                        }
                                        break;

                                    #endregion

                                    #region Rank 3

                                    case 3:
                                        eid = Entity.NextEntity();
                                        effectDuration = 0.5f;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/bubble"),
                                            SpriteBounds = new Rectangle(0, 0, 76, 48),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        motivatePosition = _game.PositionComponent[GetPlayerID()];
                                        motivatePosition.Center.X += 40;
                                        motivatePosition.Center.Y -= 30;

                                        _game.PositionComponent.Add(eid, motivatePosition);

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            eid = Entity.NextEntity();

                                            buffEffect = new Buff()
                                            {
                                                EntityID = eid,
                                                TargetID = player.EntityID,
                                                Health = 15,
                                            };
                                            _game.BuffComponent.Add(eid, buffEffect);
                                        }
                                        break;

                                    #endregion

                                    #region Rank 4

                                    case 4:
                                        eid = Entity.NextEntity();
                                        effectDuration = 0.5f;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/bubble"),
                                            SpriteBounds = new Rectangle(0, 0, 76, 48),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        motivatePosition = _game.PositionComponent[GetPlayerID()];
                                        motivatePosition.Center.X += 40;
                                        motivatePosition.Center.Y -= 30;

                                        _game.PositionComponent.Add(eid, motivatePosition);

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            eid = Entity.NextEntity();

                                            buffEffect = new Buff()
                                            {
                                                EntityID = eid,
                                                TargetID = player.EntityID,
                                                Health = 20,
                                            };
                                            _game.BuffComponent.Add(eid, buffEffect);
                                        }
                                        break;

                                    #endregion

                                    #region Rank 5

                                    case 5:
                                        eid = Entity.NextEntity();
                                        effectDuration = 0.5f;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/bubble"),
                                            SpriteBounds = new Rectangle(0, 0, 76, 48),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        motivatePosition = _game.PositionComponent[GetPlayerID()];
                                        motivatePosition.Center.X += 40;
                                        motivatePosition.Center.Y -= 30;

                                        _game.PositionComponent.Add(eid, motivatePosition);

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            eid = Entity.NextEntity();

                                            buffEffect = new Buff()
                                            {
                                                EntityID = eid,
                                                TargetID = player.EntityID,
                                                Health = 25,
                                            };
                                            _game.BuffComponent.Add(eid, buffEffect);
                                        }
                                        break;

                                    #endregion

                                    #region Rank 6

                                    case 6:
                                        eid = Entity.NextEntity();
                                        effectDuration = 0.5f;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/bubble"),
                                            SpriteBounds = new Rectangle(0, 0, 76, 48),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        motivatePosition = _game.PositionComponent[GetPlayerID()];
                                        motivatePosition.Center.X += 40;
                                        motivatePosition.Center.Y -= 30;

                                        _game.PositionComponent.Add(eid, motivatePosition);

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            eid = Entity.NextEntity();

                                            buffEffect = new Buff()
                                            {
                                                EntityID = eid,
                                                TargetID = player.EntityID,
                                                Health = 30,
                                            };
                                            _game.BuffComponent.Add(eid, buffEffect);
                                        }
                                        break;

                                    #endregion

                                    #region Rank 7

                                    case 7:
                                        eid = Entity.NextEntity();
                                        effectDuration = 0.5f;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/bubble"),
                                            SpriteBounds = new Rectangle(0, 0, 76, 48),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        motivatePosition = _game.PositionComponent[GetPlayerID()];
                                        motivatePosition.Center.X += 40;
                                        motivatePosition.Center.Y -= 30;

                                        _game.PositionComponent.Add(eid, motivatePosition);

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            eid = Entity.NextEntity();

                                            buffEffect = new Buff()
                                            {
                                                EntityID = eid,
                                                TargetID = player.EntityID,
                                                Health = 35,
                                            };
                                            _game.BuffComponent.Add(eid, buffEffect);
                                        }
                                        break;

                                    #endregion

                                    #region Rank 8

                                    case 8:
                                        eid = Entity.NextEntity();
                                        effectDuration = 0.5f;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/bubble"),
                                            SpriteBounds = new Rectangle(0, 0, 76, 48),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        motivatePosition = _game.PositionComponent[GetPlayerID()];
                                        motivatePosition.Center.X += 40;
                                        motivatePosition.Center.Y -= 30;

                                        _game.PositionComponent.Add(eid, motivatePosition);

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            eid = Entity.NextEntity();

                                            buffEffect = new Buff()
                                            {
                                                EntityID = eid,
                                                TargetID = player.EntityID,
                                                Health = 40,
                                            };
                                            _game.BuffComponent.Add(eid, buffEffect);
                                        }
                                        break;

                                    #endregion

                                    #region Rank 9

                                    case 9:
                                        eid = Entity.NextEntity();
                                        effectDuration = 0.5f;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/bubble"),
                                            SpriteBounds = new Rectangle(0, 0, 76, 48),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        motivatePosition = _game.PositionComponent[GetPlayerID()];
                                        motivatePosition.Center.X += 40;
                                        motivatePosition.Center.Y -= 30;

                                        _game.PositionComponent.Add(eid, motivatePosition);

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            eid = Entity.NextEntity();

                                            buffEffect = new Buff()
                                            {
                                                EntityID = eid,
                                                TargetID = player.EntityID,
                                                Health = 45,
                                            };
                                            _game.BuffComponent.Add(eid, buffEffect);
                                        }
                                        break;

                                    #endregion

                                    #region Rank 10

                                    case 10:
                                        eid = Entity.NextEntity();
                                        effectDuration = 0.5f;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/bubble"),
                                            SpriteBounds = new Rectangle(0, 0, 76, 48),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        motivatePosition = _game.PositionComponent[GetPlayerID()];
                                        motivatePosition.Center.X += 40;
                                        motivatePosition.Center.Y -= 30;

                                        _game.PositionComponent.Add(eid, motivatePosition);

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            eid = Entity.NextEntity();

                                            buffEffect = new Buff()
                                            {
                                                EntityID = eid,
                                                TargetID = player.EntityID,
                                                Health = 50,
                                            };
                                            _game.BuffComponent.Add(eid, buffEffect);
                                        }
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

                                Buff buffEffect;

                                Sprite sprite;
                                Position fallBackPosition;

                                #endregion

                                switch (rank)
                                {
                                    #region Checking Rank
                                    case 1:
                                        eid = Entity.NextEntity();
                                        effectDuration = 0.5f;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/fallback"),
                                            SpriteBounds = new Rectangle(0, 0, 76, 48),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        fallBackPosition = _game.PositionComponent[GetPlayerID()];
                                        fallBackPosition.Center.X += 40;
                                        fallBackPosition.Center.Y -= 30;

                                        _game.PositionComponent.Add(eid, fallBackPosition);

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            eid = Entity.NextEntity();

                                            buffEffect = new Buff()
                                            {
                                                EntityID = eid,
                                                TargetID = player.EntityID,
                                                DefenseMelee = 1,
                                                AttackMelee = -1,
                                            };
                                            _game.BuffComponent.Add(eid, buffEffect);
                                        }

                                        break;

                                    case 2:
                                        eid = Entity.NextEntity();
                                        effectDuration = 0.5f;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/fallback"),
                                            SpriteBounds = new Rectangle(0, 0, 76, 48),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        fallBackPosition = _game.PositionComponent[GetPlayerID()];
                                        fallBackPosition.Center.X += 40;
                                        fallBackPosition.Center.Y -= 30;

                                        _game.PositionComponent.Add(eid, fallBackPosition);

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            eid = Entity.NextEntity();

                                            buffEffect = new Buff()
                                            {
                                                EntityID = eid,
                                                TargetID = player.EntityID,
                                                DefenseMelee = 2,
                                                AttackMelee = -1,
                                            };
                                            _game.BuffComponent.Add(eid, buffEffect);
                                        }

                                        break;

                                    case 3:
                                        eid = Entity.NextEntity();
                                        effectDuration = 0.5f;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/fallback"),
                                            SpriteBounds = new Rectangle(0, 0, 76, 48),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        fallBackPosition = _game.PositionComponent[GetPlayerID()];
                                        fallBackPosition.Center.X += 40;
                                        fallBackPosition.Center.Y -= 30;

                                        _game.PositionComponent.Add(eid, fallBackPosition);

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            eid = Entity.NextEntity();

                                            buffEffect = new Buff()
                                            {
                                                EntityID = eid,
                                                TargetID = player.EntityID,
                                                DefenseMelee = 3,
                                                AttackMelee = -2,
                                            };
                                            _game.BuffComponent.Add(eid, buffEffect);
                                        }
                                        break;

                                    case 4:
                                        eid = Entity.NextEntity();
                                        effectDuration = 0.5f;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/fallback"),
                                            SpriteBounds = new Rectangle(0, 0, 76, 48),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        fallBackPosition = _game.PositionComponent[GetPlayerID()];
                                        fallBackPosition.Center.X += 40;
                                        fallBackPosition.Center.Y -= 30;

                                        _game.PositionComponent.Add(eid, fallBackPosition);

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            eid = Entity.NextEntity();

                                            buffEffect = new Buff()
                                            {
                                                EntityID = eid,
                                                TargetID = player.EntityID,
                                                DefenseMelee = 4,
                                                AttackMelee = -2,
                                            };
                                            _game.BuffComponent.Add(eid, buffEffect);
                                        }
                                        break;

                                    case 5:
                                        eid = Entity.NextEntity();
                                        effectDuration = 0.5f;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/fallback"),
                                            SpriteBounds = new Rectangle(0, 0, 76, 48),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        fallBackPosition = _game.PositionComponent[GetPlayerID()];
                                        fallBackPosition.Center.X += 40;
                                        fallBackPosition.Center.Y -= 30;

                                        _game.PositionComponent.Add(eid, fallBackPosition);

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            eid = Entity.NextEntity();

                                            buffEffect = new Buff()
                                            {
                                                EntityID = eid,
                                                TargetID = player.EntityID,
                                                DefenseMelee = 5,
                                                AttackMelee = -3,
                                            };
                                            _game.BuffComponent.Add(eid, buffEffect);
                                        }
                                        break;

                                    case 6:
                                        eid = Entity.NextEntity();
                                        effectDuration = 0.5f;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/fallback"),
                                            SpriteBounds = new Rectangle(0, 0, 76, 48),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        fallBackPosition = _game.PositionComponent[GetPlayerID()];
                                        fallBackPosition.Center.X += 40;
                                        fallBackPosition.Center.Y -= 30;

                                        _game.PositionComponent.Add(eid, fallBackPosition);

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            eid = Entity.NextEntity();

                                            buffEffect = new Buff()
                                            {
                                                EntityID = eid,
                                                TargetID = player.EntityID,
                                                DefenseMelee = 6,
                                                AttackMelee = -3,
                                            };
                                            _game.BuffComponent.Add(eid, buffEffect);
                                        }
                                        break;

                                    case 7:
                                        eid = Entity.NextEntity();
                                        effectDuration = 0.5f;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/fallback"),
                                            SpriteBounds = new Rectangle(0, 0, 76, 48),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        fallBackPosition = _game.PositionComponent[GetPlayerID()];
                                        fallBackPosition.Center.X += 40;
                                        fallBackPosition.Center.Y -= 30;

                                        _game.PositionComponent.Add(eid, fallBackPosition);

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            eid = Entity.NextEntity();

                                            buffEffect = new Buff()
                                            {
                                                EntityID = eid,
                                                TargetID = player.EntityID,
                                                DefenseMelee = 7,
                                                AttackMelee = -4,
                                            };
                                            _game.BuffComponent.Add(eid, buffEffect);
                                        }
                                        break;

                                    case 8:
                                        eid = Entity.NextEntity();
                                        effectDuration = 0.5f;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/fallback"),
                                            SpriteBounds = new Rectangle(0, 0, 76, 48),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        fallBackPosition = _game.PositionComponent[GetPlayerID()];
                                        fallBackPosition.Center.X += 40;
                                        fallBackPosition.Center.Y -= 30;

                                        _game.PositionComponent.Add(eid, fallBackPosition);

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            eid = Entity.NextEntity();

                                            buffEffect = new Buff()
                                            {
                                                EntityID = eid,
                                                TargetID = player.EntityID,
                                                DefenseMelee = 8,
                                                AttackMelee = -4,
                                            };
                                            _game.BuffComponent.Add(eid, buffEffect);
                                        }
                                        break;

                                    case 9:
                                        eid = Entity.NextEntity();
                                        effectDuration = 0.5f;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/fallback"),
                                            SpriteBounds = new Rectangle(0, 0, 76, 48),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        fallBackPosition = _game.PositionComponent[GetPlayerID()];
                                        fallBackPosition.Center.X += 40;
                                        fallBackPosition.Center.Y -= 30;

                                        _game.PositionComponent.Add(eid, fallBackPosition);

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            eid = Entity.NextEntity();

                                            buffEffect = new Buff()
                                            {
                                                EntityID = eid,
                                                TargetID = player.EntityID,
                                                DefenseMelee = 9,
                                                AttackMelee = -5,
                                            };
                                            _game.BuffComponent.Add(eid, buffEffect);
                                        }
                                        break;

                                    case 10:
                                        eid = Entity.NextEntity();
                                        effectDuration = 0.5f;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/fallback"),
                                            SpriteBounds = new Rectangle(0, 0, 76, 48),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        fallBackPosition = _game.PositionComponent[GetPlayerID()];
                                        fallBackPosition.Center.X += 40;
                                        fallBackPosition.Center.Y -= 30;

                                        _game.PositionComponent.Add(eid, fallBackPosition);

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            eid = Entity.NextEntity();

                                            buffEffect = new Buff()
                                            {
                                                EntityID = eid,
                                                TargetID = player.EntityID,
                                                DefenseMelee = 10,
                                                AttackMelee = -5,
                                            };
                                            _game.BuffComponent.Add(eid, buffEffect);
                                        }
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

                                Buff buffEffect;

                                Sprite sprite;
                                Position chargePosition;


                                #endregion

                                switch (rank)
                                {
                                    #region Checking Rank
                                    case 1:
                                        eid = Entity.NextEntity();
                                        effectDuration = 0.5f;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/charge"),
                                            SpriteBounds = new Rectangle(0, 0, 76, 48),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        chargePosition = _game.PositionComponent[GetPlayerID()];
                                        chargePosition.Center.X += 40;
                                        chargePosition.Center.Y -= 30;

                                        _game.PositionComponent.Add(eid, chargePosition);

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            eid = Entity.NextEntity();

                                            buffEffect = new Buff()
                                            {
                                                EntityID = eid,
                                                TargetID = player.EntityID,
                                                DefenseMelee = -1,
                                                AttackMelee = 1,
                                            };
                                            _game.BuffComponent.Add(eid, buffEffect);
                                        }

                                        break;

                                    case 2:
                                        eid = Entity.NextEntity();
                                        effectDuration = 0.5f;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/charge"),
                                            SpriteBounds = new Rectangle(0, 0, 76, 48),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        chargePosition = _game.PositionComponent[GetPlayerID()];
                                        chargePosition.Center.X += 40;
                                        chargePosition.Center.Y -= 30;

                                        _game.PositionComponent.Add(eid, chargePosition);

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            eid = Entity.NextEntity();

                                            buffEffect = new Buff()
                                            {
                                                EntityID = eid,
                                                TargetID = player.EntityID,
                                                DefenseMelee = -1,
                                                AttackMelee = 2,
                                            };
                                            _game.BuffComponent.Add(eid, buffEffect);
                                        }

                                        break;

                                    case 3:
                                        eid = Entity.NextEntity();
                                        effectDuration = 0.5f;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/charge"),
                                            SpriteBounds = new Rectangle(0, 0, 76, 48),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        chargePosition = _game.PositionComponent[GetPlayerID()];
                                        chargePosition.Center.X += 40;
                                        chargePosition.Center.Y -= 30;

                                        _game.PositionComponent.Add(eid, chargePosition);

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            eid = Entity.NextEntity();

                                            buffEffect = new Buff()
                                            {
                                                EntityID = eid,
                                                TargetID = player.EntityID,
                                                DefenseMelee = -2,
                                                AttackMelee = 3,
                                            };
                                            _game.BuffComponent.Add(eid, buffEffect);
                                        }
                                        break;

                                    case 4:
                                        eid = Entity.NextEntity();
                                        effectDuration = 0.5f;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/charge"),
                                            SpriteBounds = new Rectangle(0, 0, 76, 48),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        chargePosition = _game.PositionComponent[GetPlayerID()];
                                        chargePosition.Center.X += 40;
                                        chargePosition.Center.Y -= 30;

                                        _game.PositionComponent.Add(eid, chargePosition);

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            eid = Entity.NextEntity();

                                            buffEffect = new Buff()
                                            {
                                                EntityID = eid,
                                                TargetID = player.EntityID,
                                                DefenseMelee = -2,
                                                AttackMelee = 4,
                                            };
                                            _game.BuffComponent.Add(eid, buffEffect);
                                        }
                                        break;

                                    case 5:
                                        eid = Entity.NextEntity();
                                        effectDuration = 0.5f;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/charge"),
                                            SpriteBounds = new Rectangle(0, 0, 76, 48),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        chargePosition = _game.PositionComponent[GetPlayerID()];
                                        chargePosition.Center.X += 40;
                                        chargePosition.Center.Y -= 30;

                                        _game.PositionComponent.Add(eid, chargePosition);

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            eid = Entity.NextEntity();

                                            buffEffect = new Buff()
                                            {
                                                EntityID = eid,
                                                TargetID = player.EntityID,
                                                DefenseMelee = -3,
                                                AttackMelee = 5,
                                            };
                                            _game.BuffComponent.Add(eid, buffEffect);
                                        }
                                        break;

                                    case 6:
                                        eid = Entity.NextEntity();
                                        effectDuration = 0.5f;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/charge"),
                                            SpriteBounds = new Rectangle(0, 0, 76, 48),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        chargePosition = _game.PositionComponent[GetPlayerID()];
                                        chargePosition.Center.X += 40;
                                        chargePosition.Center.Y -= 30;

                                        _game.PositionComponent.Add(eid, chargePosition);

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            eid = Entity.NextEntity();

                                            buffEffect = new Buff()
                                            {
                                                EntityID = eid,
                                                TargetID = player.EntityID,
                                                DefenseMelee = -3,
                                                AttackMelee = 6,
                                            };
                                            _game.BuffComponent.Add(eid, buffEffect);
                                        }
                                        break;

                                    case 7:
                                        eid = Entity.NextEntity();
                                        effectDuration = 0.5f;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/charge"),
                                            SpriteBounds = new Rectangle(0, 0, 76, 48),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        chargePosition = _game.PositionComponent[GetPlayerID()];
                                        chargePosition.Center.X += 40;
                                        chargePosition.Center.Y -= 30;

                                        _game.PositionComponent.Add(eid, chargePosition);

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            eid = Entity.NextEntity();

                                            buffEffect = new Buff()
                                            {
                                                EntityID = eid,
                                                TargetID = player.EntityID,
                                                DefenseMelee = -4,
                                                AttackMelee = 7,
                                            };
                                            _game.BuffComponent.Add(eid, buffEffect);
                                        }
                                        break;

                                    case 8:
                                        eid = Entity.NextEntity();
                                        effectDuration = 0.5f;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/charge"),
                                            SpriteBounds = new Rectangle(0, 0, 76, 48),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        chargePosition = _game.PositionComponent[GetPlayerID()];
                                        chargePosition.Center.X += 40;
                                        chargePosition.Center.Y -= 30;

                                        _game.PositionComponent.Add(eid, chargePosition);

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            eid = Entity.NextEntity();

                                            buffEffect = new Buff()
                                            {
                                                EntityID = eid,
                                                TargetID = player.EntityID,
                                                DefenseMelee = -4,
                                                AttackMelee = 8,
                                            };
                                            _game.BuffComponent.Add(eid, buffEffect);
                                        }
                                        break;

                                    case 9:
                                        eid = Entity.NextEntity();
                                        effectDuration = 0.5f;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/charge"),
                                            SpriteBounds = new Rectangle(0, 0, 76, 48),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        chargePosition = _game.PositionComponent[GetPlayerID()];
                                        chargePosition.Center.X += 40;
                                        chargePosition.Center.Y -= 30;

                                        _game.PositionComponent.Add(eid, chargePosition);

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            eid = Entity.NextEntity();

                                            buffEffect = new Buff()
                                            {
                                                EntityID = eid,
                                                TargetID = player.EntityID,
                                                DefenseMelee = -5,
                                                AttackMelee = 9,
                                            };
                                            _game.BuffComponent.Add(eid, buffEffect);
                                        }
                                        break;

                                    case 10:
                                        eid = Entity.NextEntity();
                                        effectDuration = 0.5f;

                                        timedEffect = new TimedEffect()
                                        {
                                            EntityID = eid,
                                            TotalDuration = effectDuration,
                                            TimeLeft = effectDuration
                                        };
                                        _game.TimedEffectComponent.Add(eid, timedEffect);

                                        sprite = new Sprite()
                                        {
                                            EntityID = eid,
                                            SpriteSheet = _game.Content.Load<Texture2D>("Spritesheets/Skills/Effects/charge"),
                                            SpriteBounds = new Rectangle(0, 0, 76, 48),
                                        };
                                        _game.SpriteComponent.Add(eid, sprite);

                                        chargePosition = _game.PositionComponent[GetPlayerID()];
                                        chargePosition.Center.X += 40;
                                        chargePosition.Center.Y -= 30;

                                        _game.PositionComponent.Add(eid, chargePosition);

                                        foreach (Player player in _game.PlayerComponent.All)
                                        {
                                            eid = Entity.NextEntity();

                                            buffEffect = new Buff()
                                            {
                                                EntityID = eid,
                                                TargetID = player.EntityID,
                                                DefenseMelee = -5,
                                                AttackMelee = 10,
                                            };
                                            _game.BuffComponent.Add(eid, buffEffect);
                                        }
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
                            int attackDecrease = -50;
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
                                        MovementSpeed = speedIncrease,
                                        AttackMelee = attackDecrease
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
                            int offhand;
                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
                                    eid = Entity.NextEntity();
                                    offhand = -80;
                                    targetID = GetPlayerID();
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        AttackRanged = offhand,
                                        AttackMelee = offhand
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);
                                    break;

                                case 2:
                                    eid = Entity.NextEntity();
                                    offhand = -74;
                                    targetID = GetPlayerID();
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        AttackRanged = offhand,
                                        AttackMelee = offhand
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);
                                    break;

                                case 3:
                                    eid = Entity.NextEntity();
                                    offhand = -68;
                                    targetID = GetPlayerID();
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        AttackRanged = offhand,
                                        AttackMelee = offhand
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);
                                    break;

                                case 4:
                                    eid = Entity.NextEntity();
                                    offhand = -62;
                                    targetID = GetPlayerID();
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        AttackRanged = offhand,
                                        AttackMelee = offhand
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);
                                    break;

                                case 5:
                                    eid = Entity.NextEntity();
                                    offhand = -56;
                                    targetID = GetPlayerID();
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        AttackRanged = offhand,
                                        AttackMelee = offhand
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);
                                    break;

                                case 6:
                                    eid = Entity.NextEntity();
                                    offhand = -50;
                                    targetID = GetPlayerID();
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        AttackRanged = offhand,
                                        AttackMelee = offhand
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);
                                    break;

                                case 7:
                                    eid = Entity.NextEntity();
                                    offhand = -44;
                                    targetID = GetPlayerID();
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        AttackRanged = offhand,
                                        AttackMelee = offhand
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);
                                    break;

                                case 8:
                                    eid = Entity.NextEntity();
                                    offhand = -38;
                                    targetID = GetPlayerID();
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        AttackRanged = offhand,
                                        AttackMelee = offhand
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);
                                    break;

                                case 9:
                                    eid = Entity.NextEntity();
                                    offhand = -32;
                                    targetID = GetPlayerID();
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        AttackRanged = offhand,
                                        AttackMelee = offhand
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);
                                    break;

                                case 10:
                                    eid = Entity.NextEntity();
                                    offhand = -20;
                                    targetID = GetPlayerID();
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        AttackRanged = offhand,
                                        AttackMelee = offhand
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);
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
                        //Trick shot has been modified greatly to fit into our prototype game.
                        case SkillType.TrickShot:

                            #region Skill Variables
                            int TSDamage;
                            DirectDamage dd;
                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
                                    eid = Entity.NextEntity();
                                    TSDamage = 20;
                                    dd = new DirectDamage()
                                    {
                                        EntityID = eid,
                                        Damage = TSDamage
                                    };
                                    _game.DirectDamageComponent.Add(eid, dd);
                                    break;

                                case 2:
                                    eid = Entity.NextEntity();
                                    TSDamage = 40;
                                    dd = new DirectDamage()
                                    {
                                        EntityID = eid,
                                        Damage = TSDamage
                                    };
                                    _game.DirectDamageComponent.Add(eid, dd);
                                    break;

                                case 3:
                                    eid = Entity.NextEntity();
                                    TSDamage = 60;
                                    dd = new DirectDamage()
                                    {
                                        EntityID = eid,
                                        Damage = TSDamage
                                    };
                                    _game.DirectDamageComponent.Add(eid, dd);
                                    break;

                                case 4:
                                    eid = Entity.NextEntity();
                                    TSDamage = 80;
                                    dd = new DirectDamage()
                                    {
                                        EntityID = eid,
                                        Damage = TSDamage
                                    };
                                    _game.DirectDamageComponent.Add(eid, dd);
                                    break;

                                case 5:
                                    eid = Entity.NextEntity();
                                    TSDamage = 100;
                                    dd = new DirectDamage()
                                    {
                                        EntityID = eid,
                                        Damage = TSDamage
                                    };
                                    _game.DirectDamageComponent.Add(eid, dd);
                                    break;

                                case 6:
                                    eid = Entity.NextEntity();
                                    TSDamage = 120;
                                    dd = new DirectDamage()
                                    {
                                        EntityID = eid,
                                        Damage = TSDamage
                                    };
                                    _game.DirectDamageComponent.Add(eid, dd);
                                    break;

                                case 7:
                                    eid = Entity.NextEntity();
                                    TSDamage = 140;
                                    dd = new DirectDamage()
                                    {
                                        EntityID = eid,
                                        Damage = TSDamage
                                    };
                                    _game.DirectDamageComponent.Add(eid, dd);
                                    break;

                                case 8:
                                    eid = Entity.NextEntity();
                                    TSDamage = 160;
                                    dd = new DirectDamage()
                                    {
                                        EntityID = eid,
                                        Damage = TSDamage
                                    };
                                    _game.DirectDamageComponent.Add(eid, dd);
                                    break;

                                case 9:
                                    eid = Entity.NextEntity();
                                    TSDamage = 180;
                                    dd = new DirectDamage()
                                    {
                                        EntityID = eid,
                                        Damage = TSDamage
                                    };
                                    _game.DirectDamageComponent.Add(eid, dd);
                                    break;

                                case 10:
                                    eid = Entity.NextEntity();
                                    TSDamage = 200;
                                    dd = new DirectDamage()
                                    {
                                        EntityID = eid,
                                        Damage = TSDamage
                                    };
                                    _game.DirectDamageComponent.Add(eid, dd);
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
                            InstantEffect ie;
                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
                                    eid = Entity.NextEntity();
                                    PSDamageIncrease = 10;
                                    targetID = GetPlayerID();
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        AttackRanged = PSDamageIncrease
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);

                                    ie = new InstantEffect()
                                    {
                                        EntityID = eid
                                    };
                                    _game.InstantEffectComponent.Add(eid, ie);
                                    break;

                                case 2:
                                    eid = Entity.NextEntity();
                                    PSDamageIncrease = 20;
                                    targetID = GetPlayerID();
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        AttackRanged = PSDamageIncrease
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);

                                    ie = new InstantEffect()
                                    {
                                        EntityID = eid
                                    };
                                    _game.InstantEffectComponent.Add(eid, ie);
                                    break;

                                case 3:
                                    eid = Entity.NextEntity();
                                    PSDamageIncrease = 30;
                                    targetID = GetPlayerID();
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        AttackRanged = PSDamageIncrease
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);

                                    ie = new InstantEffect()
                                    {
                                        EntityID = eid
                                    };
                                    _game.InstantEffectComponent.Add(eid, ie);
                                    break;

                                case 4:
                                    eid = Entity.NextEntity();
                                    PSDamageIncrease = 40;
                                    targetID = GetPlayerID();
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        AttackRanged = PSDamageIncrease
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);

                                    ie = new InstantEffect()
                                    {
                                        EntityID = eid
                                    };
                                    _game.InstantEffectComponent.Add(eid, ie);
                                    break;

                                case 5:
                                    eid = Entity.NextEntity();
                                    PSDamageIncrease = 50;
                                    targetID = GetPlayerID();
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        AttackRanged = PSDamageIncrease
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);

                                    ie = new InstantEffect()
                                    {
                                        EntityID = eid
                                    };
                                    _game.InstantEffectComponent.Add(eid, ie);
                                    break;

                                case 6:
                                    eid = Entity.NextEntity();
                                    PSDamageIncrease = 60;
                                    targetID = GetPlayerID();
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        AttackRanged = PSDamageIncrease
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);

                                    ie = new InstantEffect()
                                    {
                                        EntityID = eid
                                    };
                                    _game.InstantEffectComponent.Add(eid, ie);
                                    break;

                                case 7:
                                    eid = Entity.NextEntity();
                                    PSDamageIncrease = 70;
                                    targetID = GetPlayerID();
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        AttackRanged = PSDamageIncrease
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);

                                    ie = new InstantEffect()
                                    {
                                        EntityID = eid
                                    };
                                    _game.InstantEffectComponent.Add(eid, ie);
                                    break;

                                case 8:
                                    eid = Entity.NextEntity();
                                    PSDamageIncrease = 80;
                                    targetID = GetPlayerID();
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        AttackRanged = PSDamageIncrease
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);

                                    ie = new InstantEffect()
                                    {
                                        EntityID = eid
                                    };
                                    _game.InstantEffectComponent.Add(eid, ie);
                                    break;

                                case 9:
                                    eid = Entity.NextEntity();
                                    PSDamageIncrease = 90;
                                    targetID = GetPlayerID();
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        AttackRanged = PSDamageIncrease
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);

                                    ie = new InstantEffect()
                                    {
                                        EntityID = eid
                                    };
                                    _game.InstantEffectComponent.Add(eid, ie);
                                    break;

                                case 10:
                                    eid = Entity.NextEntity();
                                    PSDamageIncrease = 100;
                                    targetID = GetPlayerID();
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        AttackRanged = PSDamageIncrease
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);

                                    ie = new InstantEffect()
                                    {
                                        EntityID = eid
                                    };
                                    _game.InstantEffectComponent.Add(eid, ie);
                                    break;

                                default:
                                    break;
                                #endregion
                            }
                            break;
                        #endregion

                        #region EagleShot
                        //EagleShot has been heavly modified to fit within our game prototype.
                        case SkillType.EagleShot:

                            #region Skill Variables
                            int ESA;
                            InstantEffect eie;
                            #endregion

                            switch (rank)
                            {
                                #region Checking Rank
                                case 1:
                                    eid = Entity.NextEntity();
                                    targetID = GetPlayerID();
                                    ESA = 10;
                                    eie = new InstantEffect()
                                    {
                                        EntityID = eid,
                                    };
                                    _game.InstantEffectComponent.Add(eid, eie);
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        WeaponAccuracy = ESA
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);
                                    break;

                                case 2:
                                    eid = Entity.NextEntity();
                                    targetID = GetPlayerID();
                                    ESA = 20;
                                    eie = new InstantEffect()
                                    {
                                        EntityID = eid,
                                    };
                                    _game.InstantEffectComponent.Add(eid, eie);
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        WeaponAccuracy = ESA
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);
                                    break;

                                case 3:
                                    eid = Entity.NextEntity();
                                    targetID = GetPlayerID();
                                    ESA = 30;
                                    eie = new InstantEffect()
                                    {
                                        EntityID = eid,
                                    };
                                    _game.InstantEffectComponent.Add(eid, eie);
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        WeaponAccuracy = ESA
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);
                                    break;

                                case 4:
                                    eid = Entity.NextEntity();
                                    targetID = GetPlayerID();
                                    ESA = 40;
                                    eie = new InstantEffect()
                                    {
                                        EntityID = eid,
                                    };
                                    _game.InstantEffectComponent.Add(eid, eie);
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        WeaponAccuracy = ESA
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);
                                    break;

                                case 5:
                                    eid = Entity.NextEntity();
                                    targetID = GetPlayerID();
                                    ESA = 50;
                                    eie = new InstantEffect()
                                    {
                                        EntityID = eid,
                                    };
                                    _game.InstantEffectComponent.Add(eid, eie);
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        WeaponAccuracy = ESA
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);
                                    break;

                                case 6:
                                    eid = Entity.NextEntity();
                                    targetID = GetPlayerID();
                                    ESA = 60;
                                    eie = new InstantEffect()
                                    {
                                        EntityID = eid,
                                    };
                                    _game.InstantEffectComponent.Add(eid, eie);
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        WeaponAccuracy = ESA
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);
                                    break;

                                case 7:
                                    eid = Entity.NextEntity();
                                    targetID = GetPlayerID();
                                    ESA = 70;
                                    eie = new InstantEffect()
                                    {
                                        EntityID = eid,
                                    };
                                    _game.InstantEffectComponent.Add(eid, eie);
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        WeaponAccuracy = ESA
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);
                                    break;

                                case 8:
                                    eid = Entity.NextEntity();
                                    targetID = GetPlayerID();
                                    ESA = 80;
                                    eie = new InstantEffect()
                                    {
                                        EntityID = eid,
                                    };
                                    _game.InstantEffectComponent.Add(eid, eie);
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        WeaponAccuracy = ESA
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);
                                    break;

                                case 9:
                                    eid = Entity.NextEntity();
                                    targetID = GetPlayerID();
                                    ESA = 90;
                                    eie = new InstantEffect()
                                    {
                                        EntityID = eid,
                                    };
                                    _game.InstantEffectComponent.Add(eid, eie);
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        WeaponAccuracy = ESA
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);
                                    break;

                                case 10:
                                    eid = Entity.NextEntity();
                                    targetID = GetPlayerID();
                                    ESA = 100;
                                    eie = new InstantEffect()
                                    {
                                        EntityID = eid,
                                    };
                                    _game.InstantEffectComponent.Add(eid, eie);
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        WeaponAccuracy = ESA
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);
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
                                    _game.ChanceToSucceedComponent.Add(eid, cts);
                                    break;

                                case 2:
                                    eid = Entity.NextEntity();
                                    prob = 10;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    _game.ChanceToSucceedComponent.Add(eid, cts);
                                    break;

                                case 3:
                                    eid = Entity.NextEntity();
                                    prob = 20;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    _game.ChanceToSucceedComponent.Add(eid, cts);
                                    break;

                                case 4:
                                    eid = Entity.NextEntity();
                                    prob = 30;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    _game.ChanceToSucceedComponent.Add(eid, cts);
                                    break;

                                case 5:
                                    eid = Entity.NextEntity();
                                    prob = 40;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    _game.ChanceToSucceedComponent.Add(eid, cts);
                                    break;

                                case 6:
                                    eid = Entity.NextEntity();
                                    prob = 50;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    _game.ChanceToSucceedComponent.Add(eid, cts);
                                    break;

                                case 7:
                                    eid = Entity.NextEntity();
                                    prob = 60;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    _game.ChanceToSucceedComponent.Add(eid, cts);
                                    break;

                                case 8:
                                    eid = Entity.NextEntity();
                                    prob = 70;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    _game.ChanceToSucceedComponent.Add(eid, cts);
                                    break;

                                case 9:
                                    eid = Entity.NextEntity();
                                    prob = 80;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    _game.ChanceToSucceedComponent.Add(eid, cts);
                                    break;

                                case 10:
                                    eid = Entity.NextEntity();
                                    prob = 90;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    _game.ChanceToSucceedComponent.Add(eid, cts);
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
                            int mugA;
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
                                    _game.ChanceToSucceedComponent.Add(eid, cts);

                                    mugA = -75;
                                    targetID = GetPlayerID();
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        AttackMelee = mugA
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);
                                    break;

                                case 2:
                                    eid = Entity.NextEntity();
                                    prob = 10;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    _game.ChanceToSucceedComponent.Add(eid, cts);

                                    mugA = -75;
                                    targetID = GetPlayerID();
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        AttackMelee = mugA
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);
                                    break;

                                case 3:
                                    eid = Entity.NextEntity();
                                    prob = 20;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    _game.ChanceToSucceedComponent.Add(eid, cts);

                                    mugA = -75;
                                    targetID = GetPlayerID();
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        AttackMelee = mugA
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);
                                    break;

                                case 4:
                                    eid = Entity.NextEntity();
                                    prob = 30;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    _game.ChanceToSucceedComponent.Add(eid, cts);

                                    mugA = -75;
                                    targetID = GetPlayerID();
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        AttackMelee = mugA
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);
                                    break;

                                case 5:
                                    eid = Entity.NextEntity();
                                    prob = 40;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    _game.ChanceToSucceedComponent.Add(eid, cts);

                                    mugA = -75;
                                    targetID = GetPlayerID();
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        AttackMelee = mugA
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);
                                    break;

                                case 6:
                                    eid = Entity.NextEntity();
                                    prob = 50;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    _game.ChanceToSucceedComponent.Add(eid, cts);

                                    mugA = -75;
                                    targetID = GetPlayerID();
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        AttackMelee = mugA
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);
                                    break;

                                case 7:
                                    eid = Entity.NextEntity();
                                    prob = 60;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    _game.ChanceToSucceedComponent.Add(eid, cts);

                                    mugA = -75;
                                    targetID = GetPlayerID();
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        AttackMelee = mugA
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);
                                    break;

                                case 8:
                                    eid = Entity.NextEntity();
                                    prob = 70;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    _game.ChanceToSucceedComponent.Add(eid, cts);

                                    mugA = -75;
                                    targetID = GetPlayerID();
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        AttackMelee = mugA
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);
                                    break;

                                case 9:
                                    eid = Entity.NextEntity();
                                    prob = 80;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    _game.ChanceToSucceedComponent.Add(eid, cts);

                                    mugA = -75;
                                    targetID = GetPlayerID();
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        AttackMelee = mugA
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);
                                    break;

                                case 10:
                                    eid = Entity.NextEntity();
                                    prob = 90;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    _game.ChanceToSucceedComponent.Add(eid, cts);

                                    mugA = -75;
                                    targetID = GetPlayerID();
                                    buffeffect = new Buff()
                                    {
                                        EntityID = eid,
                                        TargetID = targetID,
                                        AttackMelee = mugA
                                    };
                                    _game.BuffComponent.Add(eid, buffeffect);
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
                                    _game.ChanceToSucceedComponent.Add(eid, cts);
                                    break;

                                case 2:
                                    eid = Entity.NextEntity();
                                    prob = 10;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    _game.ChanceToSucceedComponent.Add(eid, cts);
                                    break;

                                case 3:
                                    eid = Entity.NextEntity();
                                    prob = 20;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    _game.ChanceToSucceedComponent.Add(eid, cts);
                                    break;

                                case 4:
                                    eid = Entity.NextEntity();
                                    prob = 30;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    _game.ChanceToSucceedComponent.Add(eid, cts);
                                    break;

                                case 5:
                                    eid = Entity.NextEntity();
                                    prob = 40;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    _game.ChanceToSucceedComponent.Add(eid, cts);
                                    break;

                                case 6:
                                    eid = Entity.NextEntity();
                                    prob = 50;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    _game.ChanceToSucceedComponent.Add(eid, cts);
                                    break;

                                case 7:
                                    eid = Entity.NextEntity();
                                    prob = 60;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    _game.ChanceToSucceedComponent.Add(eid, cts);
                                    break;

                                case 8:
                                    eid = Entity.NextEntity();
                                    prob = 70;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    _game.ChanceToSucceedComponent.Add(eid, cts);
                                    break;

                                case 9:
                                    eid = Entity.NextEntity();
                                    prob = 80;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    _game.ChanceToSucceedComponent.Add(eid, cts);
                                    break;

                                case 10:
                                    eid = Entity.NextEntity();
                                    prob = 90;
                                    cts = new ChanceToSucceed()
                                    {
                                        EntityID = eid,
                                        SuccessRateAsPercentage = prob
                                    };
                                    _game.ChanceToSucceedComponent.Add(eid, cts);
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


        public void TriggerEffect(SkillType type, int rank, bool friendly, uint target, uint owner)
        {
            uint eid = Entity.NextEntity();
            HealOverTime HoT;
            TimedEffect time;
            DamageOverTime DoT;
            Buff buff;
            InstantEffect instantEffect;
            DirectDamage directDamage;
            InstantEffect instant;

            switch (type)
            {
                #region Vermis Triggered SKills

                #region ThrownBlade
                case SkillType.ThrownBlades:
                    if (friendly)
                        return;

                    eid = Entity.NextEntity();
                    instantEffect = new InstantEffect() { EntityID = eid, isTriggered = true };
                    DirectDamage damage;
                    switch (rank)
                    {
                        #region Rank 1
                        case 1:
                            damage = new DirectDamage()
                            {
                                Damage = 10,
                                EntityID = eid,
                                TargetID = target,
                            };
                            break;
                        #endregion

                        #region Rank 2
                        case 2:
                            damage = new DirectDamage()
                            {
                                Damage = 10,
                                EntityID = eid,
                                TargetID = target,
                            };
                            break;
                        #endregion

                        #region Rank 3
                        case 3:
                            damage = new DirectDamage()
                            {
                                Damage = 11,
                                EntityID = eid,
                                TargetID = target,
                            };
                            break;
                        #endregion

                        #region Rank 4
                        case 4:
                            damage = new DirectDamage()
                            {
                                Damage = 11,
                                EntityID = eid,
                                TargetID = target,
                            };
                            break;
                        #endregion

                        #region Rank 5
                        case 5:
                            damage = new DirectDamage()
                            {
                                Damage = 12,
                                EntityID = eid,
                                TargetID = target,
                            };
                            break;
                        #endregion

                        #region Rank 6
                        case 6:
                            damage = new DirectDamage()
                            {
                                Damage = 12,
                                EntityID = eid,
                                TargetID = target,
                            };
                            break;
                        #endregion

                        #region Rank 7
                        case 7:
                            damage = new DirectDamage()
                            {
                                Damage = 13,
                                EntityID = eid,
                                TargetID = target,
                            };
                            break;
                        #endregion

                        #region Rank 8
                        case 8:
                            damage = new DirectDamage()
                            {
                                Damage = 13,
                                EntityID = eid,
                                TargetID = target,
                            };
                            break;
                        #endregion

                        #region Rank 9
                        case 9:
                            damage = new DirectDamage()
                            {
                                Damage = 14,
                                EntityID = eid,
                                TargetID = target,
                            };
                            break;
                        #endregion

                        #region Rank 10
                        case 10:
                            damage = new DirectDamage()
                            {
                                Damage = 15,
                                EntityID = eid,
                                TargetID = target,
                            };
                            break;
                        #endregion
                        default:
                            throw new Exception("Unimplemented Rank");
                    }
                    _game.DirectDamageComponent.Add(eid, damage);
                    instant = new InstantEffect() { EntityID = eid, isTriggered = false, };
                    _game.InstantEffectComponent.Add(eid, instant);
                    break;
                #endregion

                //unsure how to properly trigger, left unimplemented
                #region Caustic Weapons
                case SkillType.CausticWeapons:
                    switch (rank)
                    {
                        #region Rank 1
                        case 1:
                            break;
                        #endregion

                        #region Rank 2
                        case 2:
                            break;
                        #endregion

                        #region Rank 3
                        case 3:
                            break;
                        #endregion

                        #region Rank 4
                        case 4:
                            break;
                        #endregion

                        #region Rank 5
                        case 5:
                            break;
                        #endregion

                        #region Rank 6
                        case 6:
                            break;
                        #endregion

                        #region Rank 7
                        case 7:
                            break;
                        #endregion

                        #region Rank 8
                        case 8:
                            break;
                        #endregion

                        #region Rank 9
                        case 9:
                            break;
                        #endregion

                        #region Rank 10
                        case 10:
                            break;
                        #endregion
                        default:
                            throw new Exception("Unimplemented Rank");
                    }
                    break;
                #endregion

                #region BenignParasite
                case SkillType.BenignParasite:
                    if (!friendly)
                        return;
                    eid = Entity.NextEntity();

                    switch (rank)
                    {
                        #region Rank 1
                        case 1:
                            HoT = new HealOverTime()
                            {
                                AmountPerTick = 1,
                                CurrentStack = 1,
                                CurrentTime = 0,
                                EntityID = eid,
                                MaxStack = 1,
                                TargetID = target,
                                TickTime = 1,
                            };
                            _game.HealOverTimeComponent.Add(eid, HoT);

                            time = new TimedEffect()
                            {
                                EntityID = eid,
                                TimeLeft = 10,
                                TotalDuration = 10,
                            };
                            _game.TimedEffectComponent.Add(eid, time);
                            break;
                        #endregion

                        #region Rank 2
                        case 2:
                            HoT = new HealOverTime()
                            {
                                AmountPerTick = 1,
                                CurrentStack = 1,
                                CurrentTime = 0,
                                EntityID = eid,
                                MaxStack = 1,
                                TargetID = target,
                                TickTime = 1,
                            };
                            _game.HealOverTimeComponent.Add(eid, HoT);

                            time = new TimedEffect()
                            {
                                EntityID = eid,
                                TimeLeft = 12,
                                TotalDuration = 12,
                            };
                            _game.TimedEffectComponent.Add(eid, time);
                            break;
                        #endregion

                        #region Rank 3
                        case 3:
                            HoT = new HealOverTime()
                            {
                                AmountPerTick = 2,
                                CurrentStack = 1,
                                CurrentTime = 0,
                                EntityID = eid,
                                MaxStack = 1,
                                TargetID = target,
                                TickTime = 1,
                            };
                            _game.HealOverTimeComponent.Add(eid, HoT);

                            time = new TimedEffect()
                            {
                                EntityID = eid,
                                TimeLeft = 12,
                                TotalDuration = 12,
                            };
                            _game.TimedEffectComponent.Add(eid, time);
                            break;
                        #endregion

                        #region Rank 4
                        case 4:
                            HoT = new HealOverTime()
                            {
                                AmountPerTick = 2,
                                CurrentStack = 1,
                                CurrentTime = 0,
                                EntityID = eid,
                                MaxStack = 1,
                                TargetID = target,
                                TickTime = 1,
                            };
                            _game.HealOverTimeComponent.Add(eid, HoT);

                            time = new TimedEffect()
                            {
                                EntityID = eid,
                                TimeLeft = 14,
                                TotalDuration = 14,
                            };
                            _game.TimedEffectComponent.Add(eid, time);
                            break;
                        #endregion

                        #region Rank 5
                        case 5:
                            HoT = new HealOverTime()
                            {
                                AmountPerTick = 3,
                                CurrentStack = 1,
                                CurrentTime = 0,
                                EntityID = eid,
                                MaxStack = 1,
                                TargetID = target,
                                TickTime = 1,
                            };
                            _game.HealOverTimeComponent.Add(eid, HoT);

                            time = new TimedEffect()
                            {
                                EntityID = eid,
                                TimeLeft = 14,
                                TotalDuration = 14,
                            };
                            _game.TimedEffectComponent.Add(eid, time);
                            break;
                        #endregion

                        #region Rank 6
                        case 6:
                            HoT = new HealOverTime()
                            {
                                AmountPerTick = 3,
                                CurrentStack = 1,
                                CurrentTime = 0,
                                EntityID = eid,
                                MaxStack = 1,
                                TargetID = target,
                                TickTime = 1,
                            };
                            _game.HealOverTimeComponent.Add(eid, HoT);

                            time = new TimedEffect()
                            {
                                EntityID = eid,
                                TimeLeft = 16,
                                TotalDuration = 16,
                            };
                            _game.TimedEffectComponent.Add(eid, time);
                            break;
                        #endregion

                        #region Rank 7
                        case 7:
                            HoT = new HealOverTime()
                            {
                                AmountPerTick = 4,
                                CurrentStack = 1,
                                CurrentTime = 0,
                                EntityID = eid,
                                MaxStack = 1,
                                TargetID = target,
                                TickTime = 1,
                            };
                            _game.HealOverTimeComponent.Add(eid, HoT);

                            time = new TimedEffect()
                            {
                                EntityID = eid,
                                TimeLeft = 16,
                                TotalDuration = 16,
                            };
                            _game.TimedEffectComponent.Add(eid, time);
                            break;
                        #endregion

                        #region Rank 8
                        case 8:
                            HoT = new HealOverTime()
                            {
                                AmountPerTick = 4,
                                CurrentStack = 1,
                                CurrentTime = 0,
                                EntityID = eid,
                                MaxStack = 1,
                                TargetID = target,
                                TickTime = 1,
                            };
                            _game.HealOverTimeComponent.Add(eid, HoT);

                            time = new TimedEffect()
                            {
                                EntityID = eid,
                                TimeLeft = 18,
                                TotalDuration = 18,
                            };
                            _game.TimedEffectComponent.Add(eid, time);
                            break;
                        #endregion

                        #region Rank 9
                        case 9:
                            HoT = new HealOverTime()
                            {
                                AmountPerTick = 5,
                                CurrentStack = 1,
                                CurrentTime = 0,
                                EntityID = eid,
                                MaxStack = 1,
                                TargetID = target,
                                TickTime = 1,
                            };
                            _game.HealOverTimeComponent.Add(eid, HoT);

                            time = new TimedEffect()
                            {
                                EntityID = eid,
                                TimeLeft = 18,
                                TotalDuration = 18,
                            };
                            _game.TimedEffectComponent.Add(eid, time);
                            break;
                        #endregion

                        #region Rank 10
                        case 10:
                            HoT = new HealOverTime()
                            {
                                AmountPerTick = 7,
                                CurrentStack = 1,
                                CurrentTime = 0,
                                EntityID = eid,
                                MaxStack = 1,
                                TargetID = target,
                                TickTime = 1,
                            };
                            _game.HealOverTimeComponent.Add(eid, HoT);

                            time = new TimedEffect()
                            {
                                EntityID = eid,
                                TimeLeft = 20,
                                TotalDuration = 20,
                            };
                            _game.TimedEffectComponent.Add(eid, time);

                            break;
                        #endregion
                        default:
                            throw new Exception("Unimplemented Rank");
                    }
                    break;
                #endregion

                #region Malicious Parasite
                case SkillType.MaliciousParasite:
                    if (friendly)
                        return;

                    eid = Entity.NextEntity();
                    switch (rank)
                    {
                        #region Rank 1
                        case 1:
                            DoT = new DamageOverTime()
                            {
                                AmountPerTick = 1,
                                CurrentStack = 1,
                                CurrentTime = 0,
                                EntityID = eid,
                                MaxStack = 1,
                                TargetID = target,
                                TickTime = 1,
                            };
                            _game.DamageOverTimeComponent.Add(eid, DoT);

                            time = new TimedEffect()
                            {
                                EntityID = eid,
                                TimeLeft = 10,
                                TotalDuration = 10,
                            };
                            _game.TimedEffectComponent.Add(eid, time);

                            break;
                        #endregion

                        #region Rank 2
                        case 2:
                            DoT = new DamageOverTime()
                            {
                                AmountPerTick = 1,
                                CurrentStack = 1,
                                CurrentTime = 0,
                                EntityID = eid,
                                MaxStack = 1,
                                TargetID = target,
                                TickTime = 1,
                            };
                            _game.DamageOverTimeComponent.Add(eid, DoT);

                            time = new TimedEffect()
                            {
                                EntityID = eid,
                                TimeLeft = 10,
                                TotalDuration = 12,
                            };
                            _game.TimedEffectComponent.Add(eid, time);

                            break;
                        #endregion

                        #region Rank 3
                        case 3:
                            DoT = new DamageOverTime()
                            {
                                AmountPerTick = 2,
                                CurrentStack = 1,
                                CurrentTime = 0,
                                EntityID = eid,
                                MaxStack = 1,
                                TargetID = target,
                                TickTime = 1,
                            };
                            _game.DamageOverTimeComponent.Add(eid, DoT);

                            time = new TimedEffect()
                            {
                                EntityID = eid,
                                TimeLeft = 12,
                                TotalDuration = 12,
                            };
                            _game.TimedEffectComponent.Add(eid, time);

                            break;
                        #endregion

                        #region Rank 4
                        case 4:
                            DoT = new DamageOverTime()
                            {
                                AmountPerTick = 2,
                                CurrentStack = 1,
                                CurrentTime = 0,
                                EntityID = eid,
                                MaxStack = 1,
                                TargetID = target,
                                TickTime = 1,
                            };
                            _game.DamageOverTimeComponent.Add(eid, DoT);

                            time = new TimedEffect()
                            {
                                EntityID = eid,
                                TimeLeft = 14,
                                TotalDuration = 14,
                            };
                            _game.TimedEffectComponent.Add(eid, time);
                            break;
                        #endregion

                        #region Rank 5
                        case 5:
                            DoT = new DamageOverTime()
                            {
                                AmountPerTick = 3,
                                CurrentStack = 1,
                                CurrentTime = 0,
                                EntityID = eid,
                                MaxStack = 1,
                                TargetID = target,
                                TickTime = 1,
                            };
                            _game.DamageOverTimeComponent.Add(eid, DoT);

                            time = new TimedEffect()
                            {
                                EntityID = eid,
                                TimeLeft = 14,
                                TotalDuration = 14,
                            };
                            _game.TimedEffectComponent.Add(eid, time);
                            break;
                        #endregion

                        #region Rank 6
                        case 6:
                            DoT = new DamageOverTime()
                            {
                                AmountPerTick = 3,
                                CurrentStack = 1,
                                CurrentTime = 0,
                                EntityID = eid,
                                MaxStack = 1,
                                TargetID = target,
                                TickTime = 1,
                            };
                            _game.DamageOverTimeComponent.Add(eid, DoT);

                            time = new TimedEffect()
                            {
                                EntityID = eid,
                                TimeLeft = 16,
                                TotalDuration = 16,
                            };
                            _game.TimedEffectComponent.Add(eid, time);
                            break;
                        #endregion

                        #region Rank 7
                        case 7:
                            DoT = new DamageOverTime()
                            {
                                AmountPerTick = 4,
                                CurrentStack = 1,
                                CurrentTime = 0,
                                EntityID = eid,
                                MaxStack = 1,
                                TargetID = target,
                                TickTime = 1,
                            };
                            _game.DamageOverTimeComponent.Add(eid, DoT);

                            time = new TimedEffect()
                            {
                                EntityID = eid,
                                TimeLeft = 16,
                                TotalDuration = 16,
                            };
                            _game.TimedEffectComponent.Add(eid, time);
                            break;
                        #endregion

                        #region Rank 8
                        case 8:
                            DoT = new DamageOverTime()
                            {
                                AmountPerTick = 4,
                                CurrentStack = 1,
                                CurrentTime = 0,
                                EntityID = eid,
                                MaxStack = 1,
                                TargetID = target,
                                TickTime = 1,
                            };
                            _game.DamageOverTimeComponent.Add(eid, DoT);

                            time = new TimedEffect()
                            {
                                EntityID = eid,
                                TimeLeft = 18,
                                TotalDuration = 18,
                            };
                            _game.TimedEffectComponent.Add(eid, time);
                            break;
                        #endregion

                        #region Rank 9
                        case 9:
                            DoT = new DamageOverTime()
                            {
                                AmountPerTick = 5,
                                CurrentStack = 1,
                                CurrentTime = 0,
                                EntityID = eid,
                                MaxStack = 1,
                                TargetID = target,
                                TickTime = 1,
                            };
                            _game.DamageOverTimeComponent.Add(eid, DoT);

                            time = new TimedEffect()
                            {
                                EntityID = eid,
                                TimeLeft = 18,
                                TotalDuration = 18,
                            };
                            _game.TimedEffectComponent.Add(eid, time);
                            break;
                        #endregion

                        #region Rank 10
                        case 10:
                            DoT = new DamageOverTime()
                            {
                                AmountPerTick = 7,
                                CurrentStack = 1,
                                CurrentTime = 0,
                                EntityID = eid,
                                MaxStack = 1,
                                TargetID = target,
                                TickTime = 1,
                            };
                            _game.DamageOverTimeComponent.Add(eid, DoT);

                            time = new TimedEffect()
                            {
                                EntityID = eid,
                                TimeLeft = 20,
                                TotalDuration = 20,
                            };
                            _game.TimedEffectComponent.Add(eid, time);
                            break;
                        #endregion
                        default:
                            throw new Exception("Unimplemented Rank");
                    }
                    break;
                #endregion

                #region Mindless Parasite
                case SkillType.MindlessParasites:
                    int mod;
                    eid = Entity.NextEntity();
                    if (friendly)
                        mod = 1;
                    else
                        mod = -1;

                    switch (rank)
                    {
                        #region Rank 1
                        case 1:
                            time = new TimedEffect()
                            {
                                EntityID = eid,
                                TimeLeft = 10,
                                TotalDuration = 10,
                            };
                            _game.TimedEffectComponent.Add(eid, time);
                            buff = new Buff()
                            {
                                EntityID = eid,
                                MovementSpeed = mod * ((int)(_game.MovementComponent[target].Speed * .1)),
                                DefenseMelee = mod * 1,
                                DefenseRanged = mod * 1,
                                AttackMelee = 0,
                                AttackRanged = 0,
                                AttackSpeed = 0,
                                isPercentAttackMelee = false,
                                isPercentAttackRanged = false,
                                isPercentAttackSpeed = false,
                                isPercentDefenseMelee = false,
                                isPercentDefenseRanged = false,
                                isPercentFatigue = false,
                                isPercentHealth = false,
                                isPercentMovementSpeed = true,
                                Fatigue = 0,
                                Health = 0,
                                isPercentPsi = false,
                                isPercentResistPoison = false,
                                isPercentWeaponAccuracy = false,
                                isPercentWeaponSpeed = false,
                                isPercentWeaponStrength = false,
                                Psi = 0,
                                ResistPoison = 0,
                                TargetID = target,
                                WeaponAccuracy = 0,
                                WeaponSpeed = 0,
                                WeaponStrength = 0,
                            };
                            _game.BuffComponent.Add(eid, buff);

                            break;
                        #endregion

                        #region Rank 2
                        case 2:
                            time = new TimedEffect()
                            {
                                EntityID = eid,
                                TimeLeft = 12,
                                TotalDuration = 12,
                            };
                            _game.TimedEffectComponent.Add(eid, time);
                            buff = new Buff()
                            {
                                EntityID = eid,
                                MovementSpeed = mod * ((int)(_game.MovementComponent[target].Speed * .1)),
                                DefenseMelee = mod * 1,
                                DefenseRanged = mod * 1,
                                AttackMelee = 0,
                                AttackRanged = 0,
                                AttackSpeed = 0,
                                isPercentAttackMelee = false,
                                isPercentAttackRanged = false,
                                isPercentAttackSpeed = false,
                                isPercentDefenseMelee = false,
                                isPercentDefenseRanged = false,
                                isPercentFatigue = false,
                                isPercentHealth = false,
                                isPercentMovementSpeed = true,
                                Fatigue = 0,
                                Health = 0,
                                isPercentPsi = false,
                                isPercentResistPoison = false,
                                isPercentWeaponAccuracy = false,
                                isPercentWeaponSpeed = false,
                                isPercentWeaponStrength = false,
                                Psi = 0,
                                ResistPoison = 0,
                                TargetID = target,
                                WeaponAccuracy = 0,
                                WeaponSpeed = 0,
                                WeaponStrength = 0,
                            };
                            _game.BuffComponent.Add(eid, buff);
                            break;
                        #endregion

                        #region Rank 3
                        case 3:
                            time = new TimedEffect()
                            {
                                EntityID = eid,
                                TimeLeft = 12,
                                TotalDuration = 12,
                            };
                            _game.TimedEffectComponent.Add(eid, time);
                            buff = new Buff()
                            {
                                EntityID = eid,
                                MovementSpeed = mod * ((int)(_game.MovementComponent[target].Speed * .2)),
                                DefenseMelee = mod * 2,
                                DefenseRanged = mod * 2,
                                AttackMelee = 0,
                                AttackRanged = 0,
                                AttackSpeed = 0,
                                isPercentAttackMelee = false,
                                isPercentAttackRanged = false,
                                isPercentAttackSpeed = false,
                                isPercentDefenseMelee = false,
                                isPercentDefenseRanged = false,
                                isPercentFatigue = false,
                                isPercentHealth = false,
                                isPercentMovementSpeed = true,
                                Fatigue = 0,
                                Health = 0,
                                isPercentPsi = false,
                                isPercentResistPoison = false,
                                isPercentWeaponAccuracy = false,
                                isPercentWeaponSpeed = false,
                                isPercentWeaponStrength = false,
                                Psi = 0,
                                ResistPoison = 0,
                                TargetID = target,
                                WeaponAccuracy = 0,
                                WeaponSpeed = 0,
                                WeaponStrength = 0,
                            };
                            _game.BuffComponent.Add(eid, buff);
                            break;

                        #endregion

                        #region Rank 4
                        case 4:
                            time = new TimedEffect()
                            {
                                EntityID = eid,
                                TimeLeft = 14,
                                TotalDuration = 14,
                            };
                            _game.TimedEffectComponent.Add(eid, time);
                            buff = new Buff()
                            {
                                EntityID = eid,
                                MovementSpeed = mod * ((int)(_game.MovementComponent[target].Speed * .2)),
                                DefenseMelee = mod * 2,
                                DefenseRanged = mod * 2,
                                AttackMelee = 0,
                                AttackRanged = 0,
                                AttackSpeed = 0,
                                isPercentAttackMelee = false,
                                isPercentAttackRanged = false,
                                isPercentAttackSpeed = false,
                                isPercentDefenseMelee = false,
                                isPercentDefenseRanged = false,
                                isPercentFatigue = false,
                                isPercentHealth = false,
                                isPercentMovementSpeed = true,
                                Fatigue = 0,
                                Health = 0,
                                isPercentPsi = false,
                                isPercentResistPoison = false,
                                isPercentWeaponAccuracy = false,
                                isPercentWeaponSpeed = false,
                                isPercentWeaponStrength = false,
                                Psi = 0,
                                ResistPoison = 0,
                                TargetID = target,
                                WeaponAccuracy = 0,
                                WeaponSpeed = 0,
                                WeaponStrength = 0,
                            };
                            _game.BuffComponent.Add(eid, buff);
                            break;
                        #endregion

                        #region Rank 5
                        case 5:
                            time = new TimedEffect()
                            {
                                EntityID = eid,
                                TimeLeft = 14,
                                TotalDuration = 14,
                            };
                            _game.TimedEffectComponent.Add(eid, time);
                            buff = new Buff()
                            {
                                EntityID = eid,
                                MovementSpeed = mod * ((int)(_game.MovementComponent[target].Speed * .3)),
                                DefenseMelee = mod * 3,
                                DefenseRanged = mod * 3,
                                AttackMelee = 0,
                                AttackRanged = 0,
                                AttackSpeed = 0,
                                isPercentAttackMelee = false,
                                isPercentAttackRanged = false,
                                isPercentAttackSpeed = false,
                                isPercentDefenseMelee = false,
                                isPercentDefenseRanged = false,
                                isPercentFatigue = false,
                                isPercentHealth = false,
                                isPercentMovementSpeed = true,
                                Fatigue = 0,
                                Health = 0,
                                isPercentPsi = false,
                                isPercentResistPoison = false,
                                isPercentWeaponAccuracy = false,
                                isPercentWeaponSpeed = false,
                                isPercentWeaponStrength = false,
                                Psi = 0,
                                ResistPoison = 0,
                                TargetID = target,
                                WeaponAccuracy = 0,
                                WeaponSpeed = 0,
                                WeaponStrength = 0,
                            };
                            _game.BuffComponent.Add(eid, buff);
                            break;
                        #endregion

                        #region Rank 6
                        case 6:
                            time = new TimedEffect()
                            {
                                EntityID = eid,
                                TimeLeft = 16,
                                TotalDuration = 16,
                            };
                            _game.TimedEffectComponent.Add(eid, time);
                            buff = new Buff()
                            {
                                EntityID = eid,
                                MovementSpeed = mod * ((int)(_game.MovementComponent[target].Speed * .3)),
                                DefenseMelee = mod * 3,
                                DefenseRanged = mod * 3,
                                AttackMelee = 0,
                                AttackRanged = 0,
                                AttackSpeed = 0,
                                isPercentAttackMelee = false,
                                isPercentAttackRanged = false,
                                isPercentAttackSpeed = false,
                                isPercentDefenseMelee = false,
                                isPercentDefenseRanged = false,
                                isPercentFatigue = false,
                                isPercentHealth = false,
                                isPercentMovementSpeed = true,
                                Fatigue = 0,
                                Health = 0,
                                isPercentPsi = false,
                                isPercentResistPoison = false,
                                isPercentWeaponAccuracy = false,
                                isPercentWeaponSpeed = false,
                                isPercentWeaponStrength = false,
                                Psi = 0,
                                ResistPoison = 0,
                                TargetID = target,
                                WeaponAccuracy = 0,
                                WeaponSpeed = 0,
                                WeaponStrength = 0,
                            };
                            _game.BuffComponent.Add(eid, buff);
                            break;
                        #endregion

                        #region Rank 7
                        case 7:
                            time = new TimedEffect()
                            {
                                EntityID = eid,
                                TimeLeft = 16,
                                TotalDuration = 16,
                            };
                            _game.TimedEffectComponent.Add(eid, time);
                            buff = new Buff()
                            {
                                EntityID = eid,
                                MovementSpeed = mod * ((int)(_game.MovementComponent[target].Speed * .4)),
                                DefenseMelee = mod * 4,
                                DefenseRanged = mod * 4,
                                AttackMelee = 0,
                                AttackRanged = 0,
                                AttackSpeed = 0,
                                isPercentAttackMelee = false,
                                isPercentAttackRanged = false,
                                isPercentAttackSpeed = false,
                                isPercentDefenseMelee = false,
                                isPercentDefenseRanged = false,
                                isPercentFatigue = false,
                                isPercentHealth = false,
                                isPercentMovementSpeed = true,
                                Fatigue = 0,
                                Health = 0,
                                isPercentPsi = false,
                                isPercentResistPoison = false,
                                isPercentWeaponAccuracy = false,
                                isPercentWeaponSpeed = false,
                                isPercentWeaponStrength = false,
                                Psi = 0,
                                ResistPoison = 0,
                                TargetID = target,
                                WeaponAccuracy = 0,
                                WeaponSpeed = 0,
                                WeaponStrength = 0,
                            };
                            _game.BuffComponent.Add(eid, buff);
                            break;
                        #endregion

                        #region Rank 8
                        case 8:
                            time = new TimedEffect()
                            {
                                EntityID = eid,
                                TimeLeft = 18,
                                TotalDuration = 18,
                            };
                            _game.TimedEffectComponent.Add(eid, time);
                            buff = new Buff()
                            {
                                EntityID = eid,
                                MovementSpeed = mod * ((int)(_game.MovementComponent[target].Speed * .4)),
                                DefenseMelee = mod * 4,
                                DefenseRanged = mod * 4,
                                AttackMelee = 0,
                                AttackRanged = 0,
                                AttackSpeed = 0,
                                isPercentAttackMelee = false,
                                isPercentAttackRanged = false,
                                isPercentAttackSpeed = false,
                                isPercentDefenseMelee = false,
                                isPercentDefenseRanged = false,
                                isPercentFatigue = false,
                                isPercentHealth = false,
                                isPercentMovementSpeed = true,
                                Fatigue = 0,
                                Health = 0,
                                isPercentPsi = false,
                                isPercentResistPoison = false,
                                isPercentWeaponAccuracy = false,
                                isPercentWeaponSpeed = false,
                                isPercentWeaponStrength = false,
                                Psi = 0,
                                ResistPoison = 0,
                                TargetID = target,
                                WeaponAccuracy = 0,
                                WeaponSpeed = 0,
                                WeaponStrength = 0,
                            };
                            _game.BuffComponent.Add(eid, buff);
                            break;
                        #endregion

                        #region Rank 9
                        case 9:
                            time = new TimedEffect()
                            {
                                EntityID = eid,
                                TimeLeft = 18,
                                TotalDuration = 18,
                            };
                            _game.TimedEffectComponent.Add(eid, time);
                            buff = new Buff()
                            {
                                EntityID = eid,
                                MovementSpeed = mod * ((int)(_game.MovementComponent[target].Speed * .5)),
                                DefenseMelee = mod * 5,
                                DefenseRanged = mod * 5,
                                AttackMelee = 0,
                                AttackRanged = 0,
                                AttackSpeed = 0,
                                isPercentAttackMelee = false,
                                isPercentAttackRanged = false,
                                isPercentAttackSpeed = false,
                                isPercentDefenseMelee = false,
                                isPercentDefenseRanged = false,
                                isPercentFatigue = false,
                                isPercentHealth = false,
                                isPercentMovementSpeed = true,
                                Fatigue = 0,
                                Health = 0,
                                isPercentPsi = false,
                                isPercentResistPoison = false,
                                isPercentWeaponAccuracy = false,
                                isPercentWeaponSpeed = false,
                                isPercentWeaponStrength = false,
                                Psi = 0,
                                ResistPoison = 0,
                                TargetID = target,
                                WeaponAccuracy = 0,
                                WeaponSpeed = 0,
                                WeaponStrength = 0,
                            };
                            _game.BuffComponent.Add(eid, buff);
                            break;
                        #endregion

                        #region Rank 10
                        case 10:
                            time = new TimedEffect()
                            {
                                EntityID = eid,
                                TimeLeft = 20,
                                TotalDuration = 20,
                            };
                            _game.TimedEffectComponent.Add(eid, time);
                            buff = new Buff()
                            {
                                EntityID = eid,
                                MovementSpeed = mod * ((int)(_game.MovementComponent[target].Speed * .7)),
                                DefenseMelee = mod * 7,
                                DefenseRanged = mod * 7,
                                AttackMelee = 0,
                                AttackRanged = 0,
                                AttackSpeed = 0,
                                isPercentAttackMelee = false,
                                isPercentAttackRanged = false,
                                isPercentAttackSpeed = false,

                                isPercentDefenseMelee = false,
                                isPercentDefenseRanged = false,
                                isPercentFatigue = false,
                                isPercentHealth = false,
                                isPercentMovementSpeed = true,
                                Fatigue = 0,
                                Health = 0,
                                isPercentPsi = false,
                                isPercentResistPoison = false,
                                isPercentWeaponAccuracy = false,
                                isPercentWeaponSpeed = false,
                                isPercentWeaponStrength = false,
                                Psi = 0,
                                ResistPoison = 0,
                                TargetID = target,
                                WeaponAccuracy = 0,
                                WeaponSpeed = 0,
                                WeaponStrength = 0,
                            };
                            _game.BuffComponent.Add(eid, buff);
                            break;
                        #endregion
                        default:
                            throw new Exception("Unimplemented Rank");
                    }
                    break;
                #endregion

                #endregion

                #region Cultist Triggered Skills

                #region Psionic Spear
                case SkillType.PsionicSpear:
                    {
                        #region Skill Variables

                        InstantEffect instant_Effect;
                        DirectDamage direct_damage;

                        Random damageMod = new Random();
                        int damageValue = 0;

                        #endregion

                        #region Skill Logic

                        switch (rank)
                        {
                            #region Rank 1
                            case 1:
                                {
                                    damageValue = damageMod.Next(10) + 1; //Damage from 1 to 10
                                    break;
                                }
                            #endregion

                            #region Rank 2
                            case 2:
                                {
                                    damageValue = damageMod.Next(20) + 1; //Damage from 1 to 20
                                    break;
                                }
                            #endregion

                            #region Rank 3
                            case 3:
                                {
                                    damageValue = damageMod.Next(35) + 1; //Damage from 1 to 35
                                    break;
                                }
                            #endregion

                            #region Rank 4
                            case 4:
                                {
                                    damageValue = damageMod.Next(50) + 1; //Damage from 1 to 50
                                    break;
                                }
                            #endregion

                            #region Rank 5
                            case 5:
                                {
                                    damageValue = damageMod.Next(75) + 1; //Damage from 1 to 75
                                    break;
                                }
                            #endregion

                            #region Rank 6
                            case 6:
                                {
                                    damageValue = damageMod.Next(100) + 1; //Damage from 1 to 100
                                    break;
                                }
                            #endregion

                            #region Rank 7
                            case 7:
                                {
                                    damageValue = damageMod.Next(150) + 1; //Damage from 1 to 150
                                    break;
                                }
                            #endregion

                            #region Rank 8
                            case 8:
                                {
                                    damageValue = damageMod.Next(200) + 1; //Damage from 1 to 200
                                    break;
                                }
                            #endregion

                            #region Rank 9
                            case 9:
                                {
                                    damageValue = damageMod.Next(300) + 1; //Damage from 1 to 300
                                    break;
                                }
                            #endregion

                            #region Rank 10
                            case 10:
                                {
                                    damageValue = damageMod.Next(400) + 1; //Damage from 1 to 400
                                    break;
                                }

                            #endregion

                            default:
                                {
                                    throw new Exception("Unimplemented Rank");
                                }
                        }

                        eid = Entity.NextEntity();

                        instant_Effect = new InstantEffect()
                        {
                            EntityID = eid,
                            isTriggered = false,
                        };
                        _game.InstantEffectComponent.Add(eid, instant_Effect);

                        direct_damage = new DirectDamage()
                        {
                            EntityID = eid,
                            TargetID = target,
                            Damage = damageValue,
                        };
                        _game.DirectDamageComponent.Add(eid, direct_damage);

                        #endregion

                        break;
                    }
                #endregion

                #region Enslave
                case SkillType.Enslave:
                    {
                        #region Skill Variables

                        TimedEffect timedEffect;
                        Enslave enslaveEffect;

                        uint cts_ID = 0;
                        ChanceToSucceed chanceToSucceed;

                        int percentChance = 0;
                        int maxSlaveNum = 0;
                        float duration = 0;

                        List<uint> chanceToSucceedList = new List<uint>();

                        #endregion

                        #region Skill Logic

                        switch (rank)
                        {
                            #region Rank 1
                            case 1:
                                {
                                    percentChance = 60; //60% chance to succeed
                                    maxSlaveNum = 1; //Can only Enslave 1 enemy
                                    duration = 3; //For 3 seconds
                                    break;
                                }
                            #endregion

                            #region Rank 2
                            case 2:
                                {
                                    percentChance = 60; //60% chance to succeed
                                    maxSlaveNum = 1; //Can only Enslave 1 enemy
                                    duration = 5; //For 5 seconds
                                    break;
                                }
                            #endregion

                            #region Rank 3
                            case 3:
                                {
                                    percentChance = 65; //65% chance to succeed
                                    maxSlaveNum = 1; //Can only Enslave 1 enemy
                                    duration = 5; //For 5 seconds
                                    break;
                                }
                            #endregion

                            #region Rank 4
                            case 4:
                                {
                                    percentChance = 70; //70% chance to succeed
                                    maxSlaveNum = 1; //Can only Enslave 1 enemy
                                    duration = 8; //For 8 seconds
                                    break;
                                }
                            #endregion

                            #region Rank 5
                            case 5:
                                {
                                    percentChance = 70; //70% chance to succeed
                                    maxSlaveNum = 2; //Can only Enslave 2 enemies
                                    duration = 8; //For 8 seconds
                                    break;
                                }
                            #endregion

                            #region Rank 6
                            case 6:
                                {
                                    percentChance = 75; //75% chance to succeed
                                    maxSlaveNum = 2; //Can only Enslave 2 enemies
                                    duration = 8; //For 8 seconds
                                    break;
                                }
                            #endregion

                            #region Rank 7
                            case 7:
                                {
                                    percentChance = 75; //75% chance to succeed
                                    maxSlaveNum = 2; //Can only Enslave 2 enemies
                                    duration = 10; //For 10 seconds
                                    break;
                                }
                            #endregion

                            #region Rank 8
                            case 8:
                                {
                                    percentChance = 80; //80% chance to succeed
                                    maxSlaveNum = 2; //Can only Enslave 2 enemies
                                    duration = 10; //For 10 seconds
                                    break;
                                }
                            #endregion

                            #region Rank 9
                            case 9:
                                {
                                    percentChance = 85; //85% chance to succeed
                                    maxSlaveNum = 2; //Can only Enslave 2 enemies
                                    duration = 12; //For 12 seconds
                                    break;
                                }
                            #endregion

                            #region Rank 10
                            case 10:
                                {
                                    percentChance = 85; //85% chance to succeed
                                    maxSlaveNum = 3; //Can only Enslave 3 enemies
                                    duration = 15; //For 15 seconds
                                    break;
                                }
                            #endregion

                            default:
                                {
                                    break;
                                }
                        }

                        eid = Entity.NextEntity();
                        timedEffect = new TimedEffect()
                        {
                            EntityID = eid,
                            TotalDuration = duration,
                            TimeLeft = duration,
                        };
                        _game.TimedEffectComponent.Add(eid, timedEffect);

                        enslaveEffect = new Enslave()
                        {
                            EntityID = eid,
                            OwnerID = owner,
                            TargetID = target,
                        };
                        _game.EnslaveComponent.Add(eid, enslaveEffect);

                        chanceToSucceedList.Add(eid);

                        cts_ID = Entity.NextEntity();
                        chanceToSucceed = new ChanceToSucceed()
                        {
                            EntityID = cts_ID,
                            SuccessRateAsPercentage = percentChance,
                            AffectedIDList = chanceToSucceedList,
                        };
                        _game.ChanceToSucceedComponent.Add(cts_ID, chanceToSucceed);

                        #endregion

                        break;
                    }
                #endregion

                #region Fear
                case SkillType.Fear:
                    {
                        #region Skill Variables

                        TimedEffect timedEffect;
                        Fear fear;

                        uint cts_id = 0;
                        ChanceToSucceed chanceToSucceed;
                        List<uint> chanceToSucceedList = new List<uint>();

                        int percentChance = 0;
                        float duration = 0;

                        #endregion

                        #region Skill Logic

                        switch (rank)
                        {
                            #region Rank 1
                            case 1:
                                {
                                    percentChance = 60; //60% chance to succeed
                                    duration = 2; //lasts 2 seconds
                                    break;
                                }
                            #endregion

                            #region Rank 2
                            case 2:
                                {
                                    percentChance = 65; //65% chance to succeed
                                    duration = 2; //lasts 2 seconds
                                    break;
                                }
                            #endregion

                            #region Rank 3
                            case 3:
                                {
                                    percentChance = 70; //70% chance to succeed
                                    duration = 2; //lasts 2 seconds
                                    break;
                                }
                            #endregion

                            #region Rank 4
                            case 4:
                                {
                                    percentChance = 70; //70% chance to succeed
                                    duration = 3; //lasts 3 seconds
                                    break;
                                }
                            #endregion

                            #region Rank 5
                            case 5:
                                {
                                    percentChance = 70; //70% chance to succeed
                                    duration = 5; //lasts 5 seconds
                                    break;
                                }
                            #endregion

                            #region Rank 6
                            case 6:
                                {
                                    percentChance = 75; //75% chance to succeed
                                    duration = 6; //lasts 6 seconds
                                    break;
                                }
                            #endregion

                            #region Rank 7
                            case 7:
                                {
                                    percentChance = 75; //75% chance to succeed
                                    duration = 8; //lasts 8 seconds
                                    break;
                                }
                            #endregion

                            #region Rank 8
                            case 8:
                                {
                                    percentChance = 75; //75% chance to succeed
                                    duration = 9; //lasts 9 seconds
                                    break;
                                }
                            #endregion

                            #region Rank 9
                            case 9:
                                {
                                    percentChance = 80; //80% chance to succeed
                                    duration = 10; //lasts 10 seconds
                                    break;
                                }
                            #endregion

                            #region Rank 10
                            case 10:
                                {
                                    percentChance = 85; //85% chance to succeed
                                    duration = 12; //lasts 12 seconds
                                    break;
                                }
                            #endregion

                            default:
                                {
                                    break;
                                }
                        }

                        eid = Entity.NextEntity();

                        timedEffect = new TimedEffect()
                        {
                            EntityID = eid,
                            TotalDuration = duration,
                            TimeLeft = duration,
                        };
                        _game.TimedEffectComponent.Add(eid, timedEffect);

                        fear = new Fear()
                        {
                            EntityID = eid,
                            SourceID = owner,
                            TargetID = target,
                        };
                        _game.FearComponent.Add(eid, fear);

                        chanceToSucceedList.Add(eid);

                        cts_id = Entity.NextEntity();

                        chanceToSucceed = new ChanceToSucceed()
                        {
                            EntityID = cts_id,
                            AffectedIDList = chanceToSucceedList,
                            SuccessRateAsPercentage = percentChance,
                        };
                        _game.ChanceToSucceedComponent.Add(cts_id, chanceToSucceed);

                        #endregion

                        break;
                    }
                #endregion

                #region Taint
                case SkillType.Taint:
                    {
                        #region Skill Variables

                        TimedEffect timedEffect;
                        DamageOverTime damageOverTime;

                        float duration = 0;
                        int dps = 0;

                        #endregion

                        #region Skill Logic
                        switch (rank)
                        {
                            #region Rank 1
                            case 1:
                                {
                                    dps = 2; // 2 damage per second
                                    duration = 5; // for 5 seconds
                                    break;
                                }
                            #endregion

                            #region Rank 2
                            case 2:
                                {
                                    dps = 3; // 3 damage per second
                                    duration = 5; // for 5 seconds
                                    break;
                                }
                            #endregion

                            #region Rank 3
                            case 3:
                                {
                                    dps = 5; // 5 damage per second
                                    duration = 5; // for 5 seconds
                                    break;
                                }
                            #endregion

                            #region Rank 4
                            case 4:
                                {
                                    dps = 6; // 6 damage per second
                                    duration = 6; // for 6 seconds
                                    break;
                                }
                            #endregion

                            #region Rank 5
                            case 5:
                                {
                                    dps = 8; // 8 damage per second
                                    duration = 7; // for 7 seconds
                                    break;
                                }
                            #endregion

                            #region Rank 6
                            case 6:
                                {
                                    dps = 12; // 12 damage per second
                                    duration = 8; // for 8 seconds
                                    break;
                                }
                            #endregion

                            #region Rank 7
                            case 7:
                                {
                                    dps = 15; // 15 damage per second
                                    duration = 9; // for 9 seconds
                                    break;
                                }
                            #endregion

                            #region Rank 8
                            case 8:
                                {
                                    dps = 20; // 20 damage per second
                                    duration = 10; // for 10 seconds
                                    break;
                                }
                            #endregion

                            #region Rank 9
                            case 9:
                                {
                                    dps = 30; // 30 damage per second
                                    duration = 12; // for 12 seconds
                                    break;
                                }
                            #endregion

                            #region Rank 10
                            case 10:
                                {
                                    dps = 45; // 45 damage per second
                                    duration = 15; // for 15 seconds
                                    break;
                                }
                            #endregion

                            default:
                                {
                                    break;
                                }
                        }

                        eid = Entity.NextEntity();

                        timedEffect = new TimedEffect()
                        {
                            EntityID = eid,
                            TotalDuration = duration,
                            TimeLeft = duration,
                        };
                        _game.TimedEffectComponent.Add(eid, timedEffect);

                        damageOverTime = new DamageOverTime()
                        {
                            EntityID = eid,
                            MaxStack = 1,
                            CurrentStack = 1,
                            AmountPerTick = dps,
                            CurrentTime = 0,
                            TickTime = 1,
                            TargetID = target,
                        };
                        _game.DamageOverTimeComponent.Add(eid, damageOverTime);

                        #endregion

                        break;
                    }
                #endregion

                #region Rot
                case SkillType.Rot:
                    {
                        #region Skill Variables

                        TimedEffect timedEffect;
                        Buff buffEffect;

                        bool targetTainted = false;

                        int dps = 0;
                        float duration = 0;
                        int staminaDebuff = 0;

                        #endregion

                        #region Skill Logic

                        switch (rank)
                        {
                            #region Rank 1
                            case 1:
                                {
                                    dps = 2; // 2 damage per second
                                    duration = 5; // over 5 seconds
                                    staminaDebuff = -1; // and -1 to Stamina
                                    break;
                                }
                            #endregion

                            #region Rank 2
                            case 2:
                                {
                                    dps = 3; // 3 damage per second
                                    duration = 5; // over 5 seconds
                                    staminaDebuff = -1; // and -1 to Stamina
                                    break;
                                }
                            #endregion

                            #region Rank 3
                            case 3:
                                {
                                    dps = 3; // 3 damage per second
                                    duration = 6; // over 6 seconds
                                    staminaDebuff = -1; // and -1 to Stamina
                                    break;
                                }
                            #endregion

                            #region Rank 4
                            case 4:
                                {
                                    dps = 5; // 5 damage per second
                                    duration = 6; // over 6 seconds
                                    staminaDebuff = -1; // and -1 to Stamina
                                    break;
                                }
                            #endregion

                            #region Rank 5
                            case 5:
                                {
                                    dps = 5; // 5 damage per second
                                    duration = 7; // over 7 seconds
                                    staminaDebuff = -2; // and -2 to Stamina
                                    break;
                                }
                            #endregion

                            #region Rank 6
                            case 6:
                                {
                                    dps = 8; // 8 damage per second
                                    duration = 7; // over 7 seconds
                                    staminaDebuff = -2; // and -2 to Stamina
                                    break;
                                }
                            #endregion

                            #region Rank 7
                            case 7:
                                {
                                    dps = 10; // 10 damage per second
                                    duration = 9; // over 9 seconds
                                    staminaDebuff = -3; // and -3 to Stamina
                                    break;
                                }
                            #endregion

                            #region Rank 8
                            case 8:
                                {
                                    dps = 12; // 12 damage per second
                                    duration = 11; // over 11 seconds
                                    staminaDebuff = -3; // and -3 to Stamina
                                    break;
                                }
                            #endregion

                            #region Rank 9
                            case 9:
                                {
                                    dps = 15; // 15 damage per second
                                    duration = 11; // over 11 seconds
                                    staminaDebuff = -3; // and -3 to Stamina
                                    break;
                                }
                            #endregion

                            #region Rank 10
                            case 10:
                                {
                                    dps = 17; // 17 damage per second
                                    duration = 13; // over 13 seconds
                                    staminaDebuff = -4; // and -4 to Stamina
                                    break;
                                }
                            #endregion

                            default:
                                {
                                    break;
                                }
                        }

                        //TODO: check if the target has been tainted
                        targetTainted = true;

                        if (!targetTainted) return;

                        eid = Entity.NextEntity();

                        timedEffect = new TimedEffect()
                        {
                            EntityID = eid,
                            TotalDuration = duration,
                            TimeLeft = duration,
                        };
                        _game.TimedEffectComponent.Add(eid, timedEffect);

                        buffEffect = new Buff()
                        {
                            EntityID = eid,
                            Stamina = staminaDebuff,
                        };
                        _game.BuffComponent.Add(eid, buffEffect);

                        #endregion

                        break;
                    }
                #endregion

                #region Push
                case SkillType.Push:
                    {
                        #region Skill Variables

                        InstantEffect instant_Effect;
                        KnockBack knockBack;
                        DirectDamage direct_damage;

                        Random damageMod = new Random();

                        int meterConversion = 75;
                        int pushDistance = 0;
                        int damageValue = 0;

                        #endregion

                        #region Skill Logic

                        switch (rank)
                        {
                            #region Rank 1
                            case 1:
                                {
                                    pushDistance = 1; // 1 meter
                                    damageValue = 1 + damageMod.Next(2); // Damage between 1 and 2
                                    break;
                                }
                            #endregion

                            #region Rank 2
                            case 2:
                                {
                                    pushDistance = 1; // 1 meter
                                    damageValue = 2 + damageMod.Next(4); // Damage between 2 and 5
                                    break;
                                }
                            #endregion

                            #region Rank 3
                            case 3:
                                {
                                    pushDistance = 2; // 2 meters
                                    damageValue = 2 + damageMod.Next(4); // Damage between 2 and 5
                                    break;
                                }
                            #endregion

                            #region Rank 4
                            case 4:
                                {
                                    pushDistance = 2; // 2 meters
                                    damageValue = 5 + damageMod.Next(6); // Damage between 5 and 10
                                    break;
                                }
                            #endregion

                            #region Rank 5
                            case 5:
                                {
                                    pushDistance = 2; // 2 meters
                                    damageValue = 10 + damageMod.Next(6); // Damage between 10 and 15
                                    break;
                                }
                            #endregion

                            #region Rank 6
                            case 6:
                                {
                                    pushDistance = 3; // 3 meters
                                    damageValue = 10 + damageMod.Next(6); // Damage between 10 and 15
                                    break;
                                }
                            #endregion

                            #region Rank 7
                            case 7:
                                {
                                    pushDistance = 3; // 3 meters
                                    damageValue = 15 + damageMod.Next(6); // Damage between 15 and 20
                                    break;
                                }
                            #endregion

                            #region Rank 8
                            case 8:
                                {
                                    pushDistance = 3; // 3 meters
                                    damageValue = 20 + damageMod.Next(6); // Damage between 20 and 25
                                    break;
                                }
                            #endregion

                            #region Rank 9
                            case 9:
                                {
                                    pushDistance = 4; // 4 meters
                                    damageValue = 20 + damageMod.Next(6); // Damage between 20 and 25
                                    break;
                                }
                            #endregion

                            #region Rank 10
                            case 10:
                                {
                                    pushDistance = 5; // 5 meters
                                    damageValue = 25 + damageMod.Next(6); // Damage between 25 and 30
                                    break;
                                }
                            #endregion

                            default:
                                {
                                    break;
                                }
                        }

                        eid = Entity.NextEntity();

                        instant_Effect = new InstantEffect()
                        {
                            EntityID = eid,
                            isTriggered = false,
                        };
                        _game.InstantEffectComponent.Add(eid, instant_Effect);

                        knockBack = new KnockBack()
                        {
                            EntityID = eid,
                            Distance = pushDistance * meterConversion,
                            Origin = _game.PositionComponent[owner].Center,
                            TargetID = target,
                        };
                        _game.KnockBackComponent.Add(eid, knockBack);

                        direct_damage = new DirectDamage()
                        {
                            EntityID = eid,
                            TargetID = target,
                            Damage = damageValue,
                        };
                        _game.DirectDamageComponent.Add(eid, direct_damage);

                        #endregion

                        break;
                    }
                #endregion

                #region Malice
                case SkillType.Malice:
                    {
                        #region Skill Variables

                        InstantEffect instant_Effect;
                        DirectDamage direct_Damage;

                        Random damageMod = new Random();
                        int damageValue = 0;

                        #endregion

                        #region Skill Logic

                        switch (rank)
                        {
                            #region Rank 1
                            case 1:
                                {
                                    damageValue = 20 + damageMod.Next(6); //Damage between 20 and 25
                                    break;
                                }
                            #endregion

                            #region Rank 2
                            case 2:
                                {
                                    damageValue = 20 + damageMod.Next(11); //Damage between 20 and 30
                                    break;
                                }
                            #endregion

                            #region Rank 3
                            case 3:
                                {
                                    damageValue = 25 + damageMod.Next(11); //Damage between 25 and 25
                                    break;
                                }
                            #endregion

                            #region Rank 4
                            case 4:
                                {
                                    damageValue = 40 + damageMod.Next(6); //Damage between 40 and 45
                                    break;
                                }
                            #endregion

                            #region Rank 5
                            case 5:
                                {
                                    damageValue = 50 + damageMod.Next(11); //Damage between 50 and 60
                                    break;
                                }
                            #endregion

                            #region Rank 6
                            case 6:
                                {
                                    damageValue = 65 + damageMod.Next(6); //Damage between 65 and 70
                                    break;
                                }
                            #endregion

                            #region Rank 7
                            case 7:
                                {
                                    damageValue = 70 + damageMod.Next(16); //Damage between 70 and 85
                                    break;
                                }
                            #endregion

                            #region Rank 8
                            case 8:
                                {
                                    damageValue = 90 + damageMod.Next(11); //Damage between 90 and 100
                                    break;
                                }
                            #endregion

                            #region Rank 9
                            case 9:
                                {
                                    damageValue = 105 + damageMod.Next(11); //Damage between 105 and 115
                                    break;
                                }
                            #endregion

                            #region Rank 10
                            case 10:
                                {
                                    damageValue = 120 + damageMod.Next(6); //Damage between 120 and 125
                                    break;
                                }
                            #endregion

                            default:
                                {
                                    break;
                                }
                        }

                        eid = Entity.NextEntity();

                        instant_Effect = new InstantEffect()
                        {
                            EntityID = eid,
                            isTriggered = false,
                        };
                        _game.InstantEffectComponent.Add(eid, instant_Effect);

                        direct_Damage = new DirectDamage()
                        {
                            EntityID = eid,
                            TargetID = target,
                            Damage = damageValue,
                        };
                        _game.DirectDamageComponent.Add(eid, direct_Damage);

                        #endregion

                        break;
                    }
                #endregion

                #endregion

                case SkillType.SniperShot:
                    instantEffect = new InstantEffect()
                    {
                        EntityID = eid,
                    };
                    _game.InstantEffectComponent.Add(eid, instantEffect);

                    directDamage = new DirectDamage()
                    {
                        TargetID = target,
                        Damage = 1,
                        EntityID = eid,
                    };
                    _game.DirectDamageComponent.Add(eid, directDamage);

                    break;

                case SkillType.BasicRangedAttack:
                    eid = Entity.NextEntity();

                    instantEffect = new InstantEffect()
                    {
                        EntityID = eid,
                    };
                    _game.InstantEffectComponent.Add(eid, instantEffect);

                    damage = new DirectDamage()
                    {
                        Damage = rank,
                        EntityID = eid,
                        TargetID = target,
                    };
                    _game.DirectDamageComponent.Add(eid, damage);
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

            #region Psi or Fatigue Regen

            if (_game.PsiOrFatigueRegenComponent.Contains(key))
            {
                PsiOrFatigueRegen regen = _game.PsiOrFatigueRegenComponent[key];

                regen.CurrentTime -= elapsedTime;

                if (regen.CurrentTime <= 0)
                {
                    /* in case this needs to be implemented later
                    if (_game.EnemyComponent.Contains(hOT.TargetID))
                    {
                        Enemy enemy = _game.EnemyComponent[hOT.TargetID];
                        enemy.Health += (hOT.AmountPerTick * hOT.CurrentStack);
                        _game.EnemyComponent[hOT.TargetID] = enemy;
                    }*/

                    if (_game.PlayerComponent.Contains(regen.TargetID))
                    {
                        PlayerInfo player = _game.PlayerInfoComponent[regen.TargetID];

                        player.PsiOrFatigue = Math.Min(player.PsiOrFatigue + regen.AmountPerTick, player.MaxPsiOrFatigue);
                        _game.PlayerInfoComponent[regen.TargetID] = player;
                        
                    }

                    regen.CurrentTime = regen.TickTime;
                }
                _game.PsiOrFatigueRegenComponent[key] = regen;
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

            #region Cloak

            if (_game.CloakComponent.Contains(key))
            {
                Cloak cloak = _game.CloakComponent[key];
                cloak.TimeLeft -= elapsedTime;
                _game.CloakComponent[cloak.EntityID] = cloak;

                if (!_game.PlayerComponent.Contains(cloak.TargetID) && !_game.EnemyComponent.Contains(cloak.TargetID))
                {
                    _game.GarbagemanSystem.ScheduleVisit(cloak.EntityID, GarbagemanSystem.ComponentType.Effect);
                    return;
                }

                if (cloak.StartingTime - cloak.TimeLeft < 0)
                    return;

                Sprite sprite = _game.SpriteComponent[cloak.TargetID];

                //if(cloak.spriteHeight == -1)
                  //  cloak.spriteHeight = sprite.SpriteBounds.Height;

                float timePassed = cloak.StartingTime - cloak.TimeLeft;

                if(cloak.TimeLeft < 1) //appear
                {
                    sprite.UseDifferentColor = false;
                    sprite.SpriteColor = Color.LightCyan;
                    //sprite.SpriteBounds.Height = cloak.spriteHeight;
                }
                else if (cloak.TimeLeft < 2) //fade in
                {
                    sprite.UseDifferentColor = true;
                    sprite.SpriteColor.R = sprite.SpriteColor.G = sprite.SpriteColor.B = sprite.SpriteColor.A = (byte)Math.Min(sprite.SpriteColor.A + 255 * elapsedTime, 255);
                }
                else if (timePassed > 2) //cloak
                {
                    //sprite.SpriteBounds.Height = 0;
                    sprite.UseDifferentColor = true;
                    sprite.SpriteColor = Color.Transparent;
                }
                else if (timePassed > 1) //fade out
                {
                    sprite.UseDifferentColor = true;
                    sprite.SpriteColor.A = (byte) Math.Max(sprite.SpriteColor.A - 255 * elapsedTime, 0);
                    sprite.SpriteColor.R = (byte)Math.Max(sprite.SpriteColor.R - 255 * elapsedTime, 0);
                    sprite.SpriteColor.G = (byte)Math.Max(sprite.SpriteColor.G - 255 * elapsedTime, 0);
                    sprite.SpriteColor.B = (byte)Math.Max(sprite.SpriteColor.B - 255 * elapsedTime, 0);
                    /*
                    if (sprite.SpriteBounds.Height == 0)
                        sprite.SpriteBounds.Height = cloak.spriteHeight;
                    else
                        sprite.SpriteBounds.Height = 0;
                     */
                }

                _game.CloakComponent[cloak.EntityID] = cloak;
                _game.SpriteComponent[sprite.EntityID] = sprite;
            }

            #endregion

        }

        /// <summary>
        /// Checks if you have enough psi or fatigue to use an ability.  If you have enough it drains the supplied amount
        /// and returns true, otherwise it just returns false
        /// </summary>
        /// <param name="userID">the player to drain the psi or fatigue from</param>
        /// <param name="psiOrFatigue">the amount to drain</param>
        /// <returns></returns>
        private bool DrainPsiOrFatigue(uint userID, int psiOrFatigue)
        {
            PlayerInfo info = _game.PlayerInfoComponent[userID];
            if (info.PsiOrFatigue >= psiOrFatigue)
            {
                info.PsiOrFatigue -= psiOrFatigue;
                _game.PlayerInfoComponent[userID] = info;
                return true;
            }
            else return false;
        }

        #endregion
    }
}