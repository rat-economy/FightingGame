using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager Instance;
    [SerializeField] private Sound vs_Sound;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
        foreach (var sound in sounds)
        {
            sound.m_Source = gameObject.AddComponent<AudioSource>();
            sound.m_Source.clip = sound.m_Clip;
            sound.m_Source.volume = sound.m_Volume;
        }
        // SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("loading scene!");
        foreach (var sound in sounds)
        {
            sound.m_Source = gameObject.AddComponent<AudioSource>();
            sound.m_Source.clip = sound.m_Clip;
            sound.m_Source.volume = sound.m_Volume;
        }
    }

    public void PlaySoundOnce(Sound sound)
    {
        if(sound == null || sound.m_Source == null)
        {
            return;
        }
        sound.m_Source.loop = false;
        sound.m_Source.Play();
    }
    public void PlaySoundLooped(Sound sound)
    {
        if(sound == null || sound.m_Source == null)
        {
            return;
        }
        sound.m_Source.loop = true;
        sound.m_Source.Play();
    }

    public void StopSound(Sound sound)
    {
        if(sound == null || sound.m_Source == null)
        {
            return;
        }
        sound.m_Source.Stop();
    }

    public void StopSound(string soundName)
    {
        GetAudioSource(soundName).Stop();
    }

    private AudioSource GetAudioSource(string soundName)
    {
        foreach (Sound sound in sounds)
        {
            if(sound.name == soundName) return sound.m_Source;
        }
        Debug.LogError("Sound Name Not Found!");
        return null;
    }

    public void BeginGameStart_Announcer(Sound p1, Sound p2)
    {
        StartCoroutine(GameStart_Announcer(p1, p2));
    }

    private IEnumerator GameStart_Announcer(Sound p1, Sound p2)
    {
        PlaySoundOnce(p1);
        yield return new WaitForSeconds(Constants.ANNOUNCER_DELAY);
        PlaySoundOnce(vs_Sound);
        yield return new WaitForSeconds(Constants.ANNOUNCER_DELAY);
        PlaySoundOnce(p2);
        
    }
}