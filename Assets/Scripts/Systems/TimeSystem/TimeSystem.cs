using System;
using UnityEngine;

public class TimeSystem : MonoBehaviour
{
    [SerializeField, Range(1, 1000)]
    private float timeSpeed = 1;

    [Space(20)]
    [SerializeField, TextArea(5, 10)]
    private string startDayMessage;
    [SerializeField, TextArea(5, 10)]
    private string lunchMessage;
    [SerializeField, TextArea(5, 10)]
    private string endWorkMessage;
    [SerializeField, TextArea(5, 10)]
    private string endDayMessage;
    [SerializeField, TextArea(5, 10)]
    private string overtimePrepareMessage;

    [Space(20)]
    [SerializeField]
    private int startDayHour = 9;
    [SerializeField]
    private int startLunchHour = 13;
    [SerializeField]
    private int endLunchHour = 14;
    [SerializeField]
    private int endWorkDayHour = 18;
    [SerializeField]
    private int endDayHour = 0;

    public event Action<int> hoursChanged;
    public event Action<int> minutesChanged;
    public event Action<DateTime> dateChanged;
    public event Action<int> spendTime;

    public event Action startNewDay;
    public event Action startWork;
    public event Action endWork;
    public event Action startLunch;
    public event Action overtimeAccepted;
    public event Action startOvertime;
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

    private const int prepareOvertimeHour = 17;

    private DayPart currentDayPart;
    private bool useTime;
    private float t = 0;

    /// <summary>
    /// Сравнивает поданное время с текущем временем системы
    /// </summary>
    /// <param name="hour">целевой час</param>
    /// <param name="minute">целевая минута</param>
    /// <returns>-1 если целевое время раньше текущего, 0 - если равны, 1 - если позже</returns>
    public int EqulasTime(int hour, int minute)
    {
        if(hour < CurrentHour)
        {
            return -1;
        }
        else if(hour == CurrentHour)
        {
            if(minute < CurrentMinute)
            {
                return -1;
            }
            else if (minute == CurrentMinute)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
        else
        {
            return 1;
        }
    }

    public void SetUp()
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
                spendTime?.Invoke(1);
                t = 0;
            }
        }
    }

    public void StartNewDay()
    {
        CurrentHour = 9;
        CurrentMinute = 0;
        useTime = true;
        startNewDay?.Invoke();
        startWork?.Invoke();

        if(CurrentDate.DayOfWeek == DayOfWeek.Friday)
        {
            CurrentDate = CurrentDate.AddDays(3);
        }
        else
        {
            CurrentDate = CurrentDate.AddDays(1);
        }

        GameUICenter.messageQueue.PrepareMessage("Новый день!", startDayMessage);
    }

    public void SkipLunch()
    {
        if(CurrentHour < endLunchHour)
        {
            _currentMinute = 0;
            CurrentHour = endLunchHour;
        }
    }

    public void EndDay()
    {
        endDay?.Invoke();
        StartNewDay();
    }

    public void SkipTimeToThis(int targetHour, int targetMinute)
    {
        int totalMinutes = 0;

        int hour = _currentHour;
        int minute = _currentMinute;

        while(!(hour==targetHour && minute == targetMinute))
        {
            if(hour < startLunchHour || hour >= endLunchHour)
            {
                totalMinutes++;
            }
            minute++;

            if(minute >= cycle)
            {
                hour++;
                minute = 0;
            }
        }

        spendTime?.Invoke(totalMinutes);

        CurrentHour = hour;
        CurrentMinute = minute;

        CheckDatePart();
    }

    private void CheckDatePart()
    {
        if ((CurrentHour >= startDayHour && CurrentHour < startLunchHour) ||
            (CurrentHour >= endLunchHour && CurrentHour < endWorkDayHour))
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
                GameUICenter.messageQueue.PrepareMessage("На обед!", lunchMessage, SkipLunch, () => { });
            }
        }

        if (CurrentHour >= prepareOvertimeHour && CurrentHour < endWorkDayHour)
        {
            GameUICenter.messageQueue.PrepareMessage("Успеваем?", overtimePrepareMessage, () => { overtimeAccepted?.Invoke(); }, null);
        }
        else if (CurrentHour >= endWorkDayHour)
        {
            if(currentDayPart != DayPart.HomeTime)
            {
                currentDayPart = DayPart.HomeTime;
                endWork?.Invoke();
                GameUICenter.messageQueue.PrepareMessage("Пока-пока!", endWorkMessage, EndDay, () => { startOvertime?.Invoke(); });
            }
        }
        else if (CurrentHour >= endDayHour && CurrentHour < startDayHour)
        {
            currentDayPart = DayPart.HomeTime;
            endDay?.Invoke();
            GameUICenter.messageQueue.PrepareMessage("Вы что-то заседелись", endDayMessage, EndDay);
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
