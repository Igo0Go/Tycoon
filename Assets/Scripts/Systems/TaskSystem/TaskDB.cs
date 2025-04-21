using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="IgoGo/TaskDB")]
public class TaskDB : ScriptableObject
{
    [SerializeField]
    [Tooltip("������ ��������� ��� �����")]
    private List<EmployeeTaskInfo> employeTaskInfoItems;

    /// <summary>
    /// ������������ �� ��������� ���������� �����
    /// </summary>
    /// <returns>������ ����������� �����</returns>
    public List<EmployeeTask> GetEmployeeTasks()
    {
        List<EmployeeTask> tasks = new();
        foreach (var item in employeTaskInfoItems)
        {
            tasks.Add(new EmployeeTask(item));
        }
        return tasks;
    }
}