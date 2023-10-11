using UnityEngine;

public class Tile : MonoBehaviour
{
    public VisualTile visual;

    public State state = State.none;
    public NeighborType neighborType;
    public Type type;
    public int mineCount;

    public Tile[] neighbors;

    public int x;
    public int y;

    void Start()
    {
        NeighborStrategy.CountNeighbors(x, y);

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
        state = State.revealed;

        if (type == Type.mine)
            GM.currentMinesCount--;
        else if (mineCount == 0)
            OpenNeighbors();

        UpdateVisual();
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
        UpdateVisual();
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

        UpdateVisual();
    }

    public void ReCount()
    {
        NeighborStrategy.CountNeighbors(x, y);
        UpdateVisual();
    }

    #endregion making level

    #region conection with other classes

    public void SetNeighbors(int neighbors)
        => mineCount = neighbors;

    private void UpdateVisual()
    {
        visual.SetColor(this);
        visual.SetText(mineCount.ToString());
    }

    #endregion conection with other classes
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