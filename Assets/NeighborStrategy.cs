using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighborStrategy : MonoBehaviour
{
    public static int CreateNeighbors(Tile tile)
    {
        return tile.neighborType switch
        {
            NeighborType.a8 => 8,
            NeighborType.a4 => 4,
            _ => 0
        };
    }
    public static void CountNeighbors(int i, int j)
    {
        int sum = 0;
        GM.tiles[i, j].neighbors = new Tile[CreateNeighbors(GM.tiles[i, j])];
        int k = 0;

        foreach (var pos in GetNeighbors(i, j, GM.tiles[i, j].neighborType))
        {
            sum += GM.tiles[pos.Item1, pos.Item2].type == Type.mine ? 1 : 0;
            GM.tiles[i, j].neighbors[k] = GM.tiles[pos.x, pos.y];
            k++;
        }
        GM.tiles[i, j].SetNeighbors(sum);
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

    public static (int, int)[] GetA8(int i, int j)
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

    public static (int, int)[] GetA4a(int i, int j)
    {
        (int, int)[] array = new (int, int)[4];

        if (j > 0) array[0] = (i, j - 1);
        if (i < GM.tiles.GetLength(0) - 1) array[1] = (i + 1, j);
        if (j < GM.tiles.GetLength(1) - 1) array[2] = (i, j + 1);
        if (i > 0) array[3] = (i - 1, j);

        return array;
    }

    public static (int, int)[] GetA4b(int i, int j)
    {
        (int, int)[] array = new (int, int)[4];

        if (j > 0 && i > 0) array[0] = (i - 1, j - 1);
        if (i < GM.tiles.GetLength(0) - 1 && j > 0) array[1] = (i + 1, j - 1);
        if (i < GM.tiles.GetLength(0) - 1 && j < GM.tiles.GetLength(1) - 1) array[2] = (i + 1, j + 1);
        if (i > 0 && j < GM.tiles.GetLength(1) - 1) array[3] = (i - 1, j + 1);

        return array;
    }
}