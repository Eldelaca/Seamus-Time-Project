using UnityEngine;
using System.Collections;

/// <summary>
/// How this code works
/// Player Checks the bool if !_in_Present
/// Start the Coroutine and that starts a Countdown
/// If player exits before stop coroutine
/// If player doesn't and timer hits 0, Take Damage
/// </summary>

public class TimerDamage : MonoBehaviour
{
    [SerializeField] private Player_Timeline timeline;
    [SerializeField] private DeathMenuUI deathUi;

    [Header("Settings")]
    // This should in line with the TP_Timer
    [Tooltip("Connected to TP_Timer Script Time Value and Value should always be -0.01")]
    [SerializeField] private float countdownDuration; 
    [SerializeField] private float damageAmount = 1f;

    private Health playerHealth;
    private bool wasInPresent;
    private Coroutine countdownRoutine;

    private void Start()
    {
        // find timeline if not assigned
        if (timeline == null)
            timeline = UnityEngine.Object.FindFirstObjectByType<Player_Timeline>();

        if (timeline == null)
        {
            Debug.LogError("TimerDamage: no Player_Timeline found.");
            enabled = false;
            return;
        }

        // grab player Health
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerHealth = player.GetComponent<Health>();

        if (playerHealth == null)
        {
            Debug.LogError("TimerDamage: no Health on Player.");
            enabled = false;
            return;
        }

        wasInPresent = timeline.in_Present;
    }

    private void Update()
    {
        bool nowInPresent = timeline.in_Present;

        // Check if entered past?
        if (wasInPresent && !nowInPresent)
        {
            // If entered past start the countdown
            countdownRoutine = StartCoroutine(PastCountdown());
        }

        // Check if player exited past 
        if (!wasInPresent && nowInPresent)
        {
            // Stop coroutine
            if (countdownRoutine != null)
            {
                StopCoroutine(countdownRoutine);
                countdownRoutine = null;
            }
        }

        wasInPresent = nowInPresent;
    }

    private IEnumerator PastCountdown()
    {
        float time = 0f;
        while (time < countdownDuration)
        {
            time += Time.deltaTime;
            yield return null;

            // if Player Is back in Present stop 
            if (timeline.in_Present)
                yield break;
        }

        // when timer reaches 0 take damage
        playerHealth.TakeDamage(damageAmount);

        // if zero show deathscreen
        if (playerHealth.GetCurrentHealth() <= 0f && deathUi != null)
            deathUi.TriggerDeathScreen();

        // ends coroutine start new when player goes into the past again....
    }
}
