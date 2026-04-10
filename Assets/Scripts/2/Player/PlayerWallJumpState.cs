using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    public PlayerWallJumpState(Player2 _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = 1f;
        player.SetVelocity(5 * -player.facingDir, player.jumpForce);
        AudioManager.instance.PlaySFX(38);
    }

    public override void Exit()
    {
        base.Exit();
    } 

    public override void Update()
    {
        base.Update();

        if(player.IsWallDetected()) { stateMachine.ChangeState(player.wallSlideState);return; }
        if(stateTimer<0)
        { stateMachine.ChangeState(player.airState); }
        if(player.IsGroundDetected())
        { stateMachine.ChangeState(player.idleState); }
    }
}
