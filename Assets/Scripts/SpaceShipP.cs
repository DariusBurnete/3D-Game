using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class SpaceShipP : MonoBehaviour
{
    private Transform planetCenter;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 50f;
    [SerializeField] private float acceleration = 25f;
    [SerializeField] private float turnSpeed = 0.05f;
    [SerializeField] private float alignSpeed = 10f;

    [Header("Hover")]
    [SerializeField] private float startHoverHeight = 6.5f;
    [SerializeField] private float minHoverHeight = 4.5f;
    [SerializeField] private float maxHoverHeight = 12f;
    [SerializeField] private float hoverForce = 45f;
    [SerializeField] private float hoverDamping = 10f;
    [SerializeField] private float ascendSpeed = 4f;

    [Header("Ground Check")]
    [SerializeField] private float groundCheckDistance = 20f;
    [SerializeField] private LayerMask groundMask = ~0;

    private Rigidbody rb;

    private Vector2 moveInput;
    private float mouseXInput;
    private float ascendInput;

    private bool hasGroundBelow;
    private float currentGroundDistance;
    private Vector3 currentGroundNormal = Vector3.up;
    private float targetHoverHeight;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (planetCenter == null)
        {
            GameObject planet = GameObject.FindGameObjectWithTag("TargetSurface");
            if (planet != null)
                planetCenter = planet.transform;
        }

        targetHoverHeight = Mathf.Clamp(startHoverHeight, minHoverHeight, maxHoverHeight);
    }

    private void FixedUpdate()
    {
        Vector3 localUp = GetLocalUp();

        UpdateGroundInfo(localUp);
        UpdateTargetHoverHeight();
        AlignToSurface(localUp);
        Turn(localUp);
        Move(localUp);
        Hover(localUp);
    }

    private Vector3 GetLocalUp()
    {
        if (planetCenter == null)
            return transform.up;

        return (transform.position - planetCenter.position).normalized;
    }

    private void UpdateGroundInfo(Vector3 localUp)
    {
        if (Physics.Raycast(
            transform.position,
            -localUp,
            out RaycastHit hit,
            groundCheckDistance,
            groundMask,
            QueryTriggerInteraction.Ignore))
        {
            hasGroundBelow = true;
            currentGroundDistance = hit.distance;
            currentGroundNormal = hit.normal;
        }
        else
        {
            hasGroundBelow = false;
            currentGroundDistance = groundCheckDistance;
            currentGroundNormal = localUp;
        }
    }

    private void UpdateTargetHoverHeight()
    {
        if (Mathf.Abs(ascendInput) < 0.01f)
            return;

        targetHoverHeight += ascendInput * ascendSpeed * Time.fixedDeltaTime;
        targetHoverHeight = Mathf.Clamp(targetHoverHeight, minHoverHeight, maxHoverHeight);
    }

    private void AlignToSurface(Vector3 localUp)
    {
        Vector3 targetUp = hasGroundBelow ? currentGroundNormal : localUp;

        Vector3 forwardOnSurface = Vector3.ProjectOnPlane(transform.forward, targetUp);
        if (forwardOnSurface.sqrMagnitude < 0.001f)
            forwardOnSurface = Vector3.ProjectOnPlane(transform.right, targetUp);

        forwardOnSurface.Normalize();

        Quaternion targetRotation = Quaternion.LookRotation(forwardOnSurface, targetUp);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, alignSpeed * Time.fixedDeltaTime));
    }

    private void Turn(Vector3 localUp)
    {
        if (Mathf.Abs(mouseXInput) < 0.001f)
            return;

        Quaternion yaw = Quaternion.AngleAxis(mouseXInput * turnSpeed, localUp);
        rb.MoveRotation(yaw * rb.rotation);

        mouseXInput = 0f;
    }

    private void Move(Vector3 localUp)
    {
        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, localUp).normalized;
        Vector3 right = Vector3.ProjectOnPlane(transform.right, localUp).normalized;

        Vector3 desiredMove = (forward * moveInput.y) + (right * moveInput.x);

        if (desiredMove.sqrMagnitude > 1f)
            desiredMove.Normalize();

        desiredMove *= moveSpeed;

        Vector3 currentSurfaceVelocity = Vector3.ProjectOnPlane(rb.linearVelocity, localUp);
        Vector3 velocityDelta = desiredMove - currentSurfaceVelocity;
        Vector3 accel = Vector3.ClampMagnitude(velocityDelta / Time.fixedDeltaTime, acceleration);

        rb.AddForce(accel, ForceMode.Acceleration);
    }

    private void Hover(Vector3 localUp)
    {
        if (!hasGroundBelow) return;

        float heightError = targetHoverHeight - currentGroundDistance;
        float verticalVelocity = Vector3.Dot(rb.linearVelocity, localUp);
        float lift = (heightError * hoverForce) - (verticalVelocity * hoverDamping);

        rb.AddForce(localUp * lift, ForceMode.Acceleration);
    }

    public void ResetInput()
    {
        moveInput = Vector2.zero;
        mouseXInput = 0f;
        ascendInput = 0f;
    }

    public void SnapToCurrentHoverHeight()
    {
        Vector3 localUp = GetLocalUp();
        UpdateGroundInfo(localUp);

        if (hasGroundBelow)
            targetHoverHeight = Mathf.Clamp(currentGroundDistance, minHoverHeight, maxHoverHeight);
        else
            targetHoverHeight = Mathf.Clamp(startHoverHeight, minHoverHeight, maxHoverHeight);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseXInput += context.ReadValue<Vector2>().x;
    }

    public void OnAscend(InputAction.CallbackContext context)
    {
        ascendInput = context.ReadValue<float>();
    }
}