using UnityEngine;

// Put this on every target. Change Max Hit Points and Score Value in the Inspector
// to make Standard / Tough / Fast Score variants.
public class Target : MonoBehaviour
{
    [SerializeField] int maxHitPoints = 1;      // how many hits it takes
    [SerializeField] int scoreValue = 100;      // points for destroying it
    [SerializeField] GameObject explosionPrefab; // your explosion particle prefab

    int hitPoints;              // current hit points (counts down)
    ScoreManager scoreManager;  // reference to the scoreboard script

    void Awake()
    {
        // Find the one ScoreManager in the scene so we don't have to drag it onto every target.
        scoreManager = FindFirstObjectByType<ScoreManager>();
    }

    // OnEnable runs every time the target is turned on,
    // including when RangeManager resets it. So it comes back at full health.
    void OnEnable()
    {
        hitPoints = maxHitPoints;
    }

    // "int damage" is the PARAMETER.
    // When Bullet calls TakeHit(1), the 1 is the ARGUMENT.
    public void TakeHit(int damage)
    {
        hitPoints -= damage;

        if (hitPoints <= 0)
        {
            // Add score: scoreManager = object reference, AddScore = method, scoreValue = argument
            if (scoreManager != null)
            {
                scoreManager.AddScore(scoreValue);
            }

            // Play the explosion
            if (explosionPrefab != null)
            {
                GameObject fx = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                Destroy(fx, 2f); // clean up the explosion after 2 seconds
            }

            // Disable instead of destroy, so R can bring it back
            gameObject.SetActive(false);
        }
    }
}
