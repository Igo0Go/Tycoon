using System;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeSystem : MonoBehaviour
{
    #region ���� � ��������
    [SerializeField]
    [Tooltip("����� � �������� �����������")]
    private EmployeeTeam team;

    [SerializeField]
    [Tooltip("����� � ����������� ��������� �����������")]
    private EmployeeStatsSettingsPack statsSettingsPack;

    /// <summary>
    /// ��������� �����������
    /// </summary>
    public List<Employee> Employees
    {
        get
        {
            _employees ??= team.GetEmployees();
            return _employees;
        }
    }
    private List<Employee> _employees;

    /// <summary>
    /// ��������� ��������
    /// </summary>
    public List<Employee> Recruts
    {
        get
        {
            _recruts ??= team.GetRecruts();
            return _recruts;
        }
    }
    private List<Employee> _recruts;
    #endregion

    #region �������
    public event Action<List<Employee>> TeamChanged;
    public event Action<List<Employee>> RecrutsChanged;
    public event Action<Employee> DismissEmployeeEvent;
    public event Action<Employee> NewEmployee;
    #endregion

    #region ������

    #region �����
    public void SubscribeEvents(TimeSystem timeSystem)
    {
        timeSystem.NewDayBeginning += AllStartDay;
        timeSystem.StartWork += AllToWork;
        timeSystem.StartLunch += AllToLunch;
        timeSystem.EndWork += NotOvertimeEmployeesGoHome;
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
        statsSettingsPack.AcceptThisSettings();
        TeamChanged?.Invoke(Employees);
    }

    /// <summary>
    /// ������� ���������
    /// </summary>
    /// <param name="employee">��������� �� ����������</param>
    public void DismissEmployee(Employee employee)
    {
        Employees.Remove(employee);
        TeamChanged?.Invoke(Employees);
        DismissEmployeeEvent?.Invoke(employee);
    }

    /// <summary>
    /// �������� ������� � �������, �������� �������� �� �����������
    /// </summary>
    /// <param name="e">��������� ��� ��������������</param>
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
    #endregion

    #region ������� ���� �����������
    /// <summary>
    /// ���������� ����� ���� ��� ���� �����������, ��������� ������������ ����������. 
    /// ������� � ����������� �������� ���������. 
    /// ������� � ������������ ���������� �������
    /// </summary>
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

    /// <summary>
    /// ��������� ���� ��������, ��������� ������� ��������� ������
    /// </summary>
    private void AllToWork()
    {
        foreach (Employee e in Employees)
        {
            e.SetBaseSalaryStatus();
            e.GoToWork();
        }
        TeamChanged?.Invoke(Employees);
    }

    /// <summary>
    /// ��������� ���� �� ����
    /// </summary>
    private void AllToLunch()
    {
        foreach (Employee e in Employees)
        {
            e.GoToLunch();
        }
    }

    /// <summary>
    /// ��������� ����� �����������, ������� �� �������� �����������
    /// </summary>
    private void NotOvertimeEmployeesGoHome()
    {
        foreach (Employee e in Employees)
        {
            if(!e.OverTime)
                e.GoHome();
        }
        TeamChanged?.Invoke(Employees);
    }

    /// <summary>
    /// ��� ������� ���������� ���������� ���������� ���, ���������� ������� 
    /// ��������� ������ � ��������� �����
    /// </summary>
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
    #endregion

    #region ������� �� �������
    private void OnEmployeeChanged()
    {
        TeamChanged?.Invoke(Employees);
    }
    private void OnEmployeeMaxStress(Employee e)
    {
        GameUICenter.messageQueue.PrepareMessage(e.Name + " ����������", 
            e.EmployeeSpeachPack.employeeMaxStressMessege);
    }
    private void OnEmployeeMaxFatigue(Employee e)
    {
        GameUICenter.messageQueue.PrepareMessage(e.Name + " �� ����� �� ������", 
            e.EmployeeSpeachPack.GetRandomMaxFatigueSpeach());
    }
    #endregion

    #endregion
}
