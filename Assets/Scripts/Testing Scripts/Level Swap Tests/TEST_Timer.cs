using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TEST_Timer : MonoBehaviour
{
    [SerializeField] private float timer_Max;
    [SerializeField] private TextMeshProUGUI timer_Text;
    private TEST_TP_Connector call_Origin_Script;

    public IEnumerator TP_Timer(GameObject call_Origin)
    {
        call_Origin_Script = call_Origin.GetComponent<TEST_TP_Connector>();
        
        float timer = timer_Max;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            timer_Text.text = (Mathf.Round(timer * 100.0f) * 0.01f).ToString();
            yield return null;
        }
        
        timer_Text.text = "";
        
        call_Origin_Script.TP_Sorter();

    }// end TP_Timer()
    
}// end TEST_Timer
