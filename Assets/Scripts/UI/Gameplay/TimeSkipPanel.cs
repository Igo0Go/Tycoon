using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeSkipPanel : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField hoursInputField;
    [SerializeField]
    private TMP_InputField minutesInputField;
    [SerializeField]
    private Button okButton;

    private TimeSystem timeSystem;

    private int currentHour;
    private int currentMinute;

    private void Awake()
    {
        timeSystem = FindObjectOfType<TimeSystem>();
        timeSystem.hoursChanged += SetHourTextForSkipPanel;
        timeSystem.minutesChanged += SetMinuteTextForSkipPanel;

        hoursInputField.onEndEdit.AddListener(OnInputFieldEndEdit);
        minutesInputField.onEndEdit.AddListener(OnInputFieldEndEdit);
    }

    public void SetUpPanel()
    {
        hoursInputField.text = (timeSystem.CurrentHour + 1).ToString();
        minutesInputField.text = timeSystem.CurrentMinute.ToString();
        OnInputFieldEndEdit(string.Empty);
    }
    public void PlusHour()
    {
        currentHour++;
        hoursInputField.text = currentHour.ToString();
        OnInputFieldEndEdit(string.Empty);
    }
    public void MinusHour()
    {
        currentHour--;
        hoursInputField.text = currentHour.ToString();
        OnInputFieldEndEdit(string.Empty);
    }
    public void PlusMinute()
    {
        currentMinute++;
        minutesInputField.text = currentMinute.ToString();
        OnInputFieldEndEdit(string.Empty);
    }
    public void MinusMinute()
    {
        currentMinute--;
        minutesInputField.text = currentMinute.ToString();
        OnInputFieldEndEdit(string.Empty);
    }
    public void SkipTime()
    {
        timeSystem.CurrentHour = currentHour;
        timeSystem.CurrentMinute = currentMinute;
    }

    private void SetHourTextForSkipPanel(int hours)
    {
        hoursInputField.text = hours.ToString();
        currentHour = hours;
        okButton.interactable = CheckSkipTime();
    }
    private void SetMinuteTextForSkipPanel(int minute)
    {
        minutesInputField.text = minute.ToString();
        currentMinute = minute;
        okButton.interactable = CheckSkipTime();
    }

    private void OnInputFieldEndEdit(string value)
    {
        ClampTime();
        okButton.interactable = CheckSkipTime();
    }

    private void ClampTime()
    {
        currentHour = Mathf.Clamp(int.Parse(hoursInputField.text), 0, TimeSystem.hourCycle - 1);
        currentMinute = Mathf.Clamp(int.Parse(minutesInputField.text), 0, TimeSystem.cycle - 1);

        hoursInputField.text = currentHour.ToString();
        minutesInputField.text = currentMinute.ToString();
    }

    private bool CheckSkipTime()
    {
        if(currentHour < timeSystem.CurrentHour || 
            (currentHour == timeSystem.CurrentHour && currentMinute <= timeSystem.CurrentMinute))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
