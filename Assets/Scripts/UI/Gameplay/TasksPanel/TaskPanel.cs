using System.Collections.Generic;
using UnityEngine;

public class TaskPanel : MonoBehaviour, IUIPanel
{
    [SerializeField]
    private GameObject panel;

    [SerializeField]
    private UIDropQueueSlot allTasksSlot;
    [SerializeField]
    private TaskCardUIItem taskCard;
    [SerializeField]
    private EmployeeTaskCardHolder cardHolderPrefab;
    [SerializeField]
    private Transform taskCardsHodlersContainer;

    private EmployeeTaskSystem taskSystem;

    public void SubscribeEvents(EmployeeTaskSystem employeeTaskSystem, EmployeeSystem employeeSystem)
    {
        taskSystem = employeeTaskSystem;
        taskSystem.taskToBaclog += SpawnTaskCard;
        employeeSystem.dismissEmployee += OnEmployeeRemove;
        employeeSystem.newEmployee += SpawnEmployee;
    }

    public void SetUp(EmployeeSystem employeeSystem)
    {
        SpawnTaskCardsForBaclog(taskSystem.Backlog);
        SpawnEmployees(employeeSystem.Employees);
        HidePanel();
    }

    public void OnPanelButtonClick()
    {
        if(panel.activeSelf)
        {
            HidePanel();
        }
        else
        {
            ShowPanel();
        }
    }
    public void HidePanel()
    {
        panel.SetActive(false);
    }
    public void ShowPanel()
    {
        panel.SetActive(true);
    }

    private void SpawnTaskCardsForBaclog(List<EmployeeTask> tasks)
    {
        foreach (EmployeeTask employeeTask in tasks)
        {
            SpawnTaskCard(employeeTask);
        }
    }
    private void SpawnTaskCard(EmployeeTask task)
    {
        TaskCardUIItem curentTask = Instantiate(taskCard).GetComponent<TaskCardUIItem>();
        curentTask.InitTask(task);
        UIDragItem item = curentTask.GetComponent<UIDragItem>();
        allTasksSlot.AddItemToThisSlot(item);
    }

    private void SpawnEmployees(List<Employee> employees)
    {
        for (int i = 0; i < employees.Count; i++)
        {
            Employee employee = employees[i];
            SpawnEmployee(employee);
        }
    }
    private void SpawnEmployee(Employee employee)
    {
        EmployeeTaskCardHolder holder = 
            Instantiate(cardHolderPrefab, taskCardsHodlersContainer).GetComponent<EmployeeTaskCardHolder>();

        holder.Init(employee);
        holder.taskAdded += OnEmployeeAddTask;
        holder.taskRemoved += OnEmployeeRemoveTask;
    }

    private void OnEmployeeRemove(Employee employee)
    {
        for (int i = 0; i < taskCardsHodlersContainer.childCount; i++)
        {
            EmployeeTaskCardHolder holder = taskCardsHodlersContainer.GetChild(i).GetComponent<EmployeeTaskCardHolder>();

            if(holder.Employee == employee)
            {
                Destroy(holder.gameObject);
                break;
            }
        }
    }
    private void OnEmployeeAddTask(Employee employee, EmployeeTask task)
    {
        taskSystem.AddTaskToEmployee(employee, task);
    }
    private void OnEmployeeRemoveTask(Employee employee, EmployeeTask task)
    {
        taskSystem.RemoveTaskFromEmployee(employee, task);
    }

}
