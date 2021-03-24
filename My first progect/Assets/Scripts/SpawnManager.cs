using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] string[] enemiesTags;

    private readonly float xSpawnPos = 25;
    private readonly float ySpawnMin = -7;
    private readonly float ySpawnMax = 9;
    private readonly float powerupSpawnTime = 10;
    private readonly float foodSpawnTime = 3;
    private readonly float enemySpawnTime = 2;
    private readonly float startDelay = 1;
    private readonly float powerupStartDelay = 10;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(SpawnRandomEnemy), startDelay, enemySpawnTime);
        InvokeRepeating(nameof(SpawnPowerup), powerupStartDelay, powerupSpawnTime);
        InvokeRepeating(nameof(SpawnFood), startDelay, foodSpawnTime);
    }

    void SpawnRandomEnemy()
    {
        float randomY = Random.Range(ySpawnMin, ySpawnMax);
        int randomIndex = Random.Range(0, enemiesTags.Length);
        Vector3 spawnPos = new Vector3(xSpawnPos, randomY, 0);
        ObjectPooler.SharedInstance.GetPooledObject(enemiesTags[randomIndex], spawnPos, Quaternion.Euler(Vector3.zero));
    }

    void SpawnPowerup()
    {
        float randomY = Random.Range(ySpawnMin, ySpawnMax);
        Vector3 spawnPos = new Vector3(xSpawnPos, randomY, 0);
        ObjectPooler.SharedInstance.GetPooledObject("Powerup", spawnPos, Quaternion.Euler(Vector3.zero));
    }

    void SpawnFood()
    {
        float randomY = Random.Range(ySpawnMin, ySpawnMax);
        Vector3 spawnPos = new Vector3(xSpawnPos, randomY, 0);
        ObjectPooler.SharedInstance.GetPooledObject("Food", spawnPos, Quaternion.Euler(Vector3.zero));
    }
}
