using UnityEngine;

public class DeathMenuUI : MonoBehaviour
{
    public GameObject deathScreenUI;  // Reference to the death screen UI
    public GameObject pauseMenuUI;    // Reference to the pause menu UI (to disable it during death)
    private bool isDeathScreenActive = false;

    private void Start()
    {
        // Ensure the death screen UI is hidden at the beginning
        if (deathScreenUI != null)
        {
            deathScreenUI.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only trigger the death screen if the object is the player
        if (other.CompareTag("Player"))
        {
            TriggerDeathScreen();
        }
    }

    // This function is triggered when the player collides and shows the death screen
    public void TriggerDeathScreen()
    {
        if (deathScreenUI != null)
        {
            // Disable the pause menu UI if it's active
            if (pauseMenuUI != null)
            {
                pauseMenuUI.SetActive(false);
            }

            // Show death screen
            deathScreenUI.SetActive(true);
            Time.timeScale = 0f;  // Pause the game
            isDeathScreenActive = true;
        }
        else
        {
            Debug.LogError("Death Screen UI is not assigned!");
        }
    }

    // Restart the game by reloading the current scene
    public void RestartGame()
    {
        Time.timeScale = 1f; // Resume normal time
        CheckpointManager.Instance.RestartGame();
    }

    // Restart from the last checkpoint
    public void RestartFromCheckpoint()
    {
        CheckpointManager.Instance.RestartFromCheckpoint();
        CloseDeathScreen();
    }

    // Close the death screen UI and resume the game
    private void CloseDeathScreen()
    {
        if (deathScreenUI != null)
        {
            deathScreenUI.SetActive(false);
            Time.timeScale = 1f; // Resume normal time
            isDeathScreenActive = false;
        }
        else
        {
            Debug.LogError("Death Screen UI is not assigned!");
        }
    }

    // Exit the game 
    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
