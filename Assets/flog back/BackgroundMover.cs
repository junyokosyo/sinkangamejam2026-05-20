using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class BackGroundMover : MonoBehaviour
{
    [SerializeField]
    private float _scrollSpeed;

    private RectTransform _rectTransform;
    private float _imageWidth;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        
        // 画像の横幅を取得
        _imageWidth = _rectTransform.rect.width;
        
        // 画像の横幅を二倍にする
        var sizeDelta = _rectTransform.sizeDelta;
        sizeDelta.x *= 2f;
        _rectTransform.sizeDelta = sizeDelta;
        _imageWidth *= 2f;
    }

    public void SetSpeed(float speed)
    {
        _scrollSpeed = speed;
    }

    private void Update()
    {
        if (Time.timeScale == 0f)
        {
            return;
        }

        // 時間に応じて左にスクロール
        float scrollDistance = _scrollSpeed * Time.deltaTime;
        var pos = _rectTransform.anchoredPosition;
        pos.x -= scrollDistance;
        
        // 画像が完全に左端に移動したらリセット
        if (pos.x <= -_imageWidth / 2f)
        {
            pos.x += _imageWidth / 2f;
        }
        
        _rectTransform.anchoredPosition = pos;
    }
}