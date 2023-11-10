using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchPlayerRotation : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject _player;
    public Vector3 _rotationOffset;
    void Start()
    {
        _player = GameObject.FindWithTag("Player");

        transform.rotation = _player.transform.rotation;
        transform.rotation = _player.transform.rotation * Quaternion.Euler(_rotationOffset);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
