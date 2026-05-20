using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField]
    private FadeManager fadeManager;
    public static SceneTransition Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            StartCoroutine(fadeManager.FadeIn());
            SceneManager.sceneLoaded += (_, _) => StartCoroutine(fadeManager.FadeIn());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SceneLoad(SceneName name)
    {
        StartCoroutine(fadeManager.FadeOutAndLoad(name));
    }
}

public enum SceneName
{
    Title,
    InGame,
    Clear,
    Gameover,
    Ranking,
}
