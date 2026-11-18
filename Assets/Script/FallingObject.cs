using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [SerializeField] private float fallSpeed = 3f;
    [SerializeField] private float spinSpeed = 0f;

    private float destroyY; 

    // Start is called before the first frame update
    void Start()
    {
        Camera cam = Camera.main;
        destroyY = cam.transform.position.y - cam.orthographicSize - 1f;
    }

    public void SetSpeed(float newSpeed)
    {
        fallSpeed = newSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime, Space.World);
        transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);

        if (transform.position.y < destroyY)
        {
            Destroy(gameObject);
        }
    }
}
