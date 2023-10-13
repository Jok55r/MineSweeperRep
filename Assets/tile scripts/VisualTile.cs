using TMPro;
using UnityEngine;

public class VisualTile : MonoBehaviour
{
    public float colorRND;
    public float colorDarken;
    public Color tileColor;
    public TextMeshPro tmp;

    void Start()
    {
        SetBasicColor();
        tmp.text = "";
    }

    public void Render(Tile tile)
    {
        SetColor(tile);
        if (tile.state == State.revealed)
            SetText(tile);
    }

    public void SetColor(Tile tile)
    {
        Color color = Color.white;
        if (tile.state == State.none)
        {
            SetBasicColor();
            return;
        }
        else if (tile.type == Type.mine)
        {
            color = Color.white;
        }
        else if (tile.state == State.revealed)
        {
            if (tile.mineCount == 0)
                color = new Color(tileColor.r - colorDarken, tileColor.g - colorDarken, tileColor.b - colorDarken);
            else
                color = tileColor;
        }
        gameObject.GetComponent<SpriteRenderer>().color = color;
    }

    public void SetBasicColor()
    {
        float rnd = Random.Range(0.5f - colorRND, 0.5f + colorRND);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(rnd, rnd, rnd);
    }

    public void SetText(Tile tile)
    {
        tmp.text = tile.mineCount == 0 ? "" : tile.mineCount.ToString();
        if (AverageCol() > 0.5f)
            tmp.color = Color.black;
        else
            tmp.color = Color.white;
    }

    private float AverageCol()
        => (tileColor.r + tileColor.g + tileColor.b) / 3;
}
