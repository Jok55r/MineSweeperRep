using System;
using TMPro;
using UnityEngine;
using System.IO;
using System.Diagnostics;
using JetBrains.Annotations;
using Assets;

public class GM : MonoBehaviour
{
    public static GameState gameState;

    public TextMeshProUGUI time;

    public GameObject losePanel;
    public TextMeshPro tmpMines;
    public static int minesCount;
    public static int currentMinesCount;
    public static bool lost = false;

    public GameObject tilePref;
    public NeighborType type;
    public bool sameGridType;
    public int mineChance;
    public int y;
    public int x;

    public static bool lvlMake;
    private float scale = 9;

    public static Tile[,] tiles;

    public static string path = Application.dataPath + @"/Levels/";

    void Awake()
    {
        CreateField();

        GamePreferences.x = x;
        GamePreferences.y = y;
        GamePreferences.mineChance = mineChance;
        GamePreferences.type = type;
        GamePreferences.sameGridType = sameGridType;

        gameState = GameState.preGame;
        LoadLevel();
    }

    public void NewLevel()
    {
        foreach (var tile in tiles)
            tile.Reset();
        if (lvlMake) FieldMaker();
    }

    #region Create Field

    private void FieldMaker()
    {
        foreach (var tile in tiles) 
            tile.ReCount();
    }

    private void CreateField()
    {
        tiles = new Tile[x, y];
        Array values = Enum.GetValues(typeof(NeighborType));

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                InstantiateTile(i, j);
                CreateMines(i, j);

                if (sameGridType)
                    tiles[i, j].neighborType = type;
                else
                    tiles[i, j].neighborType = (NeighborType)values.GetValue(new System.Random().Next(values.Length));
            }
        }

        currentMinesCount = minesCount;
    }

    private void InstantiateTile(int i, int j)
    {
        float scaleX = scale / x;
        float scaleY = scale / y;

        GameObject t = Instantiate(tilePref, new Vector2(j * scaleY - 4, -i * scaleX + 4), Quaternion.identity);
        t.gameObject.transform.localScale = new Vector3(scaleY, scaleX, 0);
        tiles[i, j] = t.GetComponent<Tile>();
        tiles[i, j].pos = new Position(i, j);
    }

    private void CreateMines(int i, int j)
    {
        if (!lvlMake && UnityEngine.Random.Range(0, 100) < mineChance)
        {
            tiles[i, j].type = Type.mine;
            minesCount++;
        }
    }

    #endregion Create Field

    #region LevelManagement

    public void SaveNewLevel(TextMeshProUGUI name) 
    {
        string fullPath = path + name.text + ".txt";
        UnityEngine.Debug.Log("saving to \"" + fullPath + "\"...");

        StreamWriter sw = new StreamWriter(fullPath);

        sw.WriteLine(y + ";" + x);
        for (int i = y-1; i >= 0; i--)
        {
            for (int j = 0; j < x; j++)
            {
                sw.Write(tiles[j, i].type == Type.mine ? 1 : 0);
            }
            sw.Write('\n');
        }

        sw.Close();
    }

    public void LoadLevel()
    {
        string lvlName = PlayerPrefs.GetString("level_name", "random");
        UnityEngine.Debug.Log("loaded " + lvlName);
        tiles = new Tile[x, y];

        if (lvlName == "random")
        {
            NewLevel();
            return;
        }

        using (StreamReader sr = new StreamReader(path + lvlName + ".txt"))
        {
            string[] size = sr.ReadLine().Split(';');
            x = Convert.ToInt32(size[0]);
            y = Convert.ToInt32(size[1]);

            tiles = new Tile[x, y];

            for (int i = 0; i < y; i++)
            {
                string line = sr.ReadLine();
                for (int j = 0; j < x; j++)
                {
                    InstantiateTile(i, j);
                    tiles[i, j].type = line[j] == '1' ? Type.mine : Type.normal;
                }
            }
        }
    }

    #endregion LevelManagement

    public void SmilieFunc()
    {
        foreach (Tile tile in tiles)
        {
            if (tile.mineCount == 0)
            {
                tile.Reveal(false);
                break;
            }
        }
    }

    private void Update()
    {
        tmpMines.text = $"{currentMinesCount} ({minesCount})";

        if (lost == true)
        {
            SetPanel(true);
            lost = false;
        }

        DebugMethod();
    }

    public void Creating()
    {
        lvlMake = !lvlMake;
        if (lvlMake)
        {
            foreach (Tile tile in tiles)
            {
                tile.state = State.revealed;
                tile.visual.Render(tile);
            }
        }

    }

    public void SetPanel(bool boolean)
        => losePanel.SetActive(boolean);
     

    // !!! WARNING: YOU ARE ENTERING THE ZONE OF GOVNO CODE !!!


    private void DebugMethod()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (var tile in tiles)
                tile.Reveal(false);
        }
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            foreach (var tile in tiles)
                if (tile.type != Type.mine && tile.mineCount == 0) tile.Reveal(false);
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            foreach (var tile in tiles)
                if (tile.type != Type.mine && tile.mineCount == 1) tile.Reveal(false);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            foreach (var tile in tiles)
                if (tile.type != Type.mine && tile.mineCount == 2) tile.Reveal(false    );
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            foreach (var tile in tiles)
                if (tile.type != Type.mine && tile.mineCount == 3) tile.Reveal(false);
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            foreach (var tile in tiles)
                if (tile.type != Type.mine && tile.mineCount == 4) tile.Reveal(false);
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            foreach (var tile in tiles)
                if (tile.type != Type.mine && tile.mineCount == 5) tile.Reveal(false);
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            foreach (var tile in tiles)
                if (tile.type != Type.mine && tile.mineCount == 6) tile.Reveal(false);
        }
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            foreach (var tile in tiles)
                if (tile.type != Type.mine && tile.mineCount == 7) tile.Reveal(false);
        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            foreach (var tile in tiles)
                if (tile.type != Type.mine && tile.mineCount == 8) tile.Reveal(false);
        }
    }
}

public enum GameState
{
    preGame,
    inGame,
    endGame,
    creator
}