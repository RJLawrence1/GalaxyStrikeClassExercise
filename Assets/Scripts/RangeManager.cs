using UnityEngine;
using UnityEngine.InputSystem;

// Put this on an empty GameObject called "RangeManager".
// Drag every target into the Targets array in the Inspector.
public class RangeManager : MonoBehaviour
{
    [SerializeField] GameObject[] targets;

    void Update()
    {
        // Press R to reset
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            ResetTargets();
        }
    }

    void ResetTargets()
    {
        // For each GameObject in the targets array, turn it back on.
        foreach (GameObject target in targets)
        {
            target.SetActive(true);
        }
    }
}
