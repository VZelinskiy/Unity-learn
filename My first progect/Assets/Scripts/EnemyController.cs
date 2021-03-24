using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float startingSpeed;

    private const float accelerateRatio = 0.1f;
    private const int speedChandeRate = 7;
    private readonly float xDestroy = -30;
    
    private Rigidbody objectRb;
    private float speed;

    private void Awake()
    {
        objectRb = GetComponent<Rigidbody>();

        speed = startingSpeed;

        InvokeRepeating(nameof(IncreaseCurrentSpeed), speedChandeRate, speedChandeRate);
    }

    private void OnEnable()
    {
        ResetState();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        objectRb.AddForce(Vector3.left * speed);

        if (transform.position.x < xDestroy)
        {
            ObjectPooler.SharedInstance.SetObjectToPool(gameObject);
        }
    }

    private void ResetState()
    {
        objectRb.velocity = Vector3.zero;
        objectRb.angularVelocity = Vector3.zero;
    }

    private void IncreaseCurrentSpeed()
    {
        speed += speed * accelerateRatio;
    }
}
