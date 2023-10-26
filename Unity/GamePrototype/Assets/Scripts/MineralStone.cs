using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MineralStone : MonoBehaviour
{
    public int _value;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setValue(int value) { this._value = value; }
    public int getValue() { return _value; }
}
