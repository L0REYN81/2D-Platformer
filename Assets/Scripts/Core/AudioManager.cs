using UnityEngine;

/*
    *Этот скрипт реализует менеджер звука - этот объект управляет 
    *всей музыкой и звуком на сцене
*/

public class AudioManager : MonoBehaviour
{
    //Единственная статичная ссылка экземпляра 
    public static AudioManager Instance;

    //Сереализация полей
    [Header("Музыка")]
    [SerializeField] private AudioSource musicSource;

    [Header("Звуки")]
    [SerializeField] private AudioSource sfxSource;

    void Awake()
    {
        //Если менеджер существует сохраяем его ссылку 
        // и запрещаем уничтожекние объекта
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    //Воиспроизведение звуков
    public void PlaySFX(AudioClip clip)
    {
        //проверяем есть ли у объекта его звук
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    //Воспроизведение музыки
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    /*Создаём новый объект (Audio Source) (Клип, громкость, (2d или 3d звук), дистанция)
    Воиспроизведение цикличных звуков*/
    public AudioSource PlayLoop(AudioClip clip, float volume = 1f, float spatialBlend = 0f, float maxDistance = 10f)
    {
        if (clip == null) return null;
        AudioSource source = gameObject.AddComponent<AudioSource>(); // Добовляем audio source
        source.clip = clip;
        source.loop = true;
        source.volume = volume;
        source.spatialBlend = spatialBlend;
        source.maxDistance = maxDistance;
        source.Play(); //Запускаем воспроизведение и возращаем объект
        return source;
    }

    //Остановка Цикличного звука(Удаляем объект)
    public void StopLoop(AudioSource source)
    {
        if (source != null)
            Destroy(source);
    }

    //Назначение Текущей громкости Музыки
    public void SetMusicVolume(float value)
    {
        musicSource.volume = value;
    }

    //Назначение Текущей громкости Звуков
    public void SetSFXVolume(float value)
    {
        sfxSource.volume = value;
    }
}

