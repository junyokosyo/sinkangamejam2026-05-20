using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using unityroom.Api;

public class GameUI : MonoBehaviour
{
    [Header("UI")] [SerializeField] private TextMeshProUGUI countDownText;
    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private TMP_Text velocityText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private RectTransform addSpeedTextPos;
    [SerializeField] private RectTransform addSpeedEndPos;
    [SerializeField] private TMP_Text addSpeedText;
    [SerializeField] private RectTransform clearStartAnchor;
    [SerializeField] private RectTransform clearEndAnchor;

    [Header("Player")] [SerializeField] private Transform player;
    [Header("Goal")] [SerializeField] private Transform goalPos;

    [SerializeField] private AnimationCurve curve;

    private bool gameStart;
    private float scoreTimer;
    public event Action OnCountDownComplete;
    public event Action OnClear;
    
    private readonly Queue<TMP_Text> textQueue = new();

    private void Start()
    {
        for (int i = 0; i < 20; i++)
        {
            var text = Instantiate(addSpeedText, addSpeedTextPos.position, Quaternion.identity, addSpeedTextPos);
            text.gameObject.SetActive(false);
            textQueue.Enqueue(text);
        }
        timerText.SetText($"タイム:{scoreTimer:F2}s");
        UpdateDistanceText();
        StartCoroutine(CountDown());
    }

    public void StopTimer()
    {
        gameStart = false;
    }

    public void Clear()
    {
        countDownText.enabled = true;
        countDownText.SetText("GOAL!");

        StartCoroutine(MoveStartToEndAsync(clearStartAnchor, clearEndAnchor, 3));
    }

    private IEnumerator MoveStartToEndAsync(RectTransform from, RectTransform to, float duration)
    {
        Vector3 startPos = from.position;
        Vector3 endPos = to.position;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            var t = elapsed / duration;
            countDownText.transform.position = Vector3.Lerp(startPos, endPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        countDownText.transform.position = endPos;
    }

    private void Update()
    {
        // カウント中は動かない
        if (!gameStart) return;

        var remainDistance = UpdateDistanceText();

        // ゴール
        if (remainDistance <= 0)
        {
            gameStart = false;
            distanceText.SetText("GOAL!");
            UnityroomApiClient.Instance.SendScore(1, scoreTimer, ScoreboardWriteMode.HighScoreAsc);
            RankingManager.SaveRanking(scoreTimer);
            OnClear?.Invoke();
        }
        else
        {
            ScoreTimer();
        }
    }

    public void AddSpeedText(float amount)
    {
        var text = textQueue.Dequeue();
        text.gameObject.SetActive(true);
        text.SetText($"+{amount:F2}m/s");
        StartCoroutine(MoveAndDestroy(text, addSpeedTextPos, addSpeedEndPos, 1));
    }

    private IEnumerator MoveAndDestroy(TMP_Text text, RectTransform from, RectTransform to, float duration)
    {
        Vector2 startPos = from.position;
        Vector2 endPos = to.position;
        Color startColor = text.color;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            var t = curve.Evaluate(elapsed / duration);
            text.transform.position = Vector3.Lerp(startPos, endPos, t);
            text.color = new Color(startColor.r, startColor.g, startColor.b, 1 - t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        // プールに戻す
        text.transform.position = startPos;
        text.color = startColor;
        text.gameObject.SetActive(false);
        textQueue.Enqueue(text);
    }

    private float UpdateDistanceText()
    {
        float remainDistance =
            goalPos.position.x - player.position.x;

        remainDistance = Mathf.Max(0, remainDistance);

        distanceText.SetText($"おうちまで あと {remainDistance:F1}m");
        return remainDistance;
    }

    private IEnumerator CountDown()
    {
        countDownText.enabled = false;
        // 開幕フェード分のバッファ
        yield return new WaitForSeconds(1f);
        countDownText.enabled = true;
        AudioManager.Instance.PlaySE(SoundType.CountDownSE);

        int startCount = 3;
        countDownText.SetText(startCount.ToString());
        while (startCount-- > 0)
        {
            yield return new WaitForSeconds(1);
            countDownText.SetText(startCount.ToString());
        }

        countDownText.SetText("START!");

        yield return new WaitForSeconds(1);
        OnCountDownComplete?.Invoke();

        countDownText.SetText("");
        gameStart = true;
    }

    private void ScoreTimer()
    {
        scoreTimer += Time.deltaTime;
        timerText.SetText($"タイム:{scoreTimer:F2}s");
    }

    public void UpdateVelocityText(float velocity)
    {
        velocityText.SetText($"はやさ:{velocity:F1} m/s");
    }
}