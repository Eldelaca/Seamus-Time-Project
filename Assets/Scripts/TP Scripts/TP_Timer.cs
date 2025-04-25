using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TP_Timer : MonoBehaviour
{
    [SerializeField] private float timer_Max;
    [SerializeField] private TextMeshProUGUI timer_Text;
    private GameObject player_Ref;
    private TP_Connector call_Origin_Script;
    public bool timer_Active;


    private void Start()
    {
        player_Ref = GameObject.FindGameObjectWithTag("Player");
    }

    
    private void Update()
    {
        // Reset text in UI when Timer() has been cancelled 
        if (timer_Active == false && timer_Text.text != "")
            timer_Text.text = "";
    }

    
    public IEnumerator Timer(GameObject call_Origin)
    {
        timer_Active = true;
        call_Origin_Script = call_Origin.GetComponent<TP_Connector>();
        
        float timer = timer_Max;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            timer_Text.text = (Mathf.Round(timer * 100.0f) * 0.01f).ToString();
            yield return null;
        }
        
        timer_Text.text = "";
        call_Origin_Script.objects_To_TP.Add(player_Ref);
        call_Origin_Script.TP_Sorter();
        timer_Active = false;
    }// end Timer()

    public void Start_Timer(GameObject call_Origin)
    {
        StopAllCoroutines();
        StartCoroutine(Timer(call_Origin));
    }

    public void Stop_Timer()
    {
        StopAllCoroutines();
    }
    
}// end Timer
