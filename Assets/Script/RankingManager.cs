using UnityEngine;

public static class RankingManager
{
    public static void SaveRanking(float newTime)
    {
        float[] ranking = new float[5];

        // 読み込み
        for (int i = 0; i < ranking.Length; i++)
        {
            ranking[i] =
                PlayerPrefs.GetFloat(
                    "Rank" + i,
                    9999f
                );
        }

        // 新記録追加
        ranking[4] = newTime;

        // 小さい順に並び替え
        System.Array.Sort(ranking);

        // 保存
        for (int i = 0; i < ranking.Length; i++)
        {
            PlayerPrefs.SetFloat(
                "Rank" + i,
                ranking[i]
            );
        }

        PlayerPrefs.Save();
    }
}