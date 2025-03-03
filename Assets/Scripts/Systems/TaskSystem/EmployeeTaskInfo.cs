using UnityEngine;

[System.Serializable]
public class EmployeeTaskInfo
{
    public string taskName;
    [TextArea(5, 10)]
    public string taskDescription;
    [Min(0)]
    public int workTimeHours = 1;
    [Range(0,59)]
    public int workTimeMinutes = 1;
    public EmployeeTaskType taskType;
    [Range(1,10)]
    public int complexity = 1;
}

public enum EmployeeTaskType
{
    Code,
    Docs,
    Testing
}
