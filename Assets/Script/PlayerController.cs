using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float keyboardSpeed = 8f;
    [SerializeField] private float touchFollowSpeed = 20f;
    [SerializeField] private float edgePadding = 0.5f;

    private Camera cam;
    private float minX;
    private float maxX;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        CalculateScreenBounds();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.State != GameState.Playing) return;

        if (!HandlePointerInput())
        {
            HandleKeyboardInput();
        }

        ClampPosition();
    }

    bool HandlePointerInput()
    {
        Vector2 screenPos;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                return false;
            }

            screenPos = touch.position;
        }

        else if (Input.GetMouseButton(0))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return false;
            }

            screenPos = Input.mousePosition;
        }
        else
        {
            return false;
        }

        float targetX = cam.ScreenToWorldPoint(screenPos).x;
        float newX = Mathf.MoveTowards(transform.position.x, targetX, touchFollowSpeed * Time.deltaTime);

        transform.position = new Vector3(newX, transform.position.y, transform.position.z);

        return true;
    }

    void HandleKeyboardInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        transform.Translate(Vector3.right * moveX * keyboardSpeed * Time.deltaTime, Space.World);
    }

    void CalculateScreenBounds()
    {
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

        else if (other.CompareTag("Heart"))
        {
            GameManager.Instance.AddLife(1);
            Destroy(other.gameObject);
        }
    }
}
