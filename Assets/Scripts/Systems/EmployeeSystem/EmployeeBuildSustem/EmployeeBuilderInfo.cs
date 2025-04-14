using UnityEngine;

[System.Serializable]
public class EmployeeBuilderInfo
{
    public string name;
    [Min(1)]
    public float baseSalary;
    [Min(1)]
    public float costOfAttracting = 100;
    [Min(1)]
    public int experienceInHour = 0;

    public EmployeeStatsPack statsPack;
    public EmployeeSpeachPack speachPack;
}
