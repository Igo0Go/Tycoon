using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipInvokeObject : TooltipElement
{
    [SerializeField]
    private string text;

    protected override string GetStringForTooltip()
    {
        return text;
    }
}

public abstract class TooltipElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private const float delayTime = 0.5f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(CheckCoroutine());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        UI_Tooltip.HideTooltip_Static();
    }

    private IEnumerator CheckCoroutine()
    {
        Vector3 lastMousePos = Input.mousePosition;
        float t = 0;
        while (true)
        {
            if (Input.mousePosition == lastMousePos)
            {
                t += Time.deltaTime;
            }
            else
            {
                lastMousePos = Input.mousePosition;
                t = 0;
            }

            if (t > delayTime)
            {
                UI_Tooltip.ShowTooltipInForTheObject_Static(GetStringForTooltip(), GetComponent<RectTransform>());
                break;
            }

            yield return null;
        }
    }

    protected abstract string GetStringForTooltip();
}
