using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour
{
    public GameObject tilePref;
    public float scale;

    public GameObject[,] tiles;

    public int x;
    public int y;

    void Start()
    {
        tiles = new GameObject[y, x];
        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                tiles[i, j] = Instantiate(tilePref, new Vector2(i * (scale/x) - 4f , j * (scale/y) - 4f), Quaternion.identity);
                tiles[i, j].transform.localScale = new Vector3(scale / x, scale / y, 0);
                tiles[i, j].GetComponent<TileLogic>().CreateMine(2);
                //tiles[i, j].GetComponent<TileLogic>().x = i;
                //tiles[i, j].GetComponent<TileLogic>().y = j;
            }
        }

        Calculate();
    }

    /*public static void Open()
    {
        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                int sum = 0;
                //var tile = tiles[i, j].GetComponent<TileLogic>();
                if (i > 0 && j > 0 && IsMine(tiles[i - 1, j - 1])) sum++;
                if (j > 0 && IsMine(tiles[i, j - 1])) sum++;
                if (i < tiles.GetLength(0) - 1 && j > 0 && IsMine(tiles[i + 1, j - 1])) sum++;
                if (i < tiles.GetLength(0) - 1 && IsMine(tiles[i + 1, j])) sum++;
                if (i < tiles.GetLength(0) - 1 && j < tiles.GetLength(1) - 1 && IsMine(tiles[i + 1, j + 1])) sum++;
                if (j < tiles.GetLength(1) - 1 && IsMine(tiles[i, j + 1])) sum++;
                if (i > 0 && j < tiles.GetLength(1) - 1 && IsMine(tiles[i - 1, j + 1])) sum++;
                if (i > 0 && IsMine(tiles[i - 1, j])) sum++;
                tiles[i, j].GetComponent<TileLogic>().SetNeighbors(sum);
            }
        }
    }*/

    /*private static void ZeroNeighboursCheck(int i, int j)
    {
        if (i < 0 || j < 0 || i > tiles.GetLength(0) || j > tiles.GetLength(1)) return;
    }*/

    private void Calculate()
    {
        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                int sum = 0;

                /*if (IsMine(i-1, j)) sum++;
                if (IsMine(i-1, j-1)) sum++;
                if (IsMine(i-1, j-1)) sum++;
                if (IsMine(i, j-1)) sum++;
                if (IsMine(i+1, j)) sum++;
                if (IsMine(i+1, j+1)) sum++;
                if (IsMine(i+1, j+1)) sum++;
                if (IsMine(i, j+1)) sum++;*/

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

    private bool IsMine(GameObject tile)
        => tile.GetComponent<TileLogic>().mine;

    private bool IsMine(int i, int j)
    {
        if (i < 0 || j < 0 || i > tiles.GetLength(0) || j > tiles.GetLength(1)) return false;
        return tiles[i,j].GetComponent<TileLogic>().mine;
    }
}