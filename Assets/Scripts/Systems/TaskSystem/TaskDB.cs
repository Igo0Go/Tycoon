using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="IgoGo/TaskDB")]
public class TaskDB : ScriptableObject
{
    [SerializeField]
    private List<EmployeeTaskInfo> employeTaskInfoItems;

    public List<EmployeeTask> GetEmployeeTasks()
    {
        List<EmployeeTask> tasks = new List<EmployeeTask>();
        foreach (var item in employeTaskInfoItems)
        {
            tasks.Add(new EmployeeTask(item));
        }
        return tasks;
    }
}
