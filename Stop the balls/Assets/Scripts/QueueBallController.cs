using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueBallController : MonoBehaviour
{
    [SerializeField] List<Transform> ballsInQueue;
    [SerializeField] float ballDiameter = 1;

    private List<Vector3> positions;
    private float distance;
    private Vector3 direction;

    
    // Start is called before the first frame update
    void Start()
    {
        positions = new List<Vector3>();

        for (int i = 0; i < ballsInQueue.Count; i++)
        {
            positions.Add(ballsInQueue[i].position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        distance = (ballsInQueue[0].position - positions[0]).magnitude;

        if (distance > ballDiameter)
        {
            direction = (ballsInQueue[0].position - positions[0]).normalized;

            positions.Insert(0, positions[0] + direction * ballDiameter);
            positions.RemoveAt(positions.Count - 1);
            
            distance -= ballDiameter;
        }

        for (int i = 1; i < ballsInQueue.Count; i++)
        {
            ballsInQueue[i].position = Vector3.Lerp(positions[i], positions[i - 1], distance / ballDiameter);
        }
    }
}
