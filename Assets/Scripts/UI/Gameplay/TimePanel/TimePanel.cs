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
    private GameObject goHomeButton;

    private TimeSystem timeSystem;

    public void SetUp()
    {
        goHomeButton.SetActive(false);
    }

    public void SubscribeEvents(TimeSystem timeSystem)
    {
        this.timeSystem = timeSystem;
        timeSystem.hoursChanged += OnHoursChanged;
        timeSystem.minutesChanged += OnMinutesChanged;
        timeSystem.startWork += ()=> OnDayPartChanged("Работа");
        timeSystem.startLunch += () => OnDayPartChanged("Обед");
        timeSystem.endWork += () => OnDayPartChanged("Отдых");
        timeSystem.startOvertime += OnStartOvertime;
        timeSystem.dateChanged += OnDateChanged;
        timeSystem.startNewDay += () => goHomeButton.SetActive(false);
    }

    private void OnHoursChanged(int hour)
    {
        timerText.text = hour.ToString() + ":" + timeSystem.CurrentMinute;
    }

    private void OnMinutesChanged(int minute)
    {
        string minutes = string.Empty;

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
        dateText.text = date.Day + "." + date.Month + "." + date.Year;
    }

    private void OnStartOvertime()
    {
        goHomeButton.SetActive(true);
    }

    public void OnGoHomeButtonClick()
    {
        timeSystem.EndDay();
        goHomeButton.SetActive(false);
    }
}
