using UnityEngine;

[CreateAssetMenu(menuName = "IgoGo/EmployeeStatsSettings")]
public class EmployeeStatsSettingsPack : ScriptableObject
{
    public int baseStreesGrowthSpeed = 2;
    public int baseStreesLoweringSpeed = 2;
    public int baseFatigueGrowthSpeed = 2;
    public int baseFatigueLoweringSpeed = 2;

    public int dailyStreesGrowth = 2;
    public int dailyStreesOvertimeGrowth = 2;
    public int dailyStreesLowering = 2;
    public int lunchStreesLowering = 2;
    public int dailyFatigueGrowth = 2;
    public int dailyFatigueOvertimeLowering = 2;
    public int dailyFatigueLowering = 2;
    public int lunchFatigueLowering = 2;
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


