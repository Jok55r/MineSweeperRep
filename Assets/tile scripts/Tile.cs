using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public VisualTile visual;

    public State state = State.none;
    public NeighborType neighborType;
    public Type type;
    public int mineCount;

    public List<Tile> neighbors;

    public int x;
    public int y;

    void Start()
    {
        NeighborStrategy.CountNeighbors(this);

        if (GM.lvlMake)
            Reveal();
    }

    private void OnMouseOver()
    {
        if (GM.lvlMake && Input.GetKeyDown(KeyCode.Mouse0))
            ChangeTile();
        else if (Input.GetKeyDown(KeyCode.Mouse0))
            Reveal();
        else if (Input.GetKeyDown(KeyCode.Mouse1))
            Mark();
    }

    public void Reveal()
    {
        if (type == Type.mine)
        {
            state = State.marked;

            GM.currentMinesCount--;
            GM.lost = true;
        }
        else
        {
            state = State.revealed; 
            if (mineCount == 0)
                OpenNeighbors();
        }

        visual.Render(this);
    }

    private void Mark()
    {
        if (state == State.none)
        {
            state = State.marked;
            GM.currentMinesCount--;
        }
        else if (state == State.marked)
        {
            state = State.none;
            GM.currentMinesCount++;
        }
        visual.Render(this);
    }

    public void OpenNeighbors()
    {
        foreach (var tile in neighbors)
        {
            if (tile.state == State.none) tile.Reveal();
        }
    }

    #region making level

    public void ChangeTile()
    {
        type = type == Type.mine ? Type.normal : Type.mine;
        GM.currentMinesCount += type == Type.mine ? 1 : -1;

        foreach (var tile in neighbors)
            tile.ReCount();

        visual.Render(this);
    }

    public void ReCount()
    {
        NeighborStrategy.CountNeighbors(this);
        visual.Render(this);
    }

    #endregion making level

    public void SetNeighbors(int neighbors)
        => mineCount = neighbors;
}

public enum State
{
    none,
    marked,
    revealed
}

public enum Type
{
    normal,
    mine,
}

public enum NeighborType
{
    a8,
    a4
    //b6
}