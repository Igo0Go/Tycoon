public interface SalaryStatus
{
    bool IsActive { get; }
    string StateName { get; }
    string GetSalaryInfo(Employee employee);
    float CalculateSalary(Employee employee);
}

public static class SalaryStatusSingleton
{
    public static SalaryStatus baseSalaryStatus { get; } = new BaseSalaryStatus();
    public static SalaryStatus hospitalSalaryStatus { get; } = new HospitalSalaryStatus();
    public static SalaryStatus overtimeSalaryStatus { get; } = new OvertimeSalaryStatus();
}

public class BaseSalaryStatus : SalaryStatus
{
    public bool IsActive => true;
    public string StateName => "��������";
    public float CalculateSalary(Employee employee)
    {
        return employee.BaseSalary;
    }
    public string GetSalaryInfo(Employee employee)
    {
        return employee.BaseSalary + "/�";
    }
}
public class HospitalSalaryStatus : SalaryStatus
{
    public bool IsActive => false;
    public string StateName => "�� ����������";
    public float CalculateSalary(Employee employee)
    {
        return employee.BaseSalary * employee.HospitalSalaryMultiplier;
    }
    public string GetSalaryInfo(Employee employee)
    {
        float result = employee.BaseSalary * employee.HospitalSalaryMultiplier;
        return employee.BaseSalary + " * " + employee.HospitalSalaryMultiplier + "(�����.) = " + result + "/�";
    }
}
public class OvertimeSalaryStatus : SalaryStatus
{
    public bool IsActive => true;
    public string StateName => "�����������";
    public float CalculateSalary(Employee employee)
    {
        return employee.BaseSalary * employee.OvertimeSalaryMultiplier;
    }
    public string GetSalaryInfo(Employee employee)
    {
        float result = employee.BaseSalary * employee.OvertimeSalaryMultiplier;
        return employee.BaseSalary + " * " + employee.OvertimeSalaryMultiplier + "(�����.) = " + result + "/�";
    }
}
