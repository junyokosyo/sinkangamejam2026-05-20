using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Yells", menuName = "Scriptable Objects/Yells")]
public class Yells : ScriptableObject
{
    public YellTextData[] YellTextDataArray;
}

[Serializable]
public struct YellTextData
{
    public string JapaneseText;
    public string EnglishText;
}