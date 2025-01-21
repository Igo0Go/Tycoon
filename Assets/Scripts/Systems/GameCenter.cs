using UnityEngine;

public class GameCenter : MonoBehaviour
{
    private void Start()
    {
        TimeSystem timeSystem = FindObjectOfType<TimeSystem>();
        FinanceSystem financeSystem = FindObjectOfType<FinanceSystem>();

        TimerDisplay timerDisplay = FindObjectOfType<TimerDisplay>();
        TimeSkipPanel timeSkipPanel = FindObjectOfType<TimeSkipPanel>();
        MessagePanel messagePanel = FindObjectOfType<MessagePanel>();
        ResourcePanel resourcePanel = FindObjectOfType<ResourcePanel>();
        MessageQueue messageQueue = FindObjectOfType<MessageQueue>();
        EmployeeSystem employeeSystem = FindObjectOfType<EmployeeSystem>();

        GameUICenter.messageQueue = messageQueue;
        messagePanel.SubscribeEvents(messageQueue);
        messageQueue.SubscribeEvents(messagePanel);
        timeSkipPanel.SubscribeEvents(timeSystem);
        timerDisplay.SubscribeEvents(timeSystem);

        financeSystem.SubscribeEvents(timeSystem, employeeSystem);
        employeeSystem.SubscribeEvents(timeSystem);
        resourcePanel.SubscribeEvents(financeSystem, employeeSystem);

        financeSystem.SetUp();
        employeeSystem.SetUp();
        resourcePanel.SetUp();
        timeSkipPanel.SetUp();
        timeSystem.SetUp();
    }
}

public static class GameUICenter
{
    public static MessageQueue messageQueue;
}
