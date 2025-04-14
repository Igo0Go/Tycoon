using UnityEngine;

[CreateAssetMenu(menuName = "IgoGo/DayPartMessagePack")]
public class DayPartMessagePack : ScriptableObject
{
    public MessagePanelPack startDayMessage;
    public MessagePanelPack lunchMessage;
    public MessagePanelPack endWorkMessage;
    public MessagePanelPack endDayMessage;
    public MessagePanelPack overtimePrepareMessage;
}
