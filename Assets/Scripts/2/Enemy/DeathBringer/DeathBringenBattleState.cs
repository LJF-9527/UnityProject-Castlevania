using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringenBattleState : EnemyState
{
    private Transform player;
    private Enemy_DeathBringer enemy;
    private int moveDir;
    public DeathBringenBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;

        if (player.GetComponent<PlayerStats>().isDead)
        { stateMachine.ChangeState(enemy.idleState); }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;
            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                
                if (CanAttack() || !enemy.isKnocked)
                    stateMachine.ChangeState(enemy.attackState);
                else
                    stateMachine.ChangeState(enemy.idleState);
            }
        }
        
        if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
        {

        }
        if (player.position.x > enemy.transform.position.x)
        { moveDir = 1; }
        else if (player.position.x < enemy.transform.position.x)
        { moveDir = -1; }
        if (enemy.IsPlayerDetected() && enemy.IsPlayerDetected().distance < enemy.attackDistance - .5f) return;
        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
    }
    private bool CanAttack()
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.attackCooldown = Random.Range(enemy.minAttackCooldown, enemy.maxAttackCooldown);
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
        else { return false; }
    }
}
