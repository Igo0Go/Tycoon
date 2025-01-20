using System;
using UnityEngine;

public class FinanceSystem : MonoBehaviour
{
    [SerializeField]
    private int currentSum;
    [SerializeField, Min(1)]
    private int dayRentCost = 500;
    [SerializeField, Min(1)]
    private int dayUtilityCosts = 25;
    [SerializeField, TextArea(5, 10)]
    private string financeLostText;


    public int CurrentSum
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
    public int DayRentCosts => dayRentCost;
    public int DayUtilityCosts => dayUtilityCosts;
    public int DayEmployesPayment => 0;

    public event Action<int> currentSummChanged;
    public event Action financeLost;

    public void SetUp()
    {
        currentSummChanged?.Invoke(currentSum);
    }

    public void SubscribeEvents(TimeSystem timeSystem)
    {
        timeSystem.endDay += OnDayEnded;
    }

    private void OnDayEnded()
    {
        GameUICenter.messageQueue.PrepareMessage("Ваш чек", "За аренду помещения и сопуствующие услуги требуется заплатить " + 
            (dayRentCost + dayUtilityCosts).ToString());
        CurrentSum -= (dayRentCost + dayUtilityCosts);
    }
}
