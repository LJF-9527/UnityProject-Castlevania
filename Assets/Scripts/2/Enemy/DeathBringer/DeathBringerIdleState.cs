using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerIdleState : EnemyState
{
    private Enemy_DeathBringer enemy;
    private Transform player;
    public DeathBringerIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;
        player=PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
        if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
        {
            enemy.Flip();
        }
        //AudioManager.instance.PlaySFX(24, enemy.transform);
    }

    public override void Update()
    {
        base.Update();
        if(Vector2.Distance(player.position,enemy.transform.position)<7)
            enemy.bossFigherBegun = true;
        if (stateTimer < 0&&enemy.bossFigherBegun) { stateMachine.ChangeState(enemy.battleState); }
        
    }
}
