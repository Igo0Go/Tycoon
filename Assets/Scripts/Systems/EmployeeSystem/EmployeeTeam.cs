using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "IgoGo/EmployeeTeam")]
public class EmployeeTeam : ScriptableObject
{
    [SerializeField]
    private List<EmployeeBuilderInfo> _employees;
    [SerializeField]
    private List<EmployeeBuilderInfo> _recruts;

    public List<Employee> GetEmployees()
    {
        List<Employee> result = new();

        foreach(EmployeeBuilderInfo info in _employees)
        {
            Employee e = EmployeeBuilder.GetEmployee(info);
            result.Add(e);
        }

        return result;
    }

    public List<Employee> GetRecruts()
    {
        List<Employee> result = new();

        foreach (EmployeeBuilderInfo info in _recruts)
        {
            result.Add(EmployeeBuilder.GetEmployee(info));
        }

        return result;
    }
}
