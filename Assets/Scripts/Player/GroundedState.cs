using UnityEngine;

public class GroundedState : PlayerState
{
   public override void Enter(PlayerController ctx)
    {
        ctx.JumpCount = 0;          // Сбрасываем счётчик прыжков при касании земли
        ctx.Body.gravityScale = 7;  // Стандартная гравитация
        ctx.Anim.SetBool("grounded", true);
    }

    public override void HandleInput(PlayerController ctx)
    {
        float h = Input.GetAxis("Horizontal");

        ctx.Body.linearVelocity = new Vector2(h * ctx.speed, ctx.Body.linearVelocity.y);
        ctx.FlipTo(h);
        ctx.Anim.SetBool("run", h != 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Прыжок с земли
            ctx.Body.linearVelocity = new Vector2(ctx.Body.linearVelocity.x, ctx.jumpPower);
            AudioManager.Instance.PlaySFX(ctx.jumpClip);
            ctx.Anim.SetTrigger("jump");
            ctx.JumpCount = 1;
            ctx.ChangeState(new AirborneState());
        }
    }

    public override void PhysicsUpdate(PlayerController ctx)
    {
        // Игрок сошёл с платформы без прыжка — переходим в воздух
        if (!ctx.IsGrounded())
            ctx.ChangeState(new AirborneState());
    }
}
