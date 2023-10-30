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
            NeighborType.a8 => GetSquare(tile.pos, 3),
            NeighborType.a4 => GetA4a(tile.pos),
            _ => null
        };

    public static List<Position> GetSquare(Position pos, int size)
    {
        List<Position> array = new List<Position>();

        for (int i = -(size-1)/2; i <= (size-1)/2; i++)
        {
            for (int j = -(size-1)/2; j <= (size-1)/2; j++)
            {
                int newX = pos.x + i;
                int newY = pos.y + j;
                if (newY > 0 && newX > 0 && newY < GM.tiles.GetLength(1) && newX < GM.tiles.GetLength(0))
                    array.Add(new Position(newX, newY));
            }
        }

        return array;
    }

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
}