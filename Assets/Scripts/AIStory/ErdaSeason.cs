using System.Collections.Generic;
using UnityEngine;

public enum Season
{
    Frostwake, Bloomrise, Suncrest, Harvestmoon
}

public class ErdaSeason
{
    public Season CurrentSeason { get; private set; }

    private readonly Dictionary<Month, Season> monthToSeasonMapping;

    public ErdaSeason()
    {
        monthToSeasonMapping = new Dictionary<Month, Season>
        {
            { Month.Noctilis, Season.Frostwake },
            { Month.Solstice, Season.Frostwake },
            { Month.Glacialum, Season.Frostwake },
            { Month.Umbrilis, Season.Bloomrise },
            { Month.Aetheril, Season.Bloomrise },
            { Month.Lunistice, Season.Bloomrise },
            { Month.Nycturnis, Season.Suncrest },
            { Month.Solaris, Season.Suncrest },
            { Month.Wyrth, Season.Suncrest },
            { Month.Zephyril, Season.Harvestmoon },
            { Month.Hesperil, Season.Harvestmoon },
            { Month.Emberis, Season.Harvestmoon }
        };
    }
    public void UpdateSeason(Month currentMonth)
    {
        if (monthToSeasonMapping.ContainsKey(currentMonth))
        {
            CurrentSeason = monthToSeasonMapping[currentMonth];
            Debug.Log("Season changed to: " + CurrentSeason);
        }
    }

    public string GetCurrentSeasonName()
    {
        return CurrentSeason.ToString();
    }
}