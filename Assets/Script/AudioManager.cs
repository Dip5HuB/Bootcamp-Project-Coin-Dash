using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip coinClip;
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private AudioClip gameOverClip;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] [Range(0f, 1f)] private float musicVolume = 0.3f;

    void Awake()
    {
        Instance = this;

        if (musicSource != null)
        {
            musicSource.loop = true;
            musicSource.volume = musicVolume;
            musicSource.Play();
        }
    }

    public void PlayCoin()
    {
        PlaySfx(coinClip);
    }

    public void PlayHit()
    {
        PlaySfx(hitClip);
    }

    public void PlayGameOver()
    {
        PlaySfx(gameOverClip);
    }

    void PlaySfx(AudioClip clip)
    {
        if (sfxSource == null || clip == null)
            return;
        
        // Variasi pitch kecil agar suara berulang tidak terdengar monoton
        sfxSource.pitch = Random.Range(0.95f, 1.05f);
        sfxSource.PlayOneShot(clip);
    }

}