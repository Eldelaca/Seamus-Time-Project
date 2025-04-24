using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    public DeathMenuUI deathUi;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Health health = other.GetComponent<Health>();
            Restart playerReset = other.GetComponent<Restart>();

            if (health != null && playerReset != null)
            {
                health.TakeDamage(1f); // Apply damage

                if (health.GetCurrentHealth() > 0)
                {
                    playerReset.ResetPosition();
                }
                else
                {
                    deathUi.TriggerDeathScreen();
                }
            }
        }
    }
}
