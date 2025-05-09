using TMPro;
using UnityEngine;

public class EmployeeOvertimeItem : MonoBehaviour
{
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text paymentText;
    [SerializeField]
    private GameObject onMarker;
    [SerializeField]
    private GameObject offMarker;

    private Employee emp;
    public void Init(Employee employee)
    {
        emp = employee;
        UpdateInfo();
        emp.EmployeeInfoChanged += UpdateInfo;
    }
    public void OnDestroy()
    {
        emp.EmployeeInfoChanged -= UpdateInfo;
    }

    public void OnClick()
    {
        emp.SetOvertimeState(!emp.OverTime);
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        nameText.text = emp.Name;
        paymentText.text = emp.GetSalaryInfo();
        onMarker.SetActive(emp.OverTime);
        offMarker.SetActive(!emp.OverTime);
    }
}
