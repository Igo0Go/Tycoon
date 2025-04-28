using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EmployeeProjectData : MonoBehaviour
{
    [Tooltip("����� ���� �� ���������� �������")]
    public int projectDayLimit = 20;

    [Tooltip("��������� � ������ ��������� �� ������")]
    public MessagePanelPack lostTimeMessage;
    [Tooltip("��������� � ������ �����������")]
    public MessagePanelPack lostMoneyMessage;

    [Tooltip("����� ���� �� ���������� �������")]
    public List<DayEvent> dayEvents = new();
}

[Serializable]
public class DayEvent
{
    [Tooltip("��������� ��� �������")]
    public MessagePanelPack messageData;
    [Tooltip("���� �������, ����� �������� ��������� ��� �������")]
    public int day;
    [Tooltip("�������� ��� �������")]
    public UnityEvent onDayEvent;
}
