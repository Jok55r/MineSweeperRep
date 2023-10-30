using System;
using TMPro;
using UnityEngine;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;

public class GM : MonoBehaviour
{
    public  int mineChance = 10;
    public  int negativeMineChance = 15;
    public  int y = 10;
    public  int x = 10;
    public  int questionChance = 0;
    public  int exclamationChance = 0;
    public  int morelessChance = 0;
    public  bool negativeInclude = false;


    public FieldManager fieldManager;
    public GameFlow gameFlow;
    public UIManager UImanager;

    public int revealedtiles;

    public static string path = System.IO.Directory.GetCurrentDirectory() + @"/";


    public void Awake()
    {
        Global.y = y;
        Global.x = x;
        Global.mineChance = mineChance;
        Global.negativeMineChance = negativeMineChance;
        Global.questionChance = questionChance;
        Global.exclamationChance=exclamationChance;
        Global.morelessChance = morelessChance;
        Global.negativeInclude = negativeInclude;
    }

    public void SmilieFunc()
    {
        FieldManager.AdjustTile(FieldManager.tiles[0, 0].pos);
        foreach (Tile tile in FieldManager.tiles)
        {
            if (tile.IsZero() && tile.type != Type.mine)
            {
                tile.Reveal(false);
                break;
            }
        }
    }

    public static void CreatorModeStart()
    {
        if (GameFlow.gameState == GameState.creator)
        {
            foreach (Tile tile in FieldManager.tiles)
            {
                if (tile.type == Type.mine)
                    tile.state = State.marked;
                else
                    tile.state = State.revealed;
                tile.visual.Render(tile);
            }
        }

    }

    private void Update()
    {
        if (revealedtiles == Global.x * Global.y)
        {
            GameFlow.gameState = GameState.endGame;
        }
        if (GameFlow.gameState == GameState.creator && GameFlow.gameState != GameState.endGame)
        {
            RevealAll(0);
        }

        revealedtiles = Global.revealedCount;
        DebugMethod();
    }

    public void StartNewLevel()
        => GameFlow.gameState = GameState.preGame;


    // !!! WARNING: YOU ARE ENTERING THE ZONE OF GOVNO CODE / DEBUG !!!


    private void DebugMethod()
    {
        switch (Input.inputString)
        {
            case "0": RevealAll(0); break;
            case "1": RevealAll(1); break;
            case "2": RevealAll(2); break;
            case "3": RevealAll(3); break;
            case "4": RevealAll(4); break;
            case "5": RevealAll(5); break;
            case "6": RevealAll(6); break;
            case "7": RevealAll(7); break;
            case "8": RevealAll(8); break;
            case "9": RevealAll(9); break;
        }
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (var tile in FieldManager.tiles)
                tile.Reveal(false);
        }
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            foreach (var tile in FieldManager.tiles)
                if (tile.type != Type.mine && tile.mineCount == 0) tile.Reveal(false);
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            foreach (var tile in FieldManager.tiles)
                if (tile.type != Type.mine && tile.mineCount == 1) tile.Reveal(false);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            foreach (var tile in FieldManager.tiles)
                if (tile.type != Type.mine && tile.mineCount == 2) tile.Reveal(false    );
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            foreach (var tile in FieldManager.tiles)
                if (tile.type != Type.mine && tile.mineCount == 3) tile.Reveal(false);
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            foreach (var tile in FieldManager.tiles)
                if (tile.type != Type.mine && tile.mineCount == 4) tile.Reveal(false);
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            foreach (var tile in FieldManager.tiles)
                if (tile.type != Type.mine && tile.mineCount == 5) tile.Reveal(false);
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            foreach (var tile in FieldManager.tiles)
                if (tile.type != Type.mine && tile.mineCount == 6) tile.Reveal(false);
        }
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            foreach (var tile in FieldManager.tiles)
                if (tile.type != Type.mine && tile.mineCount == 7) tile.Reveal(false);
        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            foreach (var tile in FieldManager.tiles)
                if (tile.type != Type.mine && tile.mineCount == 8) tile.Reveal(false);
        }*/
    }

    private void RevealAll(int num)
    {
        if (num == 0)
        {
            RevealAllZeros();
            return;
        }
        foreach (var tile in FieldManager.tiles)
            if (tile.type != Type.mine && tile.MineCount() == num) tile.Reveal(false);
        UnityEngine.Debug.Log($"Opened all {num}");
    }

    private void RevealAllZeros()
    {
        foreach (var tile in FieldManager.tiles)
            if (tile.type != Type.mine && tile.IsZero()) tile.Reveal(false);
    }

    public static void RevealAllMines()
    {
        foreach (var tile in FieldManager.tiles)
            if (tile.type == Type.mine) tile.Reveal(false);
        UnityEngine.Debug.Log($"Opened all mines");
    }
}