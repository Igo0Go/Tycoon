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
            _employees ??= team.GetEmployees();
            return _employees;
        }
    }
        private List<Employee> _employees;

    public List<Employee> Recruts
    {
        get
        {
            _recruts ??= team.GetRecruts();
            return _recruts;
        }
    }
    private List<Employee> _recruts;

    public event Action<List<Employee>> TeamChanged;
    public event Action<List<Employee>> RecrutsChanged;
    public event Action<Employee> DismissEmployeeEvent;
    public event Action<Employee> NewEmployee;

    public void SubscribeEvents(TimeSystem timeSystem)
    {
        timeSystem.NewDayBeginning += AllStartDay;
        timeSystem.StartWork += AllToWork;
        timeSystem.StartLunch += AllToLunch;
        timeSystem.EndWork += EmployeesGoHome;
        timeSystem.EndOfDay += AllGoHome;

        TeamChanged?.Invoke(Employees);

        foreach (Employee e in Employees)
        {
            e.EmployeeInfoChanged += OnEmployeeChanged;
            e.EmployeeMaxFatigue += OnEmployeeMaxFatigue;
            e.EmployeeMaxStress += OnEmployeeMaxStress;
        }
        foreach (Employee e in Recruts)
        {
            e.EmployeeRecruting += AddRecrutToTeam;
        }
    }
    public void SetUp()
    {
        TeamChanged?.Invoke(Employees);
    }

    public void DismissEmployee(Employee employee)
    {
        Employees.Remove(employee);
        TeamChanged?.Invoke(Employees);
        DismissEmployeeEvent?.Invoke(employee);
    }

    public void AddRecrutToTeam(Employee e)
    {
        e.EmployeeRecruting -= AddRecrutToTeam;

        Recruts.Remove(e);
        Employees.Add(e);

        e.EmployeeInfoChanged += OnEmployeeChanged;
        e.EmployeeMaxFatigue += OnEmployeeMaxFatigue;
        e.EmployeeMaxStress += OnEmployeeMaxStress;

        TeamChanged?.Invoke(Employees);
        RecrutsChanged?.Invoke(Recruts);
        NewEmployee?.Invoke(e);
        e.GoToWork();
    }

    private void AllStartDay()
    {
        for (int i = 0; i < Employees.Count; i++)
        {
            Employee e = Employees[i];
            if(e.EmployeeMaxStatsCheck())
            {
                e.SetBaseSalaryStatus();
            }
            else
            {
                DismissEmployee(e);
                i--;
            }
        }
        TeamChanged?.Invoke(Employees);
    }
    private void AllToWork()
    {
        foreach (Employee e in Employees)
        {
            e.SetBaseSalaryStatus();
            e.GoToWork();
        }
        TeamChanged?.Invoke(Employees);
    }
    private void AllToLunch()
    {
        foreach (Employee e in Employees)
        {
            e.GoToLunch();
        }
    }
    private void EmployeesGoHome()
    {
        foreach (Employee e in Employees)
        {
            if(!e.OverTime)
                e.GoHome();
        }
        TeamChanged?.Invoke(Employees);
    }
    private void AllGoHome()
    {
        foreach (Employee e in Employees)
        {
            e.EndDayResult();
            e.SetBaseSalaryStatus();
            e.GoHome();
        }
        TeamChanged?.Invoke(Employees);
    }

    private void OnEmployeeChanged()
    {
        TeamChanged?.Invoke(Employees);
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
