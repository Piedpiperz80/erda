using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class ErdaSeason
{
    public string CurrentSeason { get; private set; }

    private Dictionary<string, string> monthToSeasonMapping; // Mapping of month names to season names

    public ErdaSeason()
    {
        monthToSeasonMapping = new Dictionary<string, string>();
        LoadSeasonData(); // Load season data from JSON
    }

    private void LoadSeasonData()
    {
        string json = File.ReadAllText("Assets/Resources/Seasons.json"); // Adjust the path as needed
        var seasonsData = JsonConvert.DeserializeObject<List<SeasonData>>(json);

        foreach (var season in seasonsData)
        {
            foreach (var month in season.Months)
            {
                monthToSeasonMapping[month] = season.Name;
            }
        }
    }

    public void UpdateSeason(string currentMonth)
    {
        if (monthToSeasonMapping.ContainsKey(currentMonth))
        {
            CurrentSeason = monthToSeasonMapping[currentMonth];
        }
    }

    public string GetCurrentSeasonName()
    {
        return CurrentSeason;
    }
}
