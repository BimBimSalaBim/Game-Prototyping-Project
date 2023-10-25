using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Entity : MonoBehaviour
{
    private EntityStats _stats = new EntityStats();
    //MAX_HEALTH, HEALTH,SPEED, JUMP_HEIGHT, STAMINA, STRENGTH, HUNGER, INVENTORY_SLOTS
    public float mHealth { get { return _stats[EntityStat.HEALTH]; } set { _stats[EntityStat.HEALTH] = value; } }
    public float mMaxHealth { get { return _stats[EntityStat.MAX_HEALTH]; } set { _stats[EntityStat.MAX_HEALTH] = value; } }
    public float mSpeed { get { return _stats[EntityStat.SPEED]; } set { _stats[EntityStat.SPEED] = value; } }
    public float mJumpHeight { get { return _stats[EntityStat.JUMP_HEIGHT]; } set { _stats[EntityStat.JUMP_HEIGHT] = value; } }
    public float mStamina { get { return _stats[EntityStat.STAMINA]; } set { _stats[EntityStat.STAMINA] = value; } }
    public float mStrength { get { return _stats[EntityStat.STRENGTH]; } set { _stats[EntityStat.STRENGTH] = value; } }
    public float mHunger { get { return _stats[EntityStat.HUNGER]; } set { _stats[EntityStat.HUNGER] = value; } }
    public float mInventory_Slots { get { return _stats[EntityStat.INVENTORY_SLOTS]; } set { _stats[EntityStat.INVENTORY_SLOTS] = value; } }
    public NavMeshAgent navMeshAgent;
    

    public void Subscribe(EntityStat iStat, Value<float>.OnModify iFunction)
    {
        _stats.Subscribe(iStat, iFunction);
    }

    public void Unsubscribe(EntityStat iStat, Value<float>.OnModify iFunction)
    {
        _stats.Unsubscribe(iStat, iFunction);
    }

    public virtual void Move(Vector3 iDir)
    {
        navMeshAgent.SetDestination(iDir);
    }

    public virtual void Jump(bool iJump)
    {

    }
}