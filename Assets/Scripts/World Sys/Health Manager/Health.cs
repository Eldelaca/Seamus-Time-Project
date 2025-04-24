using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 3f;
    private float currentHealth;

    // References to your separate UI images
    public GameObject fullHealthUI;
    public GameObject midHealthUI;
    public GameObject lowHealthUI;
    public GameObject emptyHealthUI;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        Debug.Log("Health: " + currentHealth); // Debugging line
        UpdateHealthUI();
    }

    public void SetHealth(float newHealth)
    {
        currentHealth = Mathf.Clamp(newHealth, 0f, maxHealth);
        UpdateHealthUI();
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    private void UpdateHealthUI()
    {
        fullHealthUI.SetActive(false);
        midHealthUI.SetActive(false);
        lowHealthUI.SetActive(false);
        emptyHealthUI.SetActive(true);

        if (currentHealth >= 3f)
        {
            fullHealthUI.SetActive(true);
        }
        else if (currentHealth >= 2f)
        {
            midHealthUI.SetActive(true);
            lowHealthUI.SetActive(true);
        }
        else if (currentHealth >= 1f)
        {
            lowHealthUI.SetActive(true);
        }
        

    }
}
