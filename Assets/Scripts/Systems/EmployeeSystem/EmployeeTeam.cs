using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "IgoGo/EmployeeTeam")]
public class EmployeeTeam : ScriptableObject
{
    [SerializeField]
    private List<Employee> _employees;

    public List<Employee> GetEmployees()
    { 
        return new List<Employee>(_employees);
    }
}
