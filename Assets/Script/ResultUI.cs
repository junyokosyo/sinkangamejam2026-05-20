using UnityEngine;
using TMPro;

public class ResultUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] rankingTexts;

    void Start()
    {
        for (int i = 0; i < rankingTexts.Length; i++)
        {
            float time =
                PlayerPrefs.GetFloat(
                    "Rank" + i,
                    9999f
                );

            // データなし
            if (time == 9999f)
            {
                rankingTexts[i].text =
                    (i + 1) + "位 : ---";

                continue;
            }

            rankingTexts[i].text =
                (i + 1) + "位 : " +
                time.ToString("F2") +
                " 秒";
        }
    }
}