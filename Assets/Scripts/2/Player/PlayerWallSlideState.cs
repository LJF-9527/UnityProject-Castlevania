using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player2 _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        AudioManager.instance.PlaySFX(39);
    }

    public override void Exit()
    {
        base.Exit();
        AudioManager.instance.StopSFX(39);
    }

    public override void Update()
    {
        base.Update();

        if(!player.IsWallDetected()) { stateMachine.ChangeState(player.airState); }

        //µØ«ΩÃ¯
        if(Input.GetKeyDown(KeyCode.Space))
        { 
            stateMachine.ChangeState(player.wallJumpState);
            return;
        }
        //Õ—¿Î«ΩÃÂ ±
        if ((xInput != 0 && player.facingDir != xInput) || player.IsGroundDetected())
        { stateMachine.ChangeState(player.idleState); }
        //ª¨«ΩÀŸ∂»øÿ÷∆
        if (yInput < 0)
        { rb.velocity = new Vector2(0, rb.velocity.y); }
        else
        { rb.velocity = new Vector2(0, rb.velocity.y * .6f); }
    }
}
