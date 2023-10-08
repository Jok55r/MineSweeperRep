using UnityEngine;

public class Tile : MonoBehaviour
{
    public VisualTile visual;

    public State state = State.none;
    public Type type;
    public bool isMine = false;
    public int mineCount;

    public Tile[] neighbors;

    public int x;
    public int y;

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

        if (isMine)
            GM.mines--;
        else if (mineCount == 0)
            OpenNeighbors();

        UpdateVisual();
    }

    private void Mark()
    {
        if (state == State.none)
        {
            state = State.marked;
            GM.mines--;
        }
        else if (state == State.marked)
        {
            state = State.none;
            GM.mines++;
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
        isMine = !isMine;
        GM.mines += isMine ? 1 : -1;

        foreach (var tile in neighbors)
            tile.ReCount();

        UpdateVisual();
    }

    private void ReCount()
    {
        GM.CountNeighbors(x, y);
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
    // "a" is normal square grid
    // "b" is hexagonal grid

    // number represents neighbors

    a8,
    a4a
    //b6
}