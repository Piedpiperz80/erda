using UnityEngine;

public class ErdaTime : MonoBehaviour
{
    private float accumulatedTime = 0f; // Accumulator for time
    [SerializeField] private int seconds;
    [SerializeField] private int minutes;
    [SerializeField] private int hours;
    public ErdaDay erdaDay;
    public ErdaMonth erdaMonth;
    public ErdaSeason erdaSeason;

    public float SpeedMultiplier = 1.0f; // Speed of time flow

    void Awake()
    {
        erdaSeason = new ErdaSeason();
        erdaDay = new ErdaDay();
        erdaMonth = new ErdaMonth(erdaSeason);
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
                    erdaMonth.AdvanceDay(); // Also advance the day in ErdaMonth
                }
            }
        }
    }

    public string GetCurrentTime()
    {
        return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
    }
}
