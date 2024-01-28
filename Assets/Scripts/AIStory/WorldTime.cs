using UnityEngine;
using System;

public class WorldTime : MonoBehaviour
{
    public ErdaCalendar erdaCalendar;

    [Header("Time Scale Settings")]
    public float timeScale = 1.0f; // 1 real second equals 1 game day

    [Header("Current Time")]
    public bool isPaused = false;
    public string currentDayName;
    public int currentDay;
    public string currentMonthName;
    public int currentYear;

    private float timer;

    void Start()
    {
        erdaCalendar = new ErdaCalendar();
        UpdateInspectorFields();
    }

    void Update()
    {
        if (!isPaused)
        {
            timer += Time.deltaTime * timeScale;
            if (timer >= 1.0f)
            {
                timer = 0f;
                erdaCalendar.AdvanceDay();
                UpdateInspectorFields();
            }
        }
    }

    private void UpdateInspectorFields()
    {
        // Assuming erdaCalendar has methods to get the current day, month, and year
        currentDayName = erdaCalendar.GetCurrentDayOfWeek();
        currentDay = erdaCalendar.GetCurrentDayOfMonth();
        currentMonthName = erdaCalendar.GetCurrentMonth();
        currentYear = erdaCalendar.GetCurrentYear();
    }

    public string GetCurrentDayInfo()
    {
        return erdaCalendar.GetCurrentDayInfo();
    }

    public void PauseTime()
    {
        isPaused = true;
    }

    public void ResumeTime()
    {
        isPaused = false;
    }
}
