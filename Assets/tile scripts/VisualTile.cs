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
        SetText(tile);
    }
    public void Render(bool over)
    {
        ColorChange(over ? 0.1f : -0.1f);
    }
    private void ColorChange(float x)
    {
        Color col = gameObject.GetComponent<SpriteRenderer>().color;
        col.r += x;
        col.g += x;
        col.b += x;
        gameObject.GetComponent<SpriteRenderer>().color = col;
    }

    public void SetColor(Tile tile)
    {
        Color color = Color.white;
        if (tile.state == State.none)
        {
            SetBasicColor();
            return;
        }
        else if (tile.state == State.revealed)
        {
            if (tile.IsZero())
                color = new Color(tileColor.r - colorDarken, tileColor.g - colorDarken, tileColor.b - colorDarken);
            else
                color = tileColor;
        }

        else if (tile.markNum == 0)
            color = Color.white;
        else if (tile.markNum == 1)
            color = Color.blue;


        gameObject.GetComponent<SpriteRenderer>().color = color;
    }

    public void SetBasicColor()
    {
        float rnd = Random.Range(0.5f - colorRND, 0.5f + colorRND);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(rnd, rnd, rnd);
    }

    public void SetText(Tile tile)
    {
        if (tile.type == Type.mine || tile.state != State.revealed)
        {
            tmp.text = "";
            return;
        }


        tmp.text = tile.addon switch
        {
            Addon.question => "?",
            Addon.exclamation => tile.addonNum + "!",
            Addon.moreless => tile.addonNum + (tile.addonNum < tile.MineCount() ? ">" : "<"),
            _ => tile.IsZero() ? "" : tile.MineCount().ToString()
        };
        if (tile.IsZero())
            tmp.text = "";
        
        tmp.color = AverageCol() > 0.5f ? Color.black : Color.white;
    }

    private float AverageCol()
        => (tileColor.r + tileColor.g + tileColor.b) / 3;
}
