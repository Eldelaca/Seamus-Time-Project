using UnityEngine;

public class DeathMenuUI : MonoBehaviour
{
    public GameObject deathScreenUI;
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

            deathScreenUI.SetActive(true);
            Time.timeScale = 0f;
            isDeathScreenActive = true;
        }
        else
        {
            Debug.LogError("Death Screen UI is not assigned!");
        }
    }

    

}
