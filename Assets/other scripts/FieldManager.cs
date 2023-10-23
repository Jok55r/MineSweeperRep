using System;
using System.IO;
using TMPro;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    public Global global;
    public static Tile[,] tiles;
    private const float scale = 9;
    public GameObject tilePref;


    public void NewLevel()
    {
        GameFlow.gameState = GameState.preGame;

        Global.revealedCount = 0;

        if (tiles[0, 0] != null) DestroyField();

        CreateField();
    }

    public void DestroyField()
    {
        foreach (var tile in tiles)
            Destroy(tile.gameObject);
        Global.minesCount = 0;
    }

    public void CreateField()
    {
        tiles = new Tile[Global.x, Global.y];
        Array values = Enum.GetValues(typeof(NeighborType));

        for (int i = 0; i < Global.x; i++)
        {
            for (int j = 0; j < Global.y; j++)
            {
                InstantiateTile(i, j);

                if (global.sameGridType)
                    tiles[i, j].neighborType = global.type;
                else
                    tiles[i, j].neighborType = (NeighborType)values.GetValue(new System.Random().Next(values.Length));
            }
        }

        Preparations();
    }

    public void InstantiateTile(int i, int j)
    {
        float scalex = scale / Global.x;
        float scaley = scale / Global.y;

        GameObject t = Instantiate(tilePref, new Vector2(j * scaley - 4, -i * scalex + 4), Quaternion.identity);
        t.gameObject.transform.localScale = new Vector3(scaley, scalex, 0);
        tiles[i, j] = t.GetComponent<Tile>();
        tiles[i, j].pos = new Position(i, j);
    }

    public static void AdjustTile(Position firstTile) //clear this method
    {
        for (int i = 0; i < Global.x; i++)
        {
            for (int j = 0; j < Global.y; j++)
            {
                if (tiles[i, j].pos == firstTile)
                {
                    tiles[i, j].type = Type.normal;
                    continue;
                }
                else
                {
                    if (GameFlow.gameState != GameState.creator && UnityEngine.Random.Range(0, 100) < Global.mineChance)
                    {
                        tiles[i, j].type = Type.mine;
                        Global.minesCount++;
                    }

                    else if (UnityEngine.Random.Range(0, 100) < Global.questionChance)
                        tiles[i, j].addon = Addon.question;

                    else if (UnityEngine.Random.Range(0, 100) < Global.exclamationChance)
                        tiles[i, j].addon = Addon.exclamation;

                    else if (UnityEngine.Random.Range(0, 100) < Global.morelessChance)
                        tiles[i, j].addon = Addon.moreless;
                }
            }
        }
        foreach (var tile in tiles)
            NeighborStrategy.CountNeighbors(tile);
        Global.currentMinesCount = Global.minesCount;
    }

    public void SaveNewLevel(TextMeshProUGUI name)
    {
        string fullPath = GM.path + name.text + ".txt";
        UnityEngine.Debug.Log("saving to \"" + fullPath + "\"...");

        StreamWriter sw = new StreamWriter(fullPath);

        sw.WriteLine(Global.y + ";" + Global.x);
        for (int i = Global.y - 1; i >= 0; i--)
        {
            for (int j = 0; j < Global.x; j++)
            {
                sw.Write(tiles[j, i].type == Type.mine ? 1 : 0);
            }
            sw.Write('\n');
        }

        sw.Close();
    }

    public void LoadLevel()
    {
        Global.minesCount = 0;

        string lvlName = PlayerPrefs.GetString("level_name", "random");
        UnityEngine.Debug.Log("loaded " + lvlName);
        tiles = new Tile[Global.x, Global.y];

        if (lvlName == "random")
        {
            NewLevel();
            return;
        }


        using (StreamReader sr = new StreamReader(GM.path + lvlName + ".tGlobal.xt"))
        {
            string[] size = sr.ReadLine().Split(';');
            Global.x = Convert.ToInt32(size[0]);
            Global.y = Convert.ToInt32(size[1]);

            tiles = new Tile[Global.x, Global.y];

            for (int i = 0; i < Global.y; i++)
            {
                string line = sr.ReadLine();
                for (int j = 0; j < Global.x; j++)
                {
                    InstantiateTile(i, j);
                    tiles[i, j].type = line[j] == '1' ? Type.mine : Type.normal;

                    if (line[j] == '1')
                        Global.minesCount++;
                }
            }
        }
        Preparations();
    }

    public void Preparations()
    {
        Global.currentMinesCount = Global.minesCount;

        if (GameFlow.gameState == GameState.creator)
        {
            foreach (var tile in tiles)
                tile.ReCount();
        }
    }
}
