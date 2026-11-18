using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject obstaclePrefab;


    [Header("Tingkat Kesulitan")]
    [SerializeField] private float startInterval = 1.2f;
    [SerializeField] private float minInterval = 0.45f;
    [SerializeField] private float startFallSpeed = 3f;
    [SerializeField] private float maxFallSpeed = 8f;
    [SerializeField] private float difficultyRampSeconds = 60f;

    [Header("Pengaturan Spawn")]
    [Range(0f, 1f)]
    [SerializeField] private float obstacleChance = 0.6f;
    [SerializeField] private float edgePadding = 0.5f;

    private float minX;
    private float maxX;
    private float spawnY;
    private float elapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        Camera cam = Camera.main;
        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;

        minX = cam.transform.position.x - halfWidth + edgePadding;
        maxX = cam.transform.position.x + halfWidth - edgePadding;
        spawnY = cam.transform.position.y + halfHeight + 1f;

        StartCoroutine(SpawnRoutine());   
    }

        IEnumerator SpawnRoutine()
    {
        while (GameManager.Instance.State != GameState.GameOver)
        {
            // t bergerak dari 0 ke 1 selama difficultyRampSeconds detik
            float t = Mathf.Clamp01(elapsedTime / difficultyRampSeconds);
            float interval = Mathf.Lerp(startInterval, minInterval, t);
            float fallSpeed = Mathf.Lerp(startFallSpeed, maxFallSpeed, t);

            SpawnOne(fallSpeed);

            yield return new WaitForSeconds(interval);
            elapsedTime += interval;
        }
    }

    void SpawnOne(float fallSpeed)
    {
        GameObject prefab = (Random.value < obstacleChance) ? obstaclePrefab : coinPrefab;
        if (prefab == null) return;

        Vector3 spawnPos = new Vector3(Random.Range(minX, maxX), spawnY, 0f);
        GameObject obj = Instantiate(prefab, spawnPos, Quaternion.identity);

        FallingObject falling = obj.GetComponent<FallingObject>();
        if (falling != null)
        {
            falling.SetSpeed(fallSpeed);
        }
    }
}
