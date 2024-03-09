using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public Sound[] menuSongs;
    public Sound[] fightSongs;
    public static AudioManager Instance;
    [SerializeField] private Sound vs_Sound;
    private void Start()
    {
        foreach (var sound in sounds)
        {
            sound.m_Source = gameObject.AddComponent<AudioSource>();
            sound.m_Source.clip = sound.m_Clip;
            sound.m_Source.volume = sound.m_Volume;
        }
    }
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

        
        
        // SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /*void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("loading scene!");
        foreach (var sound in sounds)
        {
            sound.m_Source = gameObject.AddComponent<AudioSource>();
            sound.m_Source.clip = sound.m_Clip;
            sound.m_Source.volume = sound.m_Volume;
        }
    }*/

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
            Debug.Log("Sound broken");
            Debug.Log("Sound: " + sound);
            Debug.Log("Source: " + sound.m_Source);
            return;
        }
        Debug.Log("Playing sound");
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

    public void PickFightSong()//This is shit and i will not hardcode in next version but need to fly now
    {
        Debug.Log("Picking fight song");
        int index = Random.Range(4, 6);
        Debug.Log(index);
        PlaySoundLooped(sounds[index]);
    }
    
    public void StopFightMusic() //Fuck you roosevelt
    {
        StopSound(sounds[4]);
        StopSound(sounds[5]);
    }
}