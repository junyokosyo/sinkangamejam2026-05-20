using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource seSource;
    [SerializeField] private AudioSource bgmSource;

    [SerializeField] private List<SoundData> soundList;

    private Dictionary<SoundType, SoundData> soundDict
        = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            foreach (var sound in soundList)
            {
                if (soundDict.ContainsKey(sound.type))
                {
                    Debug.LogWarning(
                        $"{sound.type} ‚ÍŠù‚É“o˜^‚³‚ê‚Ä‚¢‚Ü‚·"
                    );

                    continue;
                }

                soundDict.Add(sound.type, sound);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // =========================
    // SE
    // =========================

    public void PlaySE(SoundType type)
    {
        if (!soundDict.TryGetValue(type, out SoundData sound))
            return;

        seSource.pitch = sound.pitch;

        seSource.PlayOneShot(
            sound.clip,
            sound.volume
        );

        seSource.pitch = 1f;
    }

    // =========================
    // BGM
    // =========================

    public void PlayBGM(SoundType type)
    {
        if (!soundDict.TryGetValue(type, out SoundData sound))
            return;

        bgmSource.clip = sound.clip;
        bgmSource.volume = sound.volume;
        bgmSource.pitch = sound.pitch;
        bgmSource.loop = sound.loop;

        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }
}