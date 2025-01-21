using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourcePanel : MonoBehaviour
{
    [Header("���� ������")]
    [SerializeField]
    private TMP_Text currentSumText;
    [SerializeField]
    private TMP_Text currentEmployeeText;

    [Space]
    [Header("���� ��������")]
    [SerializeField]
    private TMP_Text mainFinanceText;
    [SerializeField]
    private TMP_Text rentText;
    [SerializeField]
    private TMP_Text utilityText;
    [SerializeField]
    private TMP_Text employeesPaymentText;

    [Space]
    [Header("���� �����������")]
    [SerializeField]
    private GameObject employeeUIItemPrefab;
    [SerializeField]
    private Transform employeeListContentContainer;
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text stateText;
    [SerializeField]
    private TMP_Text paymentText;

    [Space]
    [SerializeField]
    private GameObject resurcePanel;

    private FinanceSystem financeSystem;
    private EmployeeSystem employeeSystem;
    private Employee currentEmployee;

    public void SubscribeEvents(FinanceSystem financeSystem, EmployeeSystem employeeSystem)
    {
        this.financeSystem = financeSystem;
        financeSystem.currentSummChanged += OnCurrentSumChanged;
        this.employeeSystem = employeeSystem;
        employeeSystem.teamChanged += OnEmployeesChanged;
        employeeSystem.teamChanged += RebuildEmployeesList;
    }
    public void SetUp()
    {
        CloseResourcePanel();
    }

    public void OnCurrentSumChanged(float newSum)
    {
        currentSumText.text = newSum.ToString();
        RedrawFinanceInfo();
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

    public void ShowResourcePanel()
    {
        resurcePanel.SetActive(true);
        RedrawFinanceInfo();
        RebuildEmployeesList(employeeSystem.Employees);
    }
    public void CloseResourcePanel()
    {
        resurcePanel.SetActive(false);
    }

    private void RebuildEmployeesList(List<Employee> employees)
    {
        for (int i = 0; i < employeeListContentContainer.childCount; i++)
        {
            Destroy(employeeListContentContainer.GetChild(i).gameObject);
        }
        foreach (Employee e in employees)
        {
            EmployeeUiItem item = Instantiate(employeeUIItemPrefab, employeeListContentContainer).GetComponent<EmployeeUiItem>();
            item.Init(e);
            item.OnEmployeeClick += SelectEmployee;
        }
    }

    private void SelectEmployee(Employee e)
    {
        currentEmployee = e;
        RedrawCurrentEmployeeInfo();
    }

    private void RedrawCurrentEmployeeInfo()
    {
        nameText.text = currentEmployee.Name;
        stateText.text = currentEmployee.State;
        paymentText.text = currentEmployee.GetSalary().ToString() + "/�";
    }

    private void RedrawFinanceInfo()
    {
        int dayPrediction = (int)Math.Ceiling(financeSystem.CurrentSum / financeSystem.CurrentDayCost);

        mainFinanceText.text = financeSystem.CurrentSum + " (- " + financeSystem.CurrentDayCost + ") ~" + dayPrediction.ToString() + "�";
        rentText.text = "������ ���������: -" + financeSystem.DayRentCosts + "/�";
        utilityText.text = "������������ ������: -" + financeSystem.DayUtilityCosts + "/�";
        employeesPaymentText.text = "������ ������ �����������: -" + financeSystem.DayEmployesPayment + "/�";
    }
}