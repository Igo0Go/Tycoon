using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeSkipPanel : MonoBehaviour, IUIPanel
{
    [SerializeField]
    private GameObject panelObject;
    [SerializeField]
    private TMP_InputField hoursInputField;
    [SerializeField]
    private TMP_InputField minutesInputField;
    [SerializeField]
    private Button okButton;

    private TimeSystem timeSystem;

    private int currentHour;
    private int currentMinute;

    public void SubscribeEvents(TimeSystem timeSystem)
    {
        this.timeSystem = timeSystem;
        timeSystem.MinuteChanged += (i)=> CheckButtonActive();
        hoursInputField.onEndEdit.AddListener(OnInputFieldEndEdit);
        minutesInputField.onEndEdit.AddListener(OnInputFieldEndEdit);
    }
    public void SetUp()
    {
        hoursInputField.text = (timeSystem.CurrentHour + 1).ToString();
        minutesInputField.text = timeSystem.CurrentMinute.ToString();
        OnInputFieldEndEdit(string.Empty);
        panelObject.SetActive(false);
    }
    public void ShowPanel()
    {
        panelObject.SetActive(true);
        SetHourTextForSkipPanel(timeSystem.CurrentHour);
        SetMinuteTextForSkipPanel(timeSystem.CurrentMinute);
    }
    public void HidePanel()
    {
        panelObject.SetActive(false);
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
        currentHour = Mathf.Clamp(int.Parse(hoursInputField.text), 0, TimeSystem.hourCycle - 1);
        currentMinute = Mathf.Clamp(int.Parse(minutesInputField.text), 0, TimeSystem.minuteCycle - 1);

        timeSystem.SkipTimeToThis(currentHour, currentMinute);
        panelObject.SetActive(false);
    }
    private void ClampTime()
    {
        currentHour = Mathf.Clamp(int.Parse(hoursInputField.text), 0, TimeSystem.hourCycle - 1);
        currentMinute = Mathf.Clamp(int.Parse(minutesInputField.text), 0, TimeSystem.minuteCycle - 1);

        hoursInputField.text = currentHour.ToString();
        minutesInputField.text = currentMinute.ToString();
    }

    private void SetHourTextForSkipPanel(int hours)
    {
        hoursInputField.text = hours.ToString();
        currentHour = hours;
        CheckButtonActive();
    }
    private void SetMinuteTextForSkipPanel(int minute)
    {
        minutesInputField.text = minute.ToString();
        currentMinute = minute;
        CheckButtonActive();
    }

    private void OnInputFieldEndEdit(string value)
    {
        ClampTime();
        CheckButtonActive();
    }

    private bool CheckSkipTime()
    {
        return timeSystem.EqulasTime(currentHour, currentMinute) > 0;
    }
    private void CheckButtonActive()
    {
        okButton.interactable = CheckSkipTime();
    }
}
