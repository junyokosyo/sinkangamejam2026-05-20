using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("参照")]
    public Transform player;

    public QTEManager qteManager;

    [Header("QTE開始距離")]
    public float qteDistance = 2f;

    [Header("ダメージ設定")]
    public int damage = 1;
    
    public bool hasQTESucceed;

    // QTE重複防止
    private bool hasStartedQTE;

    void Update()
    {
        // 一回だけQTE開始
        if (hasStartedQTE) return;

        // プレイヤーとの距離
        float distance =
            Vector2.Distance(
                transform.position,
                player.position);

        // 指定距離以下でQTE開始
        if (distance <= qteDistance)
        {
            hasStartedQTE = true;

            qteManager.StartQTE();
        }
    }

    // Triggerに入った瞬間
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // QTE成功してたら何もしない
        if (hasQTESucceed)
        {
            return;
        }
        // Playerに当たった
        if (collision.CompareTag("Player"))
        {
            PlayerController playerController =
                collision.GetComponent<PlayerController>();

            // ダメージ
            if (playerController != null)
            {
                playerController.TakeDamage(damage);
            }
        }

        // Wallに当たった
        if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}