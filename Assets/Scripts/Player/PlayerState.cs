using UnityEngine;

public abstract class PlayerState
{
    // Вызывается один раз при входе в состояние (настройка физики, анимации и т.д.)
    public virtual void Enter(PlayerController ctx) { }

    // Вызывается каждый Update — обработка ввода и переходы между состояниями
    public abstract void HandleInput(PlayerController ctx);

    // Вызывается каждый FixedUpdate — физически-зависимые переходы
    public abstract void PhysicsUpdate(PlayerController ctx);

    // Вызывается один раз при выходе из состояния (сброс временных параметров)
    public virtual void Exit(PlayerController ctx) { }
}