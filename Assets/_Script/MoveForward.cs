using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed at which the object moves forward

    void Update()
    {
        // Move the object forward in the Z direction
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
}
