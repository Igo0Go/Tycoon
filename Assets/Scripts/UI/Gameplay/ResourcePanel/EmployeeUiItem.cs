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
        emp.employeeChanged += UpdateInfo;
        buttonText.text = recrut ? "������" : "������";
        UpdateInfo();
    }
    public void OnDestroy()
    {
        emp.employeeChanged -= UpdateInfo;
    }



    public void OnClick()
    {
        OnEmployeeClick?.Invoke(emp);
    }

    private void UpdateInfo()
    {
        nameText.text = emp.Name;
        stateText.text = recrut ? "�����. ��������� �����������: " : emp.State;
        paymentText.text = recrut? emp.CostOfAttracting.ToString() : emp.GetSalaryInfo();
    }
}
