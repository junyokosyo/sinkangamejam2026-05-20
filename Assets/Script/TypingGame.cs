using UnityEngine;
using TMPro;
using System.Collections;

public class TypingGame : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI countDownText;

    float time = 60f;

    bool gameStart = false;

    void Start()
    {
        StartCoroutine(CountDown());
    }

    void Update()
    {
        // カウント中は時間止める
        if (!gameStart) return;

        // タイマー
        time -= Time.deltaTime;

        timerText.text =
            "残り " + Mathf.Ceil(time) + " 秒";

        // 0秒で終了
        if (time <= 0)
        {
            time = 0;
            gameStart = false;

            Debug.Log("ゲーム終了！");
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
}