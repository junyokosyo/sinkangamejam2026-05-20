using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public float speed = 2.0f;
    private Vector3 startPosition;
    private bool movingLeft = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
    }
}
