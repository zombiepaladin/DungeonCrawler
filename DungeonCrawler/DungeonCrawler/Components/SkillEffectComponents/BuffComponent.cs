#region File Description
//-----------------------------------------------------------------------------
// BuffComponent.cs 
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

namespace DungeonCrawler.Components
{
    public struct Buff
    {
        public uint EntityID;
        public uint TargetID;

        public int ResistPoison;
        public bool isPercentResistPoison;

        public int AttackMelee;
        public bool isPercentAttackMelee;

        public int AttackRanged;
        public bool isPercentAttackRanged;

        public int DefenseMelee;
        public bool isPercentDefenseMelee;

        public int DefenseRanged;
        public bool isPercentDefenseRanged;

        public int WeaponStrength;
        public bool isPercentWeaponStrength;

        public int WeaponAccuracy;
        public bool isPercentWeaponAccuracy;

        public int WeaponSpeed;
        public bool isPercentWeaponSpeed;

        public int AttackSpeed;
        public bool isPercentAttackSpeed;

        public int Health;
        public bool isPercentHealth;

        public int Psi;
        public bool isPercentPsi;

        public int Fatigue;
        public bool isPercentFatigue;

        public int MovementSpeed;
        public bool isPercentMovementSpeed;
    }

    public class BuffComponent : GameComponent<Buff>
    {

        new public void Add(uint entityID, Buff component)
        {
            base.Add(entityID, component);

            DungeonCrawlerGame game = DungeonCrawlerGame.game;

            if (game.StatsComponent.Contains(component.TargetID))
            {
                Stats stats = game.StatsComponent[component.TargetID];

                if (game.PlayerInfoComponent.Contains(component.TargetID))
                {
                    PlayerInfo playerInfo = game.PlayerInfoComponent[component.TargetID];

                    if (component.ResistPoison != 0)
                    {
                        int amount = component.ResistPoison;
                        if (component.isPercentResistPoison)
                        {
                            amount = (amount * stats.PoisonResistanceBase) / 100;
                        }

                        playerInfo.PoisonResistance += amount;

                    }

                    if (component.AttackMelee != 0)
                    {
                        int amount = component.AttackMelee;
                        if (component.isPercentResistPoison)
                        {
                            amount = (amount * stats.AttackMeleeBase) / 100;
                        }

                        playerInfo.AttackMelee += amount;

                    }

                    if (component.AttackRanged != 0)
                    {
                        int amount = component.AttackRanged;
                        if (component.isPercentResistPoison)
                        {
                            amount = (amount * stats.AttackRangedBase) / 100;
                        }

                        playerInfo.AttackRanged += amount;

                    }

                    if (component.DefenseMelee != 0)
                    {
                        int amount = component.DefenseMelee;
                        if (component.isPercentResistPoison)
                        {
                            amount = (amount * stats.DefenseMeleeBase) / 100;
                        }

                        playerInfo.DefenseMelee += amount;

                    }

                    if (component.DefenseRanged != 0)
                    {
                        int amount = component.DefenseRanged;
                        if (component.isPercentResistPoison)
                        {
                            amount = (amount * stats.DefenseRangedBase) / 100;
                        }

                        playerInfo.DefenseRanged += amount;

                    }

                    if (component.WeaponStrength != 0)
                    {
                        int amount = component.WeaponStrength;
                        if (component.isPercentResistPoison)
                        {
                            amount = (amount * stats.WeaponStrengthBase) / 100;
                        }

                        playerInfo.WeaponStrength += amount;

                    }

                    if (component.WeaponAccuracy != 0)
                    {
                        int amount = component.WeaponAccuracy;
                        if (component.isPercentResistPoison)
                        {
                            amount = (amount * stats.WeaponAccuracyBase) / 100;
                        }

                        playerInfo.WeaponAccuracy += amount;

                    }

                    if (component.WeaponSpeed != 0)
                    {
                        int amount = component.WeaponSpeed;
                        if (component.isPercentResistPoison)
                        {
                            amount = (amount * stats.WeaponSpeedBase) / 100;
                        }

                        playerInfo.WeaponSpeed += amount;

                    }

                    if (component.AttackSpeed != 0)
                    {
                        int amount = component.AttackSpeed;
                        if (component.isPercentResistPoison)
                        {
                            amount = (amount * stats.AttackSpeedBase) / 100;
                        }

                        playerInfo.AttackSpeed += amount;

                    }
            

                
                    if (component.Health != 0)
                    {
                        int amount = component.Health;
                        if (component.isPercentHealth)
                        {
                            amount = (amount * stats.HealthBase) / 100;
                        }

                        playerInfo.Health += amount;
                    }

                    if (component.Psi != 0)
                    {
                        int amount = component.Psi;
                        if (component.isPercentHealth)
                        {
                            amount = (amount * stats.PsiBase) / 100;
                        }

                        playerInfo.Psi += amount;
                    }

                    if (component.Fatigue != 0)
                    {
                        int amount = component.Fatigue;
                        if (component.isPercentHealth)
                        {
                            amount = (amount * stats.FatigueBase) / 100;
                        }

                        playerInfo.Psi += amount;
                    }
                }

                game.StatsComponent[component.TargetID] = stats;
            }

            if (game.MovementComponent.Contains(component.TargetID))
            {
                if (component.MovementSpeed != 0)
                {
                    Movement movementInstance = game.MovementComponent[component.TargetID];
                    movementInstance.Speed += component.MovementSpeed;
                    game.MovementComponent[component.TargetID] = movementInstance;
                }
            }
        }

        new public void Remove(uint entityID)
        {
            DungeonCrawlerGame game = DungeonCrawlerGame.game;

            Buff component = game.BuffComponent[entityID];

            if (game.StatsComponent.Contains(component.TargetID))
            {
                Stats stats = game.StatsComponent[component.TargetID];
                
                if (game.PlayerInfoComponent.Contains(component.TargetID))
                {
                    PlayerInfo playerInfo = game.PlayerInfoComponent[component.TargetID];

                    if (component.ResistPoison != 0)
                    {
                        int amount = component.ResistPoison;
                        if (component.isPercentResistPoison)
                        {
                            amount = (amount * stats.PoisonResistanceBase) / 100;
                        }

                        playerInfo.PoisonResistance -= amount;

                    }

                    if (component.AttackMelee != 0)
                    {
                        int amount = component.AttackMelee;
                        if (component.isPercentResistPoison)
                        {
                            amount = (amount * stats.AttackMeleeBase) / 100;
                        }

                        playerInfo.AttackMelee -= amount;

                    }

                    if (component.AttackRanged != 0)
                    {
                        int amount = component.AttackRanged;
                        if (component.isPercentResistPoison)
                        {
                            amount = (amount * stats.AttackRangedBase) / 100;
                        }

                        playerInfo.AttackRanged -= amount;

                    }

                    if (component.DefenseMelee != 0)
                    {
                        int amount = component.DefenseMelee;
                        if (component.isPercentResistPoison)
                        {
                            amount = (amount * stats.DefenseMeleeBase) / 100;
                        }

                        playerInfo.DefenseMelee -= amount;

                    }

                    if (component.DefenseRanged != 0)
                    {
                        int amount = component.DefenseRanged;
                        if (component.isPercentResistPoison)
                        {
                            amount = (amount * stats.DefenseRangedBase) / 100;
                        }

                        playerInfo.DefenseRanged -= amount;

                    }

                    if (component.WeaponStrength != 0)
                    {
                        int amount = component.WeaponStrength;
                        if (component.isPercentResistPoison)
                        {
                            amount = (amount * stats.WeaponStrengthBase) / 100;
                        }

                        playerInfo.WeaponStrength -= amount;

                    }

                    if (component.WeaponAccuracy != 0)
                    {
                        int amount = component.WeaponAccuracy;
                        if (component.isPercentResistPoison)
                        {
                            amount = (amount * stats.WeaponAccuracyBase) / 100;
                        }

                        playerInfo.WeaponAccuracy -= amount;

                    }

                    if (component.WeaponSpeed != 0)
                    {
                        int amount = component.WeaponSpeed;
                        if (component.isPercentResistPoison)
                        {
                            amount = (amount * stats.WeaponSpeedBase) / 100;
                        }

                        playerInfo.WeaponSpeed -= amount;

                    }

                    if (component.AttackSpeed != 0)
                    {
                        int amount = component.AttackSpeed;
                        if (component.isPercentResistPoison)
                        {
                            amount = (amount * stats.AttackSpeedBase) / 100;
                        }

                        playerInfo.AttackSpeed -= amount;

                    }


                
                    if (component.Health != 0)
                    {
                        int amount = component.Health;
                        if (component.isPercentHealth)
                        {
                            amount = (amount * stats.HealthBase) / 100;
                        }

                        playerInfo.Health -= amount;
                    }

                    if (component.Psi != 0)
                    {
                        int amount = component.Psi;
                        if (component.isPercentHealth)
                        {
                            amount = (amount * stats.PsiBase) / 100;
                        }

                        playerInfo.Psi -= amount;
                    }

                    if (component.Fatigue != 0)
                    {
                        int amount = component.Fatigue;
                        if (component.isPercentHealth)
                        {
                            amount = (amount * stats.FatigueBase) / 100;
                        }

                        playerInfo.Psi -= amount;
                    }

                    game.PlayerInfoComponent[component.TargetID] = playerInfo;
                }
            }
            if (component.MovementSpeed != 0)
            {
                Movement movementInstance = game.MovementComponent[component.TargetID];
                movementInstance.Speed -= component.MovementSpeed;
                game.MovementComponent[component.TargetID] = movementInstance;
            }

        }

    }
}
