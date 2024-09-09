using UnityEngine;

public class CircularMovement : MonoBehaviour
{
    public float radius = 5f;        // Radius of the circle
    public float speed = 2f;         // Speed of the movement
    public float yOffset = 2f;       // Y position offset
    public Vector3 circleCenter = Vector3.zero; // Center position of the circle
    public float startAngle = 0f;    // Starting angle of the movement

    private float angle;             // Current angle of the object on the circle

    void Start()
    {
        // Set the initial angle based on the starting angle
        angle = startAngle * Mathf.Deg2Rad;  // Convert to radians if starting angle is in degrees

        // Set the initial position of the object based on the circle center and starting angle
        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;
        float y = Mathf.Sin(angle) * yOffset;  // Optional: Creates an up-and-down motion

        transform.position = circleCenter + new Vector3(x, y, z);
    }

    void Update()
    {
        // Increment the angle based on the speed
        angle += (speed / 2) * Time.deltaTime;

        // Calculate the new position using sine and cosine functions
        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;
        float y = Mathf.Sin(angle) * yOffset;  // Optional: Creates an up-and-down motion

        Vector3 newPosition = circleCenter + new Vector3(x, y, z);

        // Calculate the next position using a slightly increased angle
        float nextAngle = angle + 0.1f;  // Adjust 0.1f as needed for smooth looking
        float nextX = Mathf.Cos(nextAngle) * radius;
        float nextZ = Mathf.Sin(nextAngle) * radius;
        float nextY = Mathf.Sin(nextAngle) * yOffset;

        Vector3 nextPosition = circleCenter + new Vector3(nextX, nextY, nextZ);

        // Update the position of the GameObject
        transform.position = newPosition;

        // Calculate the direction to look at
        Vector3 direction = nextPosition - newPosition;
        direction.y = 0; // Keep the y component zero to maintain horizontal orientation

        // Update the rotation of the GameObject to look in the direction of the movement
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
