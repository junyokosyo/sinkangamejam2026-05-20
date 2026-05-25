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
                    $"Rank{i}",
                    9999f
                );

            // データなし
            if (Mathf.Approximately(time, 9999f))
            {
                rankingTexts[i].SetText($"No.{(i + 1)}: ---");

                continue;
            }

            rankingTexts[i].SetText($"No.{(i + 1)}: {time:F2}[s]");
        }
    }
}