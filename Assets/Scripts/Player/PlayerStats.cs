using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStats
{
    //Player Consts  : To configure
    static public int NEXT_LEVEL_EXP { get { return level * level * 10; } }
    public const int HP_MAX_BASE = 10;
    public const int RESISTANCE_BASE = 0;
    public const int DAMAGE_BASE = 10;

    //Player upgrades : To configure
    static public int hpMaxAugment = 10;
    static public int dmgAugment = 10;
    static public int resistAugment = 10;

    //Player stats
    static public int experience = 0;
    static public int level = 1;
    static public int hp = HP_MAX_BASE;
    static public int hpMax = HP_MAX_BASE;
    static public int resistance = RESISTANCE_BASE;
    static public int damage = DAMAGE_BASE;

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

        hpMax = HP_MAX_BASE;
        hp = HP_MAX_BASE;
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

