using TMPro;
using UnityEngine;

public class TimerDisplay : MonoBehaviour
{
    [SerializeField]
    private TMP_Text timerText;

    private TimeSystem timeSystem;

    private void Awake()
    {
        timeSystem = FindObjectOfType<TimeSystem>();
        timeSystem.hoursChanged += OnHoursChanged;
        timeSystem.minutesChanged += OnMinutesChanged;
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
}
