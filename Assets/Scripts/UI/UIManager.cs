using Unity.VisualScripting;
using EasyTransition;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
    *Этот код отвечает за работу паузы
    !доработать или лучше переписать цикличные звуки
*/
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    //Главная панель (Здоровье, очки)
    [Header("HUD")]
    [SerializeField] private GameObject hudPanel;

    //Панель паузы
    [Header("Пауза")]
    [SerializeField] private GameObject pausePanel;

    //Панель настроек
    [Header("Настройки")]
    [SerializeField] private GameObject settingsPanel;

    //переход между сценой(в меню)
    [Header("Переходы")]
    [SerializeField] private TransitionSettings transition;
    [SerializeField] private float loadDelay = 0f;
    [SerializeField] private string scene = "Scene";
    private bool isPaused = false;
    void Awake()
    {
        // Не реагируем на ESC если открыты настройки
        if (IsSettingsOpen) return;

        if (Instance == null)
            Instance = this;
        else 
            Destroy(gameObject);
    }

    public bool IsSettingsOpen => settingsPanel.activeSelf;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Если настройки открыты — не трогаем паузу
            if (IsSettingsOpen)
            {
                HideSettings();
                return; // ← выходим, не доходим до паузы
            }

            if (isPaused) HidePause();
            else ShowPause();
        }
    }
    
    //отображаем паузу
    public void ShowPause()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        hudPanel.SetActive(false);
        Time.timeScale = 0f;
    }

    //скрываем паузу
    public void HidePause()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        hudPanel.SetActive(true);
        Time.timeScale = 1f;
    }

    //показывваем настройки
    public void ShowSettings()
    {
        settingsPanel.SetActive(true);
        pausePanel.SetActive(false);   // скрываем паузу
    }

    //скрываем настройки
    public void HideSettings()
    {
        settingsPanel.SetActive(false);
        pausePanel.SetActive(true);    // возвращаемся в паузу
    }

    //переходим в меню
    public void GoToMainMenu()
    {
        // Сбрасываем паузу перед переходом
        Time.timeScale = 1f;
        isPaused = false;
        TransitionManager.Instance().Transition(scene, transition, loadDelay);
    }

    //выход
    public void QuitGame()
    {
        Application.Quit();
    }
}
