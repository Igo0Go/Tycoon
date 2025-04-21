using UnityEngine;

[CreateAssetMenu(menuName = "IgoGo/EmployeeStatsSettings")]
public class EmployeeStatsSettingsPack : ScriptableObject
{
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

public static class EmployeeStatsSettings
{
    public static int baseStreesGrowthSpeed = 2;
    public static int baseStreesLoweringSpeed = 2;
    public static int baseFatigueGrowthSpeed = 2;
    public static int baseFatigueLoweringSpeed = 2;

    public static int dailyStreesGrowth = 2;
    public static int dailyStreesOvertimeGrowth = 2;
    public static int dailyStreesLowering = 2;
    public static int lunchStreesLowering = 2;
    public static int dailyFatigueGrowth = 2;
    public static int dailyFatigueOvertimeLowering = 2;
    public static int dailyFatigueLowering = 2;
    public static int lunchFatigueLowering = 2;
}


