using UnityEngine;
using UnityEngine.InputSystem;

// Put this on your turret. It aims at the mouse and fires bullets on left click.
public class TurretShooter : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;  // drag your Bullet prefab here
    [SerializeField] Transform firePoint;      // empty child object at the tip of the barrel
    [SerializeField] float aimDistance = 20f;  // how far in front of the camera the targets are

    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        AimAtMouse();

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Fire();
        }
    }

    void AimAtMouse()
    {
        // Mouse position on the screen (pixels), pushed aimDistance units into the world
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 screenPoint = new Vector3(mousePos.x, mousePos.y, aimDistance);

        // ScreenToWorldPoint: turn that screen spot into a point in the 3D world
        Vector3 worldPoint = cam.ScreenToWorldPoint(screenPoint);

        // LookRotation: face from the turret toward that point
        Vector3 direction = worldPoint - transform.position;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    void Fire()
    {
        // Instantiate: spawn a bullet at the barrel tip, facing the way the turret faces
        Instantiate(bulletPrefab, firePoint.position, transform.rotation);
    }
}
