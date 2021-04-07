using System;
using System.Collections;
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
        positions = new List<Vector3>();
        ballsIndexToDestroy = new List<int>();

        for (int i = 0; i < ballsInQueue.Count; i++)
        {
            positions.Add(ballsInQueue[i].transform.position);
            ballsInQueue[i].id = i;
            ballsInQueue[i].CollisionWithQueue += AddBallToQueue;
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
            ballsInQueue[i].transform.position = Vector3.Lerp(positions[i], positions[i - 1], distance / ballDiameter);
        }
    }

    private void AddBallToQueue(BallController launchedBall, BallController ballInQueue)
    {
        Debug.Log("Adding ball!");
        positions.Insert(0, positions[0] + direction * ballDiameter);
        ballsInQueue.Insert(ballInQueue.id, launchedBall);
        launchedBall.transform.SetParent(transform);
        ResetBallsIdFromCur(ballInQueue.id);
        launchedBall.CollisionWithQueue += AddBallToQueue;

        DestroyThreeOrMoreSameBalls(launchedBall);
    }

    private void DestroyThreeOrMoreSameBalls(BallController launchedBall)
    {
        for (int i = launchedBall.id; i < ballsInQueue.Count; i++)
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

        for (int i = launchedBall.id - 1; i >= 0; i--)
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

            //Debug.Log(ballsIndexToDestroy[0] + " " + ballsIndexToDestroy[1] + " " + ballsIndexToDestroy[2]);

            for (int i = ballsIndexToDestroy[0]; i <= ballsIndexToDestroy[ballsIndexToDestroy.Count - 1]; i++)
            {
                Destroy(ballsInQueue[i].gameObject);
            }

            ballsInQueue.RemoveRange(ballsIndexToDestroy[0], ballsIndexToDestroy.Count);

        }

        ballsIndexToDestroy.Clear();
    }

    private void ResetBallsIdFromCur(int id)
    {
        for (int i = id; i < ballsInQueue.Count; i++)
        {
            ballsInQueue[i].id = i;
        }
    }
}
