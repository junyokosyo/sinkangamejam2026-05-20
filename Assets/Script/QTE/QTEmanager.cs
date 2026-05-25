using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Utilities;

public class QTEManager : MonoBehaviour
{
    [Header("QTE設定")] public Key qteKey = Key.L;

    public float timeLimit = 3f;

    [SerializeField] private float timeLimitDecreasePerSuccess = 0.05f;

    [SerializeField] private float timeLimitMin = 0.2f;

    [Range(0f, 1f)] public float slowMotionScale = 0.05f;

    private float timer;

    private bool isQTEActive;

    [Header("参照")] public PlayerController player;

    // PUSH L UI
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Text qteTimeLimit;

    // 回避成功 UI
    public GameObject successText;
    public event Action<bool> OnQTESwitched;
    public event Action<bool> OnQTEFinished;

    private float currentQTETimeLimit;

    private Coroutine _coroutine;
    private IDisposable _inputSubscription;

    private void Start()
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0f;
        canvasGroup.gameObject.SetActive(false);
        
        currentQTETimeLimit = timeLimit;
        
        _inputSubscription = InputSystem.onAnyButtonPress.Call(OnAnyKeyPressed);
    }

    private void OnDestroy()
    {
        _inputSubscription?.Dispose();
    }

    public void GameEnd()
    {
        _inputSubscription?.Dispose();
    }

    private void OnAnyKeyPressed(InputControl control)
    {
        if (Keyboard.current == null) return;
        if (!isQTEActive) return;

        if (control is KeyControl keyCtrl)
        {
            if (keyCtrl.keyCode == qteKey)
            {
                Success();
            }
        }
    }

    private void Update()
    {
        if (!isQTEActive) return;

        // スロー中でも時間を進める
        timer -= Time.unscaledDeltaTime;
        qteTimeLimit.SetText($"{timer:F2}s");

        // 時間切れ
        if (timer <= 0f)
        {
            Fail();
        }
    }

    public void SpeedUpQTE()
    {
        // 呼び出し毎にQTEの時間制限を減らす
        currentQTETimeLimit = Mathf.Max(timeLimitMin, currentQTETimeLimit - timeLimitDecreasePerSuccess);
    }

    // QTE開始
    public void StartQTE()
    {
        // 二重起動防止
        if (isQTEActive) return;

        timer = currentQTETimeLimit;
        Debug.Log(timer);

        isQTEActive = true;

        // 超スロー
        Time.timeScale = slowMotionScale;
        AudioManager.Instance.SetBGMPitch(slowMotionScale);

        // PUSH L 表示
        if (canvasGroup != null)
        {
            _coroutine = StartCoroutine(SlowMoPanelFade(0.1f));
        }

        Debug.Log("QTE開始");
        AudioManager.Instance.PlaySE(SoundType.QteWarningSE);
        OnQTESwitched?.Invoke(true);
    }

    private IEnumerator SlowMoPanelFade(float duration)
    {
        canvasGroup.gameObject.SetActive(true);
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = true;
        var time = 0f;
        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            canvasGroup.alpha = time / duration;
            yield return null;
        }

        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 1f;
    }

    // 成功
    private void Success()
    {
        OnQTEFinished?.Invoke(true);

        // 回避成功表示
        if (successText != null)
        {
            successText.SetActive(true);

            StartCoroutine(HideSuccessText(1f));
        }

        if (player != null)
        {
            player.QTEJump();
        }

        QTEFinish();
    }

    // 失敗
    private void Fail()
    {
        player.TakeDamage(1);
        currentQTETimeLimit = timeLimit;
        OnQTEFinished?.Invoke(false);
        
        AudioManager.Instance.PlaySE(SoundType.CarCrashSE);

        QTEFinish();
    }

    private void QTEFinish()
    {
        OnQTESwitched?.Invoke(false);
        isQTEActive = false;

        // 時間を戻す
        Time.timeScale = 1f;
        AudioManager.Instance.SetBGMPitch(1f);

        // PUSH L 非表示
        if (canvasGroup != null)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = true;
        }
    }

    // 回避成功UIを消す
    private IEnumerator HideSuccessText(float wait)
    {
        yield return new WaitForSeconds(wait);
        if (successText != null)
        {
            successText.SetActive(false);
        }
    }
}