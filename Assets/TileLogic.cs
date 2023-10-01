using UnityEngine;
using TMPro;
using System.ComponentModel;

public class TileLogic : MonoBehaviour
{
    public GameObject self;
    public float colorRND;
    public Color normalColor;
    public TextMeshPro textMeshPro;

    public Type type;
    public int mines;
    public NeighborType neighborType;
    public State state;
    public TileLogic[] neighbors;

    public int x;
    public int y;

    private void Start()
    {
        float rnd = Random.Range(0.5f-colorRND, 0.5f+colorRND);
        normalColor = new Color(rnd, rnd, rnd);
        SetColor(normalColor, gameObject);
        textMeshPro.text = "";
        state = State.none;
    }

    public void Reveal()
    {
        if (state == State.revealed) return;

        state = State.revealed;
        if (type == Type.bomb)
        {
            SetColor(Color.red, gameObject);
            GM.mines--;
        }
        else
        {
            SetText(mines.ToString());
            SetColor(Color.black, gameObject);
            if (mines == 0) OpenNeighbors();
        }
    }

    public void OpenNeighbors()
    {
        foreach (var tile in neighbors)
        {
            if (tile.state == State.none) tile.Reveal();
        }
    }

    private void Mark()
    {
        if (state == State.none)
        {
            state = State.marked;
            SetColor(Color.white, gameObject);
            GM.mines--;
        }
        else if (state == State.marked)
        {
            state = State.none;
            SetColor(normalColor, gameObject);
            GM.mines++;
        }
    }

    public void ChooseType(int mineChance, int questionChance, int exclamationChance)
    {
        if (Random.Range(0, 100) < mineChance)
        {
            type = Type.bomb;
            GM.allMines++;
        }
        else if (Random.Range(0, 100) < questionChance)
            type = Type.question;
        else if (Random.Range(0, 100) < exclamationChance)
            type = Type.exclamation;
        else
            type = Type.normal;
    }
    


    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            Reveal();
        if (Input.GetKeyDown(KeyCode.Mouse1))
            Mark();
    }

    public void SetColor(Color color, GameObject obj)
        => obj.GetComponent<SpriteRenderer>().color = color;

    public void SetText(string text)
    {
        if (text == "0")
        {
            textMeshPro.text = "";
        }
        else if (type == Type.question)
        {
            textMeshPro.text = "?";
        }
        else if (type == Type.exclamation)
        {
            int rnd = Random.Range(0, 2) == 0 ? mines - 1 : mines + 1;
            textMeshPro.text = (rnd == 0 ? 2 : rnd) + "!";
        }
        else
        {
            textMeshPro.text = text;
        }
    }

    public void SetNeighbors(int neighbors)
        => mines = neighbors;
}

public enum State
{
    none,
    marked,
    revealed
}

public enum Type
{
    bomb,
    normal,
    question,
    exclamation
}

public enum NeighborType
{
    // "a" is normal square grid
    // "b" is hexagonal grid

    // number represents neighbors

    a8,
    a4a,
    a4b
    //b6
}