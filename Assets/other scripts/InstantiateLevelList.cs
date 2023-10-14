using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class InstantiateLevelList : MonoBehaviour
{
    public GameObject prefab;
    public GameObject panel;
    public List<string> levelNames;

    void Start()
    {
        int i = 0;
        int j = 0;
        foreach (string txtName in Directory.GetFiles(GM.path, "*.txt"))
        {
            string[] name = txtName.Split('/');
            float offset = prefab.GetComponent<RectTransform>().rect.width;
            GameObject obj = Instantiate(prefab, new Vector2(100 + i * offset, 500 - j * offset), Quaternion.identity, panel.transform);


            //obj.GetComponent<Button>().onClick = prefab.GetComponent<Button>().onClick;

            obj.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>().text = Path.GetFileNameWithoutExtension(name[name.Length - 1]);
            

            levelNames.Add(obj.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>().text);

            i++;
            if (i == 10)
            {
                j++;
                i = 0;
            }
        }
    }

    public void SelectedLevel(TextMeshProUGUI tmp)
    {
        PlayerPrefs.SetString("level_name", tmp.text);//"silly test");//levelNames[0]);
        SceneManager.LoadScene("GameScene");
    }
}