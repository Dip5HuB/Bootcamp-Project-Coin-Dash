using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Playing,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private int startingLives = 3;

    public GameState State { get; private set; }
    public int Score { get; private set; }
    public int Lives { get; private set; }

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
        State = GameState.Playing;
        Score = 0;
        Lives = startingLives;
    }

    public void AddScore(int amount)
    {
        if (State != GameState.Playing) return;

        Score += amount;
        Debug.Log("Skor: " + Score);
    }

    public void LoseLife()
    {
        if (State != GameState.Playing) return;

        Lives--;
        Debug.Log("Nyawa: " + Lives);

        if (Lives <= 0)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        State = GameState.GameOver;
        Time.timeScale = 0f;
        Debug.Log("Game Over! Skor Akhir : " + Score);
    }
}
