using System;
using System.Collections.Generic;
using UnityEngine;

public class MessageQueue : MonoBehaviour
{
    public event Action<MessageInfo> messageReceived;
    public event Action noMoreMessages;
    public event Action<string> newLog;

    private MessageInfo currentMessage;

    private List<MessageInfo> messages = new List<MessageInfo>();

    public void Log(string text)
    {
        newLog?.Invoke(text);
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
        MessageInfo info = new MessageInfo();
        info.Header = header;
        info.Message = message;
        info.yesAction = yesAction;
        info.noAction = noAction;
        info.okAction = okAction;
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
                messageReceived?.Invoke(currentMessage);
            }
        }
        else
        {
            noMoreMessages?.Invoke();
        }
    }

    private void ClearCurrentMessage()
    {
        currentMessage = null;
        CheckMesageQueue();
    }
}
