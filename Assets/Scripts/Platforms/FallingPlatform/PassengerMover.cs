using UnityEngine;

/*
    *Скрипт необходим чтобы персонаж двигался вместе с платформаой
*/
public class PassengerMover : MonoBehaviour
{
    //Позиция платформы
    private Vector3 lastPos;

    void Start() => lastPos = transform.position;

    //Специальный метод юнити -  Это идеальное место для реализации логики, 
    // которая должна выполняться после перемещения объектов, чтобы избежать рывков и визуальных багов
    //
    void LateUpdate()
    {
        
        Vector3 delta = transform.position - lastPos;   //смещение платформы

        //если есть смещение
        if (delta != Vector3.zero)
        {
            //перебираем все объекты
            foreach (var col in Physics2D.OverlapBoxAll(
                transform.position + Vector3.up * 0.6f, //смотрим выше платформы
                new Vector2(transform.localScale.x * 0.9f, 0.1f),   //размер просматриваемой области
                0))
            {
                //если платформа вытается двигать себя ингорируем
                if (col.gameObject != gameObject)
                    col.transform.position += delta;    //двигаем пассажиров
            }
        }

        lastPos = transform.position;   //сохраняем текущую позицию
    }
}
