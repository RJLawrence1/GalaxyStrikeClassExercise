using UnityEngine;

// Put this on your bullet PREFAB.
// The bullet needs: a Collider with "Is Trigger" checked,
// and a Rigidbody with "Is Kinematic" checked and "Use Gravity" off.
public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 30f;
    [SerializeField] float lifetime = 2f;
    [SerializeField] int damage = 1;

    void Start()
    {
        // Remove the bullet after a delay so they don't pile up forever
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move forward (the direction the turret was facing when it fired)
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // Did we hit something with a Target script?
        Target target = other.GetComponent<Target>();

        if (target != null)
        {
            target.TakeHit(damage); // damage is the argument
            Destroy(gameObject);    // bullet is used up
        }
    }
}
