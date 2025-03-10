using System;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeSystem : MonoBehaviour
{
    [SerializeField]
    private EmployeeTeam team;

    [SerializeField, TextArea(5, 10)]
    private string employeeMaxStressMessege;

    [SerializeField, TextArea(5, 10)]
    private string employeeMaxFatigueMessege;

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

    public List<Employee> Recruts
    {
        get
        {
            if (_recruts == null)
            {
                _recruts = team.GetRecruts();
            }
            return _recruts;
        }
    }
    private List<Employee> _recruts;

    public event Action<List<Employee>> teamChanged;
    public event Action<List<Employee>> recrutsChanged;
    public event Action<Employee> dismissEmployee;

    public void SubscribeEvents(TimeSystem timeSystem)
    {
        timeSystem.startNewDay += AllStartDay;
        timeSystem.startWork += AllToWork;
        timeSystem.startLunch += AllToLunch;
        timeSystem.endWork += AllEndWork;
        timeSystem.endDay += AllGoHome;

        teamChanged?.Invoke(Employees);

        foreach (Employee e in Employees)
        {
            e.employeeChanged += OnEmployeeChanged;
            e.employeeMaxFatigue += OnEmployeeMaxFatigue;
            e.employeeMaxStress += OnEmployeeMaxStress;
        }
        foreach (Employee e in Recruts)
        {
            e.employeeRecruting += AddRecrutToTeam;
        }
    }
    public void SetUp()
    {
        teamChanged?.Invoke(Employees);
    }

    public void DismissEmployee(Employee employee)
    {
        Employees.Remove(employee);
        teamChanged?.Invoke(Employees);
        dismissEmployee?.Invoke(employee);
    }

    public void AddRecrutToTeam(Employee e)
    {
        e.employeeRecruting -= AddRecrutToTeam;

        Recruts.Remove(e);
        Employees.Add(e);

        e.employeeChanged += OnEmployeeChanged;
        e.employeeMaxFatigue += OnEmployeeMaxFatigue;
        e.employeeMaxStress += OnEmployeeMaxStress;

        teamChanged?.Invoke(Employees);
        recrutsChanged?.Invoke(Recruts);
    }

    private void AllStartDay()
    {
        for (int i = 0; i < Employees.Count; i++)
        {
            Employee e = Employees[i];
            if(e.EmployeeInTheTeam())
            {
                e.SetBaseSalaryStatus();
            }
            else
            {
                DismissEmployee(e);
                i--;
            }
        }
        teamChanged?.Invoke(Employees);
    }
    private void AllToWork()
    {
        foreach (Employee e in Employees)
        {
            e.SetBaseSalaryStatus();
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
            e.EndDayResult();
            e.SetBaseSalaryStatus();
            e.StopWorkTime();
        }
        teamChanged?.Invoke(Employees);
    }
    private void AllToLunch()
    {
        foreach(Employee e in Employees)
        {
            e.StopWorkTime();
        }
    }

    private void OnEmployeeChanged()
    {
        teamChanged?.Invoke(Employees);
    }
    private void OnEmployeeMaxStress(Employee e)
    {
        GameUICenter.messageQueue.PrepareMessage(e.Name + " уволняется", employeeMaxStressMessege);
    }
    private void OnEmployeeMaxFatigue(Employee e)
    {
        GameUICenter.messageQueue.PrepareMessage(e.Name + " не вышел на работу", employeeMaxFatigueMessege);
    }
}
