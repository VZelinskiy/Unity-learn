using UnityEngine;

public class CubeRotate : MonoBehaviour
{
    private float changeRotationTime = 3;
    private float changeRotationTimer = 4;
    private float minRotationSpeed = -100;
    private float maxRotationSpeed = 100;
    private Vector3 currentRotation;

    public void ChangeCubeRotation()
    {
        if (changeRotationTimer > changeRotationTime)
        {
            currentRotation = new Vector3(Random.Range(minRotationSpeed, maxRotationSpeed), Random.Range(minRotationSpeed, maxRotationSpeed), Random.Range(minRotationSpeed, maxRotationSpeed));
            changeRotationTimer = 0;
        }
        
        transform.Rotate(currentRotation * Time.deltaTime);
        changeRotationTimer += Time.deltaTime;
    }
}
