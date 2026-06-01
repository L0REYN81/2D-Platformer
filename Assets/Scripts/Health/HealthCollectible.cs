using UnityEngine;

/*
    *Этот скрипт отвечает за изменение HealthBar Игрока
    !Изменить скрипт т.к он был расчитан на 10 хп
*/

public class HealthCollectible : MonoBehaviour
{
    [SerializeField] private float healthValue; //на сколько хилим
    [Header("Звуки")]
    [SerializeField] private AudioClip healClip;

    //если подбирает игрок лечим его и делаем объект невидимым
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<Health>().AddHealth(healthValue);
            AudioManager.Instance.PlaySFX(healClip);
            gameObject.SetActive(false);
;        }
    }
}
