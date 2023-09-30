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

    public static GameObject[,] tiles;

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

                if (sameGridType)
                    tiles[i, j].GetComponent<TileLogic>().type = type;
                else
                    ;//tiles[i, j].GetComponent<TileLogic>().type = Random.d;
            }
        }

        mines = allMines;
        Calculate();
    }

    public static void Open(int i, int j)
    {
        foreach (var pos in GetNeighbors(i, j, tiles[i, j].GetComponent<TileLogic>().type))
        {
            tiles[pos.Item1, pos.Item2].GetComponent<TileLogic>().Reveal();
        }
    }

    private void Calculate()
    {
        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                int sum = 0;

                foreach (var pos in GetNeighbors(i, j, tiles[i, j].GetComponent<TileLogic>().type))
                {
                    sum += tiles[pos.Item1, pos.Item2].GetComponent<TileLogic>().mine ? 1 : 0;
                }
                tiles[i,j].GetComponent<TileLogic>().SetNeighbors(sum);
            }
        }
    }

    public static (int, int)[] GetNeighbors(int i, int j, Type type)
    {
        if (type == Type.a8) return GetA8(i, j);
        if (type == Type.a4) return GetA4(i, j);

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
}