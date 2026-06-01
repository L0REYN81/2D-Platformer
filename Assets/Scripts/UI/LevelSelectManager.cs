using UnityEngine;
using UnityEngine.UI;
using EasyTransition;

public class LevelSelectManager : MonoBehaviour
{
    public static LevelSelectManager Instance;

    [Header("Уровни")]
    [SerializeField] private string[] levelNames;
    [SerializeField] private Button[] levelButtons;
    [SerializeField] private TransitionSettings transition;
    [SerializeField] private float loadDelay = 0f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void LoadLevel(int index)
    {
        TransitionManager.Instance().Transition(levelNames[index], transition, loadDelay);
    }
}
