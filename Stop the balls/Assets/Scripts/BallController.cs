using System;
using UnityEngine;
using DG.Tweening;

public class BallController : MonoBehaviour
{
    //public event Action<BallController, BallController> CollisionWithLaunchedBall;
    public event Action LaunchedBallDestroyed;

    public Vector3 ballDirection;
    public int id;

    [SerializeField] float launchSpeed;

    [SerializeField] bool isInQueue = false;

    private readonly float outOfBoundsDistance = 20;

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

    private void OnCollisionEnter(Collision collision)
    {
        BallController otherObj = collision.gameObject.GetComponent<BallController>();

        if (otherObj != null)
        {
            if (otherObj.isInQueue == false)
            {
                Vector3 collisionNormal = collision.GetContact(0).normal;
                bool isCollisionFront = false;
                float angle = Vector3.Angle(transform.forward, collisionNormal);
                if (angle > 90)
                {
                    isCollisionFront = true;
                }

                EventBroker.CallLaunchedBallCollsionWithQueue(otherObj, this, isCollisionFront);
                otherObj.isInQueue = true;
            }
        }
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
