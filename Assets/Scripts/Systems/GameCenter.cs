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
        EmployeeTaskSystem employeeTaskSystem = FindObjectOfType<EmployeeTaskSystem>();
        TaskPanel taskPanel = FindObjectOfType<TaskPanel>();
        ProjectDaysPanel projectDaysPanel = FindObjectOfType<ProjectDaysPanel>();

        GameUICenter.messageQueue = messageQueue;
        messagePanel.SubscribeEvents(messageQueue);
        messageQueue.SubscribeEvents(messagePanel);
        timeSkipPanel.SubscribeEvents(timeSystem);
        timerDisplay.SubscribeEvents(timeSystem);
        taskPanel.SubscribeEvents(employeeTaskSystem, employeeSystem);
        overtimeSelectorPanel.SubscribeEvents(employeeSystem, timeSystem);
        financeSystem.SubscribeEvents(timeSystem, employeeSystem);
        employeeSystem.SubscribeEvents(timeSystem);
        resourcePanel.SubscribeEvents(financeSystem, employeeSystem);
        employeeTaskSystem.SubscribeEvents(employeeSystem, timeSystem);
        projectDaysPanel.SubscribeEvents(timeSystem);
        timeSystem.SubscribeEvents(employeeTaskSystem, financeSystem);


        overtimeSelectorPanel.SetUp();
        timerDisplay.SetUp();
        financeSystem.SetUp();
        employeeSystem.SetUp();
        employeeTaskSystem.SetUp();
        taskPanel.SetUp(employeeSystem);
        resourcePanel.SetUp();
        timeSkipPanel.SetUp();
        timeSystem.SetUp();
    }
}

public static class GameUICenter
{
    public static MessageQueue messageQueue;
}
