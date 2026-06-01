using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

/*
    *Этот скрипт управляет батутом (трамплином). Когда игрок падает на него сверху:
*/
public class TrampolineControllers : MonoBehaviour
{
    //настройка прыжка
    [Header("Прыжок")]
    public float bounceForce = 20f;
    public ForceMode2D forceMode = ForceMode2D.Impulse; //Для резкого импулься силы прыжка

    //настройки камеры
    [Header("Камера при прыжке")]
    public float bounceDamping = 1.5f;  //параметр плавности
    public float bounceDampingDuration = 1.2f;  //длительность

    [Header("Звуки")]
    [SerializeField] private AudioClip trampolineClip;

    private Animator anim;
    private CinemachinePositionComposer composer;
    private Vector3 defaultDamping;

    void Awake()
    {
        anim = GetComponent<Animator>();
        // Найти CinemachineCamera в сцене
        CinemachineCamera vcam = Object.FindAnyObjectByType<CinemachineCamera>();

        if (vcam != null)
        {
            composer = vcam.GetComponent<CinemachinePositionComposer>();   //этот компонент следует за игроком

            if (composer != null)
                defaultDamping = composer.Damping;  //применяем настройки
            else
                Debug.LogWarning("CinemachinePositionComposer не найден на камере!");
        }
        else
        {
            Debug.LogWarning("CinemachineCamera не найдена в сцене!");
        }
    }

    //при столкновении коллайдера с батутом
    void OnCollisionEnter2D(Collision2D col)
    {
        // Срабатывает только для игрока
        if (!col.gameObject.CompareTag("Player")) return;

        Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();//берём физ оболочку игрока

        // Только если игрок падает вниз
        if (rb == null || rb.linearVelocityY > 0) return;

        // Сбросить вертикальную скорость и применить импульс
        rb.linearVelocity = new Vector2(rb.linearVelocityX, 0);
        rb.AddForce(Vector2.up * bounceForce, forceMode);   //резкий импульс вверх

        // Запустить анимацию плиты
        AudioManager.Instance.PlaySFX(trampolineClip);
        anim.SetTrigger("bounce");

        // Смягчить камеру
        if (composer != null)
            StartCoroutine(SoftCamera());
    }

    //сглаживаем камеру при прыжке 
    IEnumerator SoftCamera()
    {
        // Увеличить damping по Y на время прыжка
        composer.Damping = new Vector3(
            defaultDamping.x,
            bounceDamping,
            defaultDamping.z
        );

        yield return new WaitForSeconds(bounceDampingDuration);

        // Вернуть обратно
        composer.Damping = defaultDamping;
    }
}