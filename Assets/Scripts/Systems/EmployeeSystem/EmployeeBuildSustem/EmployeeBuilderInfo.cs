using UnityEngine;

[System.Serializable]
public class EmployeeBuilderInfo
{
    public string name;
    [Min(1)]
    public float baseSalary;
    [Range(0, 2)]
    public float overtimeSalaryMultiplier = 2;
    [Range(0, 2)]
    public float hospitalSalaryMultiplier = 0.5f;
}
