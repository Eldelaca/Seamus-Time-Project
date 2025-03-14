using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;

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

    public void OnEnter_Menu()
    {
        player_Map.Disable();
        ui_Map.Enable();
        Pause();
    }

    public void OnExit_Menu()
    {
        player_Map.Enable();
        ui_Map.Disable();
        Resume();
    }
    
    void Resume()
    {
        print("resume");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        print("pause");
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
