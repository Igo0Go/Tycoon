using TMPro;
using UnityEngine;
using System;

public class EmployeeUiItem : MonoBehaviour
{
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text stateText;
    [SerializeField]
    private TMP_Text paymentText;
    [SerializeField]
    private TMP_Text buttonText;

    private Employee emp;
    private bool recrut;

    public event Action<Employee> OnEmployeeClick;

    public void Init(Employee employee, bool recrut)
    {
        this.recrut = recrut;
        emp = employee;
        emp.EmployeeInfoChanged += UpdateInfo;
        buttonText.text = recrut ? "Нанять" : "Детали";
        UpdateInfo();
    }
    public void OnDestroy()
    {
        emp.EmployeeInfoChanged -= UpdateInfo;
    }

    public void OnClick()
    {
        OnEmployeeClick?.Invoke(emp);
    }

    private void UpdateInfo()
    {
        nameText.text = emp.Name + " " + emp.ExperienceHours;
        stateText.text = emp.DayState;
        paymentText.text = emp.GetSalaryInfo() + " " + emp.SalaryStrategyName;

        if(recrut)
        {
            paymentText.text += "\nСтоимость привлечения: " + emp.CostOfAttracting;
        }
    }
}
