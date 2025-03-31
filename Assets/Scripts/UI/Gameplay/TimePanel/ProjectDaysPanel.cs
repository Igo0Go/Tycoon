using TMPro;
using UnityEngine;

public class ProjectDaysPanel : MonoBehaviour, IUIPanel
{
    [SerializeField]
    private GameObject panel;

    [SerializeField]
    private TMP_Text projectDaysLimitText;

    public void SubscribeEvents(TimeSystem timeSystem)
    {
        timeSystem.currentDayChanged += RedrawInfo;
    }

    public void HidePanel()
    {
        panel.SetActive(false);
    }

    public void ShowPanel()
    {
        panel.SetActive(true);
    }

    public void RedrawInfo(int days)
    {
        projectDaysLimitText.text = "ƒней осталось: " + days;
    }


}
