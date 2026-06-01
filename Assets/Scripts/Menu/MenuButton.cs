using UnityEngine;
using UnityEngine.EventSystems;

/*
    *Этот скрипт отвечает за анимацию кнопки при наведении курсора мыши.
    Когда курсор наводится на кнопку — запускается анимация наведения, 
    когда уводится — анимация возвращается в исходное состояние.
    IPointerEnterHandler(Метод при наведении), IPointerExitHandler(Метод при уходе)
*/
public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //Блок сериализуемых полей(параметры)
    [SerializeField] private Animator iconAnimator; 
    [SerializeField] private string hoverParam = "isHovered"; 

    //*Unity автоматичеки вызывает функции при наведении мышки
    public void OnPointerEnter(PointerEventData e)
    {
        iconAnimator.SetBool(hoverParam, true);
    }

    public void OnPointerExit(PointerEventData e)
    {
        iconAnimator.SetBool(hoverParam, false);
    }
}