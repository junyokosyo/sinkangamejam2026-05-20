using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TypingWindowManager : MonoBehaviour
{
    [SerializeField] private Yells _yells;

    [SerializeField] private TMP_Text JapaneseText;

    [SerializeField] private TMP_Text EnglishText;

    [SerializeField] private Image _backgroundImage;

    [SerializeField] private TMP_Text _noMissText;

    [SerializeField] private Color _firstCharColor = Color.yellow;

    private int _selectedIndex;
    private bool _isQTEActive;
    private bool _isMissing;
    private string currentText;
    public event Action<bool> OnTypingComplete;
    private IDisposable _inputSubscription;

    private void Start()
    {
        _backgroundImage.gameObject.SetActive(false);
        _noMissText.gameObject.SetActive(false);
    }

    public void GameStart()
    {
        _selectedIndex = _yells.YellTextDataArray.Length;
        _backgroundImage.gameObject.SetActive(true);
        OnSelect();

        _inputSubscription = InputSystem.onAnyButtonPress.Call(OnAnyKeyPressed);
    }

    private void OnDestroy()
    {
        _inputSubscription?.Dispose();
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
        if (!string.IsNullOrEmpty(currentText))
        {
            char first = currentText[0];
            if (char.ToUpperInvariant(first) == char.ToUpperInvariant(keyChar))
            {
                AudioManager.Instance.PlaySE(SoundType.TypingSE);
                var newMsg = currentText[1..];
                UpdateText(newMsg);
                currentText = newMsg;
            }
            else
            {
                AudioManager.Instance.PlaySE(SoundType.TypingMissSE);
                _isMissing = true;
                return;
            }
        }


        if (string.IsNullOrEmpty(currentText))
        {
            OnTypingComplete?.Invoke(_isMissing);
            AudioManager.Instance.PlaySE(SoundType.TypingSuccessSE);
            AudioManager.Instance.PlaySE(SoundType.YellSE);
            if (!_isMissing)
            {
                StopAllCoroutines();
                StartCoroutine(NoMissText(0.5f));
            }

            OnSelect();
        }
    }

    public void GameEnd()
    {
        _backgroundImage.gameObject.SetActive(false);
        _inputSubscription?.Dispose();
    }

    private IEnumerator NoMissText(float duration)
    {
        _noMissText.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        _noMissText.gameObject.SetActive(false);
    }

    // Key を対応する文字に変換する（簡易実装: 英数字とスペース）
    private bool TryKeyToChar(Key key, out char result)
    {
        result = '\0';

        // A-Z のみ対応（大文字）
        if (key >= Key.A && key <= Key.Z)
        {
            int offset = key - Key.A;
            result = (char)('A' + offset);
            return true;
        }

        // ハイフン（マイナス）キーのみ対応
        if (key == Key.Minus)
        {
            result = '-';
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
        var yellData = _yells.YellTextDataArray[rand];
        JapaneseText.SetText(yellData.JapaneseText);
        currentText = yellData.EnglishText.ToUpper();
        UpdateText(currentText);
    }

    private void UpdateText(string msg)
    {
        // 一文字目だけ色を変える
        if (msg.Length > 0)
        {
            string colorHex = ColorUtility.ToHtmlStringRGB(_firstCharColor);
            string coloredMsg = new StringBuilder().Append("<color=#")
                .Append(colorHex)
                .Append(">")
                .Append(msg[0])
                .Append("</color>")
                .Append(msg[1..])
                .ToString();
            EnglishText.SetText(coloredMsg);
        }
        else
        {
            EnglishText.SetText(msg);
        }
    }
}