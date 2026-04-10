using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    public int comboCounter {  get; private set; }

    private float lastTimeAttacked;
    private float comboWindow = 2;
    public PlayerPrimaryAttackState(Player2 _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        xInput = 0;//这行代码用于修复bug
        
        if(comboCounter>2||Time.time>=lastTimeAttacked+comboWindow) { comboCounter = 0; }

        player.anim.SetInteger("ComboCounter", comboCounter);
        #region ChangeAttackDir
        float attackDir=(xInput!=0)?attackDir=xInput:attackDir=player.facingDir;
        #endregion
        player.SetVelocity(player.attackMovement[comboCounter].x* attackDir, player.attackMovement[comboCounter].y);
        stateTimer = .1f;
    }

    public override void Exit()
    {
        base.Exit();
        comboCounter++;
        lastTimeAttacked=Time.time;
        player.StartCoroutine("BusyFor", .15f);
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer<0)
        { rb.velocity = Vector2.zero; }
        if(triggerCalled)
        { stateMachine.ChangeState(player.idleState); }
    }
}
