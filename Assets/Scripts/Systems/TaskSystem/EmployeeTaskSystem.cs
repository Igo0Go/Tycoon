using System;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeTaskSystem : MonoBehaviour
{
    #region Поля и свойства

    /// <summary>
    /// Пакет с информацией о задачах
    /// </summary>
    public TaskDB taskDB;

    /// <summary>
    /// Список задач, которые можно назначить сотрудникам
    /// </summary>
    public List<EmployeeTask> Backlog { get; private set; } = new List<EmployeeTask>();

    private readonly Dictionary<Employee, List<EmployeeTask>> employeesAndTasks = new();
    private bool endOfProject = false;

    #endregion

    #region События

    public event Action<EmployeeTask> IncorrectTaskFound;
    public event Action<EmployeeTask> TaskToBaclog;
    public event Action ProjectComplete;

    #endregion

    #region Методы

    public void SubscribeEvents(EmployeeSystem employeeSystem, TimeSystem timeSystem)
    {
        employeeSystem.TeamChanged += OnEmployeeListChanged;
        employeeSystem.DismissEmployeeEvent += OnDismissEmployee;
        timeSystem.SpendTime += OnSkipTime;
    }
    public void SetUp()
    {
        Backlog.AddRange(taskDB.GetEmployeeTasks());
    }

    /// <summary>
    /// Добавить список задач в бэклог
    /// </summary>
    /// <param name="tasks">Новые задачи</param>
    public void AddTasksToBackLog(List<EmployeeTask> tasks)
    {
        Backlog.AddRange(tasks);
        foreach (EmployeeTask task in tasks)
        {
            TaskToBaclog?.Invoke(task);
        }
    }

    /// <summary>
    /// Добавить сотруднику новую задачу
    /// </summary>
    /// <param name="employee">Сотрудник</param>
    /// <param name="employeeTask">Задача</param>
    public void AddTaskToEmployee(Employee employee, EmployeeTask employeeTask)
    {
        employeesAndTasks[employee].Add(employeeTask);
        Backlog.Remove(employeeTask);
    }
    /// <summary>
    /// Убрать задачу у сотрудника
    /// </summary>
    /// <param name="employee">Сотрудник</param>
    /// <param name="task">Задача</param>
    public void RemoveTaskFromEmployee(Employee employee, EmployeeTask task)
    {
        if (employeesAndTasks[employee].Contains(task))
        {
            employeesAndTasks[employee].Remove(task);
            Backlog.Add(task);
        }
    }

    /// <summary>
    /// Запустить ожидание окончания проекта
    /// </summary>
    public void PrepareFinalizeProject()
    {
        endOfProject = true;
    }

    private void OnSkipTime(int minutes)
    {
        for (int i = 0; i < minutes; i++)
        {
            AllDoTasks();
        }
    }
    private void OnEmployeeListChanged(List<Employee> employees)
    {
        foreach (Employee employee in employees)
        {
            if (!employeesAndTasks.ContainsKey(employee))
            {
                employeesAndTasks.Add(employee, new List<EmployeeTask>());
            }
        }
    }
    private void OnDismissEmployee(Employee employee)
    {
        if (employeesAndTasks.ContainsKey(employee))
        {
            List<EmployeeTask> tasks = new();

            if (employee.CurrentTask != null)
            {
                employee.CurrentTask.ResetProgress();
                tasks.Add(employee.CurrentTask);
            }
            tasks.AddRange(employeesAndTasks[employee]);
            Backlog.AddRange(tasks);
            employeesAndTasks.Remove(employee);

            foreach (var item in tasks)
            {
                TaskToBaclog?.Invoke(item);
            }
        }
    }


    private void AllDoTasks()
    {
        foreach (Employee e in employeesAndTasks.Keys)
        {
            if (e.CurrentTask == null && employeesAndTasks[e].Count > 0)
            {
                e.CurrentTask = employeesAndTasks[e][0];
                employeesAndTasks[e].Remove(e.CurrentTask);

                GameUICenter.messageQueue.Log(e.Name + " берёт задачу " + e.CurrentTask.Name);

                if (e.CurrentTask.Type == EmployeeTaskType.Testing)
                {
                    e.CurrentTask.SetTesterToThisTask(e);
                }
                else
                {
                    e.CurrentTask.SetWorkerToThisTask(e);
                }
            }
            else if (e.CurrentTask != null)
            {
                if (e.CurrentTask.IsReady())
                {
                    GameUICenter.messageQueue.Log(e.Name + " выполняет задачу " + e.CurrentTask.Name);


                    if (e.CurrentTask.Testing)
                    {
                        if (!e.CurrentTask.FinalTestingThisTask())
                        {
                            Backlog.Add(e.CurrentTask);
                            TaskToBaclog?.Invoke(e.CurrentTask);
                            IncorrectTaskFound?.Invoke(e.CurrentTask);
                            GameUICenter.messageQueue.Log(e.Name + " сообщает, что задача " +
                                e.CurrentTask.Name + " выполнена некорректно.");
                        }

                        employeesAndTasks[e].Remove(e.CurrentTask);
                        e.CurrentTask = null;
                        CheckProgress();
                    }
                    else
                    {
                        e.CurrentTask.FinalWorkForThisTask();
                        Backlog.Add(e.CurrentTask);
                        TaskToBaclog?.Invoke(e.CurrentTask);
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
    private void CheckProgress()
    {
        if (Backlog.Count > 0)
        {
            return;
        }

        foreach (var task in employeesAndTasks)
        {
            if (task.Value.Count > 0)
            {
                return;
            }

            if (task.Key.CurrentTask != null)
            {
                return;
            }
        }

        if (endOfProject)
        {
            GameUICenter.messageQueue.PrepareMessage("Успех!", "Вы справились со всеми задачами!");
            ProjectComplete?.Invoke();
        }
        else
        {
            GameUICenter.messageQueue.PrepareMessage("Успех!", "Все текущие задачи выполнены. Но пока ждём." +
                " Надо показать текущий прогресс заказчику. Может, будут правки!");
        }
    }

    #endregion
}
