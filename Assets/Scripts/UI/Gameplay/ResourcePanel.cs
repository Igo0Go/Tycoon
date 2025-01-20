using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourcePanel : MonoBehaviour
{
    [Header("Маля панель")]
    [SerializeField]
    private TMP_Text currentSumText;
    [SerializeField]
    private TMP_Text currentEmployeeText;


    [SerializeField]
    private GameObject resurcePanel;

    public void SubscribeEvents(FinanceSystem system, EmployeeSystem employeeSystem)
    {
        system.currentSummChanged += OnCurrentSumChanged;
        employeeSystem.teamChanged += OnEmployeesChanged;
    }

    public void OnCurrentSumChanged(int newSum)
    {
        currentSumText.text = newSum.ToString();
    }

    public void OnEmployeesChanged(List<Employee> employees)
    {
        int allCount = employees.Count;
        int activeCount = 0;

        foreach (Employee employee in employees)
        {
            if(employee.IsActive)
            {
                activeCount++;
            }
        }

        currentEmployeeText.text = activeCount + "/" + allCount;
    }
}
