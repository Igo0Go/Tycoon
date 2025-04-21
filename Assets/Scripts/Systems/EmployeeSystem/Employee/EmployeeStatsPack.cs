using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EmployeeStatsPack : ICloneable
{
    [Min(1)]
    [Tooltip("ѕороговый уровень усталости. «начение усталости, после которого сотрудник не выйдет на работу")]
    public int fatigueThresholdValue = 90;

    [Range(1,10)]
    [Tooltip("–ассе€ние порогового уровн€ усталости, чтобы было +/- пороговый уровень")]
    public int fatigueThresholdRange = 5;

    [Min(1)]
    [Tooltip("ƒополнительный рост усталости")]
    public int addingFatigueGrowthPoints = 0;

    [Min(1)]
    [Tooltip("ƒополнительный сброс усталости")]
    public int addingFatigueLoweringPoints = 0;


    [Min(1)]
    [Tooltip("ѕороговый уровень усталости. «начение усталости, после которого сотрудник не выйдет на работу")]
    public int stressThresholdValue = 90;

    [Range(1, 10)]
    [Tooltip("–ассе€ние порогового уровн€ усталости, чтобы было +/- пороговый уровень")]
    public int stressThresholdRange = 5;

    [Min(1)]
    [Tooltip("ƒополнительный рост усталости")]
    public int addingStressGrowthPoints = 0;

    [Min(1)]
    [Tooltip("—корость сброса усталости")]
    public int addingStressLoweringPoints = 0;

    public List<EmployeeTaskSpeedItem> taskSpeedItems = new()
    {
        new EmployeeTaskSpeedItem() { taskType = EmployeeTaskType.Docs, taskSpeed = 0},
        new EmployeeTaskSpeedItem() { taskType = EmployeeTaskType.Code, taskSpeed = 0},
        new EmployeeTaskSpeedItem() { taskType = EmployeeTaskType.Testing, taskSpeed = 0}
    };

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

    public int GetThresholdFatigue()
    {
        int result = fatigueThresholdValue;
        int range = UnityEngine.Random.Range(-fatigueThresholdRange, fatigueThresholdRange);

        return result + range;
    }

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