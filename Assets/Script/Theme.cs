using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Theme : MonoBehaviour
{
    [SerializeField]
    private Yells _yells;

    [SerializeField]
    private TextMeshPro JapaneseText;

    [SerializeField]
    private TextMeshPro EnglishText;

    private int _selectedIndex;
    private UnityEngine.InputSystem.Key _lastKeyPressed = UnityEngine.InputSystem.Key.None;

    public UnityEngine.InputSystem.Key LastKeyPressed => _lastKeyPressed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _selectedIndex = _yells.YellTextDataArray.Length;
        OnSelect();
    }

    // Update is called once per frame
    void Update()
    {
        OnTyping();
    }


    private void OnTyping()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            // 押されたキー情報を記録
            foreach (var keyCtrl in Keyboard.current.allKeys)
            {
                if (keyCtrl.wasPressedThisFrame)
                {
                    _lastKeyPressed = keyCtrl.keyCode;
                    break;
                }
            }
            // キーを文字に変換して、表示中テキストに含まれていればその文字を削除する
            var ch = KeyToChar(_lastKeyPressed);
            if (ch.HasValue)
            {
                TryRemoveCharFromTexts(ch.Value);
                if (EnglishText.text == "")
                {
                    OnSelect();
                }
            }
        }
    }

    // Key を対応する文字に変換する（簡易実装: 英数字とスペース）
    private char? KeyToChar(UnityEngine.InputSystem.Key key)
    {
        // A-Z
        if (key >= UnityEngine.InputSystem.Key.A && key <= UnityEngine.InputSystem.Key.Z)
        {
            int offset = key - UnityEngine.InputSystem.Key.A;
            return (char)('a' + offset);
        }
        // Digits 0-9
        if (key >= UnityEngine.InputSystem.Key.Digit0 && key <= UnityEngine.InputSystem.Key.Digit9)
        {
            int offset = key - UnityEngine.InputSystem.Key.Digit0;
            return (char)('0' + offset);
        }
        if (key == UnityEngine.InputSystem.Key.Space) return ' ';
        if (key == UnityEngine.InputSystem.Key.Period) return '.';
        if (key == UnityEngine.InputSystem.Key.Comma) return ',';
        if (key == UnityEngine.InputSystem.Key.Minus) return '-';
        if (key == UnityEngine.InputSystem.Key.Equals) return '=';
        return null;
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

        // EnglishText が空なら JapaneseText の先頭文字をチェック
        if (!string.IsNullOrEmpty(JapaneseText.text))
        {
            char first = JapaneseText.text[0];
            if (first == c)
            {
                JapaneseText.text = JapaneseText.text.Remove(0, 1);
            }
        }
    }

    private void OnSelect()
    {
        // 0から配列の長さまでのランダムな整数を生成
        int Rand = Random.Range(0, _selectedIndex);
        // 生成されたランダムな整数を使用して、配列から要素を取得
        JapaneseText.text = _yells.YellTextDataArray[Rand].JapaneseText;
        EnglishText.text = _yells.YellTextDataArray[Rand].EnglishText;
    }
}
