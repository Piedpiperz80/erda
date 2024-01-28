using UnityEngine;

public class ErdaDay
{
    private int dayIndex; // Represents the index of the day in the week
    private readonly string[] weekDays = { "Mortday", "Ardosday", "Elynday", "Thunday", "Veriday", "Seleneaday", "Solaraday" };

    public ErdaDay()
    {
        dayIndex = 0; // Starting with Mortday
    }

    public string GetCurrentDayName()
    {
        return weekDays[dayIndex];
    }

    public void AdvanceDay()
    {
        dayIndex = (dayIndex + 1) % weekDays.Length; // Cycle through the days of the week
        Debug.Log("Advancing to the next day: " + GetCurrentDayName());
    }
}
