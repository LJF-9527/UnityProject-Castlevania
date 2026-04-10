using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeStunState : EnemyState
{
    protected Enemy_Slime enemy;
    public SlimeStunState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Slime _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
        enemy.fX.InvokeRepeating("RedColorBlink", 0, .1f);
        stateTimer = enemy.stunDuration;
        rb.velocity = new Vector2(-enemy.facingDir * enemy.stuDirection.x, enemy.stuDirection.y);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.fX.Invoke("CancelColorChange", 0);
        enemy.stats.MakeInvincible(false);
    }

    public override void Update()
    {
        base.Update();

        if(rb.velocity.y<.1f&&enemy.IsGroundDetected())
        {
            enemy.anim.SetTrigger("isStunFold");
            enemy.stats.MakeInvincible(true);
        }

        if (stateTimer < 0) { stateMachine.ChangeState(enemy.idleState); }
    }
}
