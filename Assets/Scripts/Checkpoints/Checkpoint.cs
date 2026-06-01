using UnityEngine;

/*
    *Этот код ортвечает за Сам объект чекпоинта, за его нстройки
*/
public class Checkpoint : MonoBehaviour
{
    private Animator anim;

    //Сереализация полей
    [Header("Звуки")]
    [SerializeField] private AudioClip pointClip;


    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    //Если поподаем в триггер-коллайдер , меняем положения спавна
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {   
            //Передаём точку , воспроизводим звук и анимацию
            SpawnManager.Instance.SetCheckpoint(transform);
            AudioManager.Instance.PlaySFX(pointClip);
            anim.SetTrigger("unfold");
        }
    }
}


