using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float scrollSpeed = 2f;

    public float resetPositionX = -20f;

    public float startPositionX = 20f;

    void Update()
    {
        // 左へ移動
        transform.position +=
            Vector3.left *
            scrollSpeed *
            Time.deltaTime;

        // 画面外へ行ったら右へ戻す
        if (transform.position.x <= resetPositionX)
        {
            transform.position =
                new Vector3(
                    startPositionX,
                    transform.position.y,
                    transform.position.z);
        }
    }
}