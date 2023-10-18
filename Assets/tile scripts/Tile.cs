using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public VisualTile visual;

    public State state = State.none;
    public NeighborType neighborType;
    public Type type;
    public Addon addon;
    public int addonNum;
    public int mineCount;

    public List<Tile> neighbors;

    public Position pos;

    public static bool neighborsUp = false;

    public void Start()
    {
        NeighborStrategy.CountNeighbors(this);

        int rndNum = new System.Random().Next(0, neighbors.Count);
        addonNum = rndNum == mineCount ? 8 : rndNum;

        if (GM.creatorMode)
            Reveal(false);
    }

    private void OnMouseOver()
    {
        if (GM.creatorMode && Input.GetKeyDown(KeyCode.Mouse0))
            ChangeTile();
        else if (Input.GetKeyDown(KeyCode.Mouse0))
            Reveal(true);
        else if (Input.GetKeyDown(KeyCode.Mouse1))
            Mark();

         // HIGHLIGHT NEIGHBORS IN FUTURE
        /*if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.LeftControl))
        {
            foreach (var tile in neighbors)
                tile.visual.Render(neighborsUp);
            neighborsUp = !neighborsUp;
        }*/
    }

    public void Reveal(bool counts)
    {
        if (type == Type.mine && state != State.marked)
        {
            state = State.marked;
            GM.lost = counts;
            GM.currentMinesCount--;
        }
        else if (state == State.none)
        {
            if (mineCount == 0)
            {
                state = State.revealed;
                OpenNeighbors();
            }
            state = State.revealed;
            GM.revealedCount++;
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
        state = State.revealed;
        visual.Render(this);
    }

    #endregion making level

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

public enum Addon
{
    none,
    question,
    exclamation,
    moreless,
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