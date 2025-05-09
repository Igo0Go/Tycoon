using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourcePanel : MonoBehaviour, IUIPanel
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
    private GameObject currentEmployeeInfoPanel;
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text stateText;
    [SerializeField]
    private TMP_Text paymentText;
    [SerializeField]
    private Slider stressSlider;
    [SerializeField]
    private Slider fatigueSlider;
    [SerializeField]
    private Button showEnmployeesButton;
    [SerializeField]
    private Button showRecrutsButton;

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

    private EmployeeListDrawMode Mode
    {
        get
        {
            return _mode;
        }
        set
        {
            _mode = value;
            if(_mode == EmployeeListDrawMode.recruts)
            {
                showRecrutsButton.enabled = false;
                showEnmployeesButton.enabled = true;
            }
            else
            {
                showRecrutsButton.enabled = true;
                showEnmployeesButton.enabled = false;
            }
        }
    }
    private EmployeeListDrawMode _mode;

    public void SubscribeEvents(FinanceSystem financeSystem, EmployeeSystem employeeSystem)
    {
        Mode = EmployeeListDrawMode.team;

        showEnmployeesButton.onClick.AddListener(() => SetDrawMode(EmployeeListDrawMode.team));
        showRecrutsButton.onClick.AddListener(() => SetDrawMode(EmployeeListDrawMode.recruts));

        this.financeSystem = financeSystem;
        financeSystem.CurrentSummChanged += OnCurrentSumChanged;
        financeSystem.CurrentRentCostChanged += RedrawFinanceInfo;
        this.employeeSystem = employeeSystem;
        employeeSystem.TeamChanged += RedrawEmployeesStatsPanel;
        employeeSystem.TeamChanged += (e)=> RedrawFinanceInfo();

        employeeSystem.TeamChanged += ShowEmployees;
        employeeSystem.RecrutsChanged += ShowRecruts;
    }
    public void SetUp()
    {
        HidePanel();
    }

    public void OnResourcePanelButtonClick()
    {
        if(resurcePanel.activeSelf)
        {
            HidePanel();
        }
        else
        {
            ShowPanel();
        }
    }
    public void ShowPanel()
    {
        resurcePanel.SetActive(true);
        RedrawFinanceInfo();

        SetDrawMode(EmployeeListDrawMode.team);
    }
    public void HidePanel()
    {
        CurrentEmployee = null;
        ClearEmployeeList();
        resurcePanel.SetActive(false);
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

    public void OnDissmissCurrentEmployeeClick()
    {
        if(CurrentEmployee.EmployeeSpeachPack.TryGetDissmissSpeach(out MessagePanelPack pack))
        {
            GameUICenter.messageQueue.PrepareMessage(pack.Header, pack.Message,
                DissmissCurrentEmployee, () => { });
        }
        else
        {
            DissmissCurrentEmployee();
        }
    }
    public void DissmissCurrentEmployee()
    {
        employeeSystem.DismissEmployee(CurrentEmployee);
        CurrentEmployee = null;
    }

    public void SetDrawMode(EmployeeListDrawMode drawMode)
    {
        Mode = drawMode;
        if(_mode == EmployeeListDrawMode.team)
        {
            RebuildEmployeesList(employeeSystem.Employees);
        }
        else
        {
            RebuildEmployeesList(employeeSystem.Recruts);
        }
    }

    public void ShowEmployees(List<Employee> employees)
    {
        if(_mode == EmployeeListDrawMode.team)
        {
            RebuildEmployeesList(employees);
        }
    }
    public void ShowRecruts(List<Employee> employees)
    {
        if (_mode == EmployeeListDrawMode.recruts)
        {
            RebuildEmployeesList(employees);
        }
    }

    public void PlusSalaryForCurrentEmployee()
    {
        CurrentEmployee.PlusSalary(1);
    }
    public void MinusSalaryForCurrentEmployee()
    {
        if(CurrentEmployee.BaseSalary <= 10)
        {
            GameUICenter.messageQueue.PrepareMessage("����������� ���������", "� �� ���� �������� �� ������� ��������. " +
                "���� ���������� ��� ����. ���� � ����������. ��� �������� ����?", DissmissCurrentEmployee, () => { });
        }
        else
        {
            CurrentEmployee.MinusSalary(1);
        }
    }
    private void OnEmployeeUIItemClick(Employee e)
    {
        if(Mode == EmployeeListDrawMode.team)
        {
            SelectEmployee(e);
        }
        else
        {
            AddNewEmployee(e);
        }
    }

    private void AddNewEmployee(Employee e)
    {
        if(financeSystem.CurrentSum > e.CostOfAttracting)
        {
            financeSystem.CurrentSum -= e.CostOfAttracting;
            employeeSystem.AddRecrutToTeam(e);
        }
        else
        {
            GameUICenter.messageQueue.PrepareMessage("�� �� ����!", "��� �� ������� �����, ����� ��������� ��������� ����� ���������");
        }
    }
    private void SelectEmployee(Employee e)
    {
        if (CurrentEmployee != e)
        {
            if (CurrentEmployee != null)
            {
                CurrentEmployee.EmployeeInfoChanged -= RedrawCurrentEmployeeInfo;
            }
            CurrentEmployee = e;
            e.EmployeeInfoChanged += RedrawCurrentEmployeeInfo;
            RedrawCurrentEmployeeInfo();
        }
    }

    private void ClearEmployeeList()
    {
        for (int i = 0; i < employeeListContentContainer.childCount; i++)
        {
            EmployeeUiItem item = employeeListContentContainer.GetChild(i).gameObject.GetComponent<EmployeeUiItem>();
            item.OnDestroy();
            Destroy(item.gameObject);
        }
    }
    private void RebuildEmployeesList(List<Employee> employees)
    {
        ClearEmployeeList();
        foreach (Employee e in employees)
        {
            EmployeeUiItem item = Instantiate(employeeUIItemPrefab, employeeListContentContainer).GetComponent<EmployeeUiItem>();
            item.Init(e, _mode == EmployeeListDrawMode.recruts);
            item.OnEmployeeClick += OnEmployeeUIItemClick;
        }
    }
    private void RedrawCurrentEmployeeInfo()
    {
        if(CurrentEmployee != null)
        {
            fatigueSlider.maxValue = CurrentEmployee.EmployeeStatsPack.fatigueThresholdValue;
            stressSlider.maxValue = CurrentEmployee.EmployeeStatsPack.stressThresholdValue;
            stressSlider.value = CurrentEmployee.Stress;
            fatigueSlider.value = CurrentEmployee.Fatigue;
            nameText.text = CurrentEmployee.Name;
            stateText.text = CurrentEmployee.SalaryStrategyName;
            paymentText.text = CurrentEmployee.GetSalaryInfo();
        }
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

public enum EmployeeListDrawMode
{
    team,
    recruts
}