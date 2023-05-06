using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStats
{
    //Player basics
    static public int experience = 0;
    static public int level = 1;
    static public int hp;

    //Player Consts
    static public int nextLevelExp = level * level * 10;


    //Player stats
    static public int hpMax = 10;
    static public int resistance = 100;
    static public int damage = 100;

    //Player upgrades 
    static public int hpMaxAugment = 10;
    static public int dmgAugment = 10;
    static public int resistAugment = 10;

    //RESPECTER LA COHERENCE ENTRE STATS - UPGADES ET L'ENUM !
    public enum UpgradableStats
    {
        hpMax,
        resistance,
        damage
    }



    static public void LevelUp()
    {
        level+=1;
        experience = 0;
    }



}
