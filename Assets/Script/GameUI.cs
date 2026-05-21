using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [Header("UI")] [SerializeField] private TextMeshProUGUI countDownText;
    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private TMP_Text velocityText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private Transform addSpeedTextPos;
    [SerializeField] private TMP_Text addSpeedText;

    [Header("Player")] [SerializeField] private Transform player;
    [Header("Goal")] [SerializeField] private Transform goalPos;

    [SerializeField] private AnimationCurve curve;

    private bool gameStart;
    private float scoreTimer;
    public event Action OnCountDownComplete;
    public event Action OnClear;

    private void Start()
    {
        timerText.text = $"タイム:{scoreTimer:F2}s";
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
        countDownText.text = "GOAL!";
        StartCoroutine(MoveX(10, 1900, -1900));
    }

    private IEnumerator MoveX(float duration, float from, float to)
    {
        Vector3 startPos = countDownText.transform.position;
        startPos.x = from;
        Vector3 endPos = new Vector3(to, startPos.y, startPos.z);

        float elapsed = 0f;
        while (elapsed < duration)
        {
            var t = curve.Evaluate(elapsed / duration);
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
            distanceText.text = "GOAL!";
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
        var text = Instantiate(addSpeedText, addSpeedTextPos.position, Quaternion.identity, addSpeedTextPos);
        text.SetText($"+{amount:F2}m/s");
        StartCoroutine(MoveAndDestroy(text, 100, 1));
    }

    private IEnumerator MoveAndDestroy(TMP_Text text, float distance, float duration)
    {
        Vector3 startPos = text.transform.position;
        Vector3 endPos = startPos + Vector3.up * distance;
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

        Destroy(text.gameObject);
    }

    private float UpdateDistanceText()
    {
        float remainDistance =
            goalPos.position.x - player.position.x;

        remainDistance = Mathf.Max(0, remainDistance);

        distanceText.text = $"おうちまで あと {remainDistance:F1}m";
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
        countDownText.text = startCount.ToString();
        while (startCount-- > 0)
        {
            yield return new WaitForSeconds(1);
            countDownText.text = startCount.ToString();
        }

        countDownText.text = "START!";

        yield return new WaitForSeconds(1);
        OnCountDownComplete?.Invoke();

        countDownText.text = "";
        gameStart = true;
    }

    private void ScoreTimer()
    {
        scoreTimer += Time.deltaTime;
        timerText.text = $"タイム:{scoreTimer:F2}s";
    }

    public void UpdateVelocityText(float velocity)
    {
        velocityText.text = $"はやさ:{velocity:F1} m/s";
    }
}