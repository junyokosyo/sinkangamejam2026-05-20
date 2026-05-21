using UnityEngine;
using UnityEngine.UI;
public class GameOverManeger : MonoBehaviour
{
    [SerializeField]
    private Button titleButton;

    void Start()
    {
        AudioManager.Instance.StopBGM();

        AudioManager.Instance.PlaySE(SoundType.GraveapperSE);
        titleButton.onClick.AddListener(OnTitle);
        titleButton.onClick.AddListener(() => AudioManager.Instance.PlaySE(SoundType.UISelectSE));
        titleButton.onClick.AddListener(() => AudioManager.Instance.StopBGM());
    }

    public void OnTitle()
    {
        SceneTransition.Instance.SceneLoad(SceneName.Title);
    }
}
