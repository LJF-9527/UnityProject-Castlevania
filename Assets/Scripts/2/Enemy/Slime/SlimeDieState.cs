using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlimeDieState : EnemyState
{
    protected Enemy_Slime enemy;
    public SlimeDieState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Slime _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        if(enemy.slimeTyep == SlimeTyep.small)
        {
            enemy.anim.SetBool(enemy.lastAnimBoolName, true);
            enemy.anim.speed = 0;
            enemy.cd.enabled = false;

            stateTimer = .1f;
        }
    }

    public override void Update()
    {
        base.Update();
        if(stateTimer>0)
        {
            rb.velocity = new Vector2(0, 10);
        }
        
    }
    public override void Exit()
    {
        base.Exit();
        Destroy(enemy.gameObject);
    }
}
