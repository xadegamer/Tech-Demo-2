using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioHandler : MonoBehaviour
{
    public static AudioHandler Instance { get; private set; }

    [SerializeField] AudioClip menuAudio;
    [SerializeField] AudioClip lobbyAudio;
    [SerializeField] AudioSource musicPlayer;
    [SerializeField] AudioSource ambientPlayer;
    [SerializeField] AudioSource sfxPlayer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        ChangeMusicWithFade(menuAudio, true);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
        {
            ChangeMusicWithFade(menuAudio, true);
        }
    }

    public void ChangeMusic(AudioClip audioClip, bool loop)
    {
        musicPlayer.clip = audioClip;
        musicPlayer.loop = loop;
        musicPlayer.Play();
    }

    public void PauseMusic()
    {
        musicPlayer.Pause();
    }

    public void ResumeMusic()
    {
        musicPlayer.UnPause();
    }

    public IEnumerator StopMusic()
    {
        float speed = 0.05f;

        while (musicPlayer.volume >= speed)
        {
            musicPlayer.volume -= speed;
            yield return new WaitForSeconds(0.1f);
        }

        musicPlayer.Stop();
    }

    public void ChangeMusicWithFade(AudioClip audioClip, bool loop, float speed = 0.05f)
    {
        StartCoroutine(ChangeMusicWithFadeRoutine(audioClip, loop, speed));
    }

    public IEnumerator ChangeMusicWithFadeRoutine(AudioClip audioClip, bool loop, float speed)
    {
        while (musicPlayer.volume >= speed)
        {
            musicPlayer.volume -= speed;
            yield return new WaitForSecondsRealtime(0.1f);
        }

        musicPlayer.clip = audioClip;
        musicPlayer.loop = loop;
        musicPlayer.Play();

        while (musicPlayer.volume < 1)
        {
            musicPlayer.volume += speed;
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    public void BlendTwoMusic(AudioClip audioClip, AudioClip loopMusic)
    {
        StartCoroutine(BlendTwoMusicRoutine(audioClip, loopMusic));
    }

    public IEnumerator BlendTwoMusicRoutine(AudioClip intro, AudioClip loopMusic)
    {
        ChangeMusic(intro, false);
        yield return new WaitForSecondsRealtime(musicPlayer.clip.length - 0.5f);
        ChangeMusic(loopMusic, true);
    }

    public AudioSource GetSfxAudioSource() => sfxPlayer;
}
