using System;
using UnityEngine;

[Serializable]
public class Employee
{
    [SerializeField]
    private string name;
    [Min(1)]
    [SerializeField]
    private float baseSalary;
    [Range(0,2)]
    [SerializeField]
    private float overtimeSalaryMultiplier = 2;
    [Range(0, 2)]
    [SerializeField]
    private float hospitalSalaryMultiplier = 0.5f;

    public string Name => name;
    public float BaseSalary => baseSalary;
    public float OvertimeSalaryMultiplier => overtimeSalaryMultiplier;
    public float HospitalSalaryMultiplier => hospitalSalaryMultiplier;

    public string State => status.StateName;

    public bool IsActive => workTime && status.IsActive;
    private bool workTime;

    private SalaryStatus status = SalaryStatusSingleton.baseSalaryStatus;

    public event Action employeeChanged;

    public Employee(string name, float baseSalary, float overtimeSalaryMultiplier, float hospitalSalaryMultiplier)
    {
        this.name = name;
        this.baseSalary = baseSalary;
        this.overtimeSalaryMultiplier = overtimeSalaryMultiplier;
        this.hospitalSalaryMultiplier = hospitalSalaryMultiplier;
    }

    public float GetSalary()
    {
        return (float)Math.Round(status.CalculateSalary(this), 2);
    }
    public string GetSalaryInfo()
    {
        return status.GetSalaryInfo(this);
    }

    public void SetBaseSalaryStatus()
    {
        status = SalaryStatusSingleton.baseSalaryStatus;
    }
    public void SetHospitalSalaryStatus()
    {
        status = SalaryStatusSingleton.hospitalSalaryStatus;
    }
    public void SetOvertimeSalaryStatus()
    {
        status = SalaryStatusSingleton.overtimeSalaryStatus;
    }

    public void ToWork()
    {
        workTime = true;
        employeeChanged?.Invoke();
    }
    public void GoHome()
    {
        workTime = false;
        employeeChanged?.Invoke();
    }
    public void PlusSalary(float value)
    {
        baseSalary += value;
        employeeChanged?.Invoke();
    }
    public void MinusSalary(float value)
    {
        baseSalary -= value;
        employeeChanged?.Invoke();
    }
}
