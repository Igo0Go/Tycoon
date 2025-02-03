using System;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeSystem : MonoBehaviour
{
    [SerializeField]
    private EmployeeTeam team;

    public List<Employee> Employees
    {
        get
        {
            if(_employees == null)
            {
                _employees = team.GetEmployees();
            }
            return _employees;
        }
    }
        private List<Employee> _employees;

    public event Action<List<Employee>> teamChanged;

    public void SubscribeEvents(TimeSystem timeSystem)
    {
        timeSystem.startNewDay += AllToBaseState;
        timeSystem.startWork += AllToWork;
        timeSystem.endWork += AllEndWork;
        timeSystem.endDay += AllGoHome;

        teamChanged?.Invoke(Employees);

        foreach (Employee e in Employees)
        {
            e.employeeChanged += OnEmployeeChanged;
        }
    }
    public void SetUp()
    {
        Employees[2].SetOvertimeSalaryStatus();
        teamChanged?.Invoke(Employees);
    }

    public void DismissEmployee(Employee employee)
    {
        Employees.Remove(employee);
        teamChanged?.Invoke(Employees);
    }

    private void AllToBaseState()
    {
        foreach (Employee e in Employees)
        {
            e.SetBaseSalaryStatus();
        }
        teamChanged?.Invoke(Employees);
    }
    private void AllToWork()
    {
        foreach (Employee e in Employees)
        {
            e.ToWork();
        }
        teamChanged?.Invoke(Employees);
    }
    private void AllEndWork()
    {
        foreach (Employee e in Employees)
        {
            if(!e.OverTime)
                e.StopWorkTime();
        }
        teamChanged?.Invoke(Employees);
    }
    private void AllGoHome()
    {
        foreach (Employee e in Employees)
        {
            e.SetBaseSalaryStatus();
            e.StopWorkTime();
        }
        teamChanged?.Invoke(Employees);
    }

    private void OnEmployeeChanged()
    {
        teamChanged?.Invoke(Employees);
    }
}
