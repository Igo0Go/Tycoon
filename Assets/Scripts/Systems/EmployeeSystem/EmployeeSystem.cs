using System;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeSystem : MonoBehaviour
{
    [SerializeField]
    private EmployeeTeam team;

    private List<Employee> employees;

    public event Action<List<Employee>> teamChanged;

    public void SubscribeEvents(TimeSystem timeSystem)
    {
        timeSystem.endDay += AllGoHome;
        timeSystem.startWork += AllToWork;
        teamChanged?.Invoke(employees);
    }
    public void SetUp()
    {
        employees = team.GetEmployees();
        employees[0].SetHospitalSalaryStatus();
        teamChanged?.Invoke(employees);
    }

    private void AllToWork()
    {
        foreach (Employee e in employees)
        {
            e.ToWork();
        }
        teamChanged?.Invoke(employees);
    }
    private void AllGoHome()
    {
        foreach (Employee e in employees)
        {
            e.GoHome();
        }
        teamChanged?.Invoke(employees);
    }
}
