using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Пакет с фразами сотрудника
/// </summary>
[Serializable]
public class EmployeeSpeachPack
{
    [Tooltip("Фраза приветствия при рекрутировании")]
    public MessagePanelPack recrutingSpeach;

    [Tooltip("Список фраз для ситуации с увольнением")]
    [SerializeField]
    private List<MessagePanelPack> dissmissSpeach;

    [Tooltip("Список фраз для ситуации максимального понижения заработной платы")]
    [SerializeField]
    private List<MessagePanelPack> minSalarySpeach;

    [Tooltip("Список фраз для ситуации максимального понижения заработной платы")]
    [SerializeField]
    private List<string> employeeMaxFatigueMesseges;


    [Tooltip("Фраза при максимальном стрессе")]
    [TextArea(5, 10)]
    public string employeeMaxStressMessege;

    /// <summary>
    /// Проверить, есть ли ещё у сотрудника фразы перед увольнением. Если их нет, сотрудник уйдёт, ничего не сказав.
    /// </summary>
    /// <param name="pack">Возвращаемый пакет с фразой</param>
    /// <returns>Остались ли фразы</returns>
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
    /// Проверить, есть ли ещё у сотрудника фразы при малой зарплате. Если их нет, сотрудник уйдёт, ничего не сказав.
    /// </summary>
    /// <param name="pack">Возвращаемый пакет с фразой</param>
    /// <returns>Остались ли фразы</returns>
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
    /// Получить случайную фразу при максимальной усталости. Каждый раз сотрудник будет оправыдываться случайной фразой
    /// </summary>
    /// <returns>Пакет с фарзой</returns>
    public string GetRandomMaxFatigueSpeach()
    {
        return employeeMaxFatigueMesseges[UnityEngine.Random.Range(0, employeeMaxFatigueMesseges.Count)];
    }
}
