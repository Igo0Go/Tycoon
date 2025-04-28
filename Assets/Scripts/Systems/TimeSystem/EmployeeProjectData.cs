using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EmployeeProjectData : MonoBehaviour
{
    [Tooltip("Лимит дней на выполнение проекта")]
    public int projectDayLimit = 20;

    [Tooltip("Сообщение в случае опоздания по срокам")]
    public MessagePanelPack lostTimeMessage;
    [Tooltip("Сообщение в случае банкротства")]
    public MessagePanelPack lostMoneyMessage;

    [Tooltip("Лимит дней на выполнение проекта")]
    public List<DayEvent> dayEvents = new();
}

[Serializable]
public class DayEvent
{
    [Tooltip("Сообщение при событии")]
    public MessagePanelPack messageData;
    [Tooltip("День проекта, после которого сработает это событие")]
    public int day;
    [Tooltip("Действия при событии")]
    public UnityEvent onDayEvent;
}
