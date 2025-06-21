using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnTimer;
    public float spawnInterval;
    [SerializeField] private Transform minPosition; // Minimum position for spawning
    [SerializeField] private Transform maxPosition; // Maximum position for spawning
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime; // Increment the spawn timer by the time since the last frame
        if (spawnTimer >= spawnInterval) // Check if the spawn timer has reached the spawn interval
        {
            spawnTimer = 0f; // Reset the spawn timer
            SpawnEnemyAtRandomPosition(); // Call the method to spawn an enemy at a random position
            
        }
    }
   
    void SpawnEnemyAtRandomPosition()
    {
        Instantiate(enemyPrefab, RandomSpawnPoint(), Quaternion.identity);
    }

    private Vector2 RandomSpawnPoint()
    {
        Vector2 spawnPoint;
        spawnPoint.x = minPosition.position.x;
        spawnPoint.y = Random.Range(minPosition.position.y, maxPosition.position.y);
        return spawnPoint;
    }
}
