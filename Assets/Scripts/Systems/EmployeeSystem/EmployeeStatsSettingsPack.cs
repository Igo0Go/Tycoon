using UnityEngine;

[CreateAssetMenu(menuName = "IgoGo/EmployeeStatsSettings")]
public class EmployeeStatsSettingsPack : ScriptableObject
{
    [Tooltip("Базовый уровень квалификации сотрудников при выполнении задач. От этого зависит, как часто они будут ошибаться")]
    public int baseEmployeeTaskLevel = 100;
    [Tooltip("Множитель влияния усталости на сложность выполнения задач")]
    public int fatigueTaskDificultyMultiplier = 1;
    [Tooltip("Множитель влияния стресса на сложность выполнения задач")]
    public int stressTaskDificultyMultiplier = 1;

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
/// Статический, доступный отовсюду буфер, хранящий текущие настройки статистик по сотрудникам
/// </summary>
public static class EmployeeStatsSettings
{
    /// <summary>
    /// Базовый уровень квалификации сотрудников при выполнении задач. От этого зависит, как часто они будут ошибаться
    /// </summary>
    public static int baseEmployeeTaskLevel = 100;
    /// <summary>
    /// Множитель влияния усталости на сложность выполнения задач
    /// </summary>
    public static int fatigueTaskDificultyMultiplier = 1;
    /// <summary>
    /// Множитель влияния стресса на сложность выполнения задач
    /// </summary>
    public static int stressTaskDificultyMultiplier = 1;

    /// <summary>
    /// С какой скоростью без учёта особенностей сотрудников растёт стресс
    /// </summary>
    public static int baseStreesGrowthSpeed = 2;
    /// <summary>
    /// С какой скоростью без учёта особенностей сотрудников падает стресс
    /// </summary>
    public static int baseStreesLoweringSpeed = 2;
    /// <summary>
    /// С какой скоростью без учёта особенностей сотрудников растёт стресс в конце дня
    /// </summary>
    public static int dailyStreesGrowth = 2;
    /// <summary>
    /// С какой скоростью без учёта особенностей сотрудников растёт стресс в конце дня, если сотрудник работал сверхурочно
    /// </summary>
    public static int dailyStreesOvertimeGrowth = 2;
    /// <summary>
    /// С какой скоростью без учёта особенностей сотрудников падает стресс, когда человек дома
    /// </summary>
    public static int dailyStreesLowering = 2;
    /// <summary>
    /// С какой скоростью без учёта особенностей сотрудников падает стресс во время обеда
    /// </summary>
    public static int lunchStreesLowering = 2;

    /// <summary>
    /// С какой скоростью без учёта особенностей сотрудников растёт усталость
    /// </summary>
    public static int baseFatigueGrowthSpeed = 2;
    /// <summary>
    /// С какой скоростью без учёта особенностей сотрудников растёт усталость
    /// </summary>
    public static int baseFatigueLoweringSpeed = 2;
    /// <summary>
    /// С какой скоростью без учёта особенностей сотрудников растёт усталость в конце дня
    /// </summary>
    public static int dailyFatigueGrowth = 2;
    /// <summary>
    /// Какая доля от порога усталости восстанавливается во время время сна, если сотрудник работал сверхурочно
    /// </summary>
    public static int dailyFatigueOvertimeLowering = 4;
    /// <summary>
    /// Какая доля от порога усталости восстанавливается во время время сна
    /// </summary>
    public static int dailyFatigueLowering = 2;
    /// <summary>
    /// С какой скоростью без учёта особенностей сотрудников падает усталость во время обеда
    /// </summary>
    public static int lunchFatigueLowering = 2;
}


