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
        Debug.Log(gameState.ToString());

        if (lastState == gameState)
            return;

        lastState = gameState;

        switch (gameState)
        {
            case GameState.preGame:
                fieldManager.NewLevel();
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
            GM.RevealAllMines();
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
        GM.StartCreating();
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