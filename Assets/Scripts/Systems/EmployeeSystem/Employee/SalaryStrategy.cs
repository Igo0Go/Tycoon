public interface ISalaryStrategy
{
    /// <summary>
    /// �������� ��������� ������� ���������� �����
    /// </summary>
    string StrategyName { get; }
    /// <summary>
    /// �������� ��������� ���������� � ������� ���������� �����
    /// </summary>
    /// <param name="employee">��� ������ ����������</param>
    /// <returns>������ � ����������� � ������� ���������� �����</returns>
    string GetSalaryInfo(Employee employee);
    /// <summary>
    /// ���������� ���������� �����
    /// </summary>
    /// <param name="employee">��� ����������</param>
    /// <returns>������ ���������� �����</returns>
    float CalculateSalary(Employee employee);
}

public static class SalaryStrategySingleton
{
    public static ISalaryStrategy BaseSalaryStatus { get; } = new BaseSalaryStrategy();
    public static ISalaryStrategy HospitalSalaryStatus { get; } = new HospitalSalaryStrategy();
    public static ISalaryStrategy OvertimeSalaryStatus { get; } = new OvertimeSalaryStrategy();
}

/// <summary>
/// ������� ��������� ���������� ���������� �����
/// </summary>
public class BaseSalaryStrategy : ISalaryStrategy
{
    public string StrategyName => "������� �����";
    public float CalculateSalary(Employee employee)
    {
        return employee.BaseSalary;
    }
    public string GetSalaryInfo(Employee employee)
    {
        return employee.BaseSalary + "/�";
    }
}
public class HospitalSalaryStrategy : ISalaryStrategy
{
    public const float hospitalSalaryMultiplier = 0.5f;


    public string StrategyName => "�� ����������";
    public float CalculateSalary(Employee employee)
    {
        return employee.BaseSalary * hospitalSalaryMultiplier;
    }
    public string GetSalaryInfo(Employee employee)
    {
        float result = employee.BaseSalary * hospitalSalaryMultiplier;
        return employee.BaseSalary + " * " + hospitalSalaryMultiplier + "(�����.) = " + result + "/�";
    }
}
public class OvertimeSalaryStrategy : ISalaryStrategy
{
    public const float overtimeSalaryMultiplier = 2;

    public string StrategyName => "������� ����� + ������������";
    public float CalculateSalary(Employee employee)
    {
        return employee.BaseSalary * overtimeSalaryMultiplier;
    }
    public string GetSalaryInfo(Employee employee)
    {
        float result = employee.BaseSalary * overtimeSalaryMultiplier;
        return employee.BaseSalary + " * " + overtimeSalaryMultiplier + "(�����.) = " + result + "/�";
    }
}

