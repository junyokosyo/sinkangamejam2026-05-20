using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Utilities;

public class Theme : MonoBehaviour
{
    [SerializeField]
    private Yells _yells;

    [SerializeField]
    private TextMeshPro JapaneseText;

    [SerializeField]
    private TextMeshPro EnglishText;

    private int _selectedIndex;
    private bool _isQTEActive = false;
    private bool _isMissing = false;

    void Start()
    {
        _selectedIndex = _yells.YellTextDataArray.Length;
        OnSelect();

        InputSystem.onAnyButtonPress.Call(OnAnyKeyPressed);
    }

    public void SetQTE(bool isActive)
    {
        _isQTEActive = isActive;
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
        // キーを文字に変換して、表示中テキストに含まれていればその文字を削除する
        if (keyChar != EnglishText.text[0])
        {
            _isMissing = true;
            return;
        }

        TryRemoveCharFromTexts(keyChar);
        if (EnglishText.text == "")
        {
            if (_isMissing)
            {
                Debug.Log("miss");
            }
            else
            {
                Debug.Log("clear");
            }

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
            result =(char)('a' + offset);
            return true;
        }

        return false;
    }

    // 表示中のテキストの「先頭の1文字」のみをチェックして削除する
    private void TryRemoveCharFromTexts(char c)
    {
        // まず EnglishText の先頭文字を優先してチェック
        if (!string.IsNullOrEmpty(EnglishText.text))
        {
            char first = EnglishText.text[0];
            if (char.ToLowerInvariant(first) == char.ToLowerInvariant(c))
            {
                EnglishText.text = EnglishText.text.Remove(0, 1);
            }
            return;
        }
    }

    private void OnSelect()
    {
        _isMissing = false;

        // 0から配列の長さまでのランダムな整数を生成
        int rand = Random.Range(0, _selectedIndex);
        // 生成されたランダムな整数を使用して、配列から要素を取得
        JapaneseText.text = _yells.YellTextDataArray[rand].JapaneseText;
        EnglishText.text = _yells.YellTextDataArray[rand].EnglishText;
    }
}
