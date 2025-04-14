public interface ISalaryStrategy
{
    string StateName { get; }
    string GetSalaryInfo(Employee employee);
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
    public string StateName => "Базовый оклад";
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


    public string StateName => "На больничном";
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

    public string StateName => "Базовый оклад + Сверхурочные";
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

