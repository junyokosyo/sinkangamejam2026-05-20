using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button rankingButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        rankingButton.onClick.AddListener(OnRanking);
        startButton.onClick.AddListener(OnStart);
        AudioManager.Instance.PlayBGM(SoundType.TitleBGM);
    }

    private static void OnStart()
    {
        AudioManager.Instance.StopBGM();
        AudioManager.Instance.PlaySE(SoundType.UISelectSE);
        SceneTransition.Instance.SceneLoad(SceneName.InGame);
    }

    private static void OnRanking()
    {
        AudioManager.Instance.StopBGM();
        AudioManager.Instance.PlaySE(SoundType.UISelectSE);
        SceneTransition.Instance.SceneLoad(SceneName.Clear);
    }
}