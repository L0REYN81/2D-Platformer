using UnityEngine;

/*
    *Этот скрипт управляет движущейся ловушкой(циркулярной пилой), 
    *которая постоянно ездит влево-вправо, 
    *воспроизводит звук и наносит урон игроку при столкновении.
    !Переписать код по мримеру платформы
*/
public class Enemy_Sideways : MonoBehaviour
{
    //Сереализация полей 
    [Header("Настройки")]
    [SerializeField]private float movementDistance;
    [SerializeField]private float speed;
    [SerializeField]private float damage;

    [Header("Звук")]
    [SerializeField] private AudioClip sawClip;
    private AudioSource loopSource; //Ссылка на источник воспроизводимого циклического звука.

    void Start()
    {
        loopSource = AudioManager.Instance.PlayLoop(sawClip, volume: 1.5f, spatialBlend: 1f, maxDistance: 10f);
        if (loopSource != null) //Создался ли объект воиспроизведенния 
            loopSource.transform.SetParent(transform); // звук следует за платформой
    }

    //текущие направление
    private bool movingLeft;

    //Границы движения
    private float leftEdge;
    private float rightEdge;

    //Получаем положение пилы и высчитываем границы
    void Awake()
    {
        leftEdge = transform.position.x - movementDistance;
        rightEdge = transform.position.x + movementDistance;
    }

    void Update()
    {
        //Движемся влево пока не достигнем лефой границы
        if (movingLeft)
        {
            if(transform.position.x > leftEdge)
            {
                //Вектор хранит 3 коорданаты на каждом кадре изменяем положение пилы
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
            {
                movingLeft = false;
            }
        }
        else
        {
            if(transform.position.x < rightEdge)
            {
              transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);  
            }
            else
            {
                movingLeft = true;
            } 
        }
    }

    //При столкновении с игроком наносим ему урон
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<Health>().TakeDamage(damage);
        }
    }
}
