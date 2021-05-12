using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueController : MonoBehaviour
{
    [SerializeField] List<BallController> ballsInQueue;
    //[SerializeField] BallController[] ballsPrefabs;
    [SerializeField] AudioClip ballsCollisionSFX;
    [SerializeField] AudioClip ballsDestroySFX;
    
    private AudioSource audioSource;
    private List<Vector3> positions;
    private List<int> ballsIndexToDestroy;
    private float distance;
    private Vector3 direction;
    private bool isGameOver = false;
    private int lastBallId = 0;

    private readonly float ballDiameter = 1;
    //private readonly float ballSpawnTime = 0.5f;

    private const float tweenDuration = 2f;
    private const int sequencesCapacity = 50;
    private const int tweenersCapacity = 1250;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        DOTween.SetTweensCapacity(tweenersCapacity, sequencesCapacity);

        EventBroker.LaunchedBallCollsionWithQueue += AddLaunchedBallToQueue;
        EventBroker.GameOver += GameOverHandler;
        EventBroker.NewBallIWasSpawned += SpawnNewBallHandler;

        positions = new List<Vector3>();
        ballsIndexToDestroy = new List<int>();
        
        positions.Add(ballsInQueue[0].transform.position);
        ballsInQueue[0].id = 0;
    }

    private void GameOverHandler()
    {
        isGameOver = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {
            Move();
        }
    }

    private void Move()
    {
        distance = (ballsInQueue[0].transform.position - positions[0]).magnitude;

        if (distance > ballDiameter)
        {
            direction = (ballsInQueue[0].transform.position - positions[0]).normalized;

            positions.Insert(0, positions[0] + direction * ballDiameter);
            positions.RemoveAt(positions.Count - 1);
        }

        for (int i = 1; i < ballsInQueue.Count; i++)
        {
            ballsInQueue[i].transform.DOMove(positions[ballsInQueue[i].id - 1], tweenDuration);
            ballsInQueue[i].transform.DOLookAt(positions[ballsInQueue[i].id - 1], tweenDuration);
        }
    }

    private void SpawnNewBallHandler(BallController spawnedBall)
    {
        lastBallId++;
        spawnedBall.id = lastBallId;
        positions.Insert(0, positions[0] + direction * ballDiameter);
        ballsInQueue.Add(spawnedBall);
    }

    private void AddLaunchedBallToQueue(BallController launchedBall, BallController ballInQueue, bool isCollisionFront)
    {
        positions.Insert(0, positions[0] + direction * ballDiameter);
        int ballInQueueListId;

        if (isCollisionFront)
        {
            ballInQueueListId = GetQueueBallsListIdByBallId(ballInQueue.id);
        }
        else
        {
            ballInQueueListId = GetQueueBallsListIdByBallId(ballInQueue.id + 1);
        }
        
        ballsInQueue.Insert(ballInQueueListId, launchedBall);
        launchedBall.transform.SetParent(transform);
        ResetBallsIdAfterAddingFromCur(ballInQueueListId - 1);
        lastBallId = ballsInQueue[ballsInQueue.Count - 1].id;

        if (AreThereThreeOrMoreSameBalls(launchedBall))
        {
            ballsIndexToDestroy.Sort();
            PlayDestroyBallsFX();
            StartCoroutine(DestroyThreeOrMoreSameBalls());
        }
        
        //ballsIndexToDestroy.Clear();
    }

    private int GetQueueBallsListIdByBallId(int id)
    {
        for (int i = 0; i < ballsInQueue.Count; i++)
        {
            if (ballsInQueue[i].id == id)
            {
                return i;
            }
        }

        return ballsInQueue.Count;
    }

    private bool AreThereThreeOrMoreSameBalls(BallController launchedBall)
    {
        int launchedBallListId = GetQueueBallsListIdByBallId(launchedBall.id);

        for (int i = launchedBallListId; i < ballsInQueue.Count; i++)
        {
            if (ballsInQueue[i].CompareTag(launchedBall.tag))
            {
                ballsIndexToDestroy.Add(i);
            }
            else
            {
                break;
            }
        }

        for (int i = launchedBallListId - 1; i >= 0; i--)
        {
            if (ballsInQueue[i].CompareTag(launchedBall.tag))
            {
                ballsIndexToDestroy.Add(i);
            }
            else
            {
                break;
            }
        }

        if (ballsIndexToDestroy.Count > 2)
        { 
            return true;
        }

        ballsIndexToDestroy.Clear();
        return false;
    }

    private void PlayDestroyBallsFX()
    {
        for (int i = ballsIndexToDestroy[0]; i <= ballsIndexToDestroy[ballsIndexToDestroy.Count - 1]; i++)
        {
            ballsInQueue[i].PlayBallDestroyFX();
        }
    }

    private IEnumerator DestroyThreeOrMoreSameBalls()
    {
        yield return new WaitForSeconds(tweenDuration / 2);

        for (int i = ballsIndexToDestroy[0]; i <= ballsIndexToDestroy[ballsIndexToDestroy.Count - 1]; i++)
        {
            Destroy(ballsInQueue[i].gameObject);
        }

        audioSource.PlayOneShot(ballsDestroySFX);

        ballsInQueue.RemoveRange(ballsIndexToDestroy[0], ballsIndexToDestroy.Count);
        ResetBallsIdAfterDeletingFromCur(ballsIndexToDestroy[0]);
        ballsIndexToDestroy.Clear();
    }

    private void ResetBallsIdAfterDeletingFromCur(int id)
    {
        int newId = ballsInQueue[id].id;

        for (int i = id; i >= 1; i--)
        {
            ballsInQueue[i].id = newId;
            newId--;
        }
    }

    private void ResetBallsIdAfterAddingFromCur(int id)
    {
        int newId = ballsInQueue[id].id;

        if (newId == 0)
        {
            newId = ballsInQueue[2].id;
        }

        for (int i = id; i < ballsInQueue.Count; i++)
        {
            if (i != 0)
            {
                ballsInQueue[i].id = newId;
                newId++;
            }
        }
    }
}
