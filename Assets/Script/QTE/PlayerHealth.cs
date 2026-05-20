using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 3;

    private void OnCollisionEnter(Collision collision)
    {
        // 相手のタグが Enemy のとき
        if (collision.gameObject.CompareTag("Enemy"))
        {
            health -= 1;

            Debug.Log("現在の体力: " + health);

            // 体力0以下で死亡
            if (health <= 0)
            {
                Debug.Log("ゲームオーバー");
                Destroy(gameObject);
            }
        }
    }
}