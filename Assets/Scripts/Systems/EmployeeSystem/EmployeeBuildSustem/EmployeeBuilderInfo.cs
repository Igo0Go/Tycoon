using UnityEngine;

[System.Serializable]
public class EmployeeBuilderInfo
{
    public string name;
    [Min(1)]
    public float baseSalary;
    [Min(1)]
    public float costOfAttracting = 100;
    [TextArea(5,10)]
    public string dissmisSpeach;
}
