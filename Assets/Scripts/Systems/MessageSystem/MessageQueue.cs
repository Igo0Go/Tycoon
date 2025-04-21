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
    /// Написать сообщение в систему лога
    /// </summary>
    /// <param name="text">текст сообщения</param>
    public void Log(string text)
    {
        NewLog?.Invoke(text);
    }

    /// <summary>
    /// Подготавливает сообщение для панели вывода сообщений, которое можно закрыть сразу после нажатия кнопки ОК
    /// </summary>
    /// <param name="header">Заголовок сообщения</param>
    /// <param name="message">Текст сообщения</param>
    public void PrepareMessage(string header, string message)
    {
        PrepareMessage(header, message, null, null, null);
    }

    /// <summary>
    /// Подготавливает сообщение для панели вывода сообщений, которое выполняет действие сразу после нажатия кнопки ОК
    /// </summary>
    /// <param name="header">Заголовок сообщения</param>
    /// <param name="message">Текст сообщения</param>
    /// <param name="okAction">Метод, который необходимо выполнить после нажатия кнопки ОК</param>
    public void PrepareMessage(string header, string message, Action okAction)
    {
        PrepareMessage(header, message, null, null, okAction);
    }

    /// <summary>
    /// Подготавливает сообщение для панели вывода сообщений, которое предполагает выбор с согласием или отказом
    /// </summary>
    /// <param name="header">Заголовок сообщения</param>
    /// <param name="message">Текст сообщения</param>
    /// <param name="yesAction">Метод, который будет выполнен при согласии</param>
    /// <param name="noAction">Метод, который будет выполнен при отказе</param>
    public void PrepareMessage(string header, string message, Action yesAction, Action noAction)
    {
        PrepareMessage(header, message, yesAction, noAction, null);
    }

    /// <summary>
    /// Подготавливает сообщение для панели вывода сообщений
    /// </summary>
    /// <param name="header">Заголовок сообщения</param>
    /// <param name="message">Текст сообщения</param>
    /// <param name="okAction">Метод, который необходимо выполнить после нажатия кнопки ОК</param>
    /// <param name="yesAction">Метод, который будет выполнен при согласии</param>
    /// <param name="noAction">Метод, который будет выполнен при отказе</param>
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
    /// Заголовок сообщения
    /// </summary>
    public string Header;
    /// <summary>
    /// Текст сообщения
    /// </summary>
    [TextArea(5, 10)]
    public string Message;
}
