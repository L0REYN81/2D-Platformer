using UnityEngine;
/*
    *Этот скрипт двигает острова в главном меню
*/
public class Floating : MonoBehaviour
{
    [SerializeField]public float speed = 1f;
    [SerializeField]public float height = 0.2f;

    Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.position = startPos + 
            Vector3.up * Mathf.Sin(Time.time * speed) * height; //ПЛавное движение
    }
}