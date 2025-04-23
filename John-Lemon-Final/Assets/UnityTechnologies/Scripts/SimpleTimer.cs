using UnityEngine;
using UnityEngine.UI;                     // For using regular UI Text
using UnityEngine.SceneManagement;

public class SimpleTimer : MonoBehaviour
{
    public float timeLimit = 60f;         // Total time the player has to finish
    public Text timerText;                // Assign your UI Text in the Inspector
    public GameEnding gameEnding;         // Drag the GameEnding object here

    float currentTime;                    // Tracks time left
    bool timeUp = false;                  // Prevents triggering "caught" more than once

    void Start()
    {
        currentTime = timeLimit;          // Start with full time
    }

    void Update()
    {
        // ⛔ Stop if player has already won or been caught
        if (gameEnding.gameHasEnded || timeUp)
            return;

        currentTime -= Time.deltaTime;    // Count down every frame

        if (currentTime <= 0f)
        {
            currentTime = 0f;             // Clamp to 0 so it doesn't go negative
            timeUp = true;                // Mark time as up

            gameEnding.CaughtPlayer();    // Trigger "caught" sequence from GameEnding.cs
        }

        // Update the on-screen timer text
        timerText.text = "Time: " + Mathf.Ceil(currentTime).ToString(); // Shows time as whole number
    }
}
