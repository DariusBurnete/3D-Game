using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class SpaceShip : MonoBehaviour
{

    [Header("SpaceShip Movement Settings")]

    [SerializeField]
    private float swerveTorque = 125f;
    [SerializeField]
    private float pitchTorque = 125f;
    [SerializeField]
    private float rollTorque = 125f;
    [SerializeField]
    private float thrust = 50f;
    [SerializeField]
    private float upThrust = 50f;
    [SerializeField]
    private float strafeThrust = 50f;

    [SerializeField, Range(0.001f, 0.999f)]
    private float thrustGlideReduction = 0.96f;
    [SerializeField, Range(0.001f, 0.999f)]
    private float upDownGlideReduction = 0.9f;
    [SerializeField, Range(0.001f, 0.999f)]
    private float leftRightGlideReduction = 0.9f;

    Rigidbody rb;

    private Vector2 pitchSwerve2D;
    private float roll1D;
    private float thrust1D;
    private float upThrust1D;
    private float strafe1D;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        MovementHandler();
    }

    void LinearMovementDamp(float glideReduction, int x, int y, int z)
    {
        Vector3 localVelocity = transform.InverseTransformDirection(rb.linearVelocity);
        if(z == 1) localVelocity.z *= glideReduction;
        if(y == 1) localVelocity.y *= glideReduction;
        if(x == 1) localVelocity.x *= glideReduction;
        rb.linearVelocity = transform.TransformDirection(localVelocity);
    }

    void MovementHandler()
    {
        rb.AddRelativeTorque(roll1D * rollTorque * Vector3.forward);
        rb.AddRelativeTorque(Mathf.Clamp(-pitchSwerve2D.y, -1f, 1f) * pitchTorque * Vector3.right);
        rb.AddRelativeTorque(Mathf.Clamp(pitchSwerve2D.x, -1f, 1f) * swerveTorque * Vector3.up);

        if(thrust1D > 0.1f || thrust1D < -0.1f)
        {
            float currentThrust = thrust; // Used later for boost mechanic

            rb.AddRelativeForce(thrust1D * currentThrust * Vector3.forward);
        }
        else
        {
            LinearMovementDamp(thrustGlideReduction, 0, 0, 1);
        }

        if (upThrust1D > 0.1f || upThrust1D < -0.1f)
        {
            rb.AddRelativeForce(upThrust1D * upThrust * Vector3.up);
        }
        else
        {
            LinearMovementDamp(upDownGlideReduction, 0, 1, 0);
        }

        if (strafe1D > 0.1f || strafe1D < -0.1f)
        {
            rb.AddRelativeForce(strafe1D * strafeThrust * Vector3.right);
        }
        else
        {
            LinearMovementDamp(leftRightGlideReduction, 1, 0, 0);
        }
    }

    #region Input Setter Methods
    public void OnPitchSwerve(InputAction.CallbackContext context)
    {
        pitchSwerve2D = context.ReadValue<Vector2>();
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
        roll1D = context.ReadValue<float>();
    }

    public void OnThrust(InputAction.CallbackContext context)
    {
        thrust1D = context.ReadValue<float>();
    }

    public void OnUpThrust(InputAction.CallbackContext context)
    {
        upThrust1D = context.ReadValue<float>();
    }

    public void OnStrafe(InputAction.CallbackContext context)
    {
        strafe1D = context.ReadValue<float>();
    }
    #endregion
}
