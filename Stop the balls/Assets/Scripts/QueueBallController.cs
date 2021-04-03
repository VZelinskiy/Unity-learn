using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueBallController : MonoBehaviour
{
    [SerializeField] GameObject[] waypointsObjects;

    private Vector3[] waypoints;

    // Start is called before the first frame update
    void Start()
    {
        waypoints = new Vector3[3];

        for (int i = 0; i < waypointsObjects.Length; i++)
        {
            waypoints[i] = waypointsObjects[i].transform.position;
        }

        gameObject.transform.DOPath(waypoints, 5, PathType.Linear, PathMode.Full3D, 10, Color.red);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
