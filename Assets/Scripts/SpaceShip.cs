using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(StopCollision))]
public class SpaceShip : MonoBehaviour
{

    [Header("SpaceShip Movement Settings")]

    [SerializeField]
    private float swerveTorque = 7f;
    [SerializeField]
    private float pitchTorque = 5f;
    [SerializeField]
    private float rollTorque = 4f;
    [SerializeField]
    private float thrust = 20f;
    [SerializeField]
    private float upThrust = 20f;
    [SerializeField]
    private float strafeThrust = 20f;

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

    StopCollision stopCollision;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        stopCollision = GetComponent<StopCollision>();
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

        Vector3 thrustDir = transform.forward;
        if (stopCollision != null && stopCollision.inContact)
        {
            if (Vector3.Dot(thrustDir, -stopCollision.surfaceNormal) > 0f) return;
        }

        if (Mathf.Abs(thrust1D) > 0.1f)
            rb.AddRelativeForce(thrust1D * thrust * Vector3.forward);
        else
            LinearMovementDamp(thrustGlideReduction, 0, 0, 1);

        if (Mathf.Abs(upThrust1D) > 0.1f)
            rb.AddRelativeForce(upThrust1D * upThrust * Vector3.up);
        else
            LinearMovementDamp(upDownGlideReduction, 0, 1, 0);

        if (Mathf.Abs(strafe1D) > 0.1f)
            rb.AddRelativeForce(strafe1D * strafeThrust * Vector3.right);
        else
            LinearMovementDamp(leftRightGlideReduction, 1, 0, 0);
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
