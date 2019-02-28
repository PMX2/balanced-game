using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
    private float _lookSensitivityX = 2f;
    [SerializeField]
    private float _lookSensitivityY = 3f;

    [SerializeField]
    private float thrusterForce = 1000f;

    [Header("Joint Options")]
    [SerializeField]
    private JointDriveMode jointMode = JointDriveMode.Position;

    [SerializeField]
    private float jointSpring = 12f;
    [SerializeField]
    private float jointMaxForce = 20f;

    private PlayerMotor motor;
    private ConfigurableJoint joint;

    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();
        SetJointSettings(jointSpring);
    }

    private void Update()
    {
        // Calcul de la vélocité du joueur en un vecteur 3D
        float MovX = Input.GetAxisRaw("Horizontal");
        float MovZ = Input.GetAxisRaw("Vertical");
        Vector3 HorizontalMove = transform.right * MovX;
        Vector3 VerticalMove = transform.forward * MovZ;
        Vector3 Velocity = (HorizontalMove + VerticalMove).normalized * _speed;
        motor.Move(Velocity);
        
        // Calcul de la rotation de la caméra en un vecteur 3D
        float RotY = Input.GetAxisRaw("Mouse X");
        Vector3 Rotation = new Vector3(0, RotY, 0) * _lookSensitivityX;
        motor.Rotate(Rotation);
        
        float RotX = Input.GetAxisRaw("Mouse Y");
        float CameraRotationX = RotX * _lookSensitivityY;
        motor.RotateCamera(CameraRotationX);

        Vector3 _thrusterForce = Vector3.zero;
        
        //Calcul de la force du jetpack
        if (Input.GetButton("Jump"))
        {
            _thrusterForce = Vector3.up * thrusterForce;
            SetJointSettings(0f);
        }
        else
        {
            SetJointSettings(jointSpring);
        }
        //Appliquer la variable trustherforce
        motor.ApplyThruster(_thrusterForce);
    }

    private void SetJointSettings(float _jointSpring)
    {
        joint.yDrive = new JointDrive {mode = jointMode, positionSpring = _jointSpring, maximumForce = jointMaxForce};
    }
}
