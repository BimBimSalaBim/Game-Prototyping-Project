using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Value<T>
{
    private T _value;
    private OnModify _subscriptions;
    public delegate T OnModify(T iPrev, T iNext);

    public T mValue { get { return _value; } set {_subscriptions(_value, value); _value = value; } }

    public Value()
    {
        _value = default(T);
    }

    public Value(T iValue)
    {
        _value = iValue;
    }

    public void ClearSubscriptions()
    {
        _subscriptions = null;
    }

    public void Subscribe(OnModify iOnModify)
    {
        _subscriptions += iOnModify;
    }

    public void Unsubscribe(OnModify iOnModify)
    {
        _subscriptions -= iOnModify;
    }

    public void Unsubscribe(OnModify[] iOnModifies)
    {
        foreach(OnModify lModify in iOnModifies)
        {
            _subscriptions -= lModify;
        }
    }

    public void Subscribe(OnModify[] iOnModifies)
    {
        foreach(OnModify lModify in iOnModifies)
        {
            _subscriptions += lModify;
        }
    }
}
