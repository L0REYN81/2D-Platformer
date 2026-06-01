using System.Collections;
using EasyTransition;
using UnityEngine;

/*
    *Этот скрипт отвечает за работу глвного меню (Его панелей)
*/

public class MainMenu : MonoBehaviour
{
    //Блок сериализуемых полей(параметры)
    [Header("Переходы")]
    [SerializeField] private TransitionSettings transition;
    [SerializeField] private float loadDelay = 0f; //задержка

    [Header("Сцены")]
    [SerializeField] private string firstLevelName = "Level1";

    [Header("Панели")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject selectPanel;

    [Header("Музыка")]
    [SerializeField] private AudioClip menuMusic;

    //Аниматор конкретной кнопки
    private Animator currentAnimator;

    //Запускается автоматически при запуске объекта
    private void Start()
    {
        AudioManager.Instance.PlayMusic(menuMusic);

        //Скрываем панели
        settingsPanel.SetActive(false);
        selectPanel.SetActive(false);

        currentAnimator = mainPanel.GetComponent<Animator>();
    }

    public void StartGame()
    {
        TransitionManager.Instance().Transition(firstLevelName, transition, loadDelay);
    }

    //Краткие стрелочные функции для открытия панелей
    public void OpenSettings()    => SwitchPanel(settingsPanel);
    public void OpenLevelSelect() => SwitchPanel(selectPanel);
    public void GoBack()          => SwitchPanel(mainPanel);

    //!Выход из игры Работает при полной сборке програмы
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Выход из игры");
    }

    void SwitchPanel(GameObject toPanel)
    {
        //Cохраняем текущую активную панель(Чобы использовать его аниматор)
        GameObject previousPanel = currentAnimator.gameObject;
        
        // Включаем новую панель
        toPanel.SetActive(true);
        Animator toAnimator = toPanel.GetComponent<Animator>();
        toAnimator.SetTrigger("slideIn");

        // Закрываем старую с помощью Coroutine(скроет панель)
        currentAnimator.SetTrigger("slideOut");
        StartCoroutine(HideAfterAnimation(previousPanel));

        //Меняем аниматор текущей панели
        currentAnimator = toAnimator;
    }

    //Дожидаемя оконачание анимации и скрываем панель
    IEnumerator HideAfterAnimation(GameObject panel)
    {
        Animator anim = panel.GetComponent<Animator>();

        // Ждём один кадр чтобы триггер успел примениться
        yield return null; // Приостановить выполнение метода и возобновить его со следующего кадра

        // Ждём пока играет SlideOut
        while (anim.GetCurrentAnimatorStateInfo(0).IsName("SlideOut"))
        {
            yield return null;
        }

        panel.SetActive(false);
    }
}