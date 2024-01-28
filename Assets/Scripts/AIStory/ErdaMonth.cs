using System;
using System.Collections.Generic;
using UnityEngine;

public enum Month 
{ 
    Noctilis, Solstice, Glacialum, 
    Umbrilis, Aetheril, Lunistice, 
    Nycturnis, Solaris, Wyrth, 
    Zephyril, Hesperil, Emberis 
}

public class ErdaMonth
{
    public Month CurrentMonth { get; private set; }
    private readonly ErdaSeason erdaSeason;


    private int currentDay;
    private readonly Dictionary<Month, int> daysInMonth;
    private readonly Dictionary<Month, string> monthDescriptions;


    public ErdaMonth(ErdaSeason season, Month startMonth = Month.Noctilis)
    {
        this.erdaSeason = season;
        CurrentMonth = startMonth;
        currentDay = 1;
        daysInMonth = new Dictionary<Month, int>
        {
            { Month.Noctilis, 30 },
            { Month.Solstice, 30 },
            { Month.Glacialum, 30 },
            { Month.Umbrilis, 30 },
            { Month.Aetheril, 30 },
            { Month.Lunistice, 32 },
            { Month.Nycturnis, 31 },
            { Month.Solaris, 31 },
            { Month.Wyrth, 31 },
            { Month.Zephyril, 30 },
            { Month.Hesperil, 30 },
            { Month.Emberis, 30 }
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
        if (currentDay > daysInMonth[CurrentMonth])
        {
            AdvanceMonth();
        }
    }

    private void AdvanceMonth()
    {
        currentDay = 1; // Reset day count for new month
        CurrentMonth = (Month)(((int)CurrentMonth + 1) % Enum.GetNames(typeof(Month)).Length);
        Debug.Log("Advancing to the next month: " + GetCurrentMonthName());
        erdaSeason.UpdateSeason(CurrentMonth); // Update the season
    }

    public string GetCurrentMonthName()
    {
        return CurrentMonth.ToString();
    }

    public string GetCurrentMonthDescription()
    {
        return monthDescriptions[CurrentMonth];
    }
}
