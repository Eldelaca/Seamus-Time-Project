using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;
using UnityEditor;

public class pauseMenu : MonoBehaviour
{
    [SerializeField] private InputActionAsset action_Asset;


    private InputActionMap player_Map;
    private InputActionMap ui_Map;
    
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    public Volume postProcessingVolume; 

    private DepthOfField depthOfField;
    private bool isPaused = false;

    public GameObject checkpointManager;
    public GameObject deathScreenUI;

    private void Awake()
    {
        player_Map = action_Asset.FindActionMap("Player");
        ui_Map = action_Asset.FindActionMap("UI");
    }

    void Start()
    {


        player_Map.Enable();
        ui_Map.Disable();


        if (postProcessingVolume != null)
        {
            
            postProcessingVolume.profile.TryGet<DepthOfField>(out depthOfField);
        }
    }

    // Stops Game
    public void OnPause()
    {
        player_Map.Disable();
        ui_Map.Enable();
        Pause();
    }

    // Resumes Game
    public void OnUnpause()
    {
        player_Map.Enable();
        ui_Map.Disable();
        Resume();
    }
    
    // Turn off Menu Ui
    public void Resume()
    {
        print("resume");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;

    }

    // Turns on Menu Ui
    void Pause()
    {
        print("pause");
       pauseMenuUI.SetActive(true);
       Time.timeScale = 0f;
       GameIsPaused = true;
    }

    public void ExitGame()
    {
        print("Exit Game");
        Application.Quit(); // ONLY WORKS WHEN GAME IS BUILT


        // Allows to stop the unity editor from playing
        EditorApplication.isPlaying = false;


    }

    public void RestartGame()
    {
        pauseMenuUI.SetActive(false);
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
        }
        else
        {
            Debug.LogError("Death Screen UI is not assigned!");
        }
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
