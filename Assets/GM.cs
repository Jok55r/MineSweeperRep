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


    #region Create Field

    void Start()
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
                    GM.minesCount++;
                }

                if (sameGridType)
                    tiles[i, j].neighborType = type;
                else
                    tiles[i, j].neighborType = (NeighborType)values.GetValue(new System.Random().Next(values.Length));
            }
        }

        currentMinesCount = minesCount;
        PrepareEverything();
    }

    #endregion Create Field

    #region Functions

    private void PrepareEverything()
    {
        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                CountNeighbors(i, j);

                if (lvlMake)
                    tiles[i, j].Reveal();
            }
        }
    }

    public static void CountNeighbors(int i, int j)
    {
        int sum = 0;
        tiles[i, j].neighbors = new Tile[CreateNeighbors(tiles[i, j])];
        int k = 0;

        foreach (var pos in GetNeighbors(i, j, tiles[i, j].neighborType))
        {
            sum += tiles[pos.Item1, pos.Item2].type == Type.mine ? 1 : 0;
            tiles[i, j].neighbors[k] = tiles[pos.x, pos.y];
            k++;
        }
        tiles[i, j].SetNeighbors(sum);
    }

    private static int CreateNeighbors(Tile tile)
    {
        return tile.neighborType switch
        {
            NeighborType.a8 => 8,
            NeighborType.a4 => 4,
            _ => 0
        };
    }

    public static (int x, int y)[] GetNeighbors(int i, int j, NeighborType type)
    {
        return type switch
        {
            NeighborType.a8 => GetA8(i, j),
            NeighborType.a4 => GetA4a(i, j),
            _ => null
        };
    }

    #endregion Functions

    private void Update()
    {
        tmpMines.text = $"{currentMinesCount.ToString()} ({minesCount.ToString()})";

        Debug();
    }

    public void Creating(bool check)
        => lvlMake = check;
     

    // !!! WARNING: YOU ARE ENTERING THE ZONE OF GOVNO CODE !!!


    private static (int, int)[] GetA8(int i, int j)
    {
        (int, int)[] array = new (int, int)[8];

        int k = 0;
        var a4a = GetA4a(i, j);
        var a4ab = GetA4b(i, j);

        foreach (var item in a4a)
        {
            array[k] = item;
            k++;
        }
        foreach (var item in a4ab)
        {
            array[k] = item;
            k++;
        }

        return array;
    }

    private static (int, int)[] GetA4a(int i, int j)
    {
        (int, int)[] array = new (int, int)[4];

        if (j > 0) array[0] = (i, j - 1);
        if (i < tiles.GetLength(0) - 1) array[1] = (i + 1, j);
        if (j < tiles.GetLength(1) - 1) array[2] = (i, j + 1);
        if (i > 0) array[3] = (i - 1, j);

        return array;
    }

    private static (int, int)[] GetA4b(int i, int j)
    {
        (int, int)[] array = new (int, int)[4];

        if (j > 0 && i > 0) array[0] = (i - 1, j - 1);
        if (i < tiles.GetLength(0) - 1 && j > 0) array[1] = (i + 1, j - 1);
        if (i < tiles.GetLength(0) - 1 && j < tiles.GetLength(1) - 1) array[2] = (i + 1, j + 1);
        if (i > 0 && j < tiles.GetLength(1) - 1) array[3] = (i - 1, j + 1);

        return array;
    }

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