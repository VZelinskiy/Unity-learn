using System;
using UnityEngine;

public class BillboardUI : MonoBehaviour
{
    private Transform mainCameraTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FindCamera();
    }

    private void OnEnable()
    {
        // на випадок, чкщо камера змінюється або обєкт береться з пулу
        if (mainCameraTransform == null)
        {
            FindCamera();
        }
    }

    private void FindCamera()
    {
        if (Camera.main != null)
        {
            mainCameraTransform = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning("BillboardUI: Main Camera not found on the scene!");
        }
    }

    private void LateUpdate()
    {
        if (mainCameraTransform == null) return;

        // повертаємо ui обличчям до камери
        transform.LookAt(transform.position + mainCameraTransform.rotation * Vector3.forward, mainCameraTransform.rotation * Vector3.up);
    }
}
