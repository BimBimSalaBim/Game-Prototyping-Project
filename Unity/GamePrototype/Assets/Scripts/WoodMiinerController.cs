using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodMiinerController : MonoBehaviour
{
    public int _hardnessLevel;
    public GameObject _spawnPoint;
    public bool _isPlayerInRange;
    public GameObject _woodPrefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("In range");
            _isPlayerInRange = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Out range");
            _isPlayerInRange = false;

        }
    }
    public void SpawnWood()
    {
        // Instantiate the gem prefab at the position and rotation of _pawnPoint.
        GameObject spawnedGem = Instantiate(_woodPrefab, _spawnPoint.transform.position, _spawnPoint.transform.rotation);

        // Ensure the spawned gem has a Rigidbody component.
        Rigidbody gemRb = spawnedGem.GetComponent<Rigidbody>();
        if (gemRb != null)
        {
            // Apply random force and torque to the Rigidbody.

            // Define the range for the random force and torque values.
            float forceMagnitude = 15.0f; // Adjust as needed.
            float torqueMagnitude = 15.0f; // Adjust as needed.

            Vector3 randomForce = new Vector3(
                Random.Range(-forceMagnitude, forceMagnitude),
                Random.Range(-forceMagnitude, forceMagnitude),
                Random.Range(-forceMagnitude, forceMagnitude)
            );

            Vector3 randomTorque = new Vector3(
                Random.Range(-torqueMagnitude, torqueMagnitude),
                Random.Range(-torqueMagnitude, torqueMagnitude),
                Random.Range(-torqueMagnitude, torqueMagnitude)
            );

            gemRb.AddForce(randomForce, ForceMode.Impulse);
            gemRb.AddTorque(randomTorque, ForceMode.Impulse);
        }
    }
}
