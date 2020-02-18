using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RunTimeManager : MonoBehaviour
{
    [Header("Component References")]
    public TextMeshProUGUI runTimerText;

    [Header("Properties")]
    public float runTimer = 0f;
    public bool updateTime;

    // Singleton Setup
    #region
    public static RunTimeManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    // Start + Update
    #region
    private void Start()
    {
        StartTimer();
    }
    private void Update()
    {
        if (updateTime)
        {
            UpdateTimer();
        }       
    }
    #endregion
       
    // Manipulate Timer
    #region
    private void UpdateTimer()
    {
        runTimer += Time.deltaTime;

        int seconds = (int)(runTimer % 60);
        int minutes = (int)(runTimer / 60) % 60;
        int hours = (int)(runTimer / 3600) % 24;

        string runTimerString = string.Format("{0:0}:{1:00}:{2:00}", hours, minutes, seconds);
        runTimerText.text = runTimerString;
    }
    public void StartTimer()
    {
        updateTime = true;
    }
    public void PauseTimer()
    {
        updateTime = false;
    }
    #endregion




}
