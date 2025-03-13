using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class pauseMenu : MonoBehaviour
{
   public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    public Volume postProcessingVolume; 

    private DepthOfField depthOfField;
    private bool isPaused = false;

    void Start()
    {
        if (postProcessingVolume != null)
        {
            
            postProcessingVolume.profile.TryGet<DepthOfField>(out depthOfField);
        }
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
       pauseMenuUI.SetActive(true);
       Time.timeScale = 0f;
       GameIsPaused = true;
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        // Enable or disable the pause UI here as needed.

        if (depthOfField != null)
        {
            // When paused, enable the depth of field to add a blur effect.
            // When unpaused, disable it.
            depthOfField.active = isPaused;
        }

        // Pause or resume the game.
        Time.timeScale = isPaused ? 0f : 1f;
    }
}
