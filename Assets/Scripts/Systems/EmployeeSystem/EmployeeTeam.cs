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
        List<Employee> result = new List<Employee>();

        foreach(EmployeeBuilderInfo info in _employees)
        {
            result.Add(EmployeeBuilder.GetEmployee(info));
        }

        return result;
    }

    public List<Employee> GetRecruts()
    {
        List<Employee> result = new List<Employee>();

        foreach (EmployeeBuilderInfo info in _recruts)
        {
            result.Add(EmployeeBuilder.GetEmployee(info));
        }

        return result;
    }
}
