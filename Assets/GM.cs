using System;
using TMPro;
using UnityEngine;

public class GM : MonoBehaviour
{
    public TextMeshPro tmpMines;
    public static int allMines;
    public static int mines;

    public bool sameGridType;
    public Type type;
    public int mineChance;
    public int x;
    public int y;

    public GameObject tilePref;
    public float scale;

    public static bool creating = false;

    public static TileLogic[,] tiles;

    void Start()
    {
        tiles = new TileLogic[y, x];
    }

    public void NewLevel()
    {
        DestroyField();
        CreateField();
        if (creating) FieldMaker();
    }

    private void FieldMaker()
    {
        foreach (var tile in tiles) 
            tile.Reveal();
    }

    public void Creating()
        => creating = !creating;

    private void DestroyField()
    {
        foreach (var tile in tiles)
        {
            if (tile == null) break;
            Destroy(tile.gameObject);
        }
        allMines = 0;
    }

    private void CreateField()
    {
        Array values = Enum.GetValues(typeof(Type));

        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                GameObject t = Instantiate(tilePref, new Vector2(i * (scale / x) - 5f, j * (scale / y) - 5f), Quaternion.identity);
                tiles[i, j] = t.GetComponent<TileLogic>();
                tiles[i, j].gameObject.transform.localScale = new Vector3(scale / x, scale / y, 0);
                tiles[i, j].x = i;
                tiles[i, j].y = j;
                if (creating) tiles[i, j].CreateMine(mineChance);

                if (sameGridType)
                    tiles[i, j].type = type;
                else
                    tiles[i, j].type = (Type)values.GetValue(new System.Random().Next(values.Length));
            }
        }

        mines = allMines;
        PrepareEverything();
    }

    private static int CreateNeighbors(TileLogic tile)
    {
        return tile.type switch
        {
            Type.a8 => 8,
            Type.a4a => 4,
            Type.a4b => 4,
            _ => 0
        };
    }

    private void PrepareEverything()
    {
        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                CountNeighbors(i, j);

                if (creating)
                    tiles[i, j].Reveal();
            }
        }
    }

    public static void CountNeighbors(int i, int j)
    {
        int sum = 0;
        tiles[i, j].neighbors = new TileLogic[CreateNeighbors(tiles[i, j])];
        int k = 0;

        foreach (var pos in GetNeighbors(i, j, tiles[i, j].type))
        {
            sum += tiles[pos.Item1, pos.Item2].isMine ? 1 : 0;
            tiles[i, j].neighbors[k] = tiles[pos.x, pos.y];
            k++;
        }
        tiles[i, j].SetNeighbors(sum);
    }

    public static (int x, int y)[] GetNeighbors(int i, int j, Type type)
    {
        return type switch
        {
            Type.a8 => GetA8(i, j),
            Type.a4a => GetA4a(i, j),
            Type.a4b => GetA4b(i, j),
            _ => null
        };
    }

    private void Update()
    {
        tmpMines.text = $"{mines.ToString()} ({allMines.ToString()})";

        Debug();
    }
     

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
                if (!tile.isMine && tile.mines == 0) tile.Reveal();
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            foreach (var tile in tiles)
                if (!tile.isMine && tile.mines == 1) tile.Reveal();
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            foreach (var tile in tiles)
                if (!tile.isMine && tile.mines == 2) tile.Reveal();
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            foreach (var tile in tiles)
                if (!tile.isMine && tile.mines == 3) tile.Reveal();
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            foreach (var tile in tiles)
                if (!tile.isMine && tile.mines == 4) tile.Reveal();
        }
    }
}