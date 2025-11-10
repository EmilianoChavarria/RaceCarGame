using UnityEngine;

public class FollowCar : MonoBehaviour
{
    public Transform target; // el coche
    public Vector3 offset = new Vector3(0f, 5f, -10f); // distancia y altura desde el coche
    public float smoothSpeed = 0.125f; // suavizado del movimiento

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + target.TransformDirection(offset);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(target); // siempre mira al coche
    }
}
