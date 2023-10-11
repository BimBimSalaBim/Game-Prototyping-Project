using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStats
{
    public float this[EntityStat iIndex] { get { return Get(iIndex); } set { Set(iIndex, value); } }

    public void Set(EntityStat iStat, float iValue)
    {
        stats[(int)iStat].mValue = iValue;
    }
    public float Get(EntityStat iStat)
    {
        return stats[(int)iStat].mValue;
    }
    private Value<float>[] stats = new Value<float>[Enumeration.EnumSize(typeof(EntityStat))];
    public void Subscribe(EntityStat iStat, Value<float>.OnModify iFunction)
    {
        stats[(int)iStat].Subscribe(iFunction);
    }
    public void Unsubscribe(EntityStat iStat, Value<float>.OnModify iFunction)
    {
        stats[(int)iStat].Unsubscribe(iFunction);
    }
}
