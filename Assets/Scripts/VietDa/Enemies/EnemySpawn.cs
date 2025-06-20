using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemyPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnWithDelay());
    }

    // Update is called once per frame
    void Update()
    {

    }
    System.Collections.IEnumerator SpawnWithDelay()
    {
        while (true) // Infinite loop to keep spawning enemies
        {
            SpawnEnemyAtRandomPosition();
            yield return new WaitForSeconds(2f); // Wait for 2 seconds before spawning the next enemy
        }
    }
    void SpawnEnemyAtRandomPosition()
    {
        // Get screen bounds in world coordinates
        // Generate a random position within the screen bounds
        float fixedX = 10.5f;
        float rangeY = Random.Range(-3.5f, 3.5f);
        Vector2 spawnPosition = new Vector2(fixedX, rangeY);
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
