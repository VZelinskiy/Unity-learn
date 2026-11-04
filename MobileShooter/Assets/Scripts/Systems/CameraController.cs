using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform target;

    [Header("Camera Positioning")]
    [SerializeField] private float distance = 8f;

    [SerializeField] private float height = 6f;
    [SerializeField] private float pitch = 45f;
    [SerializeField] private float smoothSpeed = 10f;

    private void LateUpdate()
    {
        if (target == null) return;

        // обчислюємо бажаний кут нахилу камери
        Quaternion targetRotation = Quaternion.Euler(pitch, 0f, 0f);

        // обчислюємо позицію: беремо позицію гравця, піднімаємо вгору і зміщуємо назад за рахунок кута та дистанції
        Vector3 targetPosition = target.position + Vector3.up * height - (targetRotation * Vector3.forward *  distance);

        // плавно переміщуємо та повертаємо камеру
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
