using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player2 _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if(Input.GetKeyDown(KeyCode.R)&&player.skillManager.blackHole.blackHoleUnlocked) 
        { 
            if(player.skillManager.blackHole.cooldownTimer > 0)
            { player.fX.CreatePopUpText("技能CD:" + player.skillManager.blackHole.cooldownTimer.ToString("f1"),Color.white);return; }
            stateMachine.ChangeState(player.blackHoleState); 
        }
        if (Input.GetMouseButtonDown(1) && HasNoSword() && player.skillManager.sword.swordUnlocked)
        { 
            if(player.skillManager.sword.cooldownTimer>0)
            {
                player.fX.CreatePopUpText("技能CD:" + player.skillManager.sword.cooldownTimer.ToString("f1"), Color.white);
                return;
            }
            stateMachine.ChangeState(player.aimSwordState); 
        }
        if(Input.GetKey(KeyCode.Q)&&player.skillManager.parry.parryUnlocked) 
        { stateMachine.ChangeState(player.counterAttackState); }

        if(Input.GetKeyDown(KeyCode.Mouse0))
        { stateMachine.ChangeState(player.primaryAttackState); }

        if (!player.IsGroundDetected()) { stateMachine.ChangeState(player.airState); }//在空中时
        if(Input.GetKeyDown(KeyCode.Space)&&player.IsGroundDetected()) { stateMachine.ChangeState(player.jumpState); }//跳跃时
    }
    private bool HasNoSword()
    {
        if (!player.sword)
        { return true; }
        player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword();
        return false;
    }
}
