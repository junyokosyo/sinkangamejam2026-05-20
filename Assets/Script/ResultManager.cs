using UnityEngine;
using UnityEngine.UI;
public class ResultManager : MonoBehaviour
{
    [SerializeField]
    private Button titleButton;

    void Start()
    {
        AudioManager.Instance.PlayBGM(SoundType.ClearBGM);
        titleButton.onClick.AddListener(OnTitle);
        titleButton.onClick.AddListener(() => AudioManager.Instance.PlaySE(SoundType.UISelectSE));
        titleButton.onClick.AddListener(() => AudioManager.Instance.StopBGM());
    }

    public void OnTitle()
    {
        SceneTransition.Instance.SceneLoad(SceneName.Title);
    }
}
