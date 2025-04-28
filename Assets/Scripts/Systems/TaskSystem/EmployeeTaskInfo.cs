using UnityEngine;

[System.Serializable]
public class EmployeeTaskInfo
{
    [Tooltip("Название задачи")]
    public string taskName;

    [TextArea(5, 10)]
    [Tooltip("Описание задачи")]
    public string taskDescription;

    [Min(0)]
    [Tooltip("Время выполнения задачи в часах")]
    public int workTimeHours = 1;

    [Range(0,59)]
    [Tooltip("Время выполнения задачи в минутах")]
    public int workTimeMinutes = 1;

    [Tooltip("Тип задачи")]
    public EmployeeTaskType taskType;

    [Range(1,10)]
    [Tooltip("Сложность задачи")]
    public int complexity = 1;
}

public enum EmployeeTaskType
{
    Code,
    Docs,
    Testing
}
