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
        timeSystem.endDay += AllGoHome;
        timeSystem.startWork += AllToWork;
        teamChanged?.Invoke(Employees);

        foreach (Employee e in Employees)
        {
            e.employeeChanged += OnEmployeeChanged;
        }
    }
    public void SetUp()
    {
        Employees[0].SetHospitalSalaryStatus();
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
    private void AllGoHome()
    {
        foreach (Employee e in Employees)
        {
            e.GoHome();
        }
        teamChanged?.Invoke(Employees);
    }

    private void OnEmployeeChanged()
    {
        teamChanged?.Invoke(Employees);
    }
}
