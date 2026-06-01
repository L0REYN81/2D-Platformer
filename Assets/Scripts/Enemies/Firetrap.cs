using UnityEngine;
using System.Collections;

/*
    *Этот скрипт управляет огненной ловушкой(циркулярной пилой), 
    *воспроизводит звук и наносит урон игроку при столкновении.
    !Переписать код логики срабатывания
*/
public class Firetrap : MonoBehaviour
{
    //Сереализация полей
    [Header ("Урон")]
    [SerializeField] private float damage;

    [Header ("Настройки таймера")]
    [SerializeField] private float activationDelay; //переключение в режим огня
    [SerializeField] private float activeTime;  //время действия

    [Header("Звуки")]
    [SerializeField] private AudioClip fireClip;

    //Объявление компонентов Объекта
    private AudioSource fireLoopSource;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    //Флаги состояния
    private bool triggered;
    private bool active;

    //Получаем компоненты объекта
    private void Awake() 
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //Если игрок попадает в зону колаядера
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.tag == "Player")
        {
            //Если не включён активируем ловушку
            if (!triggered)
            {
                StartCoroutine(ActivateFiretrap());
            }
            //Если активна наносим игроку урон
            if(active)
                collision.GetComponent<Health>().TakeDamage(damage);
        }
    }

    //Корутина активации ловушки
    private IEnumerator ActivateFiretrap()
    {
        //активирована меняем цвет
        //!Изменение цвета заменить на анимацию
        triggered = true;
        spriteRenderer.color = Color.red;

        //Ждём указанноле количекство секунд и активируем ловушку
        yield return new  WaitForSeconds(activationDelay);
        spriteRenderer.color = Color.white;
        active = true;
        animator.SetBool("activated", true);
        fireLoopSource = AudioManager.Instance.PlayLoop(fireClip, volume: 1f);
        
        //Уходим в режим ожидание 
        yield return new  WaitForSeconds(activeTime);
        AudioManager.Instance.StopLoop(fireLoopSource);

        active = false;
        triggered = false;
        animator.SetBool("activated", false);
    }
}