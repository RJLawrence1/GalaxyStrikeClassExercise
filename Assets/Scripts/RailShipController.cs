using UnityEngine;
using UnityEngine.InputSystem;

// Put this on your ship GameObject, the SAME object that has the PlayerInput component.
// PlayerInput (Behavior = Send Messages) automatically calls OnMove, OnLook, OnAttack, OnPush.
public class RailShipController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 10f;   // how fast the ship slides around
    [SerializeField] float xRange = 8f;       // how far left/right the ship can go
    [SerializeField] float yRange = 5f;       // how far up/down the ship can go

    [Header("Aiming")]
    [SerializeField] Transform aimPivot;      // drag your AimPivot child object here
    [SerializeField] float aimSpeed = 90f;    // degrees per second while holding an arrow key
    [SerializeField] float maxAimAngle = 35f; // aim can't go past +/- this many degrees
    [SerializeField] float aimSmoothing = 8f; // higher = snappier, lower = smoother/floatier

    [Header("Attack")]
    [SerializeField] ParticleSystem laserEffect; // drag your particle system here

    [Header("Push (optional dash)")]
    [SerializeField] float dashDistance = 3f;

    // Stored input values (we store the Vector2, not the InputValue itself)
    Vector2 moveInput;
    Vector2 lookInput;

    // Where we WANT to aim (target) vs. where we're aiming RIGHT NOW (current)
    float targetYaw;    // left/right angle
    float targetPitch;  // up/down angle
    float currentYaw;
    float currentPitch;

    // ---------- Input callbacks (called by PlayerInput) ----------

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    public void OnAttack(InputValue value)
    {
        if (!value.isPressed) return;

        if (laserEffect != null)
        {
            laserEffect.Play();
        }
    }

    public void OnPush(InputValue value)
    {
        if (!value.isPressed) return;

        // No direction held? Nothing to dash toward.
        if (moveInput == Vector2.zero) return;

        Vector3 pos = transform.localPosition;
        Vector2 dir = moveInput.normalized;
        pos.x += dir.x * dashDistance;
        pos.y += dir.y * dashDistance;
        transform.localPosition = ClampToFlightArea(pos);
    }

    // ---------- Every frame ----------

    void Update()
    {
        HandleMovement();
        HandleAiming();
    }

    void HandleMovement()
    {
        Vector3 pos = transform.localPosition;
        pos.x += moveInput.x * moveSpeed * Time.deltaTime;
        pos.y += moveInput.y * moveSpeed * Time.deltaTime;
        transform.localPosition = ClampToFlightArea(pos);
    }

    void HandleAiming()
    {
        if (aimPivot == null) return;

        // 1. Holding an arrow key keeps turning the target angle.
        targetYaw   += lookInput.x * aimSpeed * Time.deltaTime;
        targetPitch -= lookInput.y * aimSpeed * Time.deltaTime; // minus so Up arrow aims up

        // 2. Mathf.Clamp: don't let the target go past the allowed angles.
        targetYaw   = Mathf.Clamp(targetYaw,   -maxAimAngle, maxAimAngle);
        targetPitch = Mathf.Clamp(targetPitch, -maxAimAngle, maxAimAngle);

        // 3. Mathf.Lerp(start, target, t): move "current" part of the way toward "target" each frame.
        //    start = current angle, target = where we want to be, t = how much of the gap to close.
        float t = aimSmoothing * Time.deltaTime;
        currentYaw   = Mathf.Lerp(currentYaw,   targetYaw,   t);
        currentPitch = Mathf.Lerp(currentPitch, targetPitch, t);

        // 4. Quaternion.Euler: turn the angles (x = pitch, y = yaw, z = roll) into a rotation.
        aimPivot.localRotation = Quaternion.Euler(currentPitch, currentYaw, 0f);
    }

    // Shared by normal movement AND the dash, so neither can leave the flight area.
    Vector3 ClampToFlightArea(Vector3 pos)
    {
        pos.x = Mathf.Clamp(pos.x, -xRange, xRange);
        pos.y = Mathf.Clamp(pos.y, -yRange, yRange);
        return pos;
    }
}
