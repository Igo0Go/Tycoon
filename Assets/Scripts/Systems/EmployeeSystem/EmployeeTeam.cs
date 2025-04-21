using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Пакет с информацией обо всех сотрудниках
/// </summary>
[CreateAssetMenu(menuName = "IgoGo/EmployeeTeam")]
public class EmployeeTeam : ScriptableObject
{
    /// <summary>
    /// Споисок сотрудников, доступных на старте
    /// </summary>
    [SerializeField]
    private List<EmployeeBuilderInfo> _employees;

    /// <summary>
    /// Список сотрудников, доступных для рекрутирования
    /// </summary>
    [SerializeField]
    private List<EmployeeBuilderInfo> _recruts;

    /// <summary>
    /// Создать экземпляры сотрудников команд из пакетов
    /// </summary>
    /// <returns>Список сотрудников команды</returns>
    public List<Employee> GetEmployees()
    {
        List<Employee> result = new();

        foreach(EmployeeBuilderInfo info in _employees)
        {
            Employee e = EmployeeBuilder.GetEmployee(info);
            result.Add(e);
        }

        return result;
    }

    /// <summary>
    /// Создать экземпляры сотрудников команд из пакетов
    /// </summary>
    /// <returns>Список сотрудников, доступных для рекрутировани</returns>
    public List<Employee> GetRecruts()
    {
        List<Employee> result = new();

        foreach (EmployeeBuilderInfo info in _recruts)
        {
            result.Add(EmployeeBuilder.GetEmployee(info));
        }

        return result;
    }
}
