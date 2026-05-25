using UnityEngine;
using UnityEngine.UI;
public class GameOverManeger : MonoBehaviour
{
    [SerializeField]
    private Button titleButton;

    private void Start()
    {
        AudioManager.Instance.PlaySE(SoundType.GraveapperSE);
        titleButton.onClick.AddListener(OnTitle);
    }

    private static void OnTitle()
    {
        AudioManager.Instance.PlaySE(SoundType.UISelectSE);
        AudioManager.Instance.StopBGM();
        SceneTransition.Instance.SceneLoad(SceneName.Title);
    }
}
