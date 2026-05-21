using TMPro;
using UnityEngine;

public class TitleTextEffect : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private float floatHeight = 10f;
    [SerializeField] private float floatSpeed = 2f;

    private RectTransform _rect;
    private Vector3 _startPos;

    private void Awake()
    {
        _rect = titleText.rectTransform;
        _startPos = _rect.localPosition;
    }

    private void Update()
    {
        float offsetY = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        _rect.localPosition = _startPos + new Vector3(0f, offsetY, 0f);
    }
}