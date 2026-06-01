using UnityEngine;
using UnityEngine.Events;

// Singleton — единственный экземпляр на сцену.
// Хранит текущий счёт и считает бонус за скорость при финише.
// Счёт сбрасывается автоматически при перезагрузке сцены.
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("Бонус за скорость")]
    [SerializeField] private int timeBonus = 500;       // Максимальный бонус за скорость
    [SerializeField] private float bonusTimeLimit = 60f; // Порог времени — после него бонус = 0

    public int CurrentScore { get; private set; }

    private float _levelStartTime; // Время старта уровня — нужно для расчёта бонуса

    // Событие: все подписчики (UI и др.) получат новый счёт автоматически
    public UnityEvent<int> onScoreChanged;

    private void Awake()
    {
        // Паттерн Singleton: уничтожаем лишние копии если сцена перезагрузилась
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        // Запоминаем момент старта — Time.time считает секунды с запуска игры
        _levelStartTime = Time.time;
    }

    // Вызывается ScoreCollectible при подборе предмета
    public void AddScore(int amount)
    {
        CurrentScore += amount;
        onScoreChanged?.Invoke(CurrentScore); // Оповещаем UI и всех подписчиков
    }

    // Вызывается LevelFinish при достижении финиша.
    // Добавляет бонус за скорость и возвращает итоговый счёт.
    public int FinishLevel()
    {
        float elapsed = Time.time - _levelStartTime; // Сколько секунд прошло
        int speedBonus = CalculateSpeedBonus(elapsed);

        AddScore(speedBonus);

        Debug.Log($"Время: {elapsed:F1}с | Бонус за скорость: {speedBonus} | Итого: {CurrentScore}");
        return CurrentScore;
    }

    // Линейная формула: чем меньше времени потрачено — тем больше бонус.
    // Пример при лимите 60с и максимуме 500:
    //   30с → 250 очков
    //   60с → 0 очков
    //   90с → 0 очков
    private int CalculateSpeedBonus(float elapsed)
    {
        if (elapsed >= bonusTimeLimit) return 0; // Лимит истёк — бонуса нет

        float ratio = 1f - (elapsed / bonusTimeLimit); // От 1.0 (мгновенно) до 0.0 (в лимит)
        return Mathf.RoundToInt(timeBonus * ratio);
    }
}