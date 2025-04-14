using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EmployeeSpeachPack
{
    [SerializeField]
    private List<MessagePanelPack> dissmissSpeach;
    [SerializeField]
    private List<MessagePanelPack> minSalarySpeach;

    public MessagePanelPack recrutingSpeach;

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
}
