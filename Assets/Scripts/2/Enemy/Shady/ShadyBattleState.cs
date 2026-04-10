using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyBattleState : EnemyState
{
    private Transform player;
    private Enemy_Shady enemy;
    private float defaultMoveSpeed;
    private int moveDir;
    public ShadyBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Shady _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;
        AudioManager.instance.PlaySFX(43);
        defaultMoveSpeed = enemy.moveSpeed;
        enemy.moveSpeed=enemy.moveSpeed*enemy.battleMoveSpeedMultiple;
                if (player.GetComponent<PlayerStats>().isDead)
        { stateMachine.ChangeState(enemy.moveState); }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.moveSpeed=defaultMoveSpeed;
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
                    enemy.stats.KillEntity();
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 15)
                stateMachine.ChangeState(enemy.idleState);
        }
        if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
        {

        }
        if (player.position.x > enemy.transform.position.x)
        { moveDir = 1; }
        else if (player.position.x < enemy.transform.position.x)
        { moveDir = -1; }

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
