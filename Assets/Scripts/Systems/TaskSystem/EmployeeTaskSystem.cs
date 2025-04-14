using System;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeTaskSystem : MonoBehaviour
{
    [SerializeField]
    public TaskDB taskDB;

    private Dictionary<Employee, List<EmployeeTask>> employeesAndTasks = 
        new Dictionary<Employee, List<EmployeeTask>>();
    public List<EmployeeTask> Backlog { get; private set; }  = new List<EmployeeTask>();

    public event Action<EmployeeTask> incorrectTaskFound;
    public event Action<EmployeeTask> taskToBaclog;
    public event Action taskTick;
    public event Action projectComplete;

    private bool endOfProject = false;

    public void SetUp()
    {
        Backlog.AddRange(taskDB.GetEmployeeTasks());
    }

    public void AddTasksToBackLog(List<EmployeeTask> tasks)
    {
        Backlog.AddRange(tasks);
        foreach (EmployeeTask task in tasks)
        {
            taskToBaclog?.Invoke(task);
        }
    }

    public void SubscribeEvents(EmployeeSystem employeeSystem, TimeSystem timeSystem)
    {
        employeeSystem.teamChanged += OnEmployeeListChanged;
        employeeSystem.dismissEmployee += OnDismissEmployee;
        timeSystem.spendTime += OnSkipTime;
    }

    private void OnSkipTime(int minutes)
    {
        for (int i = 0; i < minutes; i++)
        {
            AllDoTasks();
        }
    }

    private void AllDoTasks()
    {
        foreach (Employee e in employeesAndTasks.Keys)
        {
            if(e.CurrentTask == null && employeesAndTasks[e].Count > 0)
            {
                e.CurrentTask = employeesAndTasks[e][0];
                employeesAndTasks[e].Remove(e.CurrentTask);

                GameUICenter.messageQueue.Log(e.Name + " берёт задачу " + e.CurrentTask.Name);

                if(e.CurrentTask.Type == EmployeeTaskType.Testing)
                {
                    e.CurrentTask.StartTestingThisTask(e);
                }
                else
                {
                    e.CurrentTask.StartThisTask(e);
                }
            }
            else if(e.CurrentTask != null)
            {
                if(e.CurrentTask.IsReady())
                {
                    GameUICenter.messageQueue.Log(e.Name + " выполняет задачу " + e.CurrentTask.Name);
          

                    if (e.CurrentTask.Testing)
                    {
                        if(!e.CurrentTask.EndTestingThisTask(e))
                        {
                            Backlog.Add(e.CurrentTask);
                            taskToBaclog?.Invoke(e.CurrentTask);
                            incorrectTaskFound?.Invoke(e.CurrentTask);
                            GameUICenter.messageQueue.Log(e.Name + " сообщает, что задача " + 
                                e.CurrentTask.Name + " выполнена некорректно.");
                        }

                        employeesAndTasks[e].Remove(e.CurrentTask);
                        e.CurrentTask = null;
                        CheckProgress();
                    }
                    else
                    {
                        e.CurrentTask.EndWorkForThisTask(e);
                        Backlog.Add(e.CurrentTask);
                        taskToBaclog?.Invoke(e.CurrentTask);
                        employeesAndTasks[e].Remove(e.CurrentTask);

                        e.CurrentTask = null;
                    }
                }
                else
                {
                    e.DoWork();
                }
            }
        }
    }

    public void AddTaskToEmployee(Employee employee, EmployeeTask employeeTask)
    {
        employeesAndTasks[employee].Add(employeeTask);
        Backlog.Remove(employeeTask);
    }
    public void RemoveTaskFromEmployee(Employee employee, EmployeeTask task)
    {
        if (employeesAndTasks[employee].Contains(task))
        {
            employeesAndTasks[employee].Remove(task);
            Backlog.Add(task);
        }
    }

    public void PrepareEndProject()
    {
        endOfProject = true;
    }

    private void OnEmployeeListChanged(List<Employee> employees)
    {
        foreach (Employee employee in employees)
        {
            if(!employeesAndTasks.ContainsKey(employee))
            {
                employeesAndTasks.Add(employee, new List<EmployeeTask>());
            }
        }
    }
    private void OnDismissEmployee(Employee employee)
    {
        if(employeesAndTasks.ContainsKey(employee))
        {
            List<EmployeeTask> tasks = new List<EmployeeTask>();
            
            if(employee.CurrentTask != null)
            {
                employee.CurrentTask.ResetProgress();
                tasks.Add(employee.CurrentTask);
            }
            tasks.AddRange(employeesAndTasks[employee]);
            Backlog.AddRange(tasks);
            employeesAndTasks.Remove(employee);

            foreach (var item in tasks)
            {
                taskToBaclog?.Invoke(item);
            }
        }
    }

    private void CheckProgress()
    {
        if(Backlog.Count > 0)
        {
            return;
        }

        foreach (var task in employeesAndTasks)
        {
            if (task.Value.Count > 0)
            {
                return;
            }

            if(task.Key.CurrentTask != null)
            {
                return;
            }
        }

        if(endOfProject)
        {
            GameUICenter.messageQueue.PrepareMessage("Успех!", "Вы справились со всеми задачами!");
            projectComplete?.Invoke();
        }
        else
        {
            GameUICenter.messageQueue.PrepareMessage("Успех!", "Все текущие задачи выполнены. Но пока ждём." +
                " Надо показать текущий прогресс заказчику. Может, будут правки!");
        }
    }
}
