using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessagePanel : MonoBehaviour
{
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

    private void Awake()
    {
        okButton.onClick.AddListener(OnOkButtonClick);
        yesButton.onClick.AddListener(OnYesButtonClick);
        noButton.onClick.AddListener(OnNoButtonClick);

        HideAll();
    }

    public void ShowMessage(string header, string message)
    {
        HideAll();
        panel.SetActive(true);
        headerText.text = header;
        messageText.text = message;
        okButton.gameObject.SetActive(true);
    }

    public void ShowMessage(string header, string message, Action yesAction, Action noAction)
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
        panel.SetActive(false);
    }
    private void OnYesButtonClick()
    {
        yesEvent?.Invoke();
        yesEvent = null;
        panel.SetActive(false);
    }
    private void OnNoButtonClick()
    {
        noEvent?.Invoke();
        noEvent = null;
        panel.SetActive(false);
    }
}
