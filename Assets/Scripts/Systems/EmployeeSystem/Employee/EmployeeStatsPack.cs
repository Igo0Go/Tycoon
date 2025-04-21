using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EmployeeStatsPack : ICloneable
{
    [Min(1)]
    [Tooltip("Пороговый уровень усталости. Значение усталости, после которого сотрудник не выйдет на работу")]
    public int fatigueThresholdValue = 90;

    [Range(1,10)]
    [Tooltip("Рассеяние порогового уровня усталости, чтобы было +/- пороговый уровень")]
    public int fatigueThresholdRange = 5;

    [Min(1)]
    [Tooltip("Дополнительный рост усталости")]
    public int addingFatigueGrowthPoints = 0;

    [Min(1)]
    [Tooltip("Дополнительный сброс усталости")]
    public int addingFatigueLoweringPoints = 0;


    [Min(1)]
    [Tooltip("Пороговый уровень усталости. Значение усталости, после которого сотрудник не выйдет на работу")]
    public int stressThresholdValue = 90;

    [Range(1, 10)]
    [Tooltip("Рассеяние порогового уровня усталости, чтобы было +/- пороговый уровень")]
    public int stressThresholdRange = 5;

    [Min(1)]
    [Tooltip("Дополнительный рост усталости")]
    public int addingStressGrowthPoints = 0;

    [Min(1)]
    [Tooltip("Скорость сброса усталости")]
    public int addingStressLoweringPoints = 0;

    public List<EmployeeTaskSpeedItem> taskSpeedItems = new()
    {
        new EmployeeTaskSpeedItem() { taskType = EmployeeTaskType.Docs, taskSpeed = 0},
        new EmployeeTaskSpeedItem() { taskType = EmployeeTaskType.Code, taskSpeed = 0},
        new EmployeeTaskSpeedItem() { taskType = EmployeeTaskType.Testing, taskSpeed = 0}
    };

    /// <summary>
    /// Получить полную копию пакета
    /// </summary>
    /// <returns>Новый экземпляр пакета статистик с теми же значениями</returns>
    public object Clone()
    {
        EmployeeStatsPack pack = new EmployeeStatsPack();
        pack.fatigueThresholdValue = fatigueThresholdValue;
        pack.fatigueThresholdRange = fatigueThresholdRange;
        pack.addingFatigueGrowthPoints = addingFatigueGrowthPoints;
        pack.addingFatigueLoweringPoints = addingFatigueLoweringPoints;

        pack.stressThresholdValue = stressThresholdValue;
        pack.stressThresholdRange = stressThresholdRange;
        pack.addingStressGrowthPoints = addingStressGrowthPoints;
        pack.addingStressLoweringPoints = addingStressLoweringPoints;

        return pack;
    }

    /// <summary>
    /// Получить скорость работы над задачей определённого типа
    /// </summary>
    /// <param name="taskType">Тип здачи</param>
    /// <returns>Скорость работы над задачей (единиц за тик)</returns>
    public int GetTaskSpeed(EmployeeTaskType taskType)
    {
        foreach (var item in taskSpeedItems)
        {
            if(item.taskType == taskType)
            {
                return item.taskSpeed;
            }
        }
        return 1;
    }

    /// <summary>
    /// Рассчитать пороговое значение для усталости
    /// </summary>
    /// <returns>Пороговое значение для усталости с учётом рассеяния</returns>
    public int GetThresholdFatigue()
    {
        int result = fatigueThresholdValue;
        int range = UnityEngine.Random.Range(-fatigueThresholdRange, fatigueThresholdRange);

        return result + range;
    }

    /// <summary>
    /// Рассчитать пороговое значение для стресса
    /// </summary>
    /// <returns>Пороговое значение для стресса с учётом рассеяния</returns>
    public int GetThresholdStress()
    {
        int result = stressThresholdValue;
        int range = UnityEngine.Random.Range(-fatigueThresholdRange, fatigueThresholdRange);

        return result + range;
    }
}

[Serializable]
public class EmployeeTaskSpeedItem
{
    public EmployeeTaskType taskType;
    [Min(1)]
    public int taskSpeed;
}