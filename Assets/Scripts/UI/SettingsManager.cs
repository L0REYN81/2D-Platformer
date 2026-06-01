using TMPro;
using UnityEngine;
using UnityEngine.UI;
/*
    *Этот код отвечает за работу настроек
*/
public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    //Элементы настроек
    [Header("UI элементы")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private TMP_Dropdown qualityDropdown;

    // Ключи для сохранения создаются ячейки где хранятся
    private const string MUSIC_KEY    = "MusicVolume";
    private const string SFX_KEY      = "SFXVolume";
    private const string FULLSCREEN_KEY = "Fullscreen";
    private const string QUALITY_KEY  = "Quality";

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        LoadSettings(); // Загружаем сохранённые настройки
    }

    //Изменение громкости музыки и её сохранение
    public void OnMusicChanged(float value)
    {
        AudioManager.Instance.SetMusicVolume(value);
        PlayerPrefs.SetFloat(MUSIC_KEY, value);
    }

    //Изменение громкости звуков
    public void OnSFXChanged(float value)
    {
        AudioManager.Instance.SetSFXVolume(value);
        PlayerPrefs.SetFloat(SFX_KEY, value);
    }

    //Изменение полноэкранного режима
    public void OnFullscreenChanged(bool value)
    {
        Screen.fullScreen = value;
        PlayerPrefs.SetInt(FULLSCREEN_KEY, value ? 1 : 0);
    }

    //Изменение качества
    public void OnQualityChanged(int index)
    {
        QualitySettings.SetQualityLevel(index);
        PlayerPrefs.SetInt(QUALITY_KEY, index);
    }

    //Сохранение и загрузка настроек
    private void LoadSettings()
    {
        // Загружаем значения или ставим дефолт
        float music      = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        float sfx        = PlayerPrefs.GetFloat(SFX_KEY, 1f);
        bool fullscreen  = PlayerPrefs.GetInt(FULLSCREEN_KEY, 1) == 1;
        int quality      = PlayerPrefs.GetInt(QUALITY_KEY, 2);

        // Применяем к UI
        musicSlider.value       = music;
        sfxSlider.value         = sfx;
        fullscreenToggle.isOn   = fullscreen;
        qualityDropdown.value   = quality;

        // Применяем к игре
        AudioManager.Instance.SetMusicVolume(music);
        AudioManager.Instance.SetSFXVolume(sfx);
        Screen.fullScreen       = fullscreen;
        QualitySettings.SetQualityLevel(quality);
    }
}
