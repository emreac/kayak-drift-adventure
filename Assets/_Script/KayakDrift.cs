using System.Collections;
using UnityEngine;

public class KayakDrift : MonoBehaviour
{

    public AudioSource oarSoundL;
    public AudioSource oarSoundR;
    public AudioSource winSound;
    

    public HealthSystem healthSystem;
    public GameObject confetti;
    [SerializeField] private GameObject winUI;
    [SerializeField] private Animator animator; // Reference to the Animator
    [SerializeField] private TrailRenderer leftTrail;
    [SerializeField] private TrailRenderer rightTrail;

    // Settings
    public float moveSpeed = 2f;           // Acceleration force
    public float maxSpeed = 7f;            // Maximum speed
    public float drag = 0.98f;              // Drag to slow down lateral movement
    public float steerAngle = 20f;          // Maximum steering angle
    public float rotationSpeed = 100f;      // Speed of rotation when steering
    public float traction = 1f;             // Traction controls the kayak's alignment with its forward direction
    public float maxTiltAngle = 20f;        // Maximum tilt angle on the Z-axis
    public float tiltSpeed = 5f;            // Speed at which the kayak tilts

    // Variables
    private Vector3 moveForce;              // The force that moves the kayak forward
    private float currentTilt = 0f;         // Current tilt angle on the Z-axis

    // Reference to the 3D model that will tilt
    public GameObject kayakModel;

    void Start()
    {
     

        moveSpeed = 2f;
        // Ensure both animations are off at the start (idle state)
        animator.SetBool("isLeft", false);
        animator.SetBool("isRight", false);

        // Disable trail renderers at the start
        if (leftTrail != null) leftTrail.emitting = false;
        if (rightTrail != null) rightTrail.emitting = false;
    }

    void Update()
    {
        HandleSteering();
        MoveKayak();
        ApplyDrag();
        ApplyTraction();
        ApplyTilt();
    }

    void HandleSteering()
    {
        if (Input.GetMouseButton(0)) // Left mouse button held down
        {
            // Determine if the mouse is on the left or right side of the screen
            float steerInput = Input.mousePosition.x < Screen.width / 2 ? -1f : 1f;

            if (steerInput < 0)
            {
                // When steering left
                oarSoundL.Play();
                animator.SetBool("isLeft", true);
                animator.SetBool("isRight", false); // Ensure isRight is false

                // Disable trail renderers at the start
                if (leftTrail != null) leftTrail.emitting = true;
                if (rightTrail != null) rightTrail.emitting = false;
            }
            else if (steerInput > 0)
            {
                // When steering right
                oarSoundR.Play();
                animator.SetBool("isLeft", false);  // Ensure isLeft is false
                animator.SetBool("isRight", true);

                // Enable right trail, disable left trail
                if (leftTrail != null) leftTrail.emitting = false;
                if (rightTrail != null) rightTrail.emitting = true;
            }

            // Rotate the kayak based on the steer input
            transform.Rotate(Vector3.up * steerInput * moveForce.magnitude * steerAngle * rotationSpeed * Time.deltaTime);

            // Set the target tilt based on the steering direction
            currentTilt = Mathf.Lerp(currentTilt, steerInput * maxTiltAngle, tiltSpeed * Time.deltaTime);
        }
        else
        {
         
            // Reset both isLeft and isRight when no steering input is detected
            animator.SetBool("isLeft", false);
            animator.SetBool("isRight", false);

            // Disable both trail renderers when not steering
            if (leftTrail != null) leftTrail.emitting = false;
            if (rightTrail != null) rightTrail.emitting = false;

            // Smoothly return the tilt to zero when not steering
            currentTilt = Mathf.Lerp(currentTilt, 0f, tiltSpeed * Time.deltaTime);
        }
    }

    void MoveKayak()
    {
        // Apply the forward movement force based on the acceleration and input
        moveForce += transform.forward * moveSpeed * Time.deltaTime;

        // Clamp the force to the max speed
        moveForce = Vector3.ClampMagnitude(moveForce, maxSpeed);

        // Move the kayak based on the accumulated force
        transform.position += moveForce * Time.deltaTime;
    }

    void ApplyDrag()
    {
        // Apply drag to slow down lateral movement
        moveForce *= drag;
    }

    void ApplyTraction()
    {
        // Adjust the force direction to align with the kayak's forward direction
        moveForce = Vector3.Lerp(moveForce.normalized, transform.forward, traction * Time.deltaTime) * moveForce.magnitude;
    }

    void ApplyTilt()
    {
        if (kayakModel != null)
        {
            // Apply the tilt to the 3D model's local Z-axis
            kayakModel.transform.localRotation = Quaternion.Euler(kayakModel.transform.localRotation.eulerAngles.x, kayakModel.transform.localRotation.eulerAngles.y, currentTilt);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            winSound.Play();
            Debug.Log("You Win!");
            StartCoroutine(WinScreenLoader());
            confetti.SetActive(true);
            moveSpeed = 0f;
        }

    }

    IEnumerator WinScreenLoader()
    {
        yield return new WaitForSeconds(0.5f);
        winUI.SetActive(true);
    }

    // Method to play the sound while holding
 

    // Method to stop the sound when releasing
  
}
