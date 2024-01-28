using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class ErdaMonth
{
    public string CurrentMonth { get; private set; }
    private readonly ErdaSeason erdaSeason;

    private int currentDay;
    private List<MonthData> monthsData; // List of MonthData objects

    public ErdaMonth(ErdaSeason season, string startMonth = "Noctilis")
    {
        this.erdaSeason = season;
        currentDay = 1;
        LoadMonthData(); // Load month data from JSON
        CurrentMonth = startMonth;
    }

    private void LoadMonthData()
    {
        string json = File.ReadAllText("Assets/Resources/Months.json"); // Adjust the path as needed
        monthsData = JsonConvert.DeserializeObject<List<MonthData>>(json);
    }

    public bool AdvanceDay()
    {
        currentDay++;
        if (currentDay > GetDaysInMonth(CurrentMonth))
        {
            AdvanceMonth();
            return true; // Month changed
        }
        return false; // Month did not change
    }

    private void AdvanceMonth()
    {
        currentDay = 1; // Reset day count for new month
        CurrentMonth = GetNextMonth(CurrentMonth);
        erdaSeason.UpdateSeason(CurrentMonth); // Update the season accordingly
    }

    private string GetNextMonth(string currentMonth)
    {
        int currentIndex = monthsData.FindIndex(m => m.Name == currentMonth);
        if (currentIndex < 0 || currentIndex >= monthsData.Count - 1)
            return monthsData[0].Name; // Returns to the first month if current is the last month or not found

        return monthsData[currentIndex + 1].Name;
    }

    private int GetDaysInMonth(string monthName)
    {
        var month = monthsData.Find(m => m.Name == monthName);
        return month != null ? month.days : 30; // Using 'days' instead of 'DaysInMonth'
    }

    public string GetCurrentMonthName()
    {
        return CurrentMonth;
    }

    public string GetCurrentMonthDescription()
    {
        var month = monthsData.Find(m => m.Name == CurrentMonth);
        return month != null ? month.Description : "No description available";
    }

    public int GetCurrentDayNumber()
    {
        return currentDay; // Returns the current day number in the month
    }

    public bool IsStartOfYear()
    {
        return CurrentMonth == "Noctilis";
    }
}
