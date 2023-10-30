using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public VisualTile visual;

    public State state = State.none;
    public NeighborType neighborType;
    public Type type;
    public int mineCount;

    public List<Tile> neighbors;

    public Position pos;

    public void Start()
    {
        NeighborStrategy.CountNeighbors(this);

        if (GM.lvlMake)
            Reveal(false);
    }

    private void OnMouseOver()
    {
        if (GM.lvlMake && Input.GetKeyDown(KeyCode.Mouse0))
            ChangeTile();
        else if (Input.GetKeyDown(KeyCode.Mouse0))
            Reveal(true);
        else if (Input.GetKeyDown(KeyCode.Mouse1))
            Mark();
    }

    
    
    public void Reveal(bool counts)
    {
        if (type == Type.mine && state != State.marked)
        {
            state = State.marked;
            GM.lost = counts;
            GM.currentMinesCount--;
        }
        else
        {
            if (state == State.marked)
                GM.currentMinesCount--;

            state = State.revealed; 
            if (mineCount == 0)
                OpenNeighbors();
        }

        visual.Render(this);
    }

    public void Reset()
    {
        type = Type.normal;
        state = State.none;
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
            if (tile.state == State.none) tile.Reveal(false);
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

    private void OnMouseEnter()
    {
        foreach (var tile in neighbors)
            tile.visual.Render(false);
    }
    private void OnMouseExit()
    {
        foreach (var tile in neighbors)
            tile.visual.Render(true);
    }
}
public class Position
{
    public int x;
    public int y;

    public Position(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
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