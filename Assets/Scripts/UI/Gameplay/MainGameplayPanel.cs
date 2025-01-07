using UnityEngine;

public class MainGameplayPanel : MonoBehaviour
{
    [SerializeField]
    private TimeSkipPanel skipTimePanel;

    private void Awake()
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
