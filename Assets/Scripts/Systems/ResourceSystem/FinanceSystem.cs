using System;
using UnityEngine;

public class FinanceSystem : MonoBehaviour
{
    [SerializeField]
    private float currentSum;
    [SerializeField, Min(1)]
    private float dayRentCost = 500;
    [SerializeField, Min(1)]
    private float dayUtilityCosts = 25;
    [SerializeField, TextArea(5, 10)]
    private string financeLostText;


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
                financeLost?.Invoke();
                GameUICenter.messageQueue.PrepareMessage("Вы - банкроты!", financeLostText);
            }
            currentSummChanged?.Invoke(currentSum);
        }
    }
    public float CurrentDayCost => DayRentCosts + DayUtilityCosts + DayEmployesPayment;
    public float DayRentCosts => dayRentCost;
    public float DayUtilityCosts => dayUtilityCosts;
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

    public event Action<float> currentSummChanged;
    public event Action currentRentCostChanged;
    public event Action financeLost;

    private EmployeeSystem employeeSystem;
   
    public void AddMoney(int money)
    {
        CurrentSum += money;
    }

    public void SetArendaCost(float newArendaCost)
    {
        dayRentCost = newArendaCost;
        currentRentCostChanged?.Invoke();
    }

    public void SetUp()
    {
        currentSummChanged?.Invoke(currentSum);
    }

    public void SubscribeEvents(TimeSystem timeSystem, EmployeeSystem employeeSystem)
    {
        timeSystem.endWork += OnDayEnded;
        this.employeeSystem = employeeSystem;
    }

    private void OnDayEnded()
    {
        GameUICenter.messageQueue.PrepareMessage("Ваш чек", "За аренду помещения и сопуствующие услуги требуется заплатить " + 
            CurrentDayCost.ToString());
        CurrentSum -= CurrentDayCost;
    }
}
