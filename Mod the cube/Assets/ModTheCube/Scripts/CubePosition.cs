using UnityEngine;

public class CubePosition : MonoBehaviour
{
    private float posZRange = 10;
    private float posYRange = 10;
    private float directionChangeTime = 5;
    private float directionChangeTimer = 0;
    private float minMoveSpeed = 1f;
    private float maxMoveSpeed = 10;
    private float currentMoveSpeed;
    private Vector3 currentDirection;
    private Vector3[] directions;

    // Start is called before the first frame update
    void Start()
    {
        directions = new Vector3[4];
        directions[0] = Vector3.forward;
        directions[1] = Vector3.back;
        directions[2] = Vector3.up;
        directions[3] = Vector3.down;
    }

    public void ChangeCubePosition()
    {
        currentMoveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);

        if (directionChangeTimer > directionChangeTime)
        {
            currentDirection = directions[Random.Range(0, directions.Length)];
            directionChangeTimer = 0;
        }

        if (transform.position.z > posZRange)
        {
            currentDirection = Vector3.back;
        }

        if (transform.position.z < -posZRange)
        {
            currentDirection = Vector3.forward;
        }

        if (transform.position.y > posYRange)
        {
            currentDirection = Vector3.down;
        }

        if (transform.position.y < -posYRange)
        {
            currentDirection = Vector3.up;
        }

        transform.Translate(currentDirection * Time.deltaTime * currentMoveSpeed, Space.World);

        directionChangeTimer += Time.deltaTime;
    }
}
