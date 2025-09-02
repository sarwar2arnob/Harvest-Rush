using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField]private AudioSource _musicSource;
    [SerializeField]private AudioSource _sfxSource;
    [SerializeField] private int volume;
    [SerializeField] private int sfx_volume;
    [SerializeField] private AudioClip BG;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _musicSource.loop = true;

        PlayMusic(BG);

    }

    public void PlayMusic(AudioClip musicClip)
    {
        if (_musicSource.clip == musicClip && _musicSource.isPlaying) return;
        _musicSource.clip = musicClip;
        _musicSource.Play();
        _musicSource.volume = volume / 100f;
    }

    public void PlaySFX(AudioClip sfxClip)
    {
        _sfxSource.volume = sfx_volume / 100f;
        _sfxSource.PlayOneShot(sfxClip);
        
    }
}