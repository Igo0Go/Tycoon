using UnityEngine;

public class MainGameplayPanel : MonoBehaviour
{
    [SerializeField]
    private TimeSkipPanel skipTimePanel;
    [SerializeField]
    private MessagePanel messagePanel;

    public void SetUp()
    {
        skipTimePanel.gameObject.SetActive(false);
    }

    public void ShowSkipPanel()
    {
        skipTimePanel.gameObject.SetActive(true);
        skipTimePanel.SetUpPanel();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

public static class GameUICenter
{
    public static MessagePanel messagePanel;
}
