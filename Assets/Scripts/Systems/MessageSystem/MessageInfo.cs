using System;

public class MessageInfo
{
    /// <summary>
    /// ��������� ���������
    /// </summary>
    public string Header;
    /// <summary>
    /// ����� ���������
    /// </summary>
    public string Message;

    /// <summary>
    /// �������� ��� ����� ������ ��
    /// </summary>
    public Action yesAction;
    /// <summary>
    /// �������� ��� ����� ������ ���
    /// </summary>
    public Action noAction;
    /// <summary>
    /// �������� ��� ����� ������ ��
    /// </summary>
    public Action okAction;
}
