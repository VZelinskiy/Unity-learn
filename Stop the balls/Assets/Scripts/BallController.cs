using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public event Action<BallController, BallController> CollisionWithQueue;

    public Vector3 ballDirection;
    public int id;

    [SerializeField] float launchSpeed;

    [SerializeField] bool isInQueue = false;

    // Update is called once per frame
    void Update()
    {
        if (!isInQueue)
        {
            MoveBall();
        }
    }

    private void MoveBall()
    {
        transform.Translate(ballDirection * Time.deltaTime * launchSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        BallController otherObj = other.gameObject.GetComponent<BallController>();

        if (otherObj.isInQueue == false)
        {
            Debug.Log("COLLISION! " + gameObject.name + " " + other.name);
            CollisionWithQueue?.Invoke(otherObj, this);
            otherObj.isInQueue = true;
        }
    }
}
