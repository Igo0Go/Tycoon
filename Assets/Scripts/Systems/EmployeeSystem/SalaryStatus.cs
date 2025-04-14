public interface SalaryStatus
{
    bool IsActive { get; }
    bool Overtime { get; }
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
    public bool Overtime => false;
    public string StateName => "������� �����";
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
    public bool Overtime => false;
    public string StateName => "�� ����������";
    public float CalculateSalary(Employee employee)
    {
        return employee.BaseSalary * employee.hospitalSalaryMultiplier;
    }
    public string GetSalaryInfo(Employee employee)
    {
        float result = employee.BaseSalary * employee.hospitalSalaryMultiplier;
        return employee.BaseSalary + " * " + employee.hospitalSalaryMultiplier + "(�����.) = " + result + "/�";
    }
}
public class OvertimeSalaryStatus : SalaryStatus
{
    public bool IsActive => true;
    public bool Overtime => true;
    public string StateName => "������� ����� + ������������";
    public float CalculateSalary(Employee employee)
    {
        return employee.BaseSalary * Employee.overtimeSalaryMultiplier;
    }
    public string GetSalaryInfo(Employee employee)
    {
        float result = employee.BaseSalary * Employee.overtimeSalaryMultiplier;
        return employee.BaseSalary + " * " + Employee.overtimeSalaryMultiplier + "(�����.) = " + result + "/�";
    }
}

