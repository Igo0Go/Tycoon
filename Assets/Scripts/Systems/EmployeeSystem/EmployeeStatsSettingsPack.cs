using UnityEngine;

[CreateAssetMenu(menuName = "IgoGo/EmployeeStatsSettings")]
public class EmployeeStatsSettingsPack : ScriptableObject
{
    [Tooltip("С какой скоростью без учёта особенностей сотрудников растёт стресс")]
    public int baseStreesGrowthSpeed = 2;
    [Tooltip("С какой скоростью без учёта особенностей сотрудников падает стресс")]
    public int baseStreesLoweringSpeed = 2;
    [Tooltip("С какой скоростью без учёта особенностей сотрудников растёт стресс в конце дня")]
    public int dailyStreesGrowth = 2;
    [Tooltip("С какой скоростью без учёта особенностей сотрудников растёт стресс в конце дня, если сотрудник работал сверхурочно")]
    public int dailyStreesOvertimeGrowth = 2;
    [Tooltip("С какой скоростью без учёта особенностей сотрудников падает стресс, когда человек дома")]
    public int dailyStreesLowering = 2;
    [Tooltip("С какой скоростью без учёта особенностей сотрудников падает стресс во время обеда")]
    public int lunchStreesLowering = 2;

    [Tooltip("С какой скоростью без учёта особенностей сотрудников растёт усталость")]
    public int baseFatigueGrowthSpeed = 2;
    [Tooltip("С какой скоростью без учёта особенностей сотрудников растёт усталость")]
    public int baseFatigueLoweringSpeed = 2;
    [Tooltip("С какой скоростью без учёта особенностей сотрудников растёт усталость в конце дня")]
    public int dailyFatigueGrowth = 2;
    [Tooltip("Какая доля от порога усталости восстанавливается во время время сна, " +
        "если сотрудник работал сверхурочно")]
    public int dailyFatigueOvertimeLowering = 4;
    [Tooltip("Какая доля от порога усталости восстанавливается во время время сна")]
    public int dailyFatigueLowering = 2;
    [Tooltip("С какой скоростью без учёта особенностей сотрудников падает усталость во время обеда")]
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


