using UnityEngine;

public class DeathMenuUI : MonoBehaviour
{
    public GameObject deathScreenUI;

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
        }
        else
        {
            Debug.LogError("Death Screen UI is not assigned!");
        }
    }

    

}
