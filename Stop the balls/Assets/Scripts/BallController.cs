using System;
using UnityEngine;
using DG.Tweening;

public class BallController : MonoBehaviour
{
    public enum BallState
    {
        IN_LAUNCHER,
        LAUNCHED,
        IN_QUEUE
    }

    public event Action LaunchedBallDestroyed;

    public int id;

    [SerializeField] ParticleSystem ballTrailFX;
    [SerializeField] ParticleSystem ballDestroyFX;
    [SerializeField] float launchSpeed;

    private Vector3 ballDirection;
    private BallState ballState = BallState.IN_QUEUE;
    private readonly float outOfBoundsDistance = 20;
    private const string deathColliderTag = "DeathCollider";

    public void Initialize(BallState ballState)
    {
        this.ballState = ballState;
    }

    public void LaunchBall(Vector3 direction)
    {
        ballDirection = direction;
        ballState = BallState.LAUNCHED;
        ballTrailFX.Play();
    }

    public void PlayBallDestroyFX()
    {
        ballDestroyFX.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (ballState == BallState.LAUNCHED)
        {
            MoveBall();
        }
    }

    private void MoveBall()
    {
        transform.Translate(ballDirection * Time.deltaTime * launchSpeed, Space.World);

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
        if (collision.gameObject.CompareTag(deathColliderTag) && ballState == BallState.IN_QUEUE)
        {
            //Debug.Log("GAME OVER!");
            //EventBroker.CallGameOver();
            Destroy(collision.gameObject);

            StartCoroutine(GameManager.Instance.GameOver());
            //Time.timeScale = 0;
        }

        BallController otherObj = collision.gameObject.GetComponent<BallController>();

        if (otherObj != null)
        {
            if (otherObj.ballState == BallState.LAUNCHED)
            {
                Vector3 collisionNormal = collision.GetContact(0).normal;
                bool isCollisionFront = false;
                float angle = Vector3.Angle(transform.forward, collisionNormal);
                if (angle > 90)
                {
                    isCollisionFront = true;
                }

                EventBroker.CallLaunchedBallCollsionWithQueue(otherObj, this, isCollisionFront);
                otherObj.ballState = BallState.IN_QUEUE;
                otherObj.ballTrailFX.Stop();
            }
        }
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
