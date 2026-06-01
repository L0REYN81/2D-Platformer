using NUnit.Framework;
using UnityEngine;

/*
    Патерн: Singelton - единтсвенный экзепляр доступный  глбоально
    *Этот код отвечает за Сохранение текущей точки спавна игрока
*/
public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    public Transform respawnPoint; // текущая точка
    public Transform defaultSpawnPoint; // начальная точка

    void Awake()
    {
        //Единственный экземплят класса
        Instance = this;
        respawnPoint = defaultSpawnPoint;
    }

    //Задаём новую точку спавна
    public void SetCheckpoint(Transform newPoint)
    {
        respawnPoint = newPoint;
        Debug.Log("Чекпоинт сохранён: " + newPoint.name);
    }

    //Телепорт игрока на текущую точку спавна
    public void RespawnPlayer(GameObject player)
    {
        player.transform.position = respawnPoint.position;
    }
    
}
