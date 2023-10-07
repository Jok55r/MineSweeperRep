using UnityEngine;
using TMPro;

public class TileLogic : MonoBehaviour
{
    public GameObject self;
    public float colorRND;
    public Color normalColor;
    public TextMeshPro textMeshPro;

    public bool isMine = false;
    public int mines;
    public Type type;
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
        if (isMine)
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
    
    private void MakeTile()
    {
        isMine = false;
        Reveal();
        GM.CountNeighbors(x, y);
    }

    private void MakeBomb()
    {
        isMine = true;
        Reveal();
        GM.CountNeighbors(x, y);
    }

    private void OnMouseOver()
    {
        if (!GM.creating)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
                MakeTile();
            if (Input.GetKeyDown(KeyCode.Mouse1))
                MakeBomb();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
                Reveal();
            if (Input.GetKeyDown(KeyCode.Mouse1))
                Mark();
        }
    }


    public void CreateMine(int chance)
    {
        if (Random.Range(0, 100) < chance)
        {
            isMine = true;
            GM.allMines++;
        }
    }

    public void SetColor(Color color, GameObject obj)
        => obj.GetComponent<SpriteRenderer>().color = color;

    public void SetText(string text)
        => textMeshPro.text = text == "0" ? "" : text;

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
    // "a" is normal square grid
    // "b" is hexagonal grid

    // number represents neighbors

    a8,
    a4a,
    a4b
    //b6
}