using Unity.VisualScripting;
using UnityEngine;
/*
    Контекст паттерна «Состояние».
    Хранит текущее состояние и делегирует ему весь ввод и физику.
    Сам не содержит игровой логики — только общие данные и утилиты.
    !присутствует дублирование в логике стены(доработать)
*/
public class PlayerController : MonoBehaviour
{
    //настройки движения
    [Header("Движение")]
    public float speed = 5f;
    public float jumpPower = 12f;
    public int maxJumps = 2;

    [Header("Стена")]
    public float wallSlideSpeed = 1f;
    //Сила горизонатльного и вертиканльного прыжка от стены
    public float wallJumpForceX = 8f; 
    public float wallJumpForceY = 10f;
    public float wallJumpControlLockTime = 0.2f; // Время блокировки горизонтального ввода после прыжка от стены

    [Header("Layers")]
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    
    [Header("Звуки")]
    [SerializeField] public AudioClip jumpClip;
    [SerializeField] public AudioClip doubleJumpClip;

    // Компоненты — доступны всем состояниям через контекст
    public Rigidbody2D Body { get; private set; }
    public Animator Anim { get; private set; }
    public BoxCollider2D BoxCol { get; private set; }

    // Разделяемые данные — состояния читают и пишут их напрямую
    public int JumpCount { get; set; }
    public float WallJumpTimer { get; set; }   // > 0 — горизонтальный ввод заблокирован

    private PlayerState _currentState;

    private void Awake()
    {
        Body   = GetComponent<Rigidbody2D>();
        Anim   = GetComponent<Animator>();
        BoxCol = GetComponent<BoxCollider2D>();

        // Начальное состояние — на земле
        ChangeState(new GroundedState());
    }

    // Update и FixedUpdate только транслируют вызов в текущее состояние
    private void Update()      => _currentState.HandleInput(this);
    private void FixedUpdate() => _currentState.PhysicsUpdate(this);

    // Переключение состояния: корректно завершает старое и инициализирует новое
    public void ChangeState(PlayerState next)
    {
        _currentState?.Exit(this);
        _currentState = next;
        _currentState.Enter(this);
    }

    // Проверка земли через BoxCast
    /*
        *Берётся коллацйдер персонажа и направляется вниз(Проверяем только нужный слой)
        *Остальные игнорируем
    */
    public bool IsGrounded()
    {
        return Physics2D.BoxCast(
            BoxCol.bounds.center, BoxCol.bounds.size,
            0, Vector2.down, 0.1f, groundLayer
        ).collider != null;
    }

    // Проверка стены в направлении взгляда персонажа
    //* также берём колайдер и двигаем его на 0.1 влево и право
    public bool OnWall()
    {
        bool right = Physics2D.BoxCast(
            BoxCol.bounds.center, BoxCol.bounds.size,
            0, Vector2.right, 0.1f, wallLayer).collider != null;

        bool left = Physics2D.BoxCast(
            BoxCol.bounds.center, BoxCol.bounds.size,
            0, Vector2.left, 0.1f, wallLayer).collider != null;

        return right || left;
    
    }

    // И отдельный метод — в какую сторону смотреть при прыжке(в противоположну сторону от стены)
    public float WallDirection()
    {
        bool right = Physics2D.BoxCast(BoxCol.bounds.center, BoxCol.bounds.size,
            0, Vector2.right, 0.1f, wallLayer).collider != null;

        return right ? 1f : -1f;
    }

    // Разворот персонажа: знак localScale.x задаёт направление спрайта
    public void FlipTo(float direction)
    {
        if (Mathf.Abs(direction) > 0.01f)
            transform.localScale = new Vector3(Mathf.Sign(direction) * 4, 4, 4);
    }
}