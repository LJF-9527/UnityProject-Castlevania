using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyStunState : EnemyState
{
    [Header("Shady info")]
    private Enemy_Shady enemy;
    public ShadyStunState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Shady enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
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
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0) { stateMachine.ChangeState(enemy.idleState); }
    }
}
