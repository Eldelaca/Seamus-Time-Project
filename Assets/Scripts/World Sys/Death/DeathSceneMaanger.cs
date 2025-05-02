using UnityEngine;

public class DeathMenuUI : MonoBehaviour
{
    public GameObject deathScreenUI;
    public GameObject pauseMenuUI;
    private bool isDeathScreenActive = false;

    private void Start()
    {
        if (deathScreenUI != null)
        {
            deathScreenUI.SetActive(false);
        }
    }

    public void TriggerDeathScreen()
    {
        if (deathScreenUI != null)
        {
            if (pauseMenuUI != null)
                pauseMenuUI.SetActive(false);

            deathScreenUI.SetActive(true);
            Time.timeScale = 0f;
            isDeathScreenActive = true;
        }
        else
        {
            Debug.LogError("Death Screen UI is not assigned!");
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        CheckpointManager.Instance.RestartGame();
    }

    public void RestartFromCheckpoint()
    {
        CheckpointManager.Instance.RestartFromCheckpoint();
        CloseDeathScreen();
    }

    private void CloseDeathScreen()
    {
        if (deathScreenUI != null)
        {
            deathScreenUI.SetActive(false);
            Time.timeScale = 1f;
            isDeathScreenActive = false;
        }
        else
        {
            Debug.LogError("Death Screen UI is not assigned!");
        }
    }

    public void ExitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
