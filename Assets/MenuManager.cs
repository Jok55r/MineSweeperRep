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
    public bool inMenu;
    public Button prefab;
    public TextMeshPro tmp;

    public void Start()
    {
        if (!inMenu)
            return;

        int i = 0;
        int j = 0;

        foreach (string txtName in Directory.GetFiles(GM.path, "*.txt"))
        {
            var obj = Instantiate(prefab, new Vector2(100+i*80, 500-j*80), Quaternion.identity, panel.transform);
            obj.transform.FindChild("Text").GetComponent<TextMeshPro>().text = txtName;

            i++;
            if (i == 0)
            {
                j++;
                i = 0;
            }
        }
    }

    public void StartGame()
        => SceneManager.LoadScene("GameScene");

    public void Menu()
        => SceneManager.LoadScene("Menu");
}