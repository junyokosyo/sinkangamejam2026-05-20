using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private Button startButton;
    [SerializeField]
    private Button rankingButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rankingButton.onClick.AddListener(OnRanking);
        startButton.onClick.AddListener(OnStart);
        startButton.onClick.AddListener(() => AudioManager.Instance.PlaySE(SoundType.UISelectSE));
        startButton.onClick.AddListener(() => AudioManager.Instance.StopBGM());
        AudioManager.Instance.PlayBGM(SoundType.TitleBGM);
    }
    public void OnStart()
    {
        SceneTransition.Instance.SceneLoad(SceneName.InGame);
    }
    public void OnRanking() 
    {
     SceneTransition.Instance.SceneLoad(SceneName.Clear);
    }

}
