using System;
using UnityEngine;

public class FinanceSystem : MonoBehaviour
{
    #region ����

    [SerializeField]
    [Tooltip("������� ����� ������� �� �����")]
    private float currentSum;

    [SerializeField, Min(1)]
    [Tooltip("������� ������ �� ������ ���������")]
    private float dayRentCost = 500;

    [SerializeField, Min(1)]
    [Tooltip("������� ������ ������������ �����")]
    private float dayUtilityCosts = 25;

    private EmployeeProjectData projectData;
    private EmployeeSystem employeeSystem;

    #endregion

    #region ��������

    /// <summary>
    /// ������� �����
    /// </summary>
    public float CurrentSum
    {
        get
        {
            return currentSum;
        }
        set
        {
            currentSum = value;
            if (currentSum < 0)
            {
                currentSum = 0;
                FinanceLost?.Invoke(true);
                GameUICenter.messageQueue.PrepareMessage(projectData.lostMoneyMessage.Header,
                    projectData.lostMoneyMessage.Message);
            }
            else
            {
                FinanceLost?.Invoke(false);
            }
            CurrentSummChanged?.Invoke(currentSum);
        }
    }

    /// <summary>
    /// ������� ������� ������ �� ���� �� ��: ������� �����������, ������ � ����������
    /// </summary>
    public float CurrentDayCost => DayRentCosts + DayUtilityCosts + DayEmployesPayment;

    /// <summary>
    /// ������� ������ ������
    /// </summary>
    public float DayRentCosts => dayRentCost;

    /// <summary>
    /// ������� ������ ������������ �����
    /// </summary>
    public float DayUtilityCosts => dayUtilityCosts;

    /// <summary>
    /// ������� ������ ���������� ����� ���� �����������
    /// </summary>
    public float DayEmployesPayment
    {
        get
        {
            float employeesPaymentSum = 0;

            foreach (Employee e in employeeSystem.Employees)
            {
                employeesPaymentSum += e.GetSalary();
            }

            return employeesPaymentSum;
        }
    }

    #endregion

    #region �������

    public event Action<float> CurrentSummChanged;
    public event Action CurrentRentCostChanged;
    public event Action<bool> FinanceLost;

    #endregion

    #region ������

    public void SetUp(EmployeeProjectData projectData)
    {
        this.projectData = projectData;
        CurrentSummChanged?.Invoke(currentSum);
    }

    public void SubscribeEvents(TimeSystem timeSystem, EmployeeSystem employeeSystem)
    {
        timeSystem.EndWork += OnDayEnded;
        this.employeeSystem = employeeSystem;
    }

    /// <summary>
    /// �������� �������
    /// </summary>
    /// <param name="money">�����, �� ������� ����� �������� ��� ����</param>
    public void AddMoney(int money)
    {
        CurrentSum += money;
    }

    /// <summary>
    /// ��������� ����� ����� �������� �����
    /// </summary>
    /// <param name="newArendaCost">����� ������ �������� �����</param>
    public void SetArendaCost(float newArendaCost)
    {
        dayRentCost = newArendaCost;
        CurrentRentCostChanged?.Invoke();
    }

    /// <summary>
    /// � ����� ��� ���������� ������� �������� ����. �� ���������� �� ������ �����
    /// </summary>
    private void OnDayEnded()
    {
        MessagePanelPack pack = MessagePanelPack.GetCheckMessagePack(CurrentDayCost);
        GameUICenter.messageQueue.PrepareMessage(pack.Header, pack.Message);
        CurrentSum -= CurrentDayCost;
    }

    #endregion
}
