using System;
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

    [Space]
    [Header("Блок финансов")]
    [SerializeField]
    private TMP_Text mainFinanceText;
    [SerializeField]
    private TMP_Text rentText;
    [SerializeField]
    private TMP_Text utilityText;
    [SerializeField]
    private TMP_Text employeesPaymentText;

    [Space]
    [Header("Блок сотрудников")]
    [SerializeField]
    private GameObject employeeUIItemPrefab;
    [SerializeField]
    private Transform employeeListContentContainer;
    [SerializeField]
    private GameObject currentEmployeeInfoPanel;
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

    private Employee CurrentEmployee
    {
        get
        {
            return _currentEmployee;
        }
        set
        {
            _currentEmployee = value;
            currentEmployeeInfoPanel.SetActive(_currentEmployee != null);
        }
    }
    private Employee _currentEmployee;

    public void SubscribeEvents(FinanceSystem financeSystem, EmployeeSystem employeeSystem)
    {
        this.financeSystem = financeSystem;
        financeSystem.currentSummChanged += OnCurrentSumChanged;
        this.employeeSystem = employeeSystem;
        employeeSystem.teamChanged += RedrawEmployeesStatsPanel;
        employeeSystem.teamChanged += RebuildEmployeesList;
        employeeSystem.teamChanged += (e)=> RedrawFinanceInfo();
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

    public void RedrawEmployeesStatsPanel(List<Employee> employees)
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
        CurrentEmployee = null;
        resurcePanel.SetActive(false);
    }
    public void DissmissCurrentEmployee()
    {
        employeeSystem.DismissEmployee(CurrentEmployee);
        CurrentEmployee = null;
    }

    public void PlusSalaryForCurrentEmployee()
    {
        CurrentEmployee.PlusSalary(1);
    }
    public void MinusSalaryForCurrentEmployee()
    {
        CurrentEmployee.MinusSalary(1);
    }
    private void SelectEmployee(Employee e)
    {
        if(CurrentEmployee != e)
        {
            if(CurrentEmployee != null)
            {
                CurrentEmployee.employeeChanged -= RedrawCurrentEmployeeInfo;
            }
            CurrentEmployee = e;
            e.employeeChanged += RedrawCurrentEmployeeInfo;
            RedrawCurrentEmployeeInfo();
        }
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
    private void RedrawCurrentEmployeeInfo()
    {
        nameText.text = CurrentEmployee.Name;
        stateText.text = CurrentEmployee.State;
        paymentText.text = CurrentEmployee.GetSalaryInfo();
    }
    private void RedrawFinanceInfo()
    {
        int dayPrediction = (int)Math.Ceiling(financeSystem.CurrentSum / financeSystem.CurrentDayCost);

        mainFinanceText.text = financeSystem.CurrentSum + " (- " + financeSystem.CurrentDayCost + ") ~" + dayPrediction.ToString() + "Д";
        rentText.text = "Аренда помещения: -" + financeSystem.DayRentCosts + "/Д";
        utilityText.text = "Коммунальные услуги: -" + financeSystem.DayUtilityCosts + "/Д";
        employeesPaymentText.text = "Оплата работы сотрудников: -" + financeSystem.DayEmployesPayment + "/Д";
    }
}