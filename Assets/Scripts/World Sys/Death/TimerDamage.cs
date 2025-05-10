using UnityEngine;
using UnityEngine.UI; // <-- needed for UI Image
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

    [Header("UI Elements")]
    [SerializeField] private Image warningImage;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite lowTimeSprite;

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

        // grabs player Health
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

        // Make sure UI image is hidden on start
        if (warningImage != null)
            warningImage.enabled = false;
    }

    private void Update()
    {
        bool nowInPresent = timeline.in_Present;

        // Checks the state of player enter past?
        if (wasInPresent && !nowInPresent)
        {
            // If yes,entered past start the countdown
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

            // Hide UI when exiting the past
            if (warningImage != null)
                warningImage.enabled = false;
        }

        wasInPresent = nowInPresent;
    }

    private IEnumerator PastCountdown()
    {
        float time = 0f;

        // Set default sprite and show it
        if (warningImage != null && defaultSprite != null)
        {
            warningImage.sprite = defaultSprite;
            warningImage.enabled = true;
        }

        while (time < countdownDuration)
        {
            time += Time.deltaTime;
            yield return null;

            // if Player Is back in Present stop the countdown and should reset
            if (timeline.in_Present)
                yield break;

            // If less than 5 seconds left, switch to low time sprite
            if (countdownDuration - time <= 5f && warningImage != null && lowTimeSprite != null)
                warningImage.sprite = lowTimeSprite;
        }

        // when timer reaches 0 take damage
        playerHealth.TakeDamage(damageAmount);

        // if zero show deathscreen
        if (playerHealth.GetCurrentHealth() <= 0f && deathUi != null)
            deathUi.TriggerDeathScreen();

        // Hide UI when countdown ends
        if (warningImage != null)
            warningImage.enabled = false;

        // ends coroutine start new when player goes into the past again....
    }
}
