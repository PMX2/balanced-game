using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    private Vector3 velocity;
    private Vector3 rotation;
    private float camerarotationX = 0f;
    private float currentCameraRotationX = 0f;
    [SerializeField]
    private float cameraRotationLimit = 85f;
    private Rigidbody rb;
    [SerializeField]
    private Camera cam;

    private Vector3 thrusterForce;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 Velocity)
    {
        velocity = Velocity;
    }
    
    public void Rotate(Vector3 Rotation)
    {
        rotation = Rotation;
    }
    
    public void RotateCamera(float CameraRotationX)
    {
        camerarotationX = CameraRotationX;
    }

    public void ApplyThruster(Vector3 _thrusterForce)
    {
        thrusterForce = _thrusterForce;
    }

    private void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }

    private void PerformMovement()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }

        if (thrusterForce != Vector3.zero)
        {
            rb.AddForce(thrusterForce * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
    }

    private void PerformRotation()
    {
        //Récupération de la rotation + on empeche la camera de touner en 360
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        currentCameraRotationX -= camerarotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
        cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }
}
 