using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����� � ����������� ��� ���� �����������
/// </summary>
[CreateAssetMenu(menuName = "IgoGo/EmployeeTeam")]
public class EmployeeTeam : ScriptableObject
{
    /// <summary>
    /// ������� �����������, ��������� �� ������
    /// </summary>
    [SerializeField]
    private List<EmployeeBuilderInfo> _employees;

    /// <summary>
    /// ������ �����������, ��������� ��� ��������������
    /// </summary>
    [SerializeField]
    private List<EmployeeBuilderInfo> _recruts;

    /// <summary>
    /// ������� ���������� ����������� ������ �� �������
    /// </summary>
    /// <returns>������ ����������� �������</returns>
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

    /// <summary>
    /// ������� ���������� ����������� ������ �� �������
    /// </summary>
    /// <returns>������ �����������, ��������� ��� �������������</returns>
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
