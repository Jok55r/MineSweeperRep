using System;
using TMPro;
using UnityEngine;
using Unity;
using UnityEditor;
using Unity.VisualScripting;
using System.IO;
using UnityEngine.UIElements;
using System.Xml.Linq;

public class GM : MonoBehaviour
{
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
        if (LoadLevel())
            return;
        tiles = new Tile[x, y];
        NewLevel();
    }

    public void NewLevel()
    {
        DestroyField();
        CreateField();
        if (lvlMake) FieldMaker();
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
        {
            if (tile == null) break;
            Destroy(tile.gameObject);
        }
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
        GameObject t = Instantiate(tilePref, new Vector2(i * (scale / y) - 5f, j * (scale / x) - 5f), Quaternion.identity);
        tiles[i, j] = t.GetComponent<Tile>();
        tiles[i, j].gameObject.transform.localScale = new Vector3(scale / y, scale / x, 0);
        tiles[i, j].x = i;
        tiles[i, j].y = j;
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
        Debug.Log("saving to \"" + fullPath + "\"...");

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

    public bool LoadLevel()
    {
        string lvlName = PlayerPrefs.GetString("level_name", "random");
        Debug.Log("loaded " + lvlName);
        if (lvlName == "random")
            return false;

        using (StreamReader sr = new StreamReader(path + lvlName + ".txt"))
        {
            string[] size = sr.ReadLine().Split(';');
            y = Convert.ToInt32(size[0]);
            x = Convert.ToInt32(size[1]);

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
        return true;
    }

    #endregion LevelManagement

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
                tile.Reveal();
        }
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            foreach (var tile in tiles)
                if (tile.type != Type.mine && tile.mineCount == 0) tile.Reveal();
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            foreach (var tile in tiles)
                if (tile.type != Type.mine && tile.mineCount == 1) tile.Reveal();
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            foreach (var tile in tiles)
                if (tile.type != Type.mine && tile.mineCount == 2) tile.Reveal();
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            foreach (var tile in tiles)
                if (tile.type != Type.mine && tile.mineCount == 3) tile.Reveal();
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            foreach (var tile in tiles)
                if (tile.type != Type.mine && tile.mineCount == 4) tile.Reveal();
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            foreach (var tile in tiles)
                if (tile.type != Type.mine && tile.mineCount == 5) tile.Reveal();
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            foreach (var tile in tiles)
                if (tile.type != Type.mine && tile.mineCount == 6) tile.Reveal();
        }
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            foreach (var tile in tiles)
                if (tile.type != Type.mine && tile.mineCount == 7) tile.Reveal();
        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            foreach (var tile in tiles)
                if (tile.type != Type.mine && tile.mineCount == 8) tile.Reveal();
        }
    }
}