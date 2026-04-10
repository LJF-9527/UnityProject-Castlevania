using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyGroundState : EnemyState
{

    protected Enemy_Shady enemy;
    protected Transform player;
    GameObject obj;
    public ShadyGroundState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Shady _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, player.transform.position) < enemy.alertDistance)
        { stateMachine.ChangeState(enemy.battleState); }

    }
}
