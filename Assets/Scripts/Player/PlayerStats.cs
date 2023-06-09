using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStats
{
    //Player Consts  : To configure
    static public int NEXT_LEVEL_EXP { get { return level * level * 10; } }
    static public int SLOW_GRADIENT { get { return 2+bottleSlow/10; } }
    public const int HPMAX_BASE = 10;
    public const int RESISTANCE_BASE = 0;
    public const int DAMAGE_BASE = 10;
    public const int THROWFORCE_BASE = 10;
    public const int BOTTLECD_BASE = 10;
    public const int BOTTLEDAMAGE_BASE = 10;
    public const int BOTTLEDAMAGERADIUS_BASE = 3;
    public const int BOTTLESLOW_BASE = 0;
    public const int LIFESTEAL_BASE = 1;

    //Player upgrades : To configure
    static public int hpMaxAugment = 10;
    static public int damageAugment = 10;
    static public int resistanceAugment = 10;
    static public int throwForceAugment = 1;
    static public int bottleCdAugment = -1;
    static public int bottleDamageAugment = 10;
    static public int bottleDamageRadiusAugment = 1;
    static public int bottleSlowAugment = 2;
    static public int lifestealAugment = 2;

    //Player stats
    static private int _experience = 0;
    static public int level = 1;
    static private int _hp= HPMAX_BASE;
    static public bool isBottleUp = true;

    //Player upgradable stats
    static private int _hpMax = HPMAX_BASE;
    static public int resistance = RESISTANCE_BASE;
    static public int damage = DAMAGE_BASE;
    static public int throwForce = THROWFORCE_BASE;
    static public int bottleCd = BOTTLECD_BASE;
    static public int bottleDamage = BOTTLEDAMAGE_BASE;
    static public int bottleDamageRadius = BOTTLEDAMAGERADIUS_BASE;
    static public int bottleSlow = BOTTLESLOW_BASE;
    static public int lifesteal = LIFESTEAL_BASE;

    // Properties for updates
    static public int hp { get { return _hp; } set { _hp = value; gameManager.UpdateHealthBar(); } }
    static public int hpMax { get { return _hpMax; } set { _hpMax = value; gameManager.UpdateHealthBar(); } }
    static public int experience { get { return _experience; } set { _experience = value; gameManager.updateExpBar(); } }

    // Game var
    static public bool pause;
    static public GameManager gameManager;

    public enum UpgradableStats
    {
        hpMax,
        resistance,
        damage,
        bottleSlow,
        lifesteal
    }

    public static Dictionary<UpgradableStats, int> upgrades = new Dictionary<UpgradableStats, int>()
    {
        {UpgradableStats.hpMax, hpMaxAugment},
        {UpgradableStats.resistance, resistanceAugment},
        {UpgradableStats.damage, damageAugment},
        {UpgradableStats.bottleSlow, bottleSlowAugment},
        {UpgradableStats.lifesteal , lifestealAugment}
    };

    public static Dictionary<UpgradableStats, Action> ApplyAugment = new Dictionary<UpgradableStats, Action>() {
        {UpgradableStats.hpMax, ()=>{
            hpMax += hpMaxAugment;
        }},
        {UpgradableStats.resistance, ()=>{
            resistance += resistanceAugment;
        }},
        {UpgradableStats.damage, ()=>{
            damage += damageAugment;
        }},
        {UpgradableStats.bottleSlow, ()=>{
            bottleSlow += bottleSlowAugment;
        }},
        {UpgradableStats.lifesteal , ()=>{
            lifesteal += lifestealAugment;
        }}
    };

    static public void Reset()
    {
        experience = 0;
        level = 1;
        isBottleUp = true;


        hp = hpMax = HPMAX_BASE;
        resistance = RESISTANCE_BASE;
        damage = DAMAGE_BASE;
        throwForce = THROWFORCE_BASE;
        bottleCd = BOTTLECD_BASE;
        bottleDamage = BOTTLEDAMAGE_BASE;
        bottleDamageRadius = BOTTLEDAMAGERADIUS_BASE;
        bottleSlow = BOTTLESLOW_BASE;
    }

    static public void GainXP(int value)
    {
        experience += value;
    }


    static public void LevelUp()
    {
        var overlapXP = experience - NEXT_LEVEL_EXP;
        level +=1;
        experience = overlapXP;
        gameManager.updateExpBarMax();
    }

    static public void Lifesteal()
    {
        hp = Mathf.Min(hpMax, hp + lifesteal);
    }

    static public void TakeDamage(int baseDmg)
    {
        float damageReduction = resistance / (100 + resistance); 
        int damageTaken = Mathf.RoundToInt(baseDmg * (1 - damageReduction));
        hp -= damageTaken;
    }
}

