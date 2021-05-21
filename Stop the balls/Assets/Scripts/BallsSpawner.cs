using UnityEngine;

public class BallsSpawner : MonoBehaviour
{
    [SerializeField] BallController[] ballsPrefabs;

    private readonly float ballSpawnTime = 0.3f;
    private readonly float xSpawnPos = 15;
    private readonly float ySpawnPos = 1;
    private readonly float zSpawnPos = -12;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(SpawnNewBall), 0, ballSpawnTime);
    }

    private void SpawnNewBall()
    {
        BallController newBall = Instantiate(ballsPrefabs[Random.Range(0, ballsPrefabs.Length)]);
        newBall.Initialize(BallController.BallState.IN_QUEUE);
        newBall.transform.position = new Vector3(xSpawnPos, ySpawnPos, zSpawnPos);
        newBall.transform.SetParent(transform);
        EventBroker.CallNewBallWasSpawned(newBall);
    }
}
