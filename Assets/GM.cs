using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GM : MonoBehaviour
{
    public TextMeshPro tmpMines;
    public static int allMines;
    public static int mines;

    public int mineChance;

    public GameObject tilePref;
    public float scale;

    public static GameObject[,] tiles;

    public int x;
    public int y;

    void Start()
    {
        tiles = new GameObject[y, x];
        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                tiles[i, j] = Instantiate(tilePref, new Vector2(i * (scale/x) - 5f , j * (scale/y) - 5f), Quaternion.identity);
                tiles[i, j].transform.localScale = new Vector3(scale / x, scale / y, 0);
                tiles[i, j].GetComponent<TileLogic>().CreateMine(mineChance);
                tiles[i, j].GetComponent<TileLogic>().x = i;
                tiles[i, j].GetComponent<TileLogic>().y = j;
            }
        }
        mines = allMines;
        Calculate();
    }

    public static (int, int)[] GetNeighbors(int i, int j)
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

    public static void Open(int i, int j)
    {
        foreach (var pos in GetNeighbors(i, j))
        {
            tiles[pos.Item1, pos.Item2].GetComponent<TileLogic>().Reveal();
        }
    }

    private static void ZeroNeighboursCheck(int i, int j)
    {
        if (i < 0 || j < 0 || i > tiles.GetLength(0) || j > tiles.GetLength(1)) return;
    }

    private void Calculate()
    {
        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                int sum = 0;

                foreach (var pos in GetNeighbors(i, j))
                {
                    sum += tiles[pos.Item1, pos.Item2].GetComponent<TileLogic>().mine ? 1 : 0;
                }
                tiles[i,j].GetComponent<TileLogic>().SetNeighbors(sum);
            }
        }
    }

    private void Update()
    {
        tmpMines.text = $"{mines.ToString()} ({allMines.ToString()})";
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (var tile in tiles)
                tile.GetComponent<TileLogic>().Reveal();
        }
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            foreach (var tile in tiles)
                if (!tile.GetComponent<TileLogic>().mine && tile.GetComponent<TileLogic>().neighbours == 0) tile.GetComponent<TileLogic>().Reveal();
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            foreach (var tile in tiles)
                if (!tile.GetComponent<TileLogic>().mine && tile.GetComponent<TileLogic>().neighbours == 1) tile.GetComponent<TileLogic>().Reveal();
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            foreach (var tile in tiles)
                if (!tile.GetComponent<TileLogic>().mine && tile.GetComponent<TileLogic>().neighbours == 2) tile.GetComponent<TileLogic>().Reveal();
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            foreach (var tile in tiles)
                if (!tile.GetComponent<TileLogic>().mine && tile.GetComponent<TileLogic>().neighbours == 3) tile.GetComponent<TileLogic>().Reveal();
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            foreach (var tile in tiles)
                if (!tile.GetComponent<TileLogic>().mine && tile.GetComponent<TileLogic>().neighbours == 4) tile.GetComponent<TileLogic>().Reveal();
        }
    }

    private bool IsMine(GameObject tile)
        => tile.GetComponent<TileLogic>().mine;

    private bool IsMine(int i, int j)
    {
        if (i < 0 || j < 0 || i > tiles.GetLength(0) || j > tiles.GetLength(1)) return false;
        return tiles[i,j].GetComponent<TileLogic>().mine;
    }
}