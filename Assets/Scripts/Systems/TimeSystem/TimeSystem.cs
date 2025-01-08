using System;
using UnityEngine;

public class TimeSystem : MonoBehaviour
{
    [SerializeField, Range(1, 1000)]
    private float timeSpeed = 1;

    [SerializeField, TextArea(5, 10)]
    private string lunchMessage;
    [SerializeField, TextArea(5, 10)]
    private string startDayMessage;
    [SerializeField, TextArea(5, 10)]
    private string endDayMessage;

    public event Action<int> hoursChanged;
    public event Action<int> minutesChanged;
    public event Action<DateTime> dateChanged;
    public event Action startLunch;
    public event Action startWork;
    public event Action endDay;


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
            CheckDatePart();
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
                CheckDatePart();
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
    private const int startDayHour = 9;
    private const int endDayHour = 18;
    private const int startLunchHour = 13;
    private const int endLunchHour = 14;

    private DayPart currentDayPart;
    private bool useTime;
    private float t = 0;


    private void Start()
    {
        CurrentDate = DateTime.Now;
        TimeSettings.TimeSpeed = timeSpeed;
        StartNewDay();
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

    public void StartNewDay()
    {
        CurrentHour = 9;
        CurrentMinute = 0;
        useTime = true;
        startWork?.Invoke();

        if(CurrentDate.DayOfWeek == DayOfWeek.Friday)
        {
            CurrentDate = CurrentDate.AddDays(3);
        }
        else
        {
            CurrentDate = CurrentDate.AddDays(1);
        }

        GameUICenter.messagePanel.ShowMessage("Новый день!", startDayMessage);
    }

    public void SkipLunch()
    {
        _currentMinute = 0;
        CurrentHour = endLunchHour;
    }

    private void CheckDatePart()
    {
        if ((CurrentHour >= startDayHour && CurrentHour < startLunchHour) ||
            (CurrentHour >= endLunchHour && CurrentHour < endDayHour))
        {
            if(currentDayPart != DayPart.Work)
            {
                currentDayPart = DayPart.Work;
                startWork?.Invoke();
            }
        }
        else if (CurrentHour >= startLunchHour && CurrentHour < endLunchHour)
        {
            if(currentDayPart != DayPart.Lunch)
            {
                currentDayPart = DayPart.Lunch;
                startLunch?.Invoke();
                GameUICenter.messagePanel.ShowMessage("На обед!", lunchMessage, SkipLunch, () => { });
            }
        }
        else if (CurrentHour >= endDayHour || CurrentHour < startDayHour)
        {
            if(currentDayPart != DayPart.HomeTime)
            {
                currentDayPart = DayPart.HomeTime;
                endDay?.Invoke();
                GameUICenter.messagePanel.ShowMessage("Пока-пока!", endDayMessage, StartNewDay, () => { });
            }
        }
    }
}

public static class TimeSettings
{
    public static float TimeSpeed = 1;
}

public enum DayPart
{
    Work,
    Lunch,
    HomeTime
}
