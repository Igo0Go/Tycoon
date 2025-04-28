using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EmployeeStatsPack : ICloneable
{
    [Min(1)]
    [Tooltip("��������� ������� ���������. �������� ���������, ����� �������� ��������� �� ������ �� ������")]
    public int fatigueThresholdValue = 90;

    [Range(1,10)]
    [Tooltip("��������� ���������� ������ ���������, ����� ���� +/- ��������� �������")]
    public int fatigueThresholdRange = 5;

    [Min(1)]
    [Tooltip("�������������� ���� ���������")]
    public int addingFatigueGrowthPoints = 0;

    [Min(1)]
    [Tooltip("�������������� ����� ���������")]
    public int addingFatigueLoweringPoints = 0;


    [Min(1)]
    [Tooltip("��������� ������� ���������. �������� ���������, ����� �������� ��������� �� ������ �� ������")]
    public int stressThresholdValue = 90;

    [Range(1, 10)]
    [Tooltip("��������� ���������� ������ ���������, ����� ���� +/- ��������� �������")]
    public int stressThresholdRange = 5;

    [Min(1)]
    [Tooltip("�������������� ���� ���������")]
    public int addingStressGrowthPoints = 0;

    [Min(1)]
    [Tooltip("�������� ������ ���������")]
    public int addingStressLoweringPoints = 0;

    public List<EmployeeTaskSpeedItem> taskSpeedItems = new()
    {
        new EmployeeTaskSpeedItem() { taskType = EmployeeTaskType.Docs, taskSpeed = 0},
        new EmployeeTaskSpeedItem() { taskType = EmployeeTaskType.Code, taskSpeed = 0},
        new EmployeeTaskSpeedItem() { taskType = EmployeeTaskType.Testing, taskSpeed = 0}
    };

    /// <summary>
    /// �������� ������ ����� ������
    /// </summary>
    /// <returns>����� ��������� ������ ��������� � ���� �� ����������</returns>
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
    /// �������� �������� ������ ��� ������� ������������ ����
    /// </summary>
    /// <param name="taskType">��� �����</param>
    /// <returns>�������� ������ ��� ������� (������ �� ���)</returns>
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
    /// ���������� ��������� �������� ��� ���������
    /// </summary>
    /// <returns>��������� �������� ��� ��������� � ������ ���������</returns>
    public int GetThresholdFatigue()
    {
        int result = fatigueThresholdValue;
        int range = UnityEngine.Random.Range(-fatigueThresholdRange, fatigueThresholdRange);

        return result + range;
    }

    /// <summary>
    /// ���������� ��������� �������� ��� �������
    /// </summary>
    /// <returns>��������� �������� ��� ������� � ������ ���������</returns>
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