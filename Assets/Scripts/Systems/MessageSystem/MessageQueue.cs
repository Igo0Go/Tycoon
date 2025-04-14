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

    public void Log(string text)
    {
        NewLog?.Invoke(text);
    }

    public void SubscribeEvents(MessagePanel panel)
    {
        panel.messageHiden += ClearCurrentMessage;
    }

    public void PrepareMessage(string header, string message)
    {
        PrepareMessage(header, message, null, null, null);
    }

    public void PrepareMessage(string header, string message, Action okAction)
    {
        PrepareMessage(header, message, null, null, okAction);
    }

    public void PrepareMessage(string header, string message, Action yesAction, Action noAction)
    {
        PrepareMessage(header, message, yesAction, noAction, null);
    }

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
    public string Header;
    [TextArea(5, 10)]
    public string Message;
}
