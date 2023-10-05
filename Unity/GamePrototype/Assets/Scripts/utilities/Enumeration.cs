using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enumeration
{
    int EnumSize(Enum iEnum)
    {
        return ((Enum[])Enum.GetValues(iEnum.GetType())).Length;
    }

    T[] EnumValues<T>(T iEnum)
    {
        return ((T[])Enum.GetValues(iEnum.GetType()));
    }
}
