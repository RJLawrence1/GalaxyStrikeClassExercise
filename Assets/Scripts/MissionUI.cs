using System.Collections;
using TMPro;
using UnityEngine;

// Put this on an empty GameObject called "MissionUI".
// Timeline Signals call ShowWaveWarning() / ShowFinalWave().
public class MissionUI : MonoBehaviour
{
    [SerializeField] GameObject messagePanel; // the panel that holds the message
    [SerializeField] TMP_Text messageText;    // the text inside the panel
    [SerializeField] float displayTime = 2f;  // seconds before it hides

    Coroutine hideRoutine; // remembers the running "hide" timer

    void Start()
    {
        // Start hidden
        messagePanel.SetActive(false);
    }

    public void ShowWaveWarning()
    {
        ShowMessage("TOUGH TARGETS INCOMING");
    }

    public void ShowFinalWave()
    {
        ShowMessage("FINAL WAVE");
    }

    void ShowMessage(string message)
    {
        messageText.text = message;
        messagePanel.SetActive(true);

        // If an old timer is still running, cancel it so the new message gets its full time
        if (hideRoutine != null)
        {
            StopCoroutine(hideRoutine);
        }

        hideRoutine = StartCoroutine(HideMessageAfterDelay());
    }

    // Coroutine: a method that can pause partway through and continue later
    IEnumerator HideMessageAfterDelay()
    {
        yield return new WaitForSeconds(displayTime); // wait, without freezing the game
        messagePanel.SetActive(false);
        hideRoutine = null;
    }
}
