using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class ErdaTime : MonoBehaviour
{
    private float accumulatedTime = 0f; // Accumulator for time
    [SerializeField] private int seconds;
    [SerializeField] private int minutes;
    [SerializeField] private int hours;
    public ErdaDay erdaDay;
    public ErdaMonth erdaMonth;
    public ErdaSeason erdaSeason;
    [SerializeField]
    private int currentYear = 1; // Serialized private field with initial value 1.
    public int CurrentYear { get { return currentYear; } private set { currentYear = value; } }
    public float SpeedMultiplier = 1.0f; // Speed of time flow

    void Awake()
    {
        // Load data for months and seasons from JSON
        var monthData = LoadDataFromJson<List<MonthData>>("Assets/Resources/Months.json");
        var seasonData = LoadDataFromJson<List<SeasonData>>("Assets/Resources/Seasons.json");

        erdaSeason = new ErdaSeason(); // Initialize ErdaSeason first
        erdaMonth = new ErdaMonth(erdaSeason); // Then initialize ErdaMonth with ErdaSeason
        erdaDay = new ErdaDay();

        // Set the initial season based on the starting month
        erdaSeason.UpdateSeason(erdaMonth.GetCurrentMonthName());

        Debug.Log("ErdaTime initialized.");
    }

    void Update()
    {
        // Accumulate the time passed, considering the speed multiplier
        accumulatedTime += Time.deltaTime * SpeedMultiplier;

        // Convert accumulated time to whole seconds and advance time
        while (accumulatedTime >= 1.0f)
        {
            accumulatedTime -= 1.0f;
            AdvanceTime();
        }
    }

    private void AdvanceTime()
    {
        seconds++;
        if (seconds >= 60)
        {
            seconds = 0;
            minutes++;
            if (minutes >= 60)
            {
                minutes = 0;
                hours++;
                if (hours >= 24)
                {
                    hours = 0;
                    erdaDay.AdvanceDay(); // Advance to the next day

                    // New day has started, log the current date
                    Debug.Log("New Day: " + GetCurrentDate());

                    if (erdaMonth.AdvanceDay()) // Advance the day in ErdaMonth, check if month changed
                    {
                        if (erdaMonth.IsStartOfYear()) // Check if it's the start of the year
                        {
                            CurrentYear++; // Increment the year
                        }
                    }

                    // Here, you can also check and update the season if needed
                    erdaSeason.UpdateSeason(erdaMonth.GetCurrentMonthName());
                }
            }
        }
    }

    public string GetCurrentDate()
    {
        string currentDayName = erdaDay.GetCurrentDayName();
        int currentDayNumber = erdaMonth.GetCurrentDayNumber();
        string currentMonthName = erdaMonth.GetCurrentMonthName();
        string currentSeasonName = erdaSeason.GetCurrentSeasonName();
        return $"{currentDayName}, Day {currentDayNumber} of {currentMonthName}, {currentSeasonName}, Year {CurrentYear}";
    }

    public string GetSceneDescription()
    {
        // Gather details
        string monthDescription = erdaMonth.GetCurrentMonthDescription();
        string time = GetCurrentTime();

        // Format the scene description
        return $"It is {time} {monthDescription}";
    }

    public string GetCurrentTime()
    {
        return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
    }

    private T LoadDataFromJson<T>(string jsonPath)
    {
        string json = File.ReadAllText(jsonPath);
        return JsonConvert.DeserializeObject<T>(json);
    }
}
