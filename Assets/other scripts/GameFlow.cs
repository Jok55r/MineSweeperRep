using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlow : MonoBehaviour
{
    public FieldManager fieldManager;
    public static GameState gameState = GameState.preGame;
    private static GameState lastState = gameState;

    public UIManager uiManager;
    public GM gameManager;

    public void Update()
    {
        if (lastState == gameState)
            return;
        lastState = gameState;


        Debug.Log(gameState.ToString());


        switch (gameState)
        {
            case GameState.preGame:
                fieldManager.LoadLevel();
                break;
            case GameState.endGame:
                EndGame();
                break;
        }
    }

    public void EndGame()
    {
        if (Global.revealedCount == Global.x * Global.y)
        {
            uiManager.ChangeBackgroundCol(Color.green);
        }
        else
        {
            //GM.RevealAllMines();
            uiManager.ChangeBackgroundCol(Color.red);
        }
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "GameScene")
            gameState = GameState.preGame;
        else if (SceneManager.GetActiveScene().name == "Menu")
            gameState = GameState.menu;
    }

    public void CreatorChecker()
    {
        if (gameState == GameState.creator)
            gameState = GameState.endGame;
        else
            gameState = GameState.creator;
        GM.CreatorModeStart();
    }
}

public enum GameState
{
    menu,
    preGame,
    inGame,
    endGame,
    creator
}