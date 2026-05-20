using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    [Header("ジャンプ設定")]
    public float jumpPower = 20f;

    public float fallMultiplier = 3f;

    [Header("HP設定")]
    public int hp = 3;

    [Header("死亡状態")]
    public bool isDead;

    [Header("無敵設定")]
    public bool isInvincible;

    public float invincibleTime = 1.5f;

    [Header("UI")]
    public GameObject damageText;

    public GameObject gameOverText;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 死亡中は処理しない
        if (isDead) return;

        // 落下速度を速くする
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity +=
                Vector2.up *
                Physics2D.gravity.y *
                (fallMultiplier - 1) *
                Time.deltaTime;
        }
    }

    // QTE成功時ジャンプ
    public void QTEJump()
    {
        // 死亡中はジャンプしない
        if (isDead) return;

        // 速度リセット
        rb.linearVelocity = Vector2.zero;

        // ジャンプ
        rb.AddForce(
            Vector2.up * jumpPower,
            ForceMode2D.Impulse);

        Debug.Log("回避ジャンプ！");
    }

    // ダメージ処理
    public void TakeDamage(int damage)
    {
        // 無敵中 or 死亡中
        if (isInvincible || isDead) return;

        hp -= damage;

        Debug.Log("ダメージ！ HP : " + hp);

        // 「痛い！」表示
        if (damageText != null)
        {
            damageText.SetActive(true);

            Invoke(nameof(HideDamageText), 1f);
        }

        // 点滅開始
        StartCoroutine(Invincible());

        // HP0以下
        if (hp <= 0)
        {
            StartCoroutine(GameOverDelay());
        }
    }

    // ゲームオーバーまで待つ
    IEnumerator GameOverDelay()
    {
        // 「痛い！」が消えるまで待つ
        yield return new WaitForSecondsRealtime(1f);

        GameOver();
    }

    // ゲームオーバー処理
    void GameOver()
    {
        isDead = true;

        Debug.Log("土に還る");

        // 土に還る表示
        if (gameOverText != null)
        {
            gameOverText.SetActive(true);
        }

        // 45度傾ける
        transform.rotation =
            Quaternion.Euler(0, 0, 45);

        // プレイヤー停止
        rb.linearVelocity = Vector2.zero;

        rb.simulated = false;
    }

    // 無敵＆点滅
    IEnumerator Invincible()
    {
        isInvincible = true;

        float elapsed = 0f;

        while (elapsed < invincibleTime)
        {
            // 非表示
            sr.enabled = false;

            yield return new WaitForSeconds(0.1f);

            // 表示
            sr.enabled = true;

            yield return new WaitForSeconds(0.1f);

            elapsed += 0.2f;
        }

        // 最後に表示状態へ戻す
        sr.enabled = true;

        isInvincible = false;
    }

    // 「痛い！」UIを消す
    void HideDamageText()
    {
        if (damageText != null)
        {
            damageText.SetActive(false);
        }
    }
}