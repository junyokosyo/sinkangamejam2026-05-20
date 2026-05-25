using UnityEngine;
using UnityEngine.UI;
public class ResultManager : MonoBehaviour
{
    [SerializeField]
    private Button titleButton;

    private void Start()
    {
        AudioManager.Instance.PlayBGM(SoundType.ClearBGM);
        titleButton.onClick.AddListener(OnTitle);
    }

    private static void OnTitle()
    {
        AudioManager.Instance.PlaySE(SoundType.UISelectSE);
        AudioManager.Instance.StopBGM();
        SceneTransition.Instance.SceneLoad(SceneName.Title);
    }
}
