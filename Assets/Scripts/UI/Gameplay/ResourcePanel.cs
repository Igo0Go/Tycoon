using TMPro;
using UnityEngine;

public class ResourcePanel : MonoBehaviour
{
    [Header("Маля панель")]
    [SerializeField]
    private TMP_Text currentSumText;


    [SerializeField]
    private GameObject resurcePanel;

    public void SubscribeEvents(FinanceSystem system)
    {
        system.currentSummChanged += OnCurrentSumChanged;
    }


    public void OnCurrentSumChanged(int newSum)
    {
        currentSumText.text = newSum.ToString();
    }
}
