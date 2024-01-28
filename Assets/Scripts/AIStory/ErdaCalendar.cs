using System;
using System.Collections.Generic;
using UnityEngine;

public class ErdaCalendar
{
    public enum Season { Frostwake, Bloomrise, Suncrest, Harvestmoon }
    public enum Month { Noctilis, Solstice, Glacialum, Umbrilis, Aetheril, Lunistice, Nycturnis, Solaris, Wyrth, Zephyril, Hesperil, Emberis }
    private readonly Dictionary<Season, List<Month>> seasonMonths;
    private Dictionary<Month, string> monthDescriptions;
    private Dictionary<Season, string> seasonDescriptions;

    private int currentDay; // Day of the month
    private Month currentMonth;
    private Season currentSeason;
    private int currentYear;
    private int dayOfYear; // 1 through 365/366

    public ErdaCalendar()
    {
        // Starting year
        currentYear = 112;

        // Initialize the seasonMonths mapping
        seasonMonths = new Dictionary<Season, List<Month>>
        {
            { Season.Frostwake, new List<Month> { Month.Noctilis, Month.Solstice, Month.Glacialum } },
            { Season.Bloomrise, new List<Month> { Month.Umbrilis, Month.Aetheril, Month.Lunistice } },
            { Season.Suncrest, new List<Month> { Month.Nycturnis, Month.Solaris, Month.Wyrth } },
            { Season.Harvestmoon, new List<Month> { Month.Zephyril, Month.Hesperil, Month.Emberis } }
        };

        // Start at the beginning of the year
        currentMonth = Month.Nycturnis;
        currentDay = 1;
        dayOfYear = 1;

        // Determine the current season based on the month
        foreach (var season in seasonMonths.Keys)
        {
            if (seasonMonths[season].Contains(currentMonth))
            {
                currentSeason = season;
                break;
            }
        }

        InitializeDescriptions();
    }

    private void InitializeDescriptions()
    {
        seasonDescriptions = new Dictionary<Season, string>
        {
            { Season.Frostwake, "A time of cold and darkness, when the world is at its most harsh and unforgiving." },
            { Season.Bloomrise, "A time of rebirth and new beginnings, when the world starts to thaw and life returns." },
            { Season.Suncrest, "A time of warmth and light, when the world is vibrant and full of energy." },
            { Season.Harvestmoon, "A time of change and transformation, as the world cools and prepares for winter." }
        };

        monthDescriptions = new Dictionary<Month, string>
        {
            { Month.Noctilis, "Deep winter nights envelop the world in darkness, aligning with the cold and harshness of Frostwake." },
            { Month.Solstice, "The turning point of winter, marking the shortest day and the longest night of the year." },
            { Month.Glacialum, "Signifies the end of winter's coldness, with the frost beginning to recede as the world prepares for spring." },
            { Month.Umbrilis, "Embodies early spring, where shadows lengthen and the secrets hidden in darkness become more pronounced." },
            { Month.Aetheril, "Associated with the ethereal realm and the thinning of the veil between the mortal world and the realm of spirits." },
            { Month.Lunistice, "Symbolizes the month of celestial harmony with the strongest influence of the moon, awakening life and enchantment." },
            { Month.Nycturnis, "Encompasses the first days of summer with shorter nights and enchanting moonlit landscapes." },
            { Month.Solaris, "Signifies the peak of summer with the sun at its most fiery, bringing warmth, light, and vibrant energy." },
            { Month.Wyrth, "A period of change and magical energy surge, representing the vitality and spirit of summer." },
            { Month.Zephyril, "Marks the transitional phase between summer and autumn with gentle breezes and ancient whispers." },
            { Month.Hesperil, "Draws its name from the twilight hour, inviting reflection on the passage of time in the serene ambiance of late autumn." },
            { Month.Emberis, "Marks the end of autumn, with the fading embers of warmth and light, preparing for the coming winter." }
        };
    }

    public void AdvanceDay()
    {
        currentDay++;
        dayOfYear++;

        // Check if the current month needs to change
        if (currentDay > GetDaysInMonth(currentMonth))
        {
            currentDay = 1; // Reset day to 1
            int monthIndex = seasonMonths[currentSeason].IndexOf(currentMonth) + 1;

            if (monthIndex >= seasonMonths[currentSeason].Count)
            {
                // Move to next season
                currentSeason = (Season)(((int)currentSeason + 1) % Enum.GetNames(typeof(Season)).Length);
                currentMonth = seasonMonths[currentSeason][0]; // Start of the new season
                if (currentSeason == Season.Frostwake) // If it's the first season, increment the year
                {
                    AdvanceYear();
                }
            }
            else
            {
                // Move to next month within the current season
                currentMonth = seasonMonths[currentSeason][monthIndex];
            }
        }
    }

    public void AdvanceYear()
    {
        currentYear++;
        dayOfYear = 1; // Reset day of year
        currentSeason = Season.Frostwake; // Reset to the first season
        currentMonth = Month.Noctilis; // Reset to the first month
        currentDay = 1; // Reset day of the month
    }

    private int GetDaysInSeason(Season season)
    {
        switch (season)
        {
            case Season.Frostwake: return 90;
            case Season.Bloomrise: return 92; // Adjust for the extra day in Bloomrise
            case Season.Suncrest: return 94;
            case Season.Harvestmoon: return 89;
            default: throw new ArgumentOutOfRangeException(nameof(season), season, null);
        }
    }

    private int GetDaysInMonth(Month month)
    {
        // Each month within a season has an equal number of days,
        // except for Bloomrise which has the extra day accounted for in GetDaysInSeason.
        return GetDaysInSeason(currentSeason) / 3;
    }

    public string GetCurrentDayOfWeek()
    {
        string[] weekDays = { "Mortday", "Ardosday", "Elynday", "Thunday", "Veriday", "Seleneaday", "Solaraday" };
        // Assuming week starts on Mortday, calculate day of the week
        return weekDays[(dayOfYear - 1) % 7];
    }

    public int GetCurrentDayOfMonth()
    {
        return currentDay;
    }

    public string GetCurrentMonth()
    {
        return currentMonth.ToString();
    }

    public string GetCurrentSeason()
    {
        return currentSeason.ToString();
    }

    public int GetCurrentYear()
    {
        return currentYear;
    }

    public string GetCurrentDayInfo()
    {
        string seasonDesc = seasonDescriptions[currentSeason];
        string monthDesc = monthDescriptions[currentMonth];

        return $"It is {GetCurrentDayOfWeek()}, the {currentDay} of {GetCurrentMonth()}, {currentSeason} ({seasonDesc}). " +
               $"{monthDesc} The year is {currentYear}.";
    }
}
