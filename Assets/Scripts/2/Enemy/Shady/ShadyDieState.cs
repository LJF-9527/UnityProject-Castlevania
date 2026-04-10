using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyDieState : EnemyState
{
    private Enemy_Shady enemy;
    public ShadyDieState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Shady _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
        {
            enemy.myItemDrop.GenerateDrop();
            enemy.SelfDestroy();
        }
    }
}
