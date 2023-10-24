using JetBrains.Annotations;
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

    public List<Tile> neighbors;

    public Position pos;

    public static bool neighborsUp = false;

    public int markNum = 0;

    public int MineCount()
    {
        int count = 0;
        foreach (var neighbor in neighbors)
        {
            if (neighbor.type == Type.mine)
                count++;
            if (neighbor.type == Type.negativeMine)
                count--;
        }
        return count;
    }

    public bool IsZero()
    {
        foreach (var neighbor in neighbors)
        {
            if (neighbor.type == Type.mine || neighbor.type == Type.negativeMine)
                return false;
        }
        return true;
    }

    public void Start()
    {
        int rndNum = new System.Random().Next(0, neighbors.Count);
        addonNum = rndNum == MineCount() ? 8 : rndNum;

        if (GameFlow.gameState == GameState.creator)
            Reveal(false);
    }

    private void OnMouseOver()
    {
        if (GameFlow.gameState == GameState.creator && Input.GetKeyDown(KeyCode.Mouse0))
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

    private void OnMouseEnter()
    {
        foreach (var tile in neighbors)
            tile.visual.Render(true);
    }
    private void OnMouseExit()
    {
        foreach (var tile in neighbors)
            tile.visual.Render(false);
    }

    public void Reveal(bool counts)
    {
        if (GameFlow.gameState == GameState.endGame)
            return;
        if (GameFlow.gameState == GameState.preGame)
        {
            FieldManager.AdjustTile(pos);

            foreach (var tile in FieldManager.tiles)
                NeighborStrategy.CountNeighbors(tile);

            GameFlow.gameState = GameState.inGame;
        }


        if (type != Type.normal && state != State.marked )
        {
            GameFlow.gameState = GameState.endGame;
            Global.currentMinesCount--;
        }
        else if (state == State.none)
        {
            if (IsZero())
            {
                state = State.revealed;
                OpenNeighbors();
            }
            state = State.revealed;
            Global.revealedCount++;
        }

        visual.Render(this);
    }

    private void Mark()
    {
        if (state == State.none)
        {
            state = State.marked;
            Global.currentMinesCount--;
            Global.revealedCount++;
        }
        else if (state == State.marked)
        {
            if (markNum == 0)
            {
                markNum = 1;
            }
            else
            {
                markNum = 0;
                state = State.none;
                Global.currentMinesCount++;
                Global.revealedCount--;
            }
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

        if (type == Type.mine)
            state = State.marked;
        else
            state = State.revealed;

        Global.minesCount += type == Type.mine ? 1 : -1;

        foreach (var tile in neighbors)
            tile.ReCount();

        visual.Render(this);
    }

    public void ReCount()
    {
        if (type == Type.mine)
            state = State.marked;
        else
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
    negativeMine,
}