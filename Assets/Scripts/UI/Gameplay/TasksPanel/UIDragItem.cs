using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public UIDropQueueSlot mySlot;
    public UIDropQueueSlot currentActiveSlot;

    private Canvas myCanvas;
    private GraphicRaycaster graphicRaycaster;

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.localPosition += new Vector3(eventData.delta.x, eventData.delta.y, 0) / transform.lossyScale.x;

        if (!myCanvas)
        {
            myCanvas = FindObjectOfType<Canvas>();
            graphicRaycaster = myCanvas.GetComponent<GraphicRaycaster>();
        }

        mySlot.RemoveItem(this);
        transform.SetParent(myCanvas.transform, true);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.localPosition += new Vector3(eventData.delta.x, eventData.delta.y, 0) / transform.lossyScale.x;
        var results = new List<RaycastResult>();
        graphicRaycaster.Raycast(eventData, results);

        foreach (var hit in results)
        {
            var slot = hit.gameObject.GetComponent<UIDropQueueSlot>();

            if(slot != null)
            {
                if (slot != currentActiveSlot)
                {
                    currentActiveSlot?.HidePlaceHolder();
                    currentActiveSlot = slot;
                    currentActiveSlot.CheckPosition(this);
                    break;
                }
                else if (slot == currentActiveSlot)
                {
                    currentActiveSlot.CheckPosition(this);
                }
            }
            else
            {
                currentActiveSlot?.HidePlaceHolder();
                currentActiveSlot = null;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var results = new List<RaycastResult>();
        graphicRaycaster.Raycast(eventData, results);

        bool needReturn = true;

        foreach (var hit in results)
        {
            var slot = hit.gameObject.GetComponent<UIDropQueueSlot>();
            if (slot)
            {
                slot.AddItemToThisSlot(this);
                needReturn = false;
                break;
            }
        }
        if (needReturn)
        {
            mySlot.AddItemToThisSlot(this);
        }
        
    }
}
