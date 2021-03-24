using UnityEngine;

public class HorizontalRotation : MonoBehaviour
{
    [SerializeField] float turnSpeed;

    private float horizontalInput;

    private void FixedUpdate()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, turnSpeed * horizontalInput * Time.deltaTime);
    }
}
