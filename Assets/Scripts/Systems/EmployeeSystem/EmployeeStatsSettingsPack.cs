using UnityEngine;

[CreateAssetMenu(menuName = "IgoGo/EmployeeStatsSettings")]
public class EmployeeStatsSettingsPack : ScriptableObject
{
    [Tooltip("������� ������� ������������ ����������� ��� ���������� �����. �� ����� �������, ��� ����� ��� ����� ���������")]
    public int baseEmployeeTaskLevel = 100;
    [Tooltip("��������� ������� ��������� �� ��������� ���������� �����")]
    public int fatigueTaskDificultyMultiplier = 1;
    [Tooltip("��������� ������� ������� �� ��������� ���������� �����")]
    public int stressTaskDificultyMultiplier = 1;

    [Tooltip("� ����� ��������� ��� ����� ������������ ����������� ����� ������")]
    public int baseStreesGrowthSpeed = 2;
    [Tooltip("� ����� ��������� ��� ����� ������������ ����������� ������ ������")]
    public int baseStreesLoweringSpeed = 2;
    [Tooltip("� ����� ��������� ��� ����� ������������ ����������� ����� ������ � ����� ���")]
    public int dailyStreesGrowth = 2;
    [Tooltip("� ����� ��������� ��� ����� ������������ ����������� ����� ������ � ����� ���, ���� ��������� ������� �����������")]
    public int dailyStreesOvertimeGrowth = 2;
    [Tooltip("� ����� ��������� ��� ����� ������������ ����������� ������ ������, ����� ������� ����")]
    public int dailyStreesLowering = 2;
    [Tooltip("� ����� ��������� ��� ����� ������������ ����������� ������ ������ �� ����� �����")]
    public int lunchStreesLowering = 2;

    [Tooltip("� ����� ��������� ��� ����� ������������ ����������� ����� ���������")]
    public int baseFatigueGrowthSpeed = 2;
    [Tooltip("� ����� ��������� ��� ����� ������������ ����������� ����� ���������")]
    public int baseFatigueLoweringSpeed = 2;
    [Tooltip("� ����� ��������� ��� ����� ������������ ����������� ����� ��������� � ����� ���")]
    public int dailyFatigueGrowth = 2;
    [Tooltip("����� ���� �� ������ ��������� ����������������� �� ����� ����� ���, " +
        "���� ��������� ������� �����������")]
    public int dailyFatigueOvertimeLowering = 4;
    [Tooltip("����� ���� �� ������ ��������� ����������������� �� ����� ����� ���")]
    public int dailyFatigueLowering = 2;
    [Tooltip("� ����� ��������� ��� ����� ������������ ����������� ������ ��������� �� ����� �����")]
    public int lunchFatigueLowering = 2;

    public void AcceptThisSettings()
    {
        EmployeeStatsSettings.baseEmployeeTaskLevel = baseEmployeeTaskLevel;
        EmployeeStatsSettings.fatigueTaskDificultyMultiplier = fatigueTaskDificultyMultiplier;
        EmployeeStatsSettings.stressTaskDificultyMultiplier = stressTaskDificultyMultiplier;

        EmployeeStatsSettings.baseStreesGrowthSpeed = baseStreesGrowthSpeed;
        EmployeeStatsSettings.baseStreesLoweringSpeed = baseStreesLoweringSpeed;
        EmployeeStatsSettings.dailyStreesGrowth = dailyStreesGrowth;
        EmployeeStatsSettings.dailyStreesOvertimeGrowth = dailyStreesOvertimeGrowth;
        EmployeeStatsSettings.dailyStreesLowering = dailyStreesLowering;
        EmployeeStatsSettings.lunchStreesLowering = lunchStreesLowering;

        EmployeeStatsSettings.baseFatigueGrowthSpeed= baseFatigueGrowthSpeed;
        EmployeeStatsSettings.baseFatigueLoweringSpeed= baseFatigueLoweringSpeed;
        EmployeeStatsSettings.dailyFatigueGrowth= dailyFatigueGrowth;
        EmployeeStatsSettings.dailyFatigueOvertimeLowering = dailyFatigueOvertimeLowering;
        EmployeeStatsSettings.dailyFatigueLowering = dailyFatigueLowering;
        EmployeeStatsSettings.lunchFatigueLowering= lunchFatigueLowering;
    }
}

/// <summary>
/// �����������, ��������� �������� �����, �������� ������� ��������� ��������� �� �����������
/// </summary>
public static class EmployeeStatsSettings
{
    /// <summary>
    /// ������� ������� ������������ ����������� ��� ���������� �����. �� ����� �������, ��� ����� ��� ����� ���������
    /// </summary>
    public static int baseEmployeeTaskLevel = 100;
    /// <summary>
    /// ��������� ������� ��������� �� ��������� ���������� �����
    /// </summary>
    public static int fatigueTaskDificultyMultiplier = 1;
    /// <summary>
    /// ��������� ������� ������� �� ��������� ���������� �����
    /// </summary>
    public static int stressTaskDificultyMultiplier = 1;

    /// <summary>
    /// � ����� ��������� ��� ����� ������������ ����������� ����� ������
    /// </summary>
    public static int baseStreesGrowthSpeed = 2;
    /// <summary>
    /// � ����� ��������� ��� ����� ������������ ����������� ������ ������
    /// </summary>
    public static int baseStreesLoweringSpeed = 2;
    /// <summary>
    /// � ����� ��������� ��� ����� ������������ ����������� ����� ������ � ����� ���
    /// </summary>
    public static int dailyStreesGrowth = 2;
    /// <summary>
    /// � ����� ��������� ��� ����� ������������ ����������� ����� ������ � ����� ���, ���� ��������� ������� �����������
    /// </summary>
    public static int dailyStreesOvertimeGrowth = 2;
    /// <summary>
    /// � ����� ��������� ��� ����� ������������ ����������� ������ ������, ����� ������� ����
    /// </summary>
    public static int dailyStreesLowering = 2;
    /// <summary>
    /// � ����� ��������� ��� ����� ������������ ����������� ������ ������ �� ����� �����
    /// </summary>
    public static int lunchStreesLowering = 2;

    /// <summary>
    /// � ����� ��������� ��� ����� ������������ ����������� ����� ���������
    /// </summary>
    public static int baseFatigueGrowthSpeed = 2;
    /// <summary>
    /// � ����� ��������� ��� ����� ������������ ����������� ����� ���������
    /// </summary>
    public static int baseFatigueLoweringSpeed = 2;
    /// <summary>
    /// � ����� ��������� ��� ����� ������������ ����������� ����� ��������� � ����� ���
    /// </summary>
    public static int dailyFatigueGrowth = 2;
    /// <summary>
    /// ����� ���� �� ������ ��������� ����������������� �� ����� ����� ���, ���� ��������� ������� �����������
    /// </summary>
    public static int dailyFatigueOvertimeLowering = 4;
    /// <summary>
    /// ����� ���� �� ������ ��������� ����������������� �� ����� ����� ���
    /// </summary>
    public static int dailyFatigueLowering = 2;
    /// <summary>
    /// � ����� ��������� ��� ����� ������������ ����������� ������ ��������� �� ����� �����
    /// </summary>
    public static int lunchFatigueLowering = 2;
}


