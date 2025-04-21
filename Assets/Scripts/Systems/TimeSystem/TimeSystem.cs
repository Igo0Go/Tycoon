using System;
using UnityEngine;

public class TimeSystem : MonoBehaviour
{
    #region ��������

    /// <summary>
    /// ������� ���. ��� ��������� ��������� ������� HourChanged. �������� �������� ����� ���.
    /// </summary>
    public int CurrentHour
    {
        get
        {
            return _currentHour;
        }
        set
        {
            _currentHour = Math.Clamp(value, 0, 23);
            HoursChanged?.Invoke(_currentHour);
            CheckDatePart();
        }
    }
    private int _currentHour;

    /// <summary>
    /// ������� ������. ��� ��������� ��������� ������� MinuteChanged.
    /// ��� ���������� ������ 59 ���������� ���. �������� �������� ����� ���
    /// </summary>
    public int CurrentMinute
    {
        get
        {
            return _currentMinute;
        }
        set
        {
            _currentMinute = value;

            if (CurrentMinute >= minuteCycle)
            {
                _currentMinute = 0;
                _currentHour++;
                if (_currentHour >= hourCycle)
                {
                    _currentHour = 0;
                }
                CheckDatePart();
            }
            MinuteChanged?.Invoke(_currentMinute);
        }
    }
    private int _currentMinute;

    /// <summary>
    /// ������� ����. ��� ��������� ��������� ������� DateChanged
    /// </summary>
    public DateTime CurrentDate
    {
        get
        {
            return _currentDate;
        }
        set
        {
            _currentDate = value;
            DateChanged?.Invoke(_currentDate);
        }
    }
    private DateTime _currentDate;
    #endregion

    #region ����

    /// <summary>
    /// ������� �������� ������� �������
    /// </summary>
    [SerializeField, Range(1, 1000)]
    private float timeSpeed = 1;

    /// <summary>
    /// ����� � ����������� ������ ���
    /// </summary>
    [Space(20)]
    [SerializeField]
    private DayPartMessagePack dayPartMessagePack;

    /// <summary>
    /// ����� � ����������� � �������. ��������� �� ����, ����� ���� � �.�.
    /// </summary>
    [SerializeField]
    private EmployeeProjectData projectData;


    //�������
    private bool useTime;
    private bool financeLost = false;
    private float timer = 0;
    private int currentDayCount = 0;
    private DayPart currentDayPart;
    #endregion

    #region ���������
    private const int startDayHour = 9;
    private const int startLunchHour = 13;
    private const int endLunchHour = 14;
    private const int prepareOvertimeHour = 17;
    private const int endWorkDayHour = 18;
    private const int endDayHour = 0;

    public const int minuteCycle = 60;
    public const int hourCycle = 24;
    #endregion

    #region �������
    public event Action<int> HoursChanged;
    public event Action<int> MinuteChanged;
    public event Action<DateTime> DateChanged;
    public event Action<int> SpendTime;
    public event Action<int> ProjectDayLimitChanged;

    public event Action NewDayBeginning;
    public event Action StartWork;
    public event Action EndWork;
    public event Action StartLunch;
    public event Action OvertimeAccepted;
    public event Action StartOvertime;
    public event Action EndOfDay;
    #endregion

    #region ������
    public void SubscribeEvents(EmployeeTaskSystem taskSystem, FinanceSystem financeSystem)
    {
        taskSystem.ProjectComplete += () => useTime = false;
        financeSystem.financeLost += (value) => financeLost = value;
    }

    public void SetUp()
    {
        CurrentDate = DateTime.Now;
        TimeSettings.TimeSpeed = timeSpeed;
        CurrentHour = 9;
        CurrentMinute = 0;
        useTime = true;
        NewDayBeginning?.Invoke();
        StartWork?.Invoke();
        GameUICenter.messageQueue.PrepareMessage(dayPartMessagePack.startDayMessage.Header,
            dayPartMessagePack.startDayMessage.Message);
        CheckDayEvents();
    }

    /// <summary>
    /// ���������� �������� ����� � ������� �������� �������
    /// </summary>
    /// <param name="hour">������� ���</param>
    /// <param name="minute">������� ������</param>
    /// <returns>-1 ���� ������� ����� ������ ��������, 0 - ���� �����, 1 - ���� �����</returns>
    public int EqulasTime(int hour, int minute)
    {
        if (hour < CurrentHour)
        {
            return -1;
        }
        else if (hour == CurrentHour)
        {
            if (minute < CurrentMinute)
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

    /// <summary>
    /// ��������� ����� ���� � ��������� �������� � ������ ���� �� ������
    /// </summary>
    public void StartNewDay()
    {
        CurrentHour = 9;
        CurrentMinute = 0;
        useTime = true;
        NewDayBeginning?.Invoke();
        StartWork?.Invoke();

        if (CurrentDate.DayOfWeek == DayOfWeek.Friday)
        {
            CurrentDate = CurrentDate.AddDays(3);
            currentDayCount += 3;
        }
        else
        {
            CurrentDate = CurrentDate.AddDays(1);
            currentDayCount += 1;
        }
        ProjectDayLimitChanged?.Invoke(projectData.projectDayLimit - currentDayCount);

        if (currentDayCount > projectData.projectDayLimit)
        {
            GameUICenter.messageQueue.PrepareMessage(projectData.lostTimeMessage.Header,
                projectData.lostTimeMessage.Message);
            useTime = false;
        }
        else
        {
            GameUICenter.messageQueue.PrepareMessage(dayPartMessagePack.startDayMessage.Header,
                dayPartMessagePack.startDayMessage.Message);
            CheckDayEvents();
        }
    }

    /// <summary>
    /// ���������� ����, ����� ������� �� ������� ��� ���������
    /// </summary>
    public void SkipLunch()
    {
        if (CurrentHour < endLunchHour)
        {
            _currentMinute = 0;
            CurrentHour = endLunchHour;
            StartWork?.Invoke();
        }
    }

    /// <summary>
    /// ��������� ���� � ������ ����� � ��������� ����, �������� �� ������
    /// </summary>
    public void EndDay()
    {
        EndOfDay?.Invoke();
        StartNewDay();

        if (financeLost)
        {
            GameUICenter.messageQueue.PrepareMessage(projectData.lostTimeMessage.Header,
                projectData.lostTimeMessage.Message);
            useTime = false;
        }
    }

    /// <summary>
    /// ����������� ����� � ����������� ���� � ������
    /// </summary>
    /// <param name="targetHour">������� ���</param>
    /// <param name="targetMinute">������� ������</param>
    public void SkipTimeToThis(int targetHour, int targetMinute)
    {
        int totalMinutes = 0;

        int hour = _currentHour;
        int minute = _currentMinute;

        while (!(hour == targetHour && minute == targetMinute))
        {
            if (hour < startLunchHour || hour >= endLunchHour)
            {
                totalMinutes++;
            }
            minute++;

            if (minute >= minuteCycle)
            {
                hour++;
                minute = 0;
            }
        }

        SpendTime?.Invoke(totalMinutes);

        CurrentHour = hour;
        CurrentMinute = minute;

        CheckDatePart();
    }

    /// <summary>
    /// ��������� ����� ���. ����� �� ����, ��������� ����� ������������ � �.�.
    /// </summary>
    private void CheckDatePart()
    {
        if ((CurrentHour >= startDayHour && CurrentHour < startLunchHour) ||
            (CurrentHour >= endLunchHour && CurrentHour < endWorkDayHour))
        {
            if (currentDayPart != DayPart.Work)
            {
                currentDayPart = DayPart.Work;
                StartWork?.Invoke();
            }
        }
        else if (CurrentHour >= startLunchHour && CurrentHour < endLunchHour)
        {
            if (currentDayPart != DayPart.Lunch)
            {
                currentDayPart = DayPart.Lunch;
                StartLunch?.Invoke();
                GameUICenter.messageQueue.PrepareMessage(dayPartMessagePack.lunchMessage.Header,
                    dayPartMessagePack.lunchMessage.Message, SkipLunch, () => { });
            }
        }

        if (CurrentHour >= prepareOvertimeHour && CurrentHour < endWorkDayHour)
        {
            GameUICenter.messageQueue.PrepareMessage(dayPartMessagePack.overtimePrepareMessage.Header,
                dayPartMessagePack.overtimePrepareMessage.Message, () => { OvertimeAccepted?.Invoke(); }, null);
        }
        else if (CurrentHour >= endWorkDayHour)
        {
            if (currentDayPart != DayPart.HomeTime)
            {
                currentDayPart = DayPart.HomeTime;
                EndWork?.Invoke();
                GameUICenter.messageQueue.PrepareMessage(dayPartMessagePack.endWorkMessage.Header,
                    dayPartMessagePack.endWorkMessage.Message, EndDay, () => { StartOvertime?.Invoke(); });
            }
        }
        else if (CurrentHour >= endDayHour && CurrentHour < startDayHour)
        {
            currentDayPart = DayPart.HomeTime;
            GameUICenter.messageQueue.PrepareMessage(dayPartMessagePack.endDayMessage.Header,
                dayPartMessagePack.endDayMessage.Message, EndDay);
        }
    }

    /// <summary>
    /// �������� ������� ����� ��� �������
    /// </summary>
    private void CheckDayEvents()
    {
        for (int i = 0; i < projectData.dayEvents.Count; i++)
        {
            DayEvent currentEvent = projectData.dayEvents[i];

            if (currentDayCount >= currentEvent.day)
            {
                GameUICenter.messageQueue.PrepareMessage(currentEvent.messageData.Header, currentEvent.messageData.Message);
                currentEvent.onDayEvent.Invoke();
                projectData.dayEvents.RemoveAt(i);
                i--;
            }
        }
    }

    /// <summary>
    /// �������� �������� ������� ������� �� ������
    /// </summary>
    private void ChangeSpeed()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TimeSettings.TimeSpeedMultiplier = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TimeSettings.TimeSpeedMultiplier = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TimeSettings.TimeSpeedMultiplier = 4;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            TimeSettings.TimeSpeedMultiplier = 10;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            TimeSettings.TimeSpeedMultiplier = 100;
        }
    }
    #endregion

    private void Update()
    {
        if (useTime)
        {
            timer += Time.deltaTime * TimeSettings.TimeSpeed * TimeSettings.TimeSpeedMultiplier;

            if (timer >= minuteCycle)
            {
                CurrentMinute++;
                SpendTime?.Invoke(1);
                timer = 0;
            }
        }
        ChangeSpeed();
    }
}



public static class TimeSettings
{
    public static float TimeSpeed = 1;

    public static float TimeSpeedMultiplier
    {
        get
        { 
            return _timeSpeedMultiplier;
        }
        set
        {
            _timeSpeedMultiplier = value;
            TimeSpeedChanged?.Invoke(_timeSpeedMultiplier);
        }
    }
    private static float _timeSpeedMultiplier = 1;

    public static void ClearEvents()
    {
        TimeSpeedChanged = null;
    }
    public static event Action<float> TimeSpeedChanged;
}

public enum DayPart
{
    Work,
    Lunch,
    HomeTime
}
