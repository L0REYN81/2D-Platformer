using UnityEngine;

// Состояние «в воздухе»: двойной прыжок, падение.
// Выход: земля → GroundedState, стена → WallSlideState.
public class AirborneState : PlayerState
{
    private float _wallCheckDelay = 0.1f; // пауза перед проверкой стены
    private float _timeInAir;

    //Если вошли в состояние отправляем ссылку на объект игрока и обнуляем таймер
    public override void Enter(PlayerController ctx)
    {
        _timeInAir = 0f;
        ctx.Body.gravityScale = 7;
        ctx.Anim.SetBool("grounded", false);
    }

    // На каждом кадре
    public override void HandleInput(PlayerController ctx)
    {
        if (ctx.WallJumpTimer > 0)
        {   
            //Не даём игроку некоторое врмя двигаться после прыжка от стены
            ctx.WallJumpTimer -= Time.deltaTime;    
        }
        else
        {
            float h = Input.GetAxis("Horizontal");
            ctx.Body.linearVelocity = new Vector2(h * ctx.speed, ctx.Body.linearVelocity.y);
            ctx.FlipTo(h);  //поворачиваем спрайт
            ctx.Anim.SetBool("run", h != 0);
        }

        ctx.Anim.SetFloat("yVelocity", ctx.Body.linearVelocity.y); //передаём аниматору вуртикальную скорость
        //если y > прыжок  y < падения

        //если нажата клавиша прыжок и ещё есть прыжки производим 2 прыжок
        if (Input.GetKeyDown(KeyCode.Space) && ctx.JumpCount < ctx.maxJumps)
        {
            ctx.Body.linearVelocity = new Vector2(ctx.Body.linearVelocity.x, ctx.jumpPower);
            AudioManager.Instance.PlaySFX(ctx.doubleJumpClip);
            ctx.Anim.SetTrigger("doubleJump");
            ctx.JumpCount++;
        }
    }

    public override void PhysicsUpdate(PlayerController ctx)
    {
        _timeInAir += Time.fixedDeltaTime;

        //Если есть земля меняем состояние
        if (ctx.IsGrounded() && ctx.Body.linearVelocity.y <= 0)
        {
            ctx.ChangeState(new GroundedState());
        }
        // Проверяем стену только после задержки и только при падении
        /*
            Три условия: задержка истекла + есть стена + падаем вниз. 
            Третье условие «y < 0» — не переходим в WallSlide при прыжке вдоль стены вверх.
        */
        else if (_timeInAir > _wallCheckDelay && ctx.OnWall() && ctx.Body.linearVelocity.y < 0)
        {
            ctx.ChangeState(new WallSlideState()); //состояние стена
        }
    }
}