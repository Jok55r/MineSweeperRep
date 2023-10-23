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
        mineText.text = Global.mineChance.ToString();
        questionText.text = Global.questionChance.ToString();
        exclamationText.text = Global.exclamationChance.ToString();
        morelessText.text = Global.morelessChance.ToString();
        xText.text = Global.x.ToString();
        yText.text = Global.y.ToString();
    }

    private void Update()
    {
        tmpMines.text = $"{Global.currentMinesCount} ({Global.minesCount})";

        if (GameFlow.gameState == GameState.preGame)
            ChangeBackgroundCol(normalCol);
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
        => Global.mineChance = Convert.ToInt32(tmp.text);
    public void ChangeQuestionChance(TMP_InputField tmp)
        => Global.questionChance = Convert.ToInt32(tmp.text);
    public void ChangeExclamationChance(TMP_InputField tmp)
        => Global.exclamationChance = Convert.ToInt32(tmp.text);
    public void ChangeMoreLessChance(TMP_InputField tmp)
        => Global.morelessChance = Convert.ToInt32(tmp.text);
    public void ChangeX(TMP_InputField tmp)
        => Global.x = Convert.ToInt32(tmp.text);
    public void ChangeY(TMP_InputField tmp)
        => Global.y = Convert.ToInt32(tmp.text);
}
