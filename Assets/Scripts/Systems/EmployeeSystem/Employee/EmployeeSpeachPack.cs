using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����� � ������� ����������
/// </summary>
[Serializable]
public class EmployeeSpeachPack
{
    [Tooltip("����� ����������� ��� ��������������")]
    public MessagePanelPack recrutingSpeach;

    [Tooltip("������ ���� ��� �������� � �����������")]
    [SerializeField]
    private List<MessagePanelPack> dissmissSpeach;

    [Tooltip("������ ���� ��� �������� ������������� ��������� ���������� �����")]
    [SerializeField]
    private List<MessagePanelPack> minSalarySpeach;

    [Tooltip("������ ���� ��� �������� ������������� ��������� ���������� �����")]
    [SerializeField]
    private List<string> employeeMaxFatigueMesseges;


    [Tooltip("����� ��� ������������ �������")]
    [TextArea(5, 10)]
    public string employeeMaxStressMessege;

    /// <summary>
    /// ���������, ���� �� ��� � ���������� ����� ����� �����������. ���� �� ���, ��������� ����, ������ �� ������.
    /// </summary>
    /// <param name="pack">������������ ����� � ������</param>
    /// <returns>�������� �� �����</returns>
    public bool TryGetDissmissSpeach(out MessagePanelPack pack)
    {
        if(dissmissSpeach.Count > 0)
        {
            pack = dissmissSpeach[0];
            dissmissSpeach.RemoveAt(0);
            return true;
        }

        pack = null;
        return false;
    }

    /// <summary>
    /// ���������, ���� �� ��� � ���������� ����� ��� ����� ��������. ���� �� ���, ��������� ����, ������ �� ������.
    /// </summary>
    /// <param name="pack">������������ ����� � ������</param>
    /// <returns>�������� �� �����</returns>
    public bool TryGetMinSalarySpeach(out MessagePanelPack pack)
    {
        if (minSalarySpeach.Count > 0)
        {
            pack = minSalarySpeach[0];
            dissmissSpeach.RemoveAt(0);
            return true;
        }

        pack = null;
        return false;
    }

    /// <summary>
    /// �������� ��������� ����� ��� ������������ ���������. ������ ��� ��������� ����� �������������� ��������� ������
    /// </summary>
    /// <returns>����� � ������</returns>
    public string GetRandomMaxFatigueSpeach()
    {
        return employeeMaxFatigueMesseges[UnityEngine.Random.Range(0, employeeMaxFatigueMesseges.Count)];
    }
}
