using UnityEngine;
/*
    *Этот скрипт управляет летающей платформой, которая движется между заданными точками, 
    *может останавливаться на них, двигаться туда-сюда или по кругу
*/
public class FlyingPlatform : MonoBehaviour
{
    //Объявление переменных
    [Header("Путевые-точки")]
    public Transform[] waypoints;
    public float speed = 3f;
    public float waitTime = 0f;

    [Header("Настройки")]
    public bool loop = true;
    public bool pingPong = true;

    [Header("Звук")]
    [SerializeField] private AudioClip platformClip;
    [SerializeField] private float maxVolume = 1f;
    private AudioSource loopSource;

    //Внутренние переменные
    private int currentIndex = 0; //текущая точка
    private int direction = 1;  //направление в режиме ping pong 
    private float waitTimer = 0f;// ТАЙМЕР ОЖИДАНИЯ 
    private bool waitting = false;  //Стоит ли платформа

    //При запуске воиспроизводим звук
    void Start()
    {
        loopSource = AudioManager.Instance.PlayLoop(platformClip, volume: 1.5f, spatialBlend: 1f, maxDistance: 10f);
        if (loopSource != null)
            loopSource.transform.SetParent(transform); // звук следует за платформой
    }

    void Update()
    {
        //если нету точек стоим на месте
        if (waypoints.Length == 0) return;

        if (waitting)
        {
            if (loopSource != null) loopSource.volume = 0.1f; // тихо пока стоит
            waitTimer -= Time.deltaTime; 
            if (waitTimer <= 0f) waitting = false;
            return;
        }

        if (loopSource != null) loopSource.volume = maxVolume; // громко пока едет

        //Получаем индекс текущей точки и плавно двигаемся к ней
        Transform target = waypoints[currentIndex]; 
        transform.position = Vector2.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        //Если почти приблизились к точке устанавливаем точную позицию точки
        //И если есть ожидание ожидаем
        if (Vector2.Distance(transform.position, target.position) < 0.05f)
        {
            transform.position = target.position;

            if (waitTime > 0f)
            {
                waitting = true;
                waitTimer = waitTime;
            }

            NextWayPoint();
        }
    }

    //Выбираем следующую точку 
    void NextWayPoint()
    {
        if (pingPong)
        {   
            //ПРоверяем не вышли мы за границы массива(Не идём на несущ точку)
            if (currentIndex + direction >= waypoints.Length || currentIndex + direction < 0)
                direction *= -1; //меняем направление

            currentIndex += direction; //следующая точка
        }
        else
        {
            currentIndex++;

            if (currentIndex >= waypoints.Length)
                currentIndex = loop ? 0 : waypoints.Length - 1; //если LOOP ездим по кругу
        }
    }

    void OnDestroy()
    {
        AudioManager.Instance.StopLoop(loopSource);
    }

    //Отображение маршрута платформы(Специальный метод юнити)
    void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length < 2) return;

        Gizmos.color = Color.cyan;

        //Орисовываем точки
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            if (waypoints[i] && waypoints[i + 1])
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
        }

        //Если LOOP замыкаем 1 и конечную точку 
        if (loop && waypoints[0] && waypoints[waypoints.Length - 1])
            Gizmos.DrawLine(waypoints[waypoints.Length - 1].position, waypoints[0].position);
    }
}