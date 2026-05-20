using UnityEngine;

public class BackgroundLooper : MonoBehaviour
{
    [SerializeField] private Transform bgA;
    [SerializeField] private Transform bgB;
    [SerializeField] private float moveSpeed = 2f;

    private float width;

    private void Start()
    {
        width = bgA.GetComponent<SpriteRenderer>().bounds.size.x;

        bgA.position = Vector3.zero;
        bgB.position = new Vector3(width, 0, 0);
    }

    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }

    private void Update()
    {
        Vector3 move = Vector3.left * (moveSpeed * Time.deltaTime);

        bgA.position += move;
        bgB.position += move;

        // A が左側へ行ったら B の右へ
        if (bgA.position.x <= -width)
        {
            bgA.position += Vector3.right * width;
            bgB.position += Vector3.right * width;
        }
    }
}