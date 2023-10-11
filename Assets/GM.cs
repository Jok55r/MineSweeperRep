using System;
using TMPro;
using UnityEngine;

public class GM : MonoBehaviour
{
    public TextMeshPro tmpMines;
    public static int minesCount;
    public static int currentMinesCount;

    public GameObject tilePref;
    public NeighborType type;
    public bool sameGridType;
    public int mineChance;
    public int x;
    public int y;

    public static bool lvlMake;
    private float scale = 9;

    public static Tile[,] tiles;


    void Awake()
    {
        tiles = new Tile[y, x];
        NewLevel();
    }

    public void NewLevel()
    {
        DestroyField();
        CreateField();
        if (lvlMake) FieldMaker();
    }

    #region Create Field

    private void FieldMaker()
    {
        foreach (var tile in tiles) 
            tile.ReCount();
    }

    private void DestroyField()
    {
        foreach (var tile in tiles)
        {
            if (tile == null) break;
            Destroy(tile.gameObject);
        }
        minesCount = 0;
    }

    private void CreateField()
    {
        tiles = new Tile[y, x];
        Array values = Enum.GetValues(typeof(NeighborType));

        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                GameObject t = Instantiate(tilePref, new Vector2(i * (scale / x) - 5f, j * (scale / y) - 5f), Quaternion.identity);
                tiles[i, j] = t.GetComponent<Tile>();
                tiles[i, j].gameObject.transform.localScale = new Vector3(scale / x, scale / y, 0);
                tiles[i, j].x = i;
                tiles[i, j].y = j;
                if (!lvlMake && UnityEngine.Random.Range(0, 100) < mineChance)
                {
                    tiles[i, j].type = Type.mine;
                    minesCount++;
                }

                if (sameGridType)
                    tiles[i, j].neighborType = type;
                else
                    tiles[i, j].neighborType = (NeighborType)values.GetValue(new System.Random().Next(values.Length));
            }
        }

        currentMinesCount = minesCount;
    }

    #endregion Create Field


    private void Update()
    {
        tmpMines.text = $"{currentMinesCount.ToString()} ({minesCount.ToString()})";

        Debug();
    }

    public void Creating(bool check)
        => lvlMake = check;
     

    // !!! WARNING: YOU ARE ENTERING THE ZONE OF GOVNO CODE !!!


    private void Debug()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (var tile in tiles)
                tile.Reveal();
        }
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            foreach (var tile in tiles)
                if (tile.type != Type.mine && tile.mineCount == 0) tile.Reveal();
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            foreach (var tile in tiles)
                if (tile.type != Type.mine && tile.mineCount == 1) tile.Reveal();
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            foreach (var tile in tiles)
                if (tile.type != Type.mine && tile.mineCount == 2) tile.Reveal();
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            foreach (var tile in tiles)
                if (tile.type != Type.mine && tile.mineCount == 3) tile.Reveal();
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            foreach (var tile in tiles)
                if (tile.type != Type.mine && tile.mineCount == 4) tile.Reveal();
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            foreach (var tile in tiles)
                if (tile.type != Type.mine && tile.mineCount == 5) tile.Reveal();
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            foreach (var tile in tiles)
                if (tile.type != Type.mine && tile.mineCount == 6) tile.Reveal();
        }
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            foreach (var tile in tiles)
                if (tile.type != Type.mine && tile.mineCount == 7) tile.Reveal();
        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            foreach (var tile in tiles)
                if (tile.type != Type.mine && tile.mineCount == 8) tile.Reveal();
        }
    }
}