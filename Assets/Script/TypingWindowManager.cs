using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TypingWindowManager : MonoBehaviour
{
    [SerializeField]
    private Yells _yells;

    [SerializeField]
    private TMP_Text JapaneseText;

    [SerializeField]
    private TMP_Text EnglishText;
    
    [SerializeField]
    private Image _backgroundImage;

    [SerializeField]
    private Color _firstCharColor = Color.yellow;

    private int _selectedIndex;
    private bool _isQTEActive;
    private bool _isMissing;
    private string currentText;
    public event Action<bool> OnTypingComplete;

    private void Start()
    {
        _backgroundImage.gameObject.SetActive(false);
    }

    public void GameStart()
    {
        _selectedIndex = _yells.YellTextDataArray.Length;
        _backgroundImage.gameObject.SetActive(true);
        OnSelect();
        
        InputSystem.onAnyButtonPress.Call(OnAnyKeyPressed);
    }

    public void SetQTE(bool isActive)
    {
        _isQTEActive = isActive;
        _backgroundImage.gameObject.SetActive(!isActive);
    }

    private void OnAnyKeyPressed(InputControl control)
    {
        if (Keyboard.current == null) return;
        if (_isQTEActive) return;

        if (control is KeyControl keyCtrl)
        {
            if (TryKeyToChar(keyCtrl.keyCode, out char ch))
            {
                CheckInputKey(ch);
            }
        }
    }

    private void CheckInputKey(char keyChar)
    {
        // 不一致でミス扱い
        if (keyChar != currentText[0])
        {
            _isMissing = true;
            return;
        }

        if (!string.IsNullOrEmpty(currentText))
        {
            char first = currentText[0];
            if (char.ToUpperInvariant(first) == char.ToUpperInvariant(keyChar))
            {
                var newMsg = currentText[1..];
                UpdateText(newMsg);
                currentText = newMsg;
            }
        }
        
        
        if (string.IsNullOrEmpty(currentText))
        {
            OnTypingComplete?.Invoke(_isMissing);

            OnSelect();
        }
    }

    // Key を対応する文字に変換する（簡易実装: 英数字とスペース）
    private bool TryKeyToChar(Key key, out char result)
    {
        result = '\0';
        // A-Z
        if (key >= Key.A && key <= Key.Z)
        {
            int offset = key - Key.A;
            result =(char)('A' + offset);
            return true;
        }

        return false;
    }

    private void OnSelect()
    {
        _isMissing = false;

        // 0から配列の長さまでのランダムな整数を生成
        int rand = Random.Range(0, _selectedIndex);
        // 生成されたランダムな整数を使用して、配列から要素を取得
        JapaneseText.text = _yells.YellTextDataArray[rand].JapaneseText;
        currentText = _yells.YellTextDataArray[rand].EnglishText.ToUpper();
        UpdateText(currentText);
    }

    private void UpdateText(string msg)
    {
        // 一文字目だけ色を変える
        if (msg.Length > 0)
        {
            string colorHex = ColorUtility.ToHtmlStringRGB(_firstCharColor);
            string coloredMsg = $"<color=#{colorHex}>{msg[0]}</color>{msg[1..]}";
            EnglishText.text = coloredMsg;
        }
        else
        {
            EnglishText.text = msg;
        }
    }
}
