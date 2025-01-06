using TMPro;
using UnityEngine;

public class TimerDisplay : MonoBehaviour
{
    [SerializeField]
    private TMP_Text timerText;

    private void Awake()
    {
        FindObjectOfType<TimeSystem>().timeChanged += OnTimeChanged;
    }

    private void OnTimeChanged(int hour, int minute)
    {
        string minutes = string.Empty;

        if(minute < 10)
        {
            minutes = "0" + minute;
        }
        else
        {
            minutes = minute.ToString();
        }

        timerText.text = hour.ToString() + ":" + minutes;
    }
}
