using UnityEngine;

public class CubeColor : MonoBehaviour
{
    public MeshRenderer Renderer;

    private Material material;

    private void Start()
    {
        material = Renderer.material;
    }

    public void ChangeCubeColor()
    {
        material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }
}
