using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public float speed = 2.0f;

    private void Update()
    {
        transform.position += Vector3.left * (speed * Time.deltaTime);
    }
}
