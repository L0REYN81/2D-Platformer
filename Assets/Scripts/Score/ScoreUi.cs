using UnityEngine;
using TMPro;

// Отображает текущий счёт на экране.
// Подписывается на событие ScoreManager — обновляется только при изменении счёта,
// а не каждый кадр в Update (это экономит ресурсы).
public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText; // Текстовый компонент на Canvas
    [SerializeField] private string prefix = "Score: "; // Префикс перед числом

    private void Start()
    {
        // Подписываемся на событие — при каждом AddScore UI обновится автоматически
        ScoreManager.Instance.onScoreChanged.AddListener(UpdateUI);
        UpdateUI(0); // Инициализируем начальное значение
    }

    private void UpdateUI(int score)
    {
        scoreText.text = prefix + score.ToString();
    }

    private void OnDestroy()
    {
        // Обязательно отписываемся при уничтожении объекта —
        // иначе ScoreManager будет держать ссылку на мёртвый объект (утечка памяти)
        ScoreManager.Instance?.onScoreChanged.RemoveListener(UpdateUI);
    }
}