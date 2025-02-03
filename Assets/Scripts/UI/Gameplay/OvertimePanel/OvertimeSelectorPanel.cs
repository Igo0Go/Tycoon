using System.Collections.Generic;
using UnityEngine;

public class OvertimeSelectorPanel : MonoBehaviour
{
    [Space]
    [SerializeField]
    private GameObject overtimePanel;
    [SerializeField]
    private GameObject employeeOvertimeItemPrefab;
    [SerializeField]
    private Transform employeeListContentContainer;

    private EmployeeSystem employeeSystem;

    public void SubscribeEvents(EmployeeSystem employeeSystem, TimeSystem timeSystem)
    {
        this.employeeSystem = employeeSystem;
        employeeSystem.teamChanged += RebuildEmployeesList;
        timeSystem.overtimeAccepted += ShowPanel;
    }
    public void SetUp()
    {
        ClosePanel();
    }
    public void ShowPanel()
    {
        overtimePanel.SetActive(true);
        RebuildEmployeesList(employeeSystem.Employees);
    }
    public void ClosePanel()
    {
        ClearItemsList();
        overtimePanel.SetActive(false);
    }

    private void ClearItemsList()
    {
        for (int i = 0; i < employeeListContentContainer.childCount; i++)
        {
            EmployeeOvertimeItem item = employeeListContentContainer.GetChild(i).gameObject.GetComponent<EmployeeOvertimeItem>();
            item.OnDestroy();
            Destroy(employeeListContentContainer.GetChild(i).gameObject);
        }
    }
    private void RebuildEmployeesList(List<Employee> employees)
    {
        ClearItemsList();
        foreach (Employee e in employees)
        {
            EmployeeOvertimeItem item = Instantiate(employeeOvertimeItemPrefab, employeeListContentContainer).GetComponent<EmployeeOvertimeItem>();
            item.Init(e);
        }
    }
}
