using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefabToSpawn; // The prefab to spawn
    public float spawnInterval = 3f; // Time interval between spawns
    public Vector3 spawnArea = new Vector3(5f, 0f, 5f); // Area within which to spawn

    private float timer; // Timer to keep track of spawn interval

    void Start()
    {
        timer = spawnInterval; // Initialize the timer
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            SpawnPrefab();
            timer = spawnInterval; // Reset the timer
        }
    }

    void SpawnPrefab()
    {
        Vector3 spawnPosition = transform.position + new Vector3(
            Random.Range(-spawnArea.x / 2, spawnArea.x / 2),
            Random.Range(-spawnArea.y / 2, spawnArea.y / 2),
            Random.Range(-spawnArea.z / 2, spawnArea.z / 2)
        );
        
        Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
    }
}