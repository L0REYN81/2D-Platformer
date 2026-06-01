using UnityEngine;
using UnityEngine.UI;

/*
    *Этот скрипт отвечает за изменение HealthBar Игрока
    !Изменить скрипт т.к он был расчитан на 10 хп
*/
public class Healthbar : MonoBehaviour
{   
    [SerializeField] private Health playerHealth;   //ссылка на текущие здоровье
    [SerializeField] private Image totalhealthBar;  //фон
    [SerializeField] private Image currenthealthBar; //Заполненность

    //Заполняем здоровье
    private void Start()
    {
        totalhealthBar. fillAmount = playerHealth.currentHealth / 10;
    }

    //ПРи изменения здоровья изменяем HealthBar
    private void Update()
    {
        currenthealthBar.fillAmount = playerHealth.currentHealth / 10;
    }
}
