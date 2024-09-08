using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SurfaceEffector3D : MonoBehaviour
{
    public Vector3 forceDirection = Vector3.forward; // Direction of the force applied
    public float forceMagnitude = 10f;               // Magnitude of the force
    public bool applyTorque = false;                 // Should the effector apply torque (rotation)?
    public float torqueMagnitude = 10f;              // Magnitude of the torque
    public float directionChangeSpeed = 2f;          // Speed of directional change for smooth transition

    private Vector3 currentForceDirection;           // Current interpolated force direction

    void Start()
    {
        currentForceDirection = forceDirection;      // Initialize the current force direction
    }

    private void OnCollisionStay(Collision collision)
    {
        // Get the Rigidbody of the colliding object
        Rigidbody rb = collision.rigidbody;

        if (rb != null)
        {
            // Smoothly transition the force direction to the new direction
            currentForceDirection = Vector3.Lerp(currentForceDirection, forceDirection, Time.deltaTime * directionChangeSpeed);

            // Apply force in the smoothly transitioned direction
            Vector3 force = currentForceDirection.normalized * forceMagnitude;
            rb.AddForce(force, ForceMode.Force);

            // Optionally, apply torque to rotate the object
            if (applyTorque)
            {
                Vector3 torque = transform.up * torqueMagnitude;
                rb.AddTorque(torque, ForceMode.Force);
            }
        }
    }

    // Optional: Visualize the force direction in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + currentForceDirection.normalized * 2f);
        Gizmos.DrawSphere(transform.position + currentForceDirection.normalized * 2f, 0.1f);
    }
}
