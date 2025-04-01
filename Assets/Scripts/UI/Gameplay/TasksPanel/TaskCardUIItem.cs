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
        return Task.Name + "\n\n" + Task.Description + "\n\n���������:" + Task.Complexity;
    }

    private void DrawTaskinfo()
    {
        taskNameText.text = Task.Name + (Task.Testing ? " T[" + (Task.IsCorrectTask? "+" : "x") + "]": "");
        taskTypeText.text = Task.Type.ToString();

        if(Task.Testing)
        {
            taskTimeToCompleteText.text = (Task.TestingTime / 60) + " �. " + (Task.TestingTime % 60) + "�.";
        }
        else
        {
            taskTimeToCompleteText.text = (Task.AllTaskTime / 60) + " �. " + (Task.AllTaskTime % 60) + "�.";
        }
    }
}
