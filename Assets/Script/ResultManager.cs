using UnityEngine;
using UnityEngine.UI;
public class ResultManager : MonoBehaviour
{
    [SerializeField]
    private Button titleButton;

    void Start()
    {
        titleButton.onClick.AddListener(OnTitle);
    }

    public void OnTitle()
    {
        SceneTransition.Instance.SceneLoad(SceneName.Title);
    }
}
