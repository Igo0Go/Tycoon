using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "IgoGo/EmployeeProjectData")]
public class EmployeeProjectData : ScriptableObject
{
    public int projectDayLimit = 20;
    public List<DayEvent> dayEvents = new();

    public MessagePanelPack lostTimeMessage;
    public MessagePanelPack lostMoneyMessage;
}

[Serializable]
public class DayEvent
{
    public MessagePanelPack messageData;
    public int day;
    public UnityEvent onDayEvent;
}
