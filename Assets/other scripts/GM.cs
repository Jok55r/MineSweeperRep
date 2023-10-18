using System;
using TMPro;
using UnityEngine;
using System.IO;
using System.Diagnostics;

public class GM : MonoBehaviour
{
    public UIManager UImanager;
    public TextMeshProUGUI time;

    public static int minesCount;
    public static int revealedCount;
    public static int currentMinesCount;

    public static bool lost = false;
    public static bool won = false;

    public GameObject tilePref;
    public NeighborType type;
    public bool sameGridType;
    public static int mineChance = 10;
    public static int y = 10;
    public static int x = 10;
    public static int questionChance = 0;
    public static int exclamationChance = 0;
    public static int morelessChance = 0;

    public int revealedTiles;

    public static bool creatorMode;
    private float scale = 9;

    public static Tile[,] tiles;

    public static string path = System.IO.Directory.GetCurrentDirectory() + @"/";

    void Awake()
    {
        LoadLevel();
    }


    public void Preparations()
    {
        currentMinesCount = minesCount;

        if (creatorMode) 
            FieldMaker();
    }

    public void NewLevel()
    {
        UImanager.NewLevel();
        lost = false;
        won = false;

        revealedCount = 0;
        Stopwatch sw1 = Stopwatch.StartNew();
        sw1 = Stopwatch.StartNew();

        if (tiles[0, 0] != null) DestroyField();

        sw1.Stop();
        Stopwatch sw2 = Stopwatch.StartNew();
        sw2 = Stopwatch.StartNew();

        CreateField();

        sw2.Stop();
        TimeSpan ts1 = sw1.Elapsed;
        TimeSpan ts2 = sw2.Elapsed;
        time.text = "Destruction time: " + (ts1*4).ToString("ss\\.fff") + "\n"
                    + "Creation time: " + (ts2*4).ToString("ss\\.fff") + "\n";
    }

    private void FieldMaker()
    {
        foreach (var tile in tiles) 
            tile.ReCount();
    }


    #region Create Field

    private void DestroyField()
    {
        foreach (var tile in tiles)
            Destroy(tile.gameObject);
        minesCount = 0;
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
                AdjustTile(i, j);

                if (sameGridType)
                    tiles[i, j].neighborType = type;
                else
                    tiles[i, j].neighborType = (NeighborType)values.GetValue(new System.Random().Next(values.Length));
            }
        }

        Preparations();
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

    private void AdjustTile(int i, int j)
    {
        if (!creatorMode && UnityEngine.Random.Range(0, 100) < mineChance)
        {
            tiles[i, j].type = Type.mine;
            minesCount++;
        }
        else if (UnityEngine.Random.Range(0, 100) < questionChance)
        {
            tiles[i, j].addon = Addon.question;
        }
        else if (UnityEngine.Random.Range(0, 100) < exclamationChance)
        {
            tiles[i, j].addon = Addon.exclamation;
        }
        else if (UnityEngine.Random.Range(0, 100) < morelessChance)
        {
            tiles[i, j].addon = Addon.moreless;
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
        minesCount = 0;
        UImanager.ChangeBackgroundCol(UImanager.normalCol);

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

                    if (line[j] == '1')
                        minesCount++;
                }
            }
        }
        Preparations();
    }

    #endregion LevelManagement

    public void SmilieFunc()
    {
        foreach (Tile tile in tiles)
        {
            if (tile.mineCount == 0 && tile.type != Type.mine)
            {
                tile.Reveal(false);
                break;
            }
        }
    }

    private void Update()
    {
        if (lost == true)
        {
            UImanager.SetLosePanel(true);
            lost = false;
        }
        if (!creatorMode && revealedCount + minesCount - currentMinesCount == x * y)
        {
            won = true;
        }
        if (creatorMode && !won)
        {
            Reveal0();
        }

        revealedTiles = revealedCount;
        //DebugMethod();
    }

    public void Creating()
    {
        creatorMode = !creatorMode;
        if (creatorMode)
        {
            foreach (Tile tile in tiles)
            {
                if (tile.type == Type.mine)
                    tile.state = State.marked;
                else
                    tile.state = State.revealed;
                tile.visual.Render(tile);
            }
        }

    }


    // !!! WARNING: YOU ARE ENTERING THE ZONE OF GOVNO CODE / DEBUG !!!


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

    private void Reveal0()
    {
        foreach (var tile in tiles)
            if (tile.type != Type.mine && tile.mineCount == 0) tile.Reveal(false);
        UnityEngine.Debug.Log("Opened zeros");
    }
}