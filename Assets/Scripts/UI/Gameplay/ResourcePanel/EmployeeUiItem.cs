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

    private Employee emp;

    public event Action<Employee> OnEmployeeClick;

    public void Init(Employee employee)
    {
        emp = employee;
        emp.employeeChanged += UpdateInfo;
        UpdateInfo();
    }
    private void OnDestroy()
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
        stateText.text = emp.State;
        paymentText.text = emp.GetSalary().ToString() + "/Ä";
    }
}
