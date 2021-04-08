using System;
using UnityEngine;

public class BallController : MonoBehaviour
{
    //public event Action<BallController, BallController> CollisionWithLaunchedBall;
    public event Action LaunchedBallDestroyed;

    public Vector3 ballDirection;
    public int id;

    [SerializeField] float launchSpeed;

    [SerializeField] bool isInQueue = false;

    private float outOfBoundsDistance = 20;

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

        if (IsOutOfBounds())
        {
            Destroy(gameObject);
            LaunchedBallDestroyed?.Invoke();
        }
    }

    private bool IsOutOfBounds()
    {
        if (transform.position.x > outOfBoundsDistance)
            return true;
        if (transform.position.x < -outOfBoundsDistance)
            return true;
        if (transform.position.z > outOfBoundsDistance)
            return true;
        if (transform.position.z < -outOfBoundsDistance)
            return true;

        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        BallController otherObj = other.gameObject.GetComponent<BallController>();

        if (otherObj != null)
        {
            if (otherObj.isInQueue == false)
            {
                //CollisionWithLaunchedBall?.Invoke(otherObj, this);
                EventBroker.CallLaunchedBallCollsionWithQueue(otherObj, this);
                otherObj.isInQueue = true;
            }  
        }
        else
        {
            Debug.Log("obj was destroyed");
        }
    }
}
