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

    public TileLogic[] GetNeighbors(GameObject[,] tiles)
    {
        TileLogic[] array = new TileLogic[8];
        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                if (j > 0 && i > 0) array[0] = tiles[i - 1, j - 1].AddComponent<TileLogic>();
                if (j > 0) array[1] = tiles[i, j - 1].AddComponent<TileLogic>();
                if (i < tiles.GetLength(0) - 1 && j > 0) array[2] = tiles[i + 1, j - 1].AddComponent<TileLogic>();
                if (i < tiles.GetLength(0) - 1) array[3] = tiles[i + 1, j].AddComponent<TileLogic>();
                if (i < tiles.GetLength(0) - 1 && j < tiles.GetLength(1) - 1) array[4] = tiles[i + 1, j + 1].AddComponent<TileLogic>();
                if (j < tiles.GetLength(1) - 1) array[5] = tiles[i, j + 1].AddComponent<TileLogic>();
                if (i > 0 && j < tiles.GetLength(1) - 1) array[6] = tiles[i - 1, j + 1].AddComponent<TileLogic>();
                if (i > 0) array[7] = tiles[i - 1, j].AddComponent<TileLogic>();
            }
        } 
        /*if (j > 0 && i > 0) array[0] = tiles[i - 1, j - 1].AddComponent<TileLogic>();
        if (j > 0) array[1] = tiles[i, j - 1].AddComponent<TileLogic>();
        if (i < tiles.GetLength(0) - 1 && j > 0) array[2] = tiles[i + 1, j - 1].AddComponent<TileLogic>();
        if (i < tiles.GetLength(0) - 1) array[3] = tiles[i + 1, j].AddComponent<TileLogic>();
        if (i < tiles.GetLength(0) - 1 && j < tiles.GetLength(1) - 1) array[4] = tiles[i + 1, j + 1].AddComponent<TileLogic>();
        if (j < tiles.GetLength(1) - 1) array[5] = tiles[i, j + 1].AddComponent<TileLogic>();
        if (i > 0 && j < tiles.GetLength(1) - 1) array[6] = tiles[i - 1, j + 1].AddComponent<TileLogic>();
        if (i > 0) array[7] = tiles[i - 1, j].AddComponent<TileLogic>();*/
        return array;
    }

    public static void Open(int i, int j)
    {
        /*foreach (var item in GetNeighbors(i, j))
        {

        }*/

        if (i > 0) tiles[i - 1, j].GetComponent<TileLogic>().Reveal();
        if (j > 0 && i > 0) tiles[i - 1, j - 1].GetComponent<TileLogic>().Reveal();
        if (j > 0) tiles[i, j - 1].GetComponent<TileLogic>().Reveal();
        if (i < tiles.GetLength(0) - 1 && j > 0) tiles[i + 1, j - 1].GetComponent<TileLogic>().Reveal();
        if (i < tiles.GetLength(0) - 1) tiles[i + 1, j].GetComponent<TileLogic>().Reveal();
        if (i < tiles.GetLength(0) - 1 && j < tiles.GetLength(1) - 1) tiles[i + 1, j + 1].GetComponent<TileLogic>().Reveal();
        if (j < tiles.GetLength(1) - 1) tiles[i, j + 1].GetComponent<TileLogic>().Reveal();
        if (i > 0 && j < tiles.GetLength(1) - 1) tiles[i - 1, j + 1].GetComponent<TileLogic>().Reveal();
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

                /*foreach (var item in GetNeighbors(i, j))
                {
                    sum += item.mine ? 1 : 0;
                }*/

                if (j > 0 && i > 0 && IsMine(tiles[i - 1, j - 1])) sum++;
                if (j > 0 && IsMine(tiles[i, j - 1])) sum++;
                if (i < tiles.GetLength(0)-1 && j > 0 && IsMine(tiles[i + 1, j - 1])) sum++;
                if (i < tiles.GetLength(0)-1 && IsMine(tiles[i + 1, j])) sum++;
                if (i < tiles.GetLength(0)-1 && j < tiles.GetLength(1)-1 && IsMine(tiles[i + 1, j + 1])) sum++;
                if (j < tiles.GetLength(1)-1 && IsMine(tiles[i, j + 1])) sum++;
                if (i > 0 && j < tiles.GetLength(1)-1 && IsMine(tiles[i - 1, j + 1])) sum++;
                if (i > 0 && IsMine(tiles[i - 1, j])) sum++;
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