using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Playerタグを持つ相手に当たったとき
        if (collision.gameObject.CompareTag("Player"))
        {
            // PlayerHealthスクリプトを取得
            PlayerHealth playerHealth =
                collision.gameObject.GetComponent<PlayerHealth>();

            // スクリプトが存在したらダメージ
            if (playerHealth != null)
            {
                playerHealth.health -= 1;

                Debug.Log("プレイヤーに1ダメージ！");
                Debug.Log("残り体力: " + playerHealth.health);

                // 体力0で削除
                if (playerHealth.health <= 0)
                {
                    Debug.Log("プレイヤー死亡");
                    Destroy(collision.gameObject);
                }
            }
        }
    }
}