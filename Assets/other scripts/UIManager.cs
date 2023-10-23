using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Camera mainCamera;
    public Color normalCol;
    public Color winCol;

    public TMP_InputField mineText;
    public TMP_InputField questionText;
    public TMP_InputField exclamationText;
    public TMP_InputField morelessText;
    public TMP_InputField xText;
    public TMP_InputField yText;

    public GameObject tutorPanel;

    public TextMeshProUGUI tmpMines;

    void Start()
    {
        mineText.text = GM.mineChance.ToString();
        questionText.text = GM.questionChance.ToString();
        exclamationText.text = GM.exclamationChance.ToString();
        morelessText.text = GM.morelessChance.ToString();
        xText.text = GM.x.ToString();
        yText.text = GM.y.ToString();
    }

    private void Update()
    {
        tmpMines.text = $"{GM.currentMinesCount} ({GM.minesCount})";

        if (GM.won)
        {
            ChangeBackgroundCol(winCol);
        }
    }

    public void NewLevel()
    {
        ChangeBackgroundCol(normalCol);
    }

    public void ChangeBackgroundCol(Color color)
        => mainCamera.backgroundColor = color;


    public void SetTutorPanel(bool set)
        => tutorPanel.SetActive(set);


    public void ChangeMineChance(TMP_InputField tmp)
        => GM.mineChance = Convert.ToInt32(tmp.text);
    public void ChangeQuestionChance(TMP_InputField tmp)
        => GM.questionChance = Convert.ToInt32(tmp.text);
    public void ChangeExclamationChance(TMP_InputField tmp)
        => GM.exclamationChance = Convert.ToInt32(tmp.text);
    public void ChangeMoreLessChance(TMP_InputField tmp)
        => GM.morelessChance = Convert.ToInt32(tmp.text);
    public void ChangeX(TMP_InputField tmp)
        => GM.x = Convert.ToInt32(tmp.text);
    public void ChangeY(TMP_InputField tmp)
        => GM.y = Convert.ToInt32(tmp.text);
}
