using UnityEngine;

public class OrbitAround : MonoBehaviour
{
    public Transform centerPoint; // The point to orbit around
    public float orbitSpeed = 20.0f; // Orbit speed in degrees per second
    
    void Update()
    {
        // Rotate the object around the centerPoint
        transform.RotateAround(centerPoint.position, Vector3.up, orbitSpeed * Time.deltaTime);
    }
}
