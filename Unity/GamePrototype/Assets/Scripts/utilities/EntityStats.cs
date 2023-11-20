using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStats
{
    
    public float this[EntityStat stat] { get { return stats[(int)stat].mValue; } set { stats[(int)stat].mValue = value; }}
    private Value<float>[] stats = new Value<float>[Enumeration.EnumSize(typeof(EntityStat))];

    public EntityStats()
    {
        for (int i = 0; i < stats.Length; i++)
        {
            stats[i] = new Value<float>(0); 
        }
        Set(EntityStat.HEALTH, 100f); // Default health
        Set(EntityStat.HUNGER, 50f);  // Default hunger
        Set(EntityStat.MAX_HEALTH, 100f); // Default max health
        Set(EntityStat.SPEED, 5f); // Default speed
        Set(EntityStat.STAMINA, 100f); // Default stamina
        Set(EntityStat.STRENGTH, 10f); // Default strength
        Set(EntityStat.JUMP_HEIGHT, 0.5f); // Default jump height
        Set(EntityStat.INVENTORY_SLOTS, 0f); // Default inventory slots

    }
    public void Set(EntityStat iStat, float iValue)
    {
        stats[(int)iStat].mValue = iValue;
    }
    public float Get(EntityStat iStat)
    {
        return stats[(int)iStat].mValue;
    }
    // private Value<float>[] stats = new Value<float>[Enumeration.EnumSize(typeof(EntityStat))];
    public void Subscribe(EntityStat iStat, Value<float>.OnModify iFunction)
    {
        stats[(int)iStat].Subscribe(iFunction);
    }
    public void Unsubscribe(EntityStat iStat, Value<float>.OnModify iFunction)
    {
        stats[(int)iStat].Unsubscribe(iFunction);
    }
}
