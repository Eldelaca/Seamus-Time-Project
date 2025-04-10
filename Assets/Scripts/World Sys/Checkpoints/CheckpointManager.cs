using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;
    private Vector3 lastCheckpointPosition;
    private bool hasCheckpoint = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Preserve between scene reloads
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Unsubscribe when this object is destroyed
    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    // This method is called each time a new scene is loaded.
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (hasCheckpoint)
        {
            // Attempt to find the player by tag
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = lastCheckpointPosition;
            }
        }
    }

    // Save a new checkpoint position.
    public void SetCheckpoint(Vector3 position)
    {
        lastCheckpointPosition = position;
        hasCheckpoint = true;
    }

    // Retrieve the last checkpoint position.
    public Vector3 GetCheckpointPosition()
    {
        return lastCheckpointPosition;
    }

    // Boolean check for a valid checkpoint.
    public bool HasCheckpoint()
    {
        return hasCheckpoint;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Resume normal time
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Restart from the last checkpoint
    public void RestartFromCheckpoint()
    {
        if (hasCheckpoint)
        {
            // Retrieve the last checkpoint position
            Vector3 lastCheckpoint = GetCheckpointPosition();

            // Move the player to the last checkpoint
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = lastCheckpoint;
            }
        }
        else
        {
            Debug.LogError("No checkpoint set. Restarting the game instead.");
            RestartGame();
        }
    }
}
