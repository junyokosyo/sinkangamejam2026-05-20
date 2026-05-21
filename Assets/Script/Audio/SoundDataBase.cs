using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundData", menuName = "ScriptableObjects/SoundData", order = 1)]
public class SoundDataBase : ScriptableObject
{
    public SoundData[] soundDataArray;
}

[Serializable]
public class SoundData
{
    public SoundType type;

    public AudioClip clip;

    [Range(0f, 1f)] public float volume = 1f;

    [Range(0.1f, 3f)] public float pitch = 1f;

    public bool loop = false;
}