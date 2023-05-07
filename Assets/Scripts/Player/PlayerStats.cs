using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStats
{
    //Player Consts  : To configure
    static public int NEXT_LEVEL_EXP { get { return level * level * 10; } }
    public const int HPMAX_BASE = 10;
    public const int RESISTANCE_BASE = 0;
    public const int DAMAGE_BASE = 10;
    public const int THROWFORCE_BASE = 10;

    //Player upgrades : To configure
    static public int hpMaxAugment = 10;
    static public int dmgAugment = 10;
    static public int resistAugment = 10;
    static public int throwForceAugment = 1;

    //Player stats
    static public int experience = 0;
    static public int level = 1;
    static public int hp = HPMAX_BASE;
    static public int hpMax = HPMAX_BASE;
    static public int resistance = RESISTANCE_BASE;
    static public int damage = DAMAGE_BASE;
    static public int throwForce = THROWFORCE_BASE;

    public enum UpgradableStats
    {
        hpMax,
        resistance,
        damage
    }

    static public void Reset()
    {
        experience = 0;
        level = 1;

        hpMax = HPMAX_BASE;
        hp = HPMAX_BASE;
        resistance = RESISTANCE_BASE;
        damage = DAMAGE_BASE;
    }


    static public void LevelUp()
    {
        level+=1;
        experience = 0;
    }

    static public void TakeDamage(int baseDmg)
    {
        float damageReduction = resistance / (100 + resistance); 
        int damageTaken = Mathf.RoundToInt(baseDmg * (1 - damageReduction));
        hp -= damageTaken;
    }
}

