using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enumeration
{
    public static int EnumSize(Type iEnum)
    {
        return ((Enum[])Enum.GetValues(iEnum.GetType())).Length;
    }
    //public static int EnumSize(Enum iEnum)
    //{
    //    return ((Enum[])Enum.GetValues(iEnum.GetType())).Length;
    //}

    public static T[] EnumValues<T>(T iEnum)
    {
        return ((T[])Enum.GetValues(iEnum.GetType()));
    }
}

public enum EntityStat
{
    MAX_HEALTH, HEALTH,SPEED, JUMP_HEIGHT, STAMINA, STRENGTH, HUNGER, INVENTORY_SLOTS
}

public enum CreatureStat
{
    TAME, AGGRESSION, FEAR
}