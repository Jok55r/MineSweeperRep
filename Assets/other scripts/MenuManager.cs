using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject panel;

    public void PanelToggle()
        => panel.SetActive(!panel.active);

    public void StartGame()
    {
        PlayerPrefs.SetString("level_name", "random");
        SceneManager.LoadScene("GameScene");
    }

    public void Menu()
    {
        GM.creatorMode = false;
        SceneManager.LoadScene("Menu");
    }

    public void ExitGame()
    {
        UnityEngine.Application.Quit();
        Debug.Log("Exited Application...");
    }
}