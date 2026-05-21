using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI countDownText;
    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private TMP_Text velocityText;
    [SerializeField] private TMP_Text timerText;

    [Header("Player")] 
    [SerializeField] private Transform player;

    [Header("Goal")]
    [SerializeField] private Transform goalPos;

    private bool gameStart;
    private float scoreTimer;
    public event Action OnCountDownComplete;

    private void Start()
    {
        timerText.text = $"タイム:{scoreTimer:F2}s";
        UpdateDistanceText();
        StartCoroutine(CountDown());
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
            SceneTransition.Instance.SceneLoad(SceneName.Clear);
        }
        else
        {
            ScoreTimer();
        }
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

        int startCount = 3;
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