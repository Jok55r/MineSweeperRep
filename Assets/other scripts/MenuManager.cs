using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.WSA;
using UnityEngine.UI;
using TMPro;

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
        => SceneManager.LoadScene("Menu");
}