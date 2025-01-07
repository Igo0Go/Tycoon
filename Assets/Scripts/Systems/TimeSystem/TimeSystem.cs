using System;
using UnityEngine;

public class TimeSystem : MonoBehaviour
{
    [SerializeField, Range(1, 1000)]
    private float timeSpeed = 1;

    public event Action<int> hoursChanged;
    public event Action<int> minutesChanged;
    public event Action<DateTime> dateChanged;

    public int CurrentHour
    {
        get
        {
            return _currentHour;
        }
        set
        {
            _currentHour = Math.Clamp(value, 0, 23);
            hoursChanged?.Invoke(_currentHour);
        }
    }
    private int _currentHour;

    public int CurrentMinute
    {
        get
        {
            return _currentMinute;
        }
        set
        {
            _currentMinute = value;

            if(CurrentMinute >= cycle)
            {
                _currentMinute = 0;
                _currentHour++;
                if(_currentHour >= hourCycle)
                {
                    _currentHour = 0;
                }
            }
            minutesChanged?.Invoke(_currentMinute);
        }
    }
    private int _currentMinute;

    public DateTime CurrentDate
    {
        get
        {
            return _currentDate;
        }
        set
        {
            _currentDate = value;
            dateChanged?.Invoke(_currentDate);
        }
    }
    private DateTime _currentDate;

    public const int cycle = 60;
    public const int hourCycle = 24;

    private bool useTime;
    private float t = 0;

    private void Start()
    {
        TimeSettings.TimeSpeed = timeSpeed;
        StartDay();
    }

    public void StartDay()
    {
        CurrentHour = 9;
        CurrentMinute = 0;
        useTime = true;
    }

    private void Update()
    {
        if (useTime)
        {
            t += Time.deltaTime * TimeSettings.TimeSpeed;
            if (t >= cycle)
            {
                CurrentMinute++;
                t = 0;
            }
        }
    }
}

public static class TimeSettings
{
    public static float TimeSpeed = 1;
}
