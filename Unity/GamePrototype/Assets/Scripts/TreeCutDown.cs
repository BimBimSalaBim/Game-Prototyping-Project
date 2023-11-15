using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeCutDown : MonoBehaviour
{
    public bool _isPlayerInRange;
    public int _treeHP = 5;
    private GameObject player; // Reference to the player GameObject
    public GameObject _woodFieldfab;
    public GameObject _SpwanWoodMiningField;
    public GameObject _TreeMeshRender1;
    public GameObject _TreeMeshRender2;
    private bool isFalling = false;

    // Update is called once per frame
    void Update()
    {
        if (_treeHP == 0 && !isFalling)
        {
            StartCoroutine(TreeFallRoutine());
            isFalling = true;
            this.GetComponentInParent<SphereCollider>().enabled = false;
            _isPlayerInRange = false;
        }
    }

    IEnumerator TreeFallRoutine()
    {
        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Vector3 directionAwayFromPlayer = transform.position - player.transform.position;
            directionAwayFromPlayer = new Vector3(directionAwayFromPlayer.x, -90, directionAwayFromPlayer.z);
            directionAwayFromPlayer.Normalize();

            Quaternion fallRotation = Quaternion.LookRotation(directionAwayFromPlayer);

            while (Quaternion.Angle(transform.rotation, fallRotation) > 0.01f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, fallRotation, Time.deltaTime * 2); // Adjust speed as needed
                yield return null;
            }
        }
       // yield return new WaitForSeconds(1);
        _TreeMeshRender1.GetComponent<MeshRenderer>().enabled = false;
        _TreeMeshRender2.GetComponent<MeshRenderer>().enabled = false;
        _TreeMeshRender1.GetComponent<MeshCollider>().enabled = false;
        _TreeMeshRender2.GetComponent<MeshCollider>().enabled = false;

        // Instantiate the wood mining field
        GameObject SpwanWoodMiningField = Instantiate(_woodFieldfab, _SpwanWoodMiningField.transform.position, _SpwanWoodMiningField.transform.rotation);
        // After falling logic (e.g., spawning wood pile)
    }

    public void CutTree()
    {
        _treeHP = Mathf.Max(_treeHP - 1, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInRange = true;
            //this.tag = "WoodField";
            player = other.gameObject; // Set the player reference
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInRange = false;
            //this.tag = "null";
            player = null; // Clear the player reference
        }
    }
}
