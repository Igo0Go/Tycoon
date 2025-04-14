using System;
using TMPro;
using UnityEngine;

public class TimePanel : MonoBehaviour
{
    [SerializeField]
    private TMP_Text timerText;
    [SerializeField]
    private TMP_Text dayPartText;
    [SerializeField]
    private TMP_Text dateText;
    [SerializeField]
    private TMP_Text timeSpeedText;
    [SerializeField]
    private GameObject goHomeButton;

    private TimeSystem timeSystem;

    public void SubscribeEvents(TimeSystem timeSystem)
    {
        this.timeSystem = timeSystem;
        timeSystem.HoursChanged += OnHoursChanged;
        timeSystem.MinuteChanged += OnMinutesChanged;
        timeSystem.StartWork += () => OnDayPartChanged("������");
        timeSystem.StartLunch += () => OnDayPartChanged("����");
        timeSystem.EndWork += () => OnDayPartChanged("�����");
        timeSystem.StartOvertime += OnStartOvertime;
        timeSystem.DateChanged += OnDateChanged;
        timeSystem.NewDayBeginning += () => goHomeButton.SetActive(false);

        TimeSettings.ClearEvents();
        TimeSettings.TimeSpeedChanged += OnChangeTimeSpeed;
    }
    public void SetUp()
    {
        goHomeButton.SetActive(false);
    }
    public void OnGoHomeButtonClick()
    {
        timeSystem.EndDay();
        goHomeButton.SetActive(false);
    }

    private void OnHoursChanged(int hour)
    {
        int minute = timeSystem.CurrentMinute;

        string minutes;
        if (minute < 10)
        {
            minutes = "0" + minute;
        }
        else
        {
            minutes = minute.ToString();
        }

        timerText.text = hour.ToString() + ":" + minutes;
    }
    private void OnMinutesChanged(int minute)
    {
        string minutes;
        if (minute < 10)
        {
            minutes = "0" + minute;
        }
        else
        {
            minutes = minute.ToString();
        }

        timerText.text = timeSystem.CurrentHour + ":" + minutes;
    }
    private void OnDayPartChanged(string dayPart)
    {
        dayPartText.text = dayPart;
    }
    private void OnDateChanged(DateTime date)
    {
        string dayOfWeek = string.Empty;
        switch(date.DayOfWeek)
        {
            case DayOfWeek.Sunday:
                dayOfWeek = "�����������";
                break;
            case DayOfWeek.Monday:
                dayOfWeek = "�����������";
                break;
            case DayOfWeek.Tuesday:
                dayOfWeek = "�������";
                break;
            case DayOfWeek.Wednesday:
                dayOfWeek = "�����";
                break;
            case DayOfWeek.Thursday:
                dayOfWeek = "�������";
                break;
            case DayOfWeek.Friday:
                dayOfWeek = "�������";
                break;
            case DayOfWeek.Saturday:
                dayOfWeek = "�������";
                break;

        }


        dateText.text = dayOfWeek + "\n" + date.Day + "." + date.Month + "." + date.Year;
    }
    private void OnStartOvertime()
    {
        goHomeButton.SetActive(true);
    }
    private void OnChangeTimeSpeed(float speed) 
    {
        timeSpeedText.text = "X" + speed.ToString();
    }
}


