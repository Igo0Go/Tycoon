using System;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeSystem : MonoBehaviour
{
    #region Поля и свойства
    [SerializeField]
    [Tooltip("Пакет с командой сотрудников")]
    private EmployeeTeam team;

    [SerializeField]
    [Tooltip("Пакет с настройками статистик сотрудников")]
    private EmployeeStatsSettingsPack statsSettingsPack;

    /// <summary>
    /// Коллекция сотрудников
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
    /// Коллекция рекрутов
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

    #region События
    public event Action<List<Employee>> TeamChanged;
    public event Action<List<Employee>> RecrutsChanged;
    public event Action<Employee> DismissEmployeeEvent;
    public event Action<Employee> NewEmployee;
    #endregion

    #region Методы

    #region Общие
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
    /// Уволить сотрудник
    /// </summary>
    /// <param name="employee">Сотрудник на увольнение</param>
    public void DismissEmployee(Employee employee)
    {
        Employees.Remove(employee);
        TeamChanged?.Invoke(Employees);
        DismissEmployeeEvent?.Invoke(employee);
    }

    /// <summary>
    /// Добавить рекрута в команду, потратив средства на привлечение
    /// </summary>
    /// <param name="e">Сотрудник для рекрутирование</param>
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

    #region Команды всем сотрудникам
    /// <summary>
    /// Стартовать новый день для всех сотрудников, проверить максимальные статистики. 
    /// Человек с максимльным стрессом уволиться. 
    /// Человек с максимальной усталостью проспит
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
    /// Отправить всех работать, установив базовую стратегию оплаты
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
    /// Отправить всех на обед
    /// </summary>
    private void AllToLunch()
    {
        foreach (Employee e in Employees)
        {
            e.GoToLunch();
        }
    }

    /// <summary>
    /// Отправить домой сотрудников, которые не работают сверхурочно
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
    /// Для каждого сотрудника подсчитать результаты дня, установить базовую 
    /// стратегию оплаты и отправить домой
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

    #region Реакции на события
    private void OnEmployeeChanged()
    {
        TeamChanged?.Invoke(Employees);
    }
    private void OnEmployeeMaxStress(Employee e)
    {
        GameUICenter.messageQueue.PrepareMessage(e.Name + " уволняется", 
            e.EmployeeSpeachPack.employeeMaxStressMessege);
    }
    private void OnEmployeeMaxFatigue(Employee e)
    {
        GameUICenter.messageQueue.PrepareMessage(e.Name + " не вышел на работу", 
            e.EmployeeSpeachPack.GetRandomMaxFatigueSpeach());
    }
    #endregion

    #endregion
}
