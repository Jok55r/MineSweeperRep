using System;
using TMPro;
using UnityEngine;
using System.IO;
using System.Diagnostics;

public class GM : MonoBehaviour
{
    public TextMeshProUGUI time;
    public TMP_InputField mineText;
    public TMP_InputField questionText;
    public TMP_InputField exclamationText;
    public TMP_InputField morelessText;
    public TMP_InputField xText;
    public TMP_InputField yText;

    public GameObject losePanel;
    public GameObject winPanel;
    public TextMeshProUGUI tmpMines;
    public static int minesCount;
    public static int revealedCount;
    public static int currentMinesCount;
    public static bool lost = false;

    public GameObject tilePref;
    public NeighborType type;
    public bool sameGridType;
    public int mineChance;
    public int y;
    public int x;
    public int questionChance;
    public int exclamationChance;
    public int morelessChance;

    public static bool creatorMode;
    private float scale = 9;
    private bool wonGame = false;

    public static Tile[,] tiles;

    public static string path = System.IO.Directory.GetCurrentDirectory() + @"/";

    void Awake()
    {
        LoadLevel();
    }

    void Start()
    {
        mineText.text = mineChance.ToString();
        questionText.text = questionChance.ToString();
        exclamationText.text = exclamationChance.ToString();
        morelessText.text = morelessChance.ToString();
        xText.text = x.ToString();
        yText.text = y.ToString();
    }

    public void NewLevel()
    {
        revealedCount = 0;
        Stopwatch sw1 = Stopwatch.StartNew();
        sw1 = Stopwatch.StartNew();

        if (tiles[0, 0] != null) DestroyField();

        sw1.Stop();
        Stopwatch sw2 = Stopwatch.StartNew();
        sw2 = Stopwatch.StartNew();

        CreateField();
        if (creatorMode) FieldMaker();

        sw2.Stop();
        TimeSpan ts1 = sw1.Elapsed;
        TimeSpan ts2 = sw2.Elapsed;
        time.text = "Destruction time: " + (ts1*4).ToString("ss\\.fff") + "\n"
                    + "Creation time: " + (ts2*4).ToString("ss\\.fff") + "\n";
    }

    #region Create Field

    private void FieldMaker()
    {
        foreach (var tile in tiles) 
            tile.ReCount();
    }

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
        wonGame = false;

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
            if (tile.mineCount == 0 && tile.type != Type.mine)
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
            SetLosePanel(true);
            lost = false;
        }
        if (!creatorMode && !wonGame && revealedCount >= x * y - minesCount)
        {
            SetWinPanel(true);
            wonGame = true;
        }

        //DebugMethod();
    }

    public void Creating()
    {
        creatorMode = !creatorMode;
        if (creatorMode)
        {
            foreach (Tile tile in tiles)
            {
                tile.state = State.revealed;
                tile.visual.Render(tile);
            }
        }

    }

    public void SetLosePanel(bool boolean)
        => losePanel.SetActive(boolean);
    public void SetWinPanel(bool boolean)
        => winPanel.SetActive(boolean);

    #region UI

    public void ChangeMineChance(TMP_InputField tmp)
        => mineChance = Convert.ToInt32(tmp.text);
    public void ChangeQuestionChance(TMP_InputField tmp)
        => questionChance = Convert.ToInt32(tmp.text);
    public void ChangeExclamationChance(TMP_InputField tmp)
        => exclamationChance = Convert.ToInt32(tmp.text);
    public void ChangeMoreLessChance(TMP_InputField tmp)
        => morelessChance = Convert.ToInt32(tmp.text);
    public void ChangeX(TMP_InputField tmp)
        => x = Convert.ToInt32(tmp.text);
    public void ChangeY(TMP_InputField tmp)
        => y = Convert.ToInt32(tmp.text);

    #endregion UI

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