using UnityEngine;

public class CubeScale : MonoBehaviour
{
    private float minScale = 0.5f;
    private float maxScale = 5;
    private float minScaleSpeed = 0.1f;
    private float maxScaleSpeed = 1f;
    private float currentScaleSpeed;
    private Vector3 currentDirection = Vector3.one;

    public void ChangeCubeScale()
    {
        currentScaleSpeed = Random.Range(minScaleSpeed, maxScaleSpeed);

        if ((transform.localScale.x > maxScale && currentDirection.x > 0) || (transform.localScale.x < minScale && currentDirection.x < 0))
        {
            currentDirection = -currentDirection;
        }

        transform.localScale += (currentDirection * Time.deltaTime * currentScaleSpeed);
    }
}
