using UnityEngine;
using UnityEngine.SceneManagement;

//!Переписать логику с прошлого finishlevel
// Вешается на финишный коллайдер (Is Trigger = true)
public class LevelFinish : MonoBehaviour
{
    [SerializeField] private string nextSceneName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        int finalScore = ScoreManager.Instance.FinishLevel();

        
        Debug.Log($"Уровень пройден! Счёт: {finalScore}");

        SceneManager.LoadScene(nextSceneName);
    }
}