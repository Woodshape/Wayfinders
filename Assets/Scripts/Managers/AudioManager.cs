using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource levelMusic, gameOverMusic, winMusic, titleScreenMusic;

    public AudioSource[] SFX;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        levelMusic.Play();
    }

    public void PlayGameOver()
    {
        levelMusic.Stop();
        gameOverMusic.Play();
    }

    public void PlayVictory()
    {
        levelMusic.Stop();
        winMusic.Play();
    }

    public void PlaySFX(int index)
    {
        SFX[index].Stop();
        SFX[index].Play();
    }
}
