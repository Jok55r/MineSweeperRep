using System.Collections.Generic;
using UnityEngine;

public class NeighborStrategy : MonoBehaviour
{
    public static void CountNeighbors(Tile tile)
    {
        int sum = 0;
        tile.neighbors = new List<Tile>();
        int k = 0;

        foreach (var pos in GetNeighbors(tile))
        {
            sum += GM.tiles[pos.Item1, pos.Item2].type == Type.mine ? 1 : 0;
            tile.neighbors.Add(GM.tiles[pos.Item1, pos.Item2]);
            k++;
        }
        tile.SetNeighbors(sum);
    }

    public static List<(int, int)> GetNeighbors(Tile tile)
    {
        return tile.neighborType switch
        {
            NeighborType.a8 => GetA8(tile),
            NeighborType.a4 => GetA4a(tile),
            _ => null
        };
    }

    public static List<(int, int)> GetA8(Tile tile)
    {
        List<(int, int)> array = new List<(int, int)>();

        int k = 0;
        var a4a = GetA4a(tile);
        var a4ab = GetA4b(tile);

        foreach (var item in a4a)
        {
            array.Add(item);
            k++;
        }
        foreach (var item in a4ab)
        {
            array.Add(item);
            k++;
        }

        return array;
    }

    public static List<(int, int)> GetA4a(Tile tile)
    {
        List<(int, int)> array = new List<(int, int)>();

        if (tile.y > 0) array.Add((tile.x, tile.y - 1));
        if (tile.x < GM.tiles.GetLength(0) - 1) array.Add((tile.x + 1, tile.y));
        if (tile.y < GM.tiles.GetLength(1) - 1) array.Add((tile.x, tile.y + 1));
        if (tile.x > 0) array.Add((tile.x - 1, tile.y));

        return array;
    }

    public static List<(int, int)> GetA4b(Tile tile)
    {
        List<(int, int)> array = new List<(int, int)>();

        if (tile.y > 0 && tile.x > 0) array.Add((tile.x - 1, tile.y - 1));
        if (tile.x < GM.tiles.GetLength(0) - 1 && tile.y > 0) array.Add((tile.x + 1, tile.y - 1));
        if (tile.x < GM.tiles.GetLength(0) - 1 && tile.y < GM.tiles.GetLength(1) - 1) array.Add((tile.x + 1, tile.y + 1));
        if (tile.x > 0 && tile.y < GM.tiles.GetLength(1) - 1) array.Add((tile.x - 1, tile.y + 1));

        return array;
    }
}