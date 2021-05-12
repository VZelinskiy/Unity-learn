using UnityEngine;

public class BallsSpawner : MonoBehaviour
{
    [SerializeField] BallController[] ballsPrefabs;

    private readonly float ballSpawnTime = 10f;
    private readonly float xSpawnPos = 7;
    private readonly float ySpawnPos = 1;
    private readonly float zSpawnPos = -11;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(SpawnNewBall), 0, ballSpawnTime);

        /*for (int i = 0; i < 20; i++)
        {
            SpawnNewBall();
        }*/
    }

    private void SpawnNewBall()
    {
        for (int i = 0; i < 20; i++)
        {
            BallController newBall = Instantiate(ballsPrefabs[Random.Range(0, ballsPrefabs.Length)]);
            newBall.Initialize(BallController.BallState.IN_QUEUE);
            newBall.transform.position = new Vector3(xSpawnPos, ySpawnPos, zSpawnPos);
            newBall.transform.SetParent(transform);
            EventBroker.CallNewBallIWasSpawned(newBall);
        }

        
    }
}
