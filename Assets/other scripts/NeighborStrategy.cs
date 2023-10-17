using System.Collections.Generic;
using UnityEngine;

public class NeighborStrategy : MonoBehaviour
{
    private static int x = GM.tiles.GetLength(0) - 1;
    private static int y = GM.tiles.GetLength(0) - 1;

    public static void CountNeighbors(Tile tile)
    {
        x = GM.tiles.GetLength(0) - 1;
        y = GM.tiles.GetLength(1) - 1;

        int sum = 0;
        tile.neighbors = new List<Tile>();

        foreach (var pos in GetNeighbors(tile))
        {
            sum += GM.tiles[pos.x, pos.y].type == Type.mine ? 1 : 0;
            tile.neighbors.Add(GM.tiles[pos.x, pos.y]);
        }
        tile.mineCount = sum;
    }

    public static List<Position> GetNeighbors(Tile tile) => 
        tile.neighborType switch
        {
            NeighborType.a8 => GetA8(tile.pos),
            NeighborType.a4 => GetA4a(tile.pos),
            _ => null
        };

    public static List<Position> GetA8(Position pos)
    {
        List<Position> array = new List<Position>();

        if (pos.y > 0) array.Add(new Position(pos.x, pos.y - 1));
        if (pos.x < x) array.Add(new Position(pos.x + 1, pos.y));
        if (pos.y < y) array.Add(new Position(pos.x, pos.y + 1));
        if (pos.x > 0) array.Add(new Position(pos.x - 1, pos.y));

        if (pos.x > 0 && pos.y > 0) array.Add(new Position(pos.x - 1, pos.y - 1));
        if (pos.x < x && pos.y > 0) array.Add(new Position(pos.x + 1, pos.y - 1));
        if (pos.x < x && pos.y < y) array.Add(new Position(pos.x + 1, pos.y + 1));
        if (pos.x > 0 && pos.y < y) array.Add(new Position(pos.x - 1, pos.y + 1));
        return array;
    }

    public static List<Position> GetA4a(Position pos)
    {
        List<Position> array = new List<Position>();

        if (pos.y > 0) array.Add(new Position(pos.x, pos.y - 1));
        if (pos.x < GM.tiles.GetLength(0) - 1) array.Add(new Position  (pos.x + 1, pos.y));
        if (pos.y < GM.tiles.GetLength(1) - 1) array.Add(new Position(pos.x, pos.y + 1));
        if (pos.x > 0) array.Add(new Position(pos.x - 1, pos.y));

        return array;
    }

    /*public static List<(int, int)> GetA4b(Tile tile)
    {
        List<(int, int)> array = new List<(int, int)>();

        if (tile.y > 0 && tile.x > 0) array.Add((tile.x - 1, tile.y - 1));
        if (tile.x < GM.tiles.GetLength(0) - 1 && tile.y > 0) array.Add((tile.x + 1, tile.y - 1));
        if (tile.x < GM.tiles.GetLength(0) - 1 && tile.y < GM.tiles.GetLength(1) - 1) array.Add((tile.x + 1, tile.y + 1));
        if (tile.x > 0 && tile.y < GM.tiles.GetLength(1) - 1) array.Add((tile.x - 1, tile.y + 1));

        return array;
    }*/
}