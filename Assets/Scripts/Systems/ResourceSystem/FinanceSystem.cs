using System;
using UnityEngine;

public class FinanceSystem : MonoBehaviour
{
    #region Поля

    [SerializeField]
    [Tooltip("Текущая сумма средств на счету")]
    private float currentSum;

    [SerializeField, Min(1)]
    [Tooltip("Дневная оплата за аренду помещения")]
    private float dayRentCost = 500;

    [SerializeField, Min(1)]
    [Tooltip("Дневная оплата коммунальных услуг")]
    private float dayUtilityCosts = 25;

    private EmployeeProjectData projectData;
    private EmployeeSystem employeeSystem;

    #endregion

    #region Свойства

    /// <summary>
    /// Текущая сумма
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
    /// Текущая дневная оплата по чеку за всё: зрплата сотрудникам, аренда и коммуналка
    /// </summary>
    public float CurrentDayCost => DayRentCosts + DayUtilityCosts + DayEmployesPayment;

    /// <summary>
    /// Дневная оплата аренды
    /// </summary>
    public float DayRentCosts => dayRentCost;

    /// <summary>
    /// Дневная оплата коммунальных услуг
    /// </summary>
    public float DayUtilityCosts => dayUtilityCosts;

    /// <summary>
    /// Дневная оплата заработной платы всем сотрудникам
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

    #region События

    public event Action<float> CurrentSummChanged;
    public event Action CurrentRentCostChanged;
    public event Action<bool> FinanceLost;

    #endregion

    #region Методы

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
    /// Добавить финансы
    /// </summary>
    /// <param name="money">Сумма, на которую будет увеличен ваш счёт</param>
    public void AddMoney(int money)
    {
        CurrentSum += money;
    }

    /// <summary>
    /// Назначить новую сумму арендной платы
    /// </summary>
    /// <param name="newArendaCost">Новый размер арендной платы</param>
    public void SetArendaCost(float newArendaCost)
    {
        dayRentCost = newArendaCost;
        CurrentRentCostChanged?.Invoke();
    }

    /// <summary>
    /// В конце дня происходит подсчёт текущего чека. Он отнимается от вашего счёта
    /// </summary>
    private void OnDayEnded()
    {
        MessagePanelPack pack = MessagePanelPack.GetCheckMessagePack(CurrentDayCost);
        GameUICenter.messageQueue.PrepareMessage(pack.Header, pack.Message);
        CurrentSum -= CurrentDayCost;
    }

    #endregion
}
