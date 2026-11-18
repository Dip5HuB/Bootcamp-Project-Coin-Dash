using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float edgePadding = 0.5f;

    private float minX;
    private float maxX;

    // Start is called before the first frame update
    void Start()
    {
        CalculateScreenBounds();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.State != GameState.Playing) return;

        float moveX = Input.GetAxis("Horizontal"); 
        transform.Translate(Vector3.right * moveX * moveSpeed * Time.deltaTime, Space.World);

        ClampPosition();
    }

    void CalculateScreenBounds()
    {
        Camera cam = Camera.main;
        float halfWidth = cam.orthographicSize * cam.aspect;

        minX = cam.transform.position.x - halfWidth + edgePadding;
        maxX = cam.transform.position.x + halfWidth - edgePadding;
    }

    void ClampPosition()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        transform.position = pos;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            GameManager.Instance.AddScore(10);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Obstacle"))
        {
            GameManager.Instance.LoseLife();
            Destroy(other.gameObject);
        }
    }
}
