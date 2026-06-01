using System;
using System.Collections;
using UnityEngine;

/*
    *Этот скрипт отвечает за систему здоровья игрока
    - Получение урона
    - Смерть возрождение
    - Временную неуязвимость
    - Лечение
    !Переписсать логику присутствует дублирование
*/
public class Health : MonoBehaviour
{
    //Настройки здоровья
    [Header("Здоровье")]
    [SerializeField] private float startingHealth;
    public float currentHealth {get; private set; } //Изменять могу только внутри класса
    private Animator animator;
    private bool dead;

    [Header("Параметры Неуязвимости")]
    [SerializeField]private float iFramesDuration;
    [SerializeField]private int numberOfFlashes;

    [Header("Звуки")]
    [SerializeField] private AudioClip hurtClip;
    [SerializeField] private AudioClip deathClip;
    [SerializeField] private AudioClip respawnClip;
    private SpriteRenderer spriteRend;

    public float StartingHealth { get; private set;} //Дублирование Проверить на необходимость
    private void Awake()
    {
        StartingHealth = startingHealth;
        currentHealth = startingHealth;
        animator = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    //Получение урона
    public void TakeDamage(float _damage)
    {
        //Урон не может быть выше максимального и ниже 0
        currentHealth = Math.Clamp(currentHealth - _damage, 0, startingHealth); 
        //Если игрок ещё жив получаем урон и кативируем неуязвимость
        if (currentHealth > 0)
        {
            AudioManager.Instance.PlaySFX(hurtClip);
            animator.SetTrigger("hurt");
            StartCoroutine(Invunerability());
        }
        //Если фатальный урон
        else
        {   if(!dead)
            {
                AudioManager.Instance.PlaySFX(deathClip);
                animator.SetTrigger("die");
                GetComponent<PlayerController>().enabled = false; //отключаем управление
                dead = true;
                StartCoroutine(RespawnAfterDeath());
            }
        }

    }

    //Возрождение игрока
    private IEnumerator RespawnAfterDeath()
    {
        yield return new WaitForSeconds(1f); // пауза пока играет анимация смерти

        // Восстанавливаем игрока
        SpawnManager.Instance.RespawnPlayer(gameObject);
        currentHealth = startingHealth;
        dead = false;
        AudioManager.Instance.PlaySFX(respawnClip);
        animator.SetTrigger("respawn");
        GetComponent<PlayerController>().enabled = true;    // возращаем управленеи
    }

    //Лечение Игрока
    public void AddHealth(float _value)
    {
        currentHealth = Math.Clamp(currentHealth + _value, 0, startingHealth); //Также ограничиваем
    }

    //Неуязвимость
    private IEnumerator Invunerability()
    {
        //Отключаем столкновение между слоями 10(Игрок) 11(Враги/ловушки)
        Physics2D.IgnoreLayerCollision(10, 11, true);   
        for (int i = 0; i < numberOfFlashes; i++)   //реализация мигания
        {
            spriteRend.color = new Color(1, 1, 1, 0.5f);    //Делаем игрока полупрозрачным
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes));   //ожидание
            spriteRend.color = Color.white; 
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes));

        }
        Physics2D.IgnoreLayerCollision(10, 11, false);  //Возращаем столкновения
    }
}