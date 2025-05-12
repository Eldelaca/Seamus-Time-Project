using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// This code ensures the saving and loading
/// Grabs health
/// Grabs player pos and
/// Checks if player reaches checkpoint player has the option to start from there.
/// </summary>

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;
    private Vector3 lastCheckpointPosition;
    private float savedHealth = 1f;
    private bool hasCheckpoint = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Makes sure it doesn't
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (hasCheckpoint)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = lastCheckpointPosition;
                Health health = player.GetComponent<Health>();
                if (health != null)
                {
                    health.SetHealth(savedHealth);
                }
            }
        }
    }

    // saving checkpoint
    public void SetCheckpoint(Vector3 position, float health)
    {
        lastCheckpointPosition = position;
        savedHealth = health;
        hasCheckpoint = true;
    }

    // saving Health Method
    public float GetSavedHealth() 
    {
        return savedHealth;
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Restart from last check method
    public void RestartFromCheckpoint()
    {
        if (hasCheckpoint)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = lastCheckpointPosition;
                Health health = player.GetComponent<Health>();
                if (health != null)
                {
                    health.SetHealth(savedHealth);
                }
            }
        }
        else
        {
            RestartGame();
        }
    }
}
