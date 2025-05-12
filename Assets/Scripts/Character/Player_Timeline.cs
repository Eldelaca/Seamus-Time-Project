using System;
using UnityEngine;

// Just to determine what timeline the player is in for teleporting, may need to add more later
public class Player_Timeline : MonoBehaviour
{
    public bool in_Present = true;

    [SerializeField] private AudioSource pastAudio;
    [SerializeField] private AudioSource presentAudio;

    private void Start()
    {
        UpdateAudio();
    }

    public void UpdateAudio()
    {
        if (in_Present)
        {
            presentAudio.gameObject.SetActive(true);
            pastAudio.gameObject.SetActive(false);
        }
        else
        {
            pastAudio.gameObject.SetActive(true);
            presentAudio.gameObject.SetActive(false);
        }
    }
}// end Player_Timeline
