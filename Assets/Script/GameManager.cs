using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public enum GameState
{
    Playing,
    Paused,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Pengaturan Game")]
    [SerializeField] private int startingLives = 3;

    [Header("Referensi UI")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private GameObject pausePanel;


    public GameState State { get; private set; }
    public int Score { get; private set; }
    public int Lives { get; private set; }

    private const string HighScoreKey = "HighScore";

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
        State = GameState.Playing;
        Score = 0;
        Lives = startingLives;
    }

    void Start()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        RefreshHud();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void AddScore(int amount)
    {
        if (State != GameState.Playing) return;

        Score += amount;
        RefreshHud();
        Debug.Log("Skor: " + Score);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayCoin();
        }
    }

    public void LoseLife()
    {
        if (State != GameState.Playing) return;

        Lives--;
        RefreshHud();
        Debug.Log("Nyawa: " + Lives);

        if (Lives <= 0)
        {
            EndGame();
        }
        else if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayHit();
        }
    }

    public void AddLife(int amount)
    {
        if (State != GameState.Playing) return;

        Lives = Mathf.Min(Lives + amount, 5);
        RefreshHud();
        Debug.Log("Nyawa: " + Lives);
    }

    void EndGame()
    {
        State = GameState.GameOver;
        Time.timeScale = 0f;
        Debug.Log("Game Over! Skor Akhir : " + Score);

        int best=PlayerPrefs.GetInt(HighScoreKey, 0);
        if (Score > best)
        {
            PlayerPrefs.SetInt(HighScoreKey, Score);
            PlayerPrefs.Save();
        }

        if (finalScoreText != null) finalScoreText.text = "Skor Akhir: " + Score;
        if (highScoreText != null) highScoreText.text = "Skor Tertinggi: " + best;
        if (gameOverPanel != null) gameOverPanel.SetActive(true);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayGameOver();
        }
    }

    public void TogglePause()
    {
        if (State == GameState.Playing)
        {
            State = GameState.Paused;
            Time.timeScale = 0f;
        }
        else if (State == GameState.Paused)
        {
            State = GameState.Playing;
            Time.timeScale = 1f;
        }
        else
        {
            return;
        }

        if (pausePanel != null) pausePanel.SetActive(State == GameState.Paused);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }   

    void RefreshHud()
    {
        if (scoreText != null) scoreText.text = "Skor: " + Score;
        if (livesText != null) livesText.text = "Nyawa: " + Lives;
    }
        
}
