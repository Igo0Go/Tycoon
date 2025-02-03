using UnityEngine;

public class GameCenter : MonoBehaviour
{
    private void Start()
    {
        TimeSystem timeSystem = FindObjectOfType<TimeSystem>();
        FinanceSystem financeSystem = FindObjectOfType<FinanceSystem>();

        TimePanel timerDisplay = FindObjectOfType<TimePanel>();
        TimeSkipPanel timeSkipPanel = FindObjectOfType<TimeSkipPanel>();
        MessagePanel messagePanel = FindObjectOfType<MessagePanel>();
        ResourcePanel resourcePanel = FindObjectOfType<ResourcePanel>();
        MessageQueue messageQueue = FindObjectOfType<MessageQueue>();
        EmployeeSystem employeeSystem = FindObjectOfType<EmployeeSystem>();
        OvertimeSelectorPanel overtimeSelectorPanel = FindObjectOfType<OvertimeSelectorPanel>();

        GameUICenter.messageQueue = messageQueue;
        messagePanel.SubscribeEvents(messageQueue);
        messageQueue.SubscribeEvents(messagePanel);
        timeSkipPanel.SubscribeEvents(timeSystem);
        timerDisplay.SubscribeEvents(timeSystem);

        overtimeSelectorPanel.SubscribeEvents(employeeSystem, timeSystem);
        financeSystem.SubscribeEvents(timeSystem, employeeSystem);
        employeeSystem.SubscribeEvents(timeSystem);
        resourcePanel.SubscribeEvents(financeSystem, employeeSystem);

        overtimeSelectorPanel.SetUp();
        timerDisplay.SetUp();
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
