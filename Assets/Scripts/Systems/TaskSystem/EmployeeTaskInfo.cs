using UnityEngine;

[System.Serializable]
public class EmployeeTaskInfo
{
    [Tooltip("�������� ������")]
    public string taskName;

    [TextArea(5, 10)]
    [Tooltip("�������� ������")]
    public string taskDescription;

    [Min(0)]
    [Tooltip("����� ���������� ������ � �����")]
    public int workTimeHours = 1;

    [Range(0,59)]
    [Tooltip("����� ���������� ������ � �������")]
    public int workTimeMinutes = 1;

    [Tooltip("��� ������")]
    public EmployeeTaskType taskType;

    [Range(1,10)]
    [Tooltip("��������� ������")]
    public int complexity = 1;
}

public enum EmployeeTaskType
{
    Code,
    Docs,
    Testing
}
