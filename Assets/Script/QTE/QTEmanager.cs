using System;
using UnityEngine;
using UnityEngine.UI;

public class QTEManager : MonoBehaviour
{
    [Header("QTE設定")]
    public KeyCode qteKey = KeyCode.L;

    public float timeLimit = 3f;

    [Range(0f, 1f)]
    public float slowMotionScale = 0.05f;

    private float timer;

    private bool isQTEActive;

    [Header("参照")]
    public PlayerController player;

    // PUSH L UI
    public GameObject qteText;
    [SerializeField] private Image qteTimeLimitImage;

    // 回避成功 UI
    public GameObject successText;
    public event Action<bool> OnQTESwitched;
    public event Action<bool> OnQTEFinished;

    private void Update()
    {
        if (!isQTEActive) return;

        // スロー中でも時間を進める
        timer -= Time.unscaledDeltaTime;
        qteTimeLimitImage.fillAmount = timer / timeLimit;

        // Lキー入力
        if (Input.GetKeyDown(qteKey))
        {
            Success();
        }

        // 時間切れ
        if (timer <= 0f)
        {
            Fail();
        }
    }

    // QTE開始
    public void StartQTE()
    {
        // 二重起動防止
        if (isQTEActive) return;

        timer = timeLimit;

        isQTEActive = true;

        // 超スロー
        Time.timeScale = slowMotionScale;

        // PUSH L 表示
        if (qteText != null)
        {
            qteText.SetActive(true);
        }

        Debug.Log("QTE開始");
        OnQTESwitched?.Invoke(true);
    }

    // 成功
    private void Success()
    {
        isQTEActive = false;

        // 時間を戻す
        Time.timeScale = 1f;

        // PUSH L 非表示
        if (qteText != null)
        {
            qteText.SetActive(false);
        }

        // 回避成功表示
        if (successText != null)
        {
            successText.SetActive(true);

            Invoke(nameof(HideSuccessText), 1f);
        }

        // プレイヤージャンプ
        if (player != null)
        {
            player.QTEJump();
        }

        Debug.Log("QTE成功");
        OnQTESwitched?.Invoke(false);
        OnQTEFinished?.Invoke(true);
    }

    // 失敗
    private void Fail()
    {
        isQTEActive = false;

        // 時間を戻す
        Time.timeScale = 1f;

        // PUSH L 非表示
        if (qteText != null)
        {
            qteText.SetActive(false);
        }

        Debug.Log("QTE失敗");
        OnQTESwitched?.Invoke(false);
        OnQTEFinished?.Invoke(false);
    }

    // 回避成功UIを消す
    private void HideSuccessText()
    {
        if (successText != null)
        {
            successText.SetActive(false);
        }
    }
}