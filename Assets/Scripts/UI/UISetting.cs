using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TranDuc;

public class UISetting : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button applyButton;

    private AudioManager audioManager;
    private float originalMusicVolume;
    private float originalSfxVolume;
    private float tempMusicVolume;
    private float tempSfxVolume;

    void Start()
    {
        audioManager = AudioManager.Instance;
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found!");
            return;
        }

        GameData.Instance.LoadStartGameData();
        originalMusicVolume = GameData.Instance.startData.musicVolume;
        originalSfxVolume = GameData.Instance.startData.soundVolume;
        tempMusicVolume = originalMusicVolume;
        tempSfxVolume = originalSfxVolume;

        if (musicSlider != null)
        {
            musicSlider.value = tempMusicVolume;
            musicSlider.onValueChanged.AddListener(UpdateMusicVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = tempSfxVolume;
            sfxSlider.onValueChanged.AddListener(UpdateSfxVolume);
        }

        if (resetButton != null)
        {
            resetButton.onClick.AddListener(ResetToDefault);
        }

        if (applyButton != null)
        {
            applyButton.onClick.AddListener(ApplyChanges);
        }
    }

    private void UpdateMusicVolume(float value)
    {
        tempMusicVolume = value;
        if (audioManager != null && audioManager.MusicSource != null)
        {
            audioManager.MusicSource.volume = tempMusicVolume;
        }
    }

    private void UpdateSfxVolume(float value)
    {
        tempSfxVolume = value;
        if (audioManager != null && audioManager.SoundSource != null)
        {
            audioManager.SoundSource.volume = tempSfxVolume;
        }
    }

    private void ResetToDefault()
    {
        AudioManager.Instance.PlaySoundClickButton();
        if (musicSlider != null) musicSlider.value = 0.5f;
        if (sfxSlider != null) sfxSlider.value = 0.5f;
        tempMusicVolume = 0.5f;
        tempSfxVolume = 0.5f;
        UpdateMusicVolume(tempMusicVolume);
        UpdateSfxVolume(tempSfxVolume);
        ApplyChanges();
    }

    private void ApplyChanges()
    {
        if (audioManager != null)
        {
            AudioManager.Instance.PlaySoundClickButton();
            audioManager.SetMusicVolume(tempMusicVolume);
            audioManager.SetSoundVolume(tempSfxVolume);
            originalMusicVolume = tempMusicVolume;
            originalSfxVolume = tempSfxVolume;
        }
    }

    void OnDisable()
    {
        if (audioManager != null)
        {
            audioManager.SetMusicVolume(originalMusicVolume);
            audioManager.SetSoundVolume(originalSfxVolume);
            if (musicSlider != null) musicSlider.value = originalMusicVolume;
            if (sfxSlider != null) sfxSlider.value = originalSfxVolume;
        }
    }

    void OnDestroy()
    {
        if (musicSlider != null)
            musicSlider.onValueChanged.RemoveListener(UpdateMusicVolume);
        if (sfxSlider != null)
            sfxSlider.onValueChanged.RemoveListener(UpdateSfxVolume);
        if (resetButton != null)
            resetButton.onClick.RemoveListener(ResetToDefault);
        if (applyButton != null)
            applyButton.onClick.RemoveListener(ApplyChanges);
    }
}