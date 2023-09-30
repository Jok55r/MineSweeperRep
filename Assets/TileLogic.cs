using UnityEngine;
using TMPro;

public class TileLogic : MonoBehaviour
{
    public GameObject self;
    public float colorRND;
    public Color normalColor;
    public TextMeshPro textMeshPro;

    public bool mine = false;
    public int neighbours;
    public Type type;
    public State state;

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

    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            Reveal();
        if (Input.GetKeyDown(KeyCode.Mouse1))
            Mark();
    }

    public void Reveal()
    {
        if (state == State.revealed) return;

        state = State.revealed;
        if (mine)
        {
            SetColor(Color.red, gameObject);
            GM.mines--;
        }
        else
        {
            SetText(neighbours.ToString());
            SetColor(Color.black, gameObject);
            if (neighbours == 0) GM.Open(x, y);
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

    public void CreateMine(int chance)
    {
        if (Random.Range(0, 100) < chance)
        {
            mine = true;
            GM.allMines++;
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

public enum Type
{
    // "a" is normal square grid
    // "b" is hexagonal grid

    // number represents neighbors

    a8,
    a4,
    //b6
}