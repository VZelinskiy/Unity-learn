using UnityEngine;

public class LauncherController : MonoBehaviour
{
    [SerializeField] GameObject ballPlace;
    [SerializeField] BallController ballPrefab;
    [SerializeField] float turnSpeed;

    //private Vector3 launcherRotation;
    private float horizontalInput;
    private bool isLaunchedBallAvailable = true;

    private readonly string launchButtonName = "Jump";
    private readonly string horizontalAxisName = "Horizontal";

    // Start is called before the first frame update
    void Start()
    {
        EventBroker.LaunchedBallCollsionWithQueue += LaunchedBallCollsionWithQueueHandler;
    }

    private void LaunchedBallCollsionWithQueueHandler(BallController arg1, BallController arg2)
    {
        isLaunchedBallAvailable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton(launchButtonName))
        {
            if (isLaunchedBallAvailable)
            {
                LaunchBall();
            }
        }

        RotateLauncher();
    }

    private void LaunchBall()
    {
        BallController ball = Instantiate(ballPrefab, ballPlace.transform.position, Quaternion.identity);
        ball.ballDirection = gameObject.transform.up;
        isLaunchedBallAvailable = false;

        ball.LaunchedBallDestroyed += MakeLaunchedBallAvailable; 
    }

    private void MakeLaunchedBallAvailable()
    {
        isLaunchedBallAvailable = true;
    }

    private void RotateLauncher()
    {
        horizontalInput = Input.GetAxis(horizontalAxisName);
        transform.Rotate(Vector3.back, turnSpeed * horizontalInput * Time.deltaTime);
    }
}
