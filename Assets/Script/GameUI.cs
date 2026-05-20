using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI countDownText;
    [SerializeField] private TextMeshProUGUI distanceText;

    [Header("Player")]
    [SerializeField] private Transform player;

    [Header("Goal")]
    [SerializeField] private Transform goalPoji;

    private bool gameStart = false;
    private float scoreTimer = 0;

    void Start()
    {
        StartCoroutine(CountDown());
    }

    void Update()
    {
        // カウント中は動かない
        if (!gameStart) return;

        float remainDistance =
            goalPoji.position.x - player.position.x;

        remainDistance = Mathf.Max(0, remainDistance);

        distanceText.text =
            "ゴールまで あと " +
            remainDistance.ToString("F1") +
            " m";

        // ゴール
        if (remainDistance <= 0)
        {
            gameStart = false;

            distanceText.text = "GOAL!";

           RankingManager.SaveRanking(scoreTimer);

            SceneManager.LoadScene("Result");
        }
        else
        {
            ScoreTimer();
        }
    }

    IEnumerator CountDown()
    {
        countDownText.text = "3";
        yield return new WaitForSeconds(1);

        countDownText.text = "2";
        yield return new WaitForSeconds(1);

        countDownText.text = "1";
        yield return new WaitForSeconds(1);

        countDownText.text = "START!";
        yield return new WaitForSeconds(1);

        countDownText.text = "";

        gameStart = true;
    }
    void ScoreTimer()
    {
        scoreTimer += Time.deltaTime;
    }
}