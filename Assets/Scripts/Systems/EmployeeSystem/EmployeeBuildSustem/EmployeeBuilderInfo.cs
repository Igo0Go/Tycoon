using UnityEngine;

/// <summary>
/// Пакет с информацией для сборщика сотрудника
/// </summary>
[System.Serializable]
public class EmployeeBuilderInfo
{
    /// <summary>
    /// Имя сотрудника
    /// </summary>
    public string name;

    /// <summary>
    /// Базовый оклад сотрудника
    /// </summary>
    [Min(1)]
    public float baseSalary;

    /// <summary>
    /// Разовая плата за рекрутирование сотрудника
    /// </summary>
    [Min(1)]
    public float costOfAttracting = 100;

    /// <summary>
    /// Опыт сотрудника в часах
    /// </summary>
    [Min(1)]
    public int experienceInHour = 0;

    /// <summary>
    /// Пакет со статистиками сотрудника
    /// </summary>
    public EmployeeStatsPack statsPack;

    /// <summary>
    /// Пакет с фразами сотрудника
    /// </summary>
    public EmployeeSpeachPack speachPack;
}
