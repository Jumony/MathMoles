using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [Header("Volume Sliders")]
    [SerializeField] private Slider sfxSlider;

    [Header("Volume Text (Optional)")]
    [SerializeField] private TextMeshProUGUI sfxVolumeText;

    void Start()
    {
        LoadVolumeSettings();
    }

    void OnEnable()
    {
        // Reload settings when settings menu is opened
        LoadVolumeSettings();
    }

    private void LoadVolumeSettings()
    {
        if (AudioManager.Instance != null)
        {
            // Set slider values from AudioManager
            if (sfxSlider != null)
            {
                sfxSlider.value = AudioManager.Instance.GetSfxVolume();
                sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
            }
            UpdateVolumeText();
        }
    }

    public void OnSFXVolumeChanged(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSfxVolume(value);
            UpdateSFXVolumeText(value);
        }
    }

    private void UpdateVolumeText()
    {
        if (sfxSlider != null)
        {
            UpdateSFXVolumeText(sfxSlider.value);
        }
    }

    private void UpdateSFXVolumeText(float value)
    {
        if (sfxVolumeText != null)
        {
            sfxVolumeText.text = $"Volume: {Mathf.RoundToInt(value * 100)}%";
        }
    }

    void OnDestroy()
    {
        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.RemoveListener(OnSFXVolumeChanged);
        }
    }
}