using UnityEngine;

// Компонент предмета который даёт очки при подборе.
// Проигрывает общую анимацию подбора перед деактивацией.
public class ScoreCollectible : MonoBehaviour
{
    [Header("Очки")]
    [SerializeField] private int scoreValue = 100;

    [Header("Звуки")]
    [SerializeField] private AudioClip coinClip;

    private Animator _animator;
    private bool _collected; // Защита от повторного срабатывания

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //проверяем игрок ли подбирает и подобрано ли
        if (!collision.CompareTag("Player") || _collected) return;

        _collected = true;
        ScoreManager.Instance.AddScore(scoreValue);

        // Запускаем анимацию — деактивация произойдёт в конце через Animation Event
        AudioManager.Instance.PlaySFX(coinClip);
        _animator.SetTrigger("collect");

        // Отключаем коллайдер сразу — чтобы не сработало дважды(при отработки анимации)
        GetComponent<Collider2D>().enabled = false;
    }

    // Этот метод вызывается через Animation Event в конце анимации collect
    // (добавь Event в Animation окне на последний кадр)
    public void OnCollectAnimationEnd()
    {
        gameObject.SetActive(false);    //убираем объект
        _collected = false; // Сбрасываем если объект будет переиспользован
    }
}