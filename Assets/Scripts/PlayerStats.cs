using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStats
{
    //Players levels
    static public int experience = 0;
    static public int level = 1;
    static public int nextLevelExp = level * level * 10;

    //Players stats
    static public int hpMax = 10;
    static public int hp;
    static public int resistance = 100;
    static public int damage = 100;
    
    static public void Reset()
    {
        experience = 0;
        level = 1;
        nextLevelExp = level * level * 10;

        hpMax = 10;
        hp = hpMax;
        resistance = 100;
        damage = 100;
    }

    static public void LevelUp()
    {
        level+=1;
        experience = 0;
    }

}
