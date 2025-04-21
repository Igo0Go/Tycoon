using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="IgoGo/TaskDB")]
public class TaskDB : ScriptableObject
{
    [SerializeField]
    [Tooltip("Список заготовок для задач")]
    private List<EmployeeTaskInfo> employeTaskInfoItems;

    /// <summary>
    /// Сформировать из заготовок экземпляры задач
    /// </summary>
    /// <returns>Список экземпляров задач</returns>
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