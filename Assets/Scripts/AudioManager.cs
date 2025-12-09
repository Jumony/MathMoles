using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip hammerHitClip;
    [SerializeField] private AudioClip correctAnswerClip;
    [SerializeField] private AudioClip incorrectAnswerClip;
    [SerializeField] private AudioClip buttonClickClip;

    private const string SfxVolumeKey = "SFXVolume";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadVolumeSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadVolumeSettings()
    {
        // Load saved volumes or use default (0.1)
        var sfxVolume = PlayerPrefs.GetFloat(SfxVolumeKey, 0.1f);
        SetSfxVolume(sfxVolume);
    }

    public void SetSfxVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        if (sfxSource != null)
        {
            sfxSource.volume = volume;
        }
        PlayerPrefs.SetFloat(SfxVolumeKey, volume);
        PlayerPrefs.Save();
    }

    public float GetSfxVolume()
    {
        return sfxSource != null ? sfxSource.volume : PlayerPrefs.GetFloat(SfxVolumeKey, 0.7f);
    }

    // Play specific sound effects
    public void PlayHammerHit()
    {
        PlaySfx(hammerHitClip);
    }

    public void PlayCorrectAnswer()
    {
        PlaySfx(correctAnswerClip);
    }

    public void PlayIncorrectAnswer()
    {
        PlaySfx(incorrectAnswerClip);
    }

    // Generic method to play any sound effect
    private void PlaySfx(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}