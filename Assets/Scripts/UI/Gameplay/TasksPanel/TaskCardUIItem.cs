using UnityEngine;
using TMPro;

public class TaskCardUIItem : TooltipElement
{
    [SerializeField]
    private TMP_Text taskNameText;
    [SerializeField]
    private TMP_Text taskTypeText;
    [SerializeField]
    private TMP_Text taskTimeToCompleteText;
    [SerializeField]
    private GameObject startTaskButton;

    public EmployeeTask Task { get; private set; }

    public void InitTask(EmployeeTask task)
    {
        Task = task;
        startTaskButton.SetActive(false);
        DrawTaskinfo();
    }

    protected override string GetStringForTooltip()
    {
        return Task.Name + "\n\n" + Task.Description + "\n\nСложность:" + Task.Complexity;
    }

    private void DrawTaskinfo()
    {
        taskNameText.text = Task.Name + (Task.Testing ? " test[" + Task.IsCorrectTask + "]": "");
        taskTypeText.text = Task.Type.ToString();
        taskTimeToCompleteText.text = (Task.AllTaskTime / 60) + " ч. " + (Task.AllTaskTime % 60) + "м.";
    }
}
