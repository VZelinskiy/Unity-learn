using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public Vector3 ballDirection;

    [SerializeField] float ballSpeed;

    // Update is called once per frame
    void Update()
    {
        MoveBall();
    }

    private void MoveBall()
    {
        transform.Translate(ballDirection * Time.deltaTime * ballSpeed);
    }
}
