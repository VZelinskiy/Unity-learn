using UnityEngine;

public class Cube : MonoBehaviour
{
    public CubePosition cubePosition;
    public CubeScale cubeScale;
    public CubeRotate cubeRotate;
    public CubeColor cubeColor;

    void Start()
    {
        transform.position = new Vector3(3, 4, 1);
        transform.localScale = Vector3.one * 1.3f;

        cubeColor.InvokeRepeating("ChangeCubeColor", 0, 10);
    }
    
    void Update()
    {
        //cube rotate
        cubeRotate.ChangeCubeRotation();

        //cube position
        cubePosition.ChangeCubePosition();

        //cube scale
        cubeScale.ChangeCubeScale();
    }
}
