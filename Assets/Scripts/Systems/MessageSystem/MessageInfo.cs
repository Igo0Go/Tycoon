using System;

public class MessageInfo
{
    /// <summary>
    /// Заголовок сообщения
    /// </summary>
    public string Header;
    /// <summary>
    /// Текст сообщения
    /// </summary>
    public string Message;

    /// <summary>
    /// Действие при клике кнопки ДА
    /// </summary>
    public Action yesAction;
    /// <summary>
    /// Действие при клике кнопки НЕТ
    /// </summary>
    public Action noAction;
    /// <summary>
    /// Действие при клике кнопки ОК
    /// </summary>
    public Action okAction;
}
