using System;
using System.Net.Http.Headers;
using TMPro;
using Unity.Mathematics;
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

    public static TileLogic[,] tiles;

    void Start()
    {
        Array values = Enum.GetValues(typeof(Type));

        tiles = new TileLogic[y, x];
        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                GameObject t = Instantiate(tilePref, new Vector2(i * (scale/x) - 5f, j * (scale/y) - 5f), Quaternion.identity);
                tiles[i, j] = t.GetComponent<TileLogic>();
                tiles[i, j].gameObject.transform.localScale = new Vector3(scale / x, scale / y, 0);
                tiles[i, j].CreateMine(mineChance);
                tiles[i, j].x = i;
                tiles[i, j].y = j;

                if (sameGridType)
                    tiles[i, j].type = type;
                else
                    tiles[i, j].type = (Type)values.GetValue(new System.Random().Next(values.Length));
            }
        }

        mines = allMines;
        PrepareEverything();
    }

    private int CreateNeighbors(TileLogic tile)
    {
        return tile.type switch
        {
            Type.a8 => 8,
            Type.a4a => 4,
            Type.a4b => 4,
        };
    }

    public static void Open(int i, int j)
    {
        foreach (var pos in GetNeighbors(i, j, tiles[i, j].type))
        {
            tiles[pos.Item1, pos.Item2].Reveal();
        }
    }

    private void PrepareEverything()
    {
        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                int sum = 0;
                tiles[i, j].neighbors = new TileLogic[CreateNeighbors(tiles[i, j])];
                int k = 0;

                foreach (var pos in GetNeighbors(i, j, tiles[i, j].type))
                {
                    sum += tiles[pos.Item1, pos.Item2].mine ? 1 : 0;
                    tiles[i, j].neighbors[k] = tiles[pos.x, pos.y];
                    k++;
                }
                tiles[i,j].SetNeighbors(sum);
            }
        }
    }

    public static (int x, int y)[] GetNeighbors(int i, int j, Type type)
    {
        if (type == Type.a8) return GetA8(i, j);
        if (type == Type.a4a) return GetA4(i, j);

        return new (int, int)[0];
    }

    private static (int, int)[] GetA8(int i, int j)
    {
        (int, int)[] array = new (int, int)[8];

        if (j > 0 && i > 0) array[0] = (i - 1, j - 1);
        if (j > 0) array[1] = (i, j - 1);
        if (i < tiles.GetLength(0) - 1 && j > 0) array[2] = (i + 1, j - 1);
        if (i < tiles.GetLength(0) - 1) array[3] = (i + 1, j);
        if (i < tiles.GetLength(0) - 1 && j < tiles.GetLength(1) - 1) array[4] = (i + 1, j + 1);
        if (j < tiles.GetLength(1) - 1) array[5] = (i, j + 1);
        if (i > 0 && j < tiles.GetLength(1) - 1) array[6] = (i - 1, j + 1);
        if (i > 0) array[7] = (i - 1, j);

        return array;
    }

    private static (int, int)[] GetA4(int i, int j)
    {
        (int, int)[] array = new (int, int)[4];

        if (j > 0) array[0] = (i, j - 1);
        if (i < tiles.GetLength(0) - 1) array[1] = (i + 1, j);
        if (j < tiles.GetLength(1) - 1) array[2] = (i, j + 1);
        if (i > 0) array[3] = (i - 1, j);

        return array;
    }

    private void Update()
    {
        tmpMines.text = $"{mines.ToString()} ({allMines.ToString()})";

        Debug();
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
                if (!tile.mine && tile.neighbours == 0) tile.Reveal();
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            foreach (var tile in tiles)
                if (!tile.mine && tile.neighbours == 1) tile.Reveal();
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            foreach (var tile in tiles)
                if (!tile.mine && tile.neighbours == 2) tile.Reveal();
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            foreach (var tile in tiles)
                if (!tile.mine && tile.neighbours == 3) tile.Reveal();
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            foreach (var tile in tiles)
                if (!tile.mine && tile.neighbours == 4) tile.Reveal();
        }
    }
}