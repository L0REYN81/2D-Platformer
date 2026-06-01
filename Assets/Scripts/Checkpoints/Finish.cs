using System.Security.Cryptography.X509Certificates;
using EasyTransition;
using UnityEngine;

/*
    *Этот код отвечает ЗА ОКОНЧАНИЕ УРОВНЯ
    *(Если игрок дошёл до финальной точки)
*/
public class Finish : MonoBehaviour
{
    //Сереализация полей
    [Header("Звуки")]
    [SerializeField] private AudioClip finishClip;

    //Настройка перехода
    [SerializeField] private string nextSceneName;
    [SerializeField] private TransitionSettings transition; 
    [SerializeField] private float loadDelay = 0f;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    //Срабатывает при входе игрока в коллайдер Финиша
    void OnTriggerEnter2D(Collider2D collision)
    {
        //Если именно игрок наступает в зону коллайдера
        //Воспроизвести Анимацию, Звук, Анимацию перехода на след уровень
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Уровень пройден");
        
            anim.SetTrigger("pressed");
            AudioManager.Instance.PlaySFX(finishClip);
            TransitionManager.Instance().Transition(nextSceneName, transition, loadDelay);
        }
        //!Заменить на вывод панели результатов с Score(Скорость прохождения, количество очков, Оценка)
        //!Объеденить с LevelFinish
    }
}
