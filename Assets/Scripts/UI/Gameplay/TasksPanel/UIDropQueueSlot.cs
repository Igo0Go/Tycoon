using System;
using System.Collections.Generic;
using UnityEngine;

public class UIDropQueueSlot : MonoBehaviour
{
    [SerializeField]
    private Transform container;
    [SerializeField]
    private UIDragItem plaseHolder;
    [SerializeField]
    private List<UIDragItem> items = new List<UIDragItem>();

    public event Action<EmployeeTask> taskAdd;
    public event Action<EmployeeTask> taskRemove;

    private void Awake()
    {
        HidePlaceHolder();
        foreach (UIDragItem item in items)
        {
            item.mySlot = this;
        }
    }

    public void RemoveItem(UIDragItem item)
    {
        items.Remove(item);
        taskRemove?.Invoke(item.GetComponent<TaskCardUIItem>().Task);
    }

    public void CheckPosition(UIDragItem item)
    {
        bool findPlace = false;

        for (int i = 0; i < items.Count; i++)
        {
            float itemY = item.transform.position.y;
            float targetY = items[i].transform.position.y;

            if (itemY > targetY -25)
            {
                plaseHolder.gameObject.SetActive(true);
                items.Remove(plaseHolder);
                items.Insert(i, plaseHolder);
                findPlace = true;
                break;
            }
        }

        if (!findPlace)
        {
            items.Remove(plaseHolder);
            items.Add(plaseHolder);
        }

        CheckItemsPositionsInTransform();
    }

    public void HidePlaceHolder()
    {
        plaseHolder.transform.SetAsLastSibling();
        plaseHolder.gameObject.SetActive(false);
        items.Remove(plaseHolder);
        items.Add(plaseHolder);
    }

    public void DestroyCardWithThisTask(EmployeeTask task)
    {
        TaskCardUIItem card = null;
        for (int i = 0;i < items.Count;i++)
        {
            if (items[i].TryGetComponent(out card))
            {
                if(card.Task == task)
                {
                    items.RemoveAt(i);
                    break;
                }
            }
        }

        if(card != null)
        {
            Destroy(card.gameObject);
        }
    }

    public void AddItemToThisSlot(UIDragItem item)
    {
        item.mySlot = this;
        item.transform.SetParent(container);

        bool findPlace = false;

        for (int i = 0; i < items.Count; i++)
        {
            if (item.transform.position.y > items[i].transform.position.y - 25)
            {
                items.Insert(i, item);
                findPlace = true;
                break;
            }
        }

        if (!findPlace)
        {
            items.Add(item);
        }

        HidePlaceHolder();
        CheckItemsPositionsInTransform();
        taskAdd?.Invoke(item.GetComponent<TaskCardUIItem>().Task);
    }

    private void CheckItemsPositionsInTransform()
    {
        for (int i = 0;i < items.Count;i++)
        {
            items[i].transform.SetAsLastSibling();
        }
    }
}