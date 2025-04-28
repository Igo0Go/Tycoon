using System;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeTaskSystem : MonoBehaviour
{
    #region ���� � ��������

    /// <summary>
    /// ����� � ����������� � �������
    /// </summary>
    public TaskDB taskDB;

    /// <summary>
    /// ������ �����, ������� ����� ��������� �����������
    /// </summary>
    public List<EmployeeTask> Backlog { get; private set; } = new List<EmployeeTask>();

    private readonly Dictionary<Employee, List<EmployeeTask>> employeesAndTasks = new();
    private bool endOfProject = false;

    #endregion

    #region �������

    public event Action<EmployeeTask> IncorrectTaskFound;
    public event Action<EmployeeTask> TaskToBaclog;
    public event Action ProjectComplete;

    #endregion

    #region ������

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
    /// �������� ������ ����� � ������
    /// </summary>
    /// <param name="tasks">����� ������</param>
    public void AddTasksToBackLog(List<EmployeeTask> tasks)
    {
        Backlog.AddRange(tasks);
        foreach (EmployeeTask task in tasks)
        {
            TaskToBaclog?.Invoke(task);
        }
    }

    /// <summary>
    /// �������� ���������� ����� ������
    /// </summary>
    /// <param name="employee">���������</param>
    /// <param name="employeeTask">������</param>
    public void AddTaskToEmployee(Employee employee, EmployeeTask employeeTask)
    {
        employeesAndTasks[employee].Add(employeeTask);
        Backlog.Remove(employeeTask);
    }
    /// <summary>
    /// ������ ������ � ����������
    /// </summary>
    /// <param name="employee">���������</param>
    /// <param name="task">������</param>
    public void RemoveTaskFromEmployee(Employee employee, EmployeeTask task)
    {
        if (employeesAndTasks[employee].Contains(task))
        {
            employeesAndTasks[employee].Remove(task);
            Backlog.Add(task);
        }
    }

    /// <summary>
    /// ��������� �������� ��������� �������
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

                GameUICenter.messageQueue.Log(e.Name + " ���� ������ " + e.CurrentTask.Name);

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
                    GameUICenter.messageQueue.Log(e.Name + " ��������� ������ " + e.CurrentTask.Name);


                    if (e.CurrentTask.Testing)
                    {
                        if (!e.CurrentTask.FinalTestingThisTask())
                        {
                            Backlog.Add(e.CurrentTask);
                            TaskToBaclog?.Invoke(e.CurrentTask);
                            IncorrectTaskFound?.Invoke(e.CurrentTask);
                            GameUICenter.messageQueue.Log(e.Name + " ��������, ��� ������ " +
                                e.CurrentTask.Name + " ��������� �����������.");
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
            GameUICenter.messageQueue.PrepareMessage("�����!", "�� ���������� �� ����� ��������!");
            ProjectComplete?.Invoke();
        }
        else
        {
            GameUICenter.messageQueue.PrepareMessage("�����!", "��� ������� ������ ���������. �� ���� ���." +
                " ���� �������� ������� �������� ���������. �����, ����� ������!");
        }
    }

    #endregion
}
