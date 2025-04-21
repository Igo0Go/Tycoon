using System;
using System.Collections.Generic;
using UnityEngine;

public class MessageQueue : MonoBehaviour
{
    public event Action<MessageInfo> MessageReceived;
    public event Action NoMoreMessages;
    public event Action<string> NewLog;

    private MessageInfo currentMessage;

    private readonly List<MessageInfo> messages = new();

    public void SubscribeEvents(MessagePanel panel)
    {
        panel.messageHiden += ClearCurrentMessage;
    }

    /// <summary>
    /// �������� ��������� � ������� ����
    /// </summary>
    /// <param name="text">����� ���������</param>
    public void Log(string text)
    {
        NewLog?.Invoke(text);
    }

    /// <summary>
    /// �������������� ��������� ��� ������ ������ ���������, ������� ����� ������� ����� ����� ������� ������ ��
    /// </summary>
    /// <param name="header">��������� ���������</param>
    /// <param name="message">����� ���������</param>
    public void PrepareMessage(string header, string message)
    {
        PrepareMessage(header, message, null, null, null);
    }

    /// <summary>
    /// �������������� ��������� ��� ������ ������ ���������, ������� ��������� �������� ����� ����� ������� ������ ��
    /// </summary>
    /// <param name="header">��������� ���������</param>
    /// <param name="message">����� ���������</param>
    /// <param name="okAction">�����, ������� ���������� ��������� ����� ������� ������ ��</param>
    public void PrepareMessage(string header, string message, Action okAction)
    {
        PrepareMessage(header, message, null, null, okAction);
    }

    /// <summary>
    /// �������������� ��������� ��� ������ ������ ���������, ������� ������������ ����� � ��������� ��� �������
    /// </summary>
    /// <param name="header">��������� ���������</param>
    /// <param name="message">����� ���������</param>
    /// <param name="yesAction">�����, ������� ����� �������� ��� ��������</param>
    /// <param name="noAction">�����, ������� ����� �������� ��� ������</param>
    public void PrepareMessage(string header, string message, Action yesAction, Action noAction)
    {
        PrepareMessage(header, message, yesAction, noAction, null);
    }

    /// <summary>
    /// �������������� ��������� ��� ������ ������ ���������
    /// </summary>
    /// <param name="header">��������� ���������</param>
    /// <param name="message">����� ���������</param>
    /// <param name="okAction">�����, ������� ���������� ��������� ����� ������� ������ ��</param>
    /// <param name="yesAction">�����, ������� ����� �������� ��� ��������</param>
    /// <param name="noAction">�����, ������� ����� �������� ��� ������</param>
    public void PrepareMessage(string header, string message, Action yesAction, Action noAction, Action okAction)
    {
        MessageInfo info = new()
        {
            Header = header,
            Message = message,
            yesAction = yesAction,
            noAction = noAction,
            okAction = okAction
        };
        messages.Add(info);
        CheckMesageQueue();
    }

    private void CheckMesageQueue()
    {
        if(messages.Count > 0)
        {
            if(currentMessage == null)
            {
                TimeSettings.TimeSpeedMultiplier = 1;
                currentMessage = messages[0];
                messages.RemoveAt(0);
                MessageReceived?.Invoke(currentMessage);
            }
        }
        else
        {
            NoMoreMessages?.Invoke();
        }
    }

    private void ClearCurrentMessage()
    {
        currentMessage = null;
        CheckMesageQueue();
    }
}

[Serializable]
public class MessagePanelPack
{
    /// <summary>
    /// ��������� ���������
    /// </summary>
    public string Header;
    /// <summary>
    /// ����� ���������
    /// </summary>
    [TextArea(5, 10)]
    public string Message;
}
