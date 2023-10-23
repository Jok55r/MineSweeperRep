using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlow : MonoBehaviour
{
    public static GameState gameState = GameState.preGame;

    public UIManager uiManager;
    public GM gameManager;

    public void ChangeState(GameState state)
    {
        switch (state)
        {
            case GameState.endGame:
                GM.RevealAllMines();
                break;
        }
    }

    public void EndGame(bool won)
    {
        if (won)
        {
            uiManager.ChangeBackgroundCol(Color.green);
        }
        else
        {
            GM.RevealAllMines();
            uiManager.ChangeBackgroundCol(Color.red);
        }
        gameState = GameState.endGame;
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