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
        timeSystem.ProjectDayLimitChanged += RedrawInfo;
    }
    public void ShowPanel()
    {
        panel.SetActive(true);
    }
    public void HidePanel()
    {
        panel.SetActive(false);
    }

    public void RedrawInfo(int days)
    {
        if(days >= 0)
        {
            projectDaysLimitText.text = "ƒней осталось: " + days;
        }
        else
        {
            projectDaysLimitText.text = "ƒень сдачи проекта";
        }
    }
}
