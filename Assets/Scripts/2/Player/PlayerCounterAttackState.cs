using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private bool canCreateClone;
    public bool isCounter = false;
    public PlayerCounterAttackState(Player2 _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        canCreateClone = true;

        player.anim.SetBool("SuccessfulCounterAttack", false);
        player.stats.MakeBlock(true);
    }

    public override void Exit()
    {
        base.Exit();
        isCounter = false;
    }

    public override void Update()
    {
        base.Update();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        if (Input.GetKeyUp(KeyCode.Q))
        {

            player.stats.MakeBlock(false);
            foreach (var hit in colliders)
            {
                if (hit.GetComponent<ArrowController>() != null)
                {
                    if (player.skillManager.parry.CanUseSkill())
                    {
                        SuccesfulConterAttackAnim();
                        hit.GetComponent<ArrowController>().FlipArrow();
                        AudioManager.instance.PlaySFX(0);
                        isCounter = true;
                        return;
                    }
                }
                if (hit.GetComponent<Enemy>() != null)
                {
                    if (hit.GetComponent<Enemy>().CanBeStunned())
                    {
                        if (player.skillManager.parry.CanUseSkill())
                        {
                            SuccesfulConterAttackAnim();
                            player.skillManager.parry.UseSkill();
                            AudioManager.instance.PlaySFX(1);
                            isCounter = true;
                            if (canCreateClone)
                            {
                                canCreateClone = false;
                                player.skillManager.parry.MakeMirageOnParry(hit.transform);
                            }
                            return;

                        }
                    }
                }
            }

            stateMachine.ChangeState(player.idleState);
        }
        player.SetZeroVelocity();
        if (triggerCalled) { stateMachine.ChangeState(player.idleState); }
    }

    private void SuccesfulConterAttackAnim()
    {

        player.anim.SetBool("SuccessfulCounterAttack", true);
    }

}
