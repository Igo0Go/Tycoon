using UnityEngine;

/// <summary>
/// ����� � ����������� ��� �������� ����������
/// </summary>
[System.Serializable]
public class EmployeeBuilderInfo
{
    /// <summary>
    /// ��� ����������
    /// </summary>
    public string name;

    /// <summary>
    /// ������� ����� ����������
    /// </summary>
    [Min(1)]
    public float baseSalary;

    /// <summary>
    /// ������� ����� �� �������������� ����������
    /// </summary>
    [Min(1)]
    public float costOfAttracting = 100;

    /// <summary>
    /// ���� ���������� � �����
    /// </summary>
    [Min(1)]
    public int experienceInHour = 0;

    /// <summary>
    /// ����� �� ������������ ����������
    /// </summary>
    public EmployeeStatsPack statsPack;

    /// <summary>
    /// ����� � ������� ����������
    /// </summary>
    public EmployeeSpeachPack speachPack;
}
