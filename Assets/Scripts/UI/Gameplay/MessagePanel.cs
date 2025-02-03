using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessagePanel : MonoBehaviour
{
    public event Action okEvent;
    public event Action yesEvent;
    public event Action noEvent;

    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private TMP_Text headerText;
    [SerializeField]
    private TMP_Text messageText;
    [SerializeField]
    private Button okButton;
    [SerializeField]
    private Button yesButton;
    [SerializeField]
    private Button noButton;

    public event Action messageHiden;

    public void SubscribeEvents(MessageQueue messageQueue)
    {
        okButton.onClick.AddListener(OnOkButtonClick);
        yesButton.onClick.AddListener(OnYesButtonClick);
        noButton.onClick.AddListener(OnNoButtonClick);
        messageQueue.messageReceived += ShowMessage;
        messageQueue.noMoreMessages += () => panel.SetActive(false);
        HideAll();
    }

    public void ShowMessage(MessageInfo info)
    {
        if(info.yesAction == null && info.noAction == null)
        {
            ShowMessage(info.Header, info.Message, info.okAction);
        }
        else
        {
            ShowMessage(info.Header, info.Message, info.yesAction, info.noAction);
        }
    }

    private void ShowMessage(string header, string message, Action okAction)
    {
        HideAll();
        okEvent += okAction;
        panel.SetActive(true);
        headerText.text = header;
        messageText.text = message;
        okButton.gameObject.SetActive(true);
    }

    private void ShowMessage(string header, string message, Action yesAction, Action noAction)
    {
        HideAll();
        panel.SetActive(true);
        headerText.text = header;
        messageText.text = message;
        yesEvent += yesAction;
        yesButton.gameObject.SetActive(true);
        noEvent += noAction;
        noButton.gameObject.SetActive(true);
    }

    private void HideAll()
    {
        okButton.gameObject.SetActive(false);
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
        panel.SetActive(false);
    }

    private void OnOkButtonClick()
    {
        okEvent?.Invoke();
        okEvent = null;
        messageHiden?.Invoke();
    }
    private void OnYesButtonClick()
    {
        yesEvent?.Invoke();
        yesEvent = null;
        messageHiden?.Invoke();
    }
    private void OnNoButtonClick()
    {
        noEvent?.Invoke();
        noEvent = null;
        messageHiden?.Invoke();
    }
}
