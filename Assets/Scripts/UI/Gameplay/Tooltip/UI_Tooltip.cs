using UnityEngine;
using TMPro;

public class UI_Tooltip : MonoBehaviour
{
    [SerializeField]
    private RectTransform canvasRectTransform;
    [SerializeField]
    private TMP_Text tooltipText;
    [SerializeField]
    private RectTransform backgroundRectTransform;

    private Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height/2);

    public static UI_Tooltip Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        HideTooltip();
    }

    public void ShowTooltip(string text, RectTransform target)
    {
        gameObject.SetActive(true);

        Vector3 anchoredPosition = Input.mousePosition;

        tooltipText.SetText(text);
        tooltipText.ForceMeshUpdate(); // Force text update to get correct size

        Vector2 textSize = tooltipText.GetRenderedValues(false);
        backgroundRectTransform.position = tooltipText.transform.position;

        Vector2 padding = new Vector2(10f, 6f);
        backgroundRectTransform.sizeDelta = textSize + padding;

        if (anchoredPosition.x + backgroundRectTransform.rect.width > canvasRectTransform.rect.width)
            anchoredPosition.x = canvasRectTransform.rect.width - backgroundRectTransform.rect.width;
        if (anchoredPosition.y + backgroundRectTransform.rect.height > canvasRectTransform.rect.height)
            anchoredPosition.y = canvasRectTransform.rect.height - backgroundRectTransform.rect.height;

        transform.position = anchoredPosition;
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }



    public static void ShowTooltipInForTheObject_Static(string tooltipString, RectTransform target)
    {
        Instance.ShowTooltip(tooltipString, target);
    }

    public static void HideTooltip_Static()
    {
        Instance.HideTooltip();
    }

    private Vector3 GetPos2D(RectTransform rectTransform) => Input.mousePosition + GetToCenterOffset(rectTransform);
    private Vector3 GetToCenterOffset(RectTransform rectTransform) => (screenCenter - Input.mousePosition).normalized *
        rectTransform.rect.width;
}
