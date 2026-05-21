using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private Button startButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startButton.onClick.AddListener(OnStart);
        AudioManager.Instance.PlayBGM(SoundType.TitleBGM);
    }
    public void OnStart()
    {
        SceneTransition.Instance.SceneLoad(SceneName.InGame);
    }
}
