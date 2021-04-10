using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class QueueController : MonoBehaviour
{
    [SerializeField] List<BallController> ballsInQueue;
    [SerializeField] float ballDiameter = 1;

    private List<Vector3> positions;
    private List<int> ballsIndexToDestroy;
    private float distance;
    private Vector3 direction;

    
    // Start is called before the first frame update
    void Start()
    {
        EventBroker.LaunchedBallCollsionWithQueue += AddBallToQueue;

        positions = new List<Vector3>();
        ballsIndexToDestroy = new List<int>();

        for (int i = 0; i < ballsInQueue.Count; i++)
        {
            positions.Add(ballsInQueue[i].transform.position);
            ballsInQueue[i].id = i;
            //ballsInQueue[i].CollisionWithLaunchedBall += AddBallToQueue;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        distance = (ballsInQueue[0].transform.position - positions[0]).magnitude;

        if (distance > ballDiameter)
        {
            direction = (ballsInQueue[0].transform.position - positions[0]).normalized;

            positions.Insert(0, positions[0] + direction * ballDiameter);
            positions.RemoveAt(positions.Count - 1);
            
            distance -= ballDiameter;
        }

        for (int i = 1; i < ballsInQueue.Count; i++)
        {
            //TO DELETE ballsInQueue[i].transform.position = Vector3.Lerp(positions[i], positions[i - 1], distance / ballDiameter);
            //ballsInQueue[i].transform.position = Vector3.Lerp(positions[ballsInQueue[i].id], positions[ballsInQueue[i].id - 1], distance / ballDiameter);
            ballsInQueue[i].transform.DOMove(positions[ballsInQueue[i].id - 1], 1f);
        }
    }

    private void AddBallToQueue(BallController launchedBall, BallController ballInQueue)
    {
        positions.Insert(0, positions[0] + direction * ballDiameter);
        int ballInQueueListId = GetQueueBallsListIdByBallId(ballInQueue.id);
        ballsInQueue.Insert(ballInQueueListId, launchedBall);
        launchedBall.transform.SetParent(transform);
        ResetBallsIdAfterAddingFromCur(ballInQueueListId - 1);
        //launchedBall.CollisionWithLaunchedBall += AddBallToQueue;

        DestroyThreeOrMoreSameBalls(launchedBall);
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

        Debug.Log("BAD error");
        return 0;
    }

    private void DestroyThreeOrMoreSameBalls(BallController launchedBall)
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
            ballsIndexToDestroy.Sort();

            for (int i = ballsIndexToDestroy[0]; i <= ballsIndexToDestroy[ballsIndexToDestroy.Count - 1]; i++)
            {
                Destroy(ballsInQueue[i].gameObject);
            }

            ballsInQueue.RemoveRange(ballsIndexToDestroy[0], ballsIndexToDestroy.Count);
            //ResetBallsIdFromCur(ballsIndexToDestroy[0]);
            ResetBallsIdAfterDeletingFromCur(ballsIndexToDestroy[0]);
        }

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
