using UnityEngine;
using UnityEngine.SceneManagement;

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
            DontDestroyOnLoad(gameObject);
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

    public void SetCheckpoint(Vector3 position, float health)
    {
        lastCheckpointPosition = position;
        savedHealth = health;
        hasCheckpoint = true;
    }

    public float GetSavedHealth() 
    {
        return savedHealth;
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

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
