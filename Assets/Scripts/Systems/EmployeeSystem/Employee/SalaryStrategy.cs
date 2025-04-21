public interface ISalaryStrategy
{
    /// <summary>
    /// Название стратегии расчёта заработной платы
    /// </summary>
    string StrategyName { get; }
    /// <summary>
    /// Получить строковую информацию о расчёте заработной платы
    /// </summary>
    /// <param name="employee">Для какого сотрудника</param>
    /// <returns>Строка с информацией о расчёте заработной платы</returns>
    string GetSalaryInfo(Employee employee);
    /// <summary>
    /// Рассчитать заработную плату
    /// </summary>
    /// <param name="employee">Для сотрудника</param>
    /// <returns>Размер заработной платы</returns>
    float CalculateSalary(Employee employee);
}

public static class SalaryStrategySingleton
{
    public static ISalaryStrategy BaseSalaryStatus { get; } = new BaseSalaryStrategy();
    public static ISalaryStrategy HospitalSalaryStatus { get; } = new HospitalSalaryStrategy();
    public static ISalaryStrategy OvertimeSalaryStatus { get; } = new OvertimeSalaryStrategy();
}

/// <summary>
/// Базовая стратегия назначения заработной платы
/// </summary>
public class BaseSalaryStrategy : ISalaryStrategy
{
    public string StrategyName => "Базовый оклад";
    public float CalculateSalary(Employee employee)
    {
        return employee.BaseSalary;
    }
    public string GetSalaryInfo(Employee employee)
    {
        return employee.BaseSalary + "/Д";
    }
}
public class HospitalSalaryStrategy : ISalaryStrategy
{
    public const float hospitalSalaryMultiplier = 0.5f;


    public string StrategyName => "На больничном";
    public float CalculateSalary(Employee employee)
    {
        return employee.BaseSalary * hospitalSalaryMultiplier;
    }
    public string GetSalaryInfo(Employee employee)
    {
        float result = employee.BaseSalary * hospitalSalaryMultiplier;
        return employee.BaseSalary + " * " + hospitalSalaryMultiplier + "(больн.) = " + result + "/Д";
    }
}
public class OvertimeSalaryStrategy : ISalaryStrategy
{
    public const float overtimeSalaryMultiplier = 2;

    public string StrategyName => "Базовый оклад + Сверхурочные";
    public float CalculateSalary(Employee employee)
    {
        return employee.BaseSalary * overtimeSalaryMultiplier;
    }
    public string GetSalaryInfo(Employee employee)
    {
        float result = employee.BaseSalary * overtimeSalaryMultiplier;
        return employee.BaseSalary + " * " + overtimeSalaryMultiplier + "(сверх.) = " + result + "/Д";
    }
}

