using UnityEngine;

//*Данный код воспроизводит музыку для каждого уровня (При запуске сцены)
public class LevelMusic : MonoBehaviour
{
    [SerializeField] private AudioClip levelMusic;

    private void Start()
    {
        AudioManager.Instance.PlayMusic(levelMusic);
    }
}