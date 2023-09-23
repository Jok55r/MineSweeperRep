using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;
using TMPro;
using UnityEditor.Search;

public class TileLogic : MonoBehaviour
{
    public GameObject self;
    public int neighbours;
    public bool mine = false;
    public State state = State.none;
    public TextMeshPro textMeshPro;
    /*public int x;
    public int y;*/

    private void Start()
    {
        SetColor(Color.grey, gameObject);
        textMeshPro.text = "";
    }

    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            Reveal();
        if (Input.GetKeyDown(KeyCode.Mouse1))
            Mark();
    }

    public void Reveal()
    {
        state = State.revealed;
        if (mine)
        {
            SetColor(Color.red, gameObject);
        }
        else
        {
            SetText(neighbours.ToString());
            SetColor(Color.black, gameObject);
        }
    }

    private void Mark()
    {
        if (state == State.none)
        {
            state = State.marked;
            SetColor(Color.white, gameObject);
        }
        else if (state == State.marked)
        {
            state = State.none;
            SetColor(Color.grey, gameObject);
        }
    }

    public void CreateMine(int chance)
    {
        if (Random.Range(0, 10) < chance)
        {
            mine = true;
        }
    }

    public void CreateShow(int chance)
    {
        if (Random.Range(0, 10) < chance)
        {
            Reveal();
        }
    }

    public void SetColor(Color color, GameObject obj)
        => obj.GetComponent<SpriteRenderer>().color = color;

    public void SetText(string text)
        => textMeshPro.text = text == "0" ? "" : text;

    public void SetNeighbors(int neighbours)
        => this.neighbours = neighbours;
}

public enum State
{
    none,
    marked,
    revealed
}