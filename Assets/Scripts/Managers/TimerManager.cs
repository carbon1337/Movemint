using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public bool isTimerRunning;
    public float elapsedTime;

    public TMP_Text timerText;

    // Update is called once per frame
    void Update()
    {
        timerText.text = elapsedTime.ToString("F2");

        if(isTimerRunning) {
            elapsedTime += Time.deltaTime;
        }
    }

    public void StartTimer() {
        elapsedTime = 0;

        isTimerRunning = true;
    }

    public void StopTimer() {
        isTimerRunning = false;
    }
}
