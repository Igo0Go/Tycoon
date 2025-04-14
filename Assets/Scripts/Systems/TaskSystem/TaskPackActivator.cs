using UnityEngine;

public class TaskPackActivator : MonoBehaviour
{
    [SerializeField]
    private TaskDB taskDB;

    private EmployeeTaskSystem employeeTaskSystem;

    private void Awake()
    {
        employeeTaskSystem = FindObjectOfType<EmployeeTaskSystem>();
    }

    public void ActivatePack()
    {
        employeeTaskSystem.AddTasksToBackLog(taskDB.GetEmployeeTasks());
    }
}
