using UnityEngine;
using TMPro;
using System.Collections;

public class CountDownText : MonoBehaviour
{
    public TextMeshProUGUI countDownText;

    bool gameStart = false;

    void Start()
    {
        StartCoroutine(CountDown());
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

    void Update()
    {
        // カウント中は入力できない
        if (!gameStart) return;

        // ここにタイピング処理
    }
}