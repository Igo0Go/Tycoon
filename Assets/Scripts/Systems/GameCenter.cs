using UnityEngine;

public class GameCenter : MonoBehaviour
{
    private void Start()
    {
        TimeSystem timeSystem = FindObjectOfType<TimeSystem>();
        FinanceSystem financeSystem = FindObjectOfType<FinanceSystem>();

        MainGameplayPanel mainGameplayPanel = FindObjectOfType<MainGameplayPanel>();
        TimerDisplay timerDisplay = FindObjectOfType<TimerDisplay>();
        TimeSkipPanel timeSkipPanel = FindObjectOfType<TimeSkipPanel>();
        MessagePanel messagePanel = FindObjectOfType<MessagePanel>();
        ResourcePanel resourcePanel = FindObjectOfType<ResourcePanel>();

        GameUICenter.messagePanel = messagePanel;
        messagePanel.SubscribeEvents();
        timeSkipPanel.SubscribeEvents(timeSystem);
        timerDisplay.SubscribeEvents(timeSystem);
        mainGameplayPanel.SetUp();


        financeSystem.SubscribeEvents(timeSystem);
        resourcePanel.SubscribeEvents(financeSystem);

        financeSystem.SetUp();
        timeSystem.SetUp();
    }
}
