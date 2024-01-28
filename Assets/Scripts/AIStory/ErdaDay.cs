using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class ErdaDay
{
    private int dayIndex; // Represents the index of the day in the week
    private List<DayData> weekDaysData; // List to store day data

    public ErdaDay()
    {
        dayIndex = 0; // Starting with the first day in the JSON file
        LoadDayData(); // Load day data from JSON file
    }

    private void LoadDayData()
    {
        string json = File.ReadAllText("Assets/Resources/Days.json");
        weekDaysData = JsonConvert.DeserializeObject<List<DayData>>(json);
    }

    public string GetCurrentDayName()
    {
        if (dayIndex >= 0 && dayIndex < weekDaysData.Count)
        {
            return weekDaysData[dayIndex].Name; // Ensure this matches the property in DayData
        }
        else
        {
            Debug.LogError("Invalid day index: " + dayIndex);
            return "Invalid Day";
        }
    }

    public void AdvanceDay()
    {
        dayIndex = (dayIndex + 1) % weekDaysData.Count; // Cycle through the days of the week
    }
}
