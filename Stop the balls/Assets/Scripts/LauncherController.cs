using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherController : MonoBehaviour
{
    [SerializeField] GameObject ballPlace;
    [SerializeField] BallController ballPrefab;
    [SerializeField] float turnSpeed;

    //private Vector3 launcherRotation;
    private float horizontalInput;

    private readonly string launchButtonName = "Jump";
    private readonly string horizontalAxisName = "Horizontal";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton(launchButtonName))
        {
            LaunchBall();
        }

        RotateLauncher();
    }

    private void LaunchBall()
    {
        BallController ball = Instantiate(ballPrefab, ballPlace.transform.position, Quaternion.identity);
        ball.ballDirection = gameObject.transform.up;
    }

    private void RotateLauncher()
    {
        horizontalInput = Input.GetAxis(horizontalAxisName);
        transform.Rotate(Vector3.back, turnSpeed * horizontalInput * Time.deltaTime);
    }
}
