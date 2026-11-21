using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileSettings : MonoBehaviour
{
    void Awake()
    {
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    // Dipanggil Unity saat aplikasi masuk ke latar belakang (pause = true)
    void OnApplicationPause(bool pause)
    {
        if (!pause) return;

        if (GameManager.Instance != null && GameManager.Instance.State == GameState.Playing)
        {
            GameManager.Instance.TogglePause();
        }
    }
}
