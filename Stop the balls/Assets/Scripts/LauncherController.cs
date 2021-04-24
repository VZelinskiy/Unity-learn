using UnityEngine;

public class LauncherController : MonoBehaviour
{
    [SerializeField] BallController[] ballPrefabs;
    [SerializeField] GameObject ballPlace;
    [SerializeField] float turnSpeed;

    private BallController curBall;
    private float horizontalInput;
    private bool isLaunchedBallAvailable = true;

    private readonly string launchButtonName = "Jump";
    private readonly string horizontalAxisName = "Horizontal";

    // Start is called before the first frame update
    void Start()
    {
        EventBroker.LaunchedBallCollsionWithQueue += LaunchedBallCollsionWithQueueHandler;

        InitCurBall();
    }

    private void LaunchedBallCollsionWithQueueHandler(BallController arg1, BallController arg2, bool arg3)
    {
        MakeLaunchedBallAvailable();
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
        curBall.transform.SetParent(null);
        curBall.LaunchBall(transform.up);
        isLaunchedBallAvailable = false;

        curBall.LaunchedBallDestroyed += MakeLaunchedBallAvailable; 
    }

    private void MakeLaunchedBallAvailable()
    {
        InitCurBall();
        isLaunchedBallAvailable = true;
    }

    private void RotateLauncher()
    {
        horizontalInput = Input.GetAxis(horizontalAxisName);
        transform.Rotate(Vector3.back, turnSpeed * horizontalInput * Time.deltaTime);
    }

    private void InitCurBall()
    {
        curBall = Instantiate(ballPrefabs[Random.Range(0, ballPrefabs.Length)], ballPlace.transform.position, Quaternion.identity, transform);
        curBall.Initialize(BallController.BallState.IN_LAUNCHER);
    }
}
