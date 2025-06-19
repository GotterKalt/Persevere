using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;     // taikinys - žaidėjas
    [SerializeField] private Vector3 offset;       // atsitraukimas toliau nuo žaidėjo
    [SerializeField] private float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        desiredPosition.z = transform.position.z; // kamera išlieka viename gylije

        // švelniai sekia
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}

