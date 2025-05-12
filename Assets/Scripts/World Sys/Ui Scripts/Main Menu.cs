using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Code use for the Main Menu Only
/// Starts the game loading by the scene name
/// Exits the game
/// and toggles the settings
/// </summary>

public class MainMenu : MonoBehaviour
{
    public GameObject settingsPanel;

    public void StartGame()
    {
        SceneManager.LoadScene("GameLevel");
    }

    public void ExitGame()
    {
        print("Exit Game");
        Application.Quit();

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }


    // Settings but we dont have the ui for controls delete
    // PLEASE DELETE AFTER IF IT COMES IN 
    // ACTUALLY DELETE THE RETURN FOR THE IF STATEMENTS AS WELL
    public void OpenSettings()
    {
        if (settingsPanel != null) return;
            settingsPanel.SetActive(!settingsPanel.activeSelf); 
    }

    public void CloseSettings()
    {
        if (settingsPanel != null) return;
            settingsPanel.SetActive(false);
    }
}
