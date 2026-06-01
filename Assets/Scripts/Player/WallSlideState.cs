using UnityEngine;

// Состояние «скольжение по стене»: замедленное падение и прыжок от стены.
// Выход: земля → GroundedState, отлип от стены → AirborneState, прыжок → AirborneState.
public class WallSlideState : PlayerState
{
    private bool _jumped; // флаг — прыжок уже был нажат

    public override void Enter(PlayerController ctx)
    {
        _jumped = false;
        ctx.Body.gravityScale = 0;
        ctx.Anim.SetBool("onWall", true); // анимация на стене
    }

    public override void HandleInput(PlayerController ctx)
    {
        if (_jumped) return; // не трогаем скорость после прыжка

        // Медленное скольжение вниз
        ctx.Body.linearVelocity = new Vector2(0, -ctx.wallSlideSpeed);

        if (Input.GetKeyDown(KeyCode.Space))
            WallJump(ctx);
    }

    public override void PhysicsUpdate(PlayerController ctx)
    {
        if (ctx.IsGrounded())
            ctx.ChangeState(new GroundedState());   //переходим в состояние земли
        else if (!ctx.OnWall() && !_jumped)
            ctx.ChangeState(new AirborneState());   //воздух
    }

    //При выходе из состояние восстанавливаем параметры
    public override void Exit(PlayerController ctx)
    {
        ctx.Body.gravityScale = 7;
        ctx.Anim.SetBool("onWall", false); // сбрасываем анимацию
    }

    private void WallJump(PlayerController ctx)
    {
        _jumped = true; // блокируем скольжение

        float dir = -ctx.WallDirection();   //прыгаем в противоположное направление
        //Задаём векторпражка
        ctx.Body.linearVelocity = new Vector2(dir * ctx.wallJumpForceX, ctx.wallJumpForceY);    
        ctx.WallJumpTimer = ctx.wallJumpControlLockTime;    //блокировка горизонтального ввода

        ctx.FlipTo(dir);    //поворачиваем игрока
        AudioManager.Instance.PlaySFX(ctx.jumpClip);
        ctx.Anim.SetTrigger("jump");
        ctx.ChangeState(new AirborneState());   //переход в другое состояние
    }
}