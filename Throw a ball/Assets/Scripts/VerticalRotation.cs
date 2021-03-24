using UnityEngine;

public class VerticalRotation : MonoBehaviour
{
    [SerializeField] float turnSpeed;

    private float verticalInput;

    private void FixedUpdate()
    {
        verticalInput = Input.GetAxis("Vertical");
        transform.Rotate(Vector3.right, turnSpeed * verticalInput * Time.deltaTime);
    }
}
